using Quixo.Core.Players;
using Quixo.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Quixo.Core.MVC
{
    public class GameController
    {
        private readonly GameEngine _gameEngine = new GameEngine();

        private Timer _timer;
        private int? _selectedPieceIndex;

        public GameEngine Model => _gameEngine;
        public int? SelectedPieceIndex => _selectedPieceIndex;

        public void SelectPlayers(PlayerType player1Type, PlayerType player2Type)
        {
            Player player1 = PlayerFactory.Instance().CreatePlayer(
                player1Type,
                1,
                "Player #1",
                PieceType.Circle
            );
            Player player2 = PlayerFactory.Instance().CreatePlayer(
                 player2Type,
                 2,
                 "Player #2",
                 PieceType.Crossmark
             );

            _gameEngine.InitializeNewGame(player1, player2);
        }

        public void StartGame(Timer timer)
        {
            _timer = timer;
            if (!_gameEngine.HasHumanPlayers())
            {
                _timer.Enabled = true;
            }

            _gameEngine.StartGame();
        }

        public void RestartCurrentGame()
        {
            _timer.Enabled = false;
            _gameEngine.RestartCurrentGame();
        }

        public void ClearExistingGame()
        {
            _timer.Enabled = false;
            _gameEngine.ClearExistingGame();
        }

        public ControllerResult SelectPiece(int index)
        {
            if (!_gameEngine.CurrentPlayer.IsInputRequired)
            {
                return new ErrorResult("Vous n'avez pas la permission de jouer pour l'instant!");
            }

            // Si une pièce était déjà sélectionnée, choisir une case de 
            // destination valide.
            if (_selectedPieceIndex != null)
            {
                // Si la pièce était déjà sélectionnée, déselectionner la pièce.
                if (index == _selectedPieceIndex)
                {
                    _selectedPieceIndex = null;

                    toggleValidMoves(index);

                    return new PieceUnselectedResult(index);
                }
                else
                {
                    if (!_gameEngine.Board.Pieces[index].IsValidMove)
                    {
                        return new ErrorResult("Vous devez choisir une case valide pour votre pièce, ou la désélectionner!");
                    }

                    // Désactiver les guides de cases valides.
                    toggleValidMoves(_selectedPieceIndex.Value);
                    _gameEngine.Board.Pieces[_selectedPieceIndex.Value].IsSelected = false;

                    //PlacePiece(index);
                    int rowStart = _selectedPieceIndex.Value / 5;
                    int colStart = _selectedPieceIndex.Value % 5;
                    int rowEnd = index / 5;
                    int colEnd = index % 5;
                    Move move = new Move();

                    if (rowStart == rowEnd)
                    {
                        if (colEnd < colStart)
                        {
                            move.Direction = MoveDirection.ShiftRight;
                        }
                        else
                        {
                            move.Direction = MoveDirection.ShiftLeft;
                        }
                    }
                    else
                    {
                        if (rowEnd < rowStart)
                        {
                            move.Direction = MoveDirection.ShiftDown;
                        }
                        else
                        {
                            move.Direction = MoveDirection.ShiftUp;
                        }
                    }
                    move.Index = _selectedPieceIndex.Value;

                    _gameEngine.Board.Play(move, _gameEngine.CurrentPlayer.PieceType);
                    _selectedPieceIndex = null;

                    // Vérifier si la partie est gagnée.
                    if (_gameEngine.Winner != null)
                    {
                        return new WinResult(_gameEngine.Winner);
                    }
                    else
                    {
                        // Passer au prochain joueur.
                        _gameEngine.NextTurn();

                        // Le garbage collector de .NET Core semble avoir un peu
                        // de difficulté avec le nombre d'allocations par minmax.
                        Console.WriteLine(GC.GetTotalMemory(true));

                        return new NextPlayerResult(_gameEngine.CurrentPlayer);
                    }
                }
            }
            else
            {
                // S'assurer que la pièce choisie se trouve sur le périmètre.
                if (!_gameEngine.Board.IsPerimeterPiece(index))
                {
                    return new ErrorResult("Vous devez choisir une pièce sur le périmètre!");
                }
                // Si la pièce choisie n'est pas vide...
                else if (!_gameEngine.Board.Pieces[index].IsEmptyPiece)
                {
                    // S'assurer que la pièce choisie correspond au jeton du joueur.
                    if ((_gameEngine.Board.Pieces[index].IsCirclePiece && _gameEngine.CurrentPlayer.PieceType != PieceType.Circle) ||
                        (_gameEngine.Board.Pieces[index].IsCrossmarkPiece && _gameEngine.CurrentPlayer.PieceType != PieceType.Crossmark))
                    {
                        return new ErrorResult("Vous devez choisir une de vos pièces!");
                    }
                }

                _selectedPieceIndex = index;
                toggleValidMoves(index);

                return new PieceSelectedResult(_selectedPieceIndex);
            }
        }

        private void toggleValidMoves(int selectedPieceIndex)
        {
            int row = selectedPieceIndex / 5;
            int left = row * 5;
            int right = left + 4;

            int col = selectedPieceIndex % 5;
            int top = col;
            int bottom = col + 20;

            if (left != selectedPieceIndex)
            {
                _gameEngine.Board.Pieces[left].IsValidMove = !_gameEngine.Board.Pieces[left].IsValidMove;
            }
            if (right != selectedPieceIndex)
            {
                _gameEngine.Board.Pieces[right].IsValidMove = !_gameEngine.Board.Pieces[right].IsValidMove;
            }
            if (top != selectedPieceIndex)
            {
                _gameEngine.Board.Pieces[top].IsValidMove = !_gameEngine.Board.Pieces[top].IsValidMove;
            }
            if (bottom != selectedPieceIndex)
            {
                _gameEngine.Board.Pieces[bottom].IsValidMove = !_gameEngine.Board.Pieces[bottom].IsValidMove;
            }
        }
    }
}
