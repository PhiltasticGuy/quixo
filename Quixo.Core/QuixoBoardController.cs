using Quixo.Core.Players;
using Quixo.Core.Results;
using System;
using System.Linq;
using System.Threading;

namespace Quixo.Core
{
    public class QuixoBoardController
    {
        private QuixoBoard _board;
        private bool _isGameRunning;
        private bool _isPlayer1Turn;
        private Player _player1;
        private Player _player2;
        private int? _selectedPieceIndex;

        public bool IsGameRunning => _isGameRunning;
        public int? SelectedPieceIndex => _selectedPieceIndex;
        public QuixoPiece SelectedPiece => (_selectedPieceIndex == null ? null : _board.Pieces[_selectedPieceIndex.Value]);
        public Player CurrentPlayer => (_isPlayer1Turn ? _player1 : _player2);
        public Player OtherPlayer => (!_isPlayer1Turn ? _player1 : _player2);

        public QuixoBoardController()
        {
            _board = new QuixoBoard();

            PrepareNewGame();
        }

        public bool HasHumanPlayers() => _player1?.IsHuman == true || _player2?.IsHuman == true;
        public void SetPlayer1(Player player) => _player1 = player;
        public void SetPlayer2(Player player) => _player2 = player;
        public QuixoPiece GetPiece(int index) => _board.Pieces[index];
        public int GetTotalPieces() => _board.Pieces.Length;

        public ControllerResult SelectPiece(int index)
        {
            if (!CurrentPlayer.IsInputRequired)
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
                    if (!_board.Pieces[index].IsValidMove)
                    {
                        return new ErrorResult("Vous devez choisir une case valide pour votre pièce, ou la désélectionner!");
                    }

                    // Désactiver les guides de cases valides.
                    toggleValidMoves(_selectedPieceIndex.Value);
                    _board.Pieces[_selectedPieceIndex.Value].IsSelected = false;

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

                    _board.Play(move, CurrentPlayer.PieceType);
                    _selectedPieceIndex = null;

                    // Vérifier si la partie est gagnée.
                    var winner = CheckGameState();
                    if (winner != null)
                    {
                        return new WinResult(winner);
                    }
                    else
                    {
                        // Passer au prochain joueur.
                        HandleTurns();

                        // Le garbage collector de .NET Core semble avoir un peu
                        // de difficulté avec le nombre d'allocations par minmax.
                        Console.WriteLine(GC.GetTotalMemory(true));

                        return new NextPlayerResult(CurrentPlayer);
                    }
                }
            }
            else
            {
                // S'assurer que la pièce choisie se trouve sur le périmètre.
                if (!_board.IsPerimeterPiece(index))
                {
                    return new ErrorResult("Vous devez choisir une pièce sur le périmètre!");
                }
                // Si la pièce choisie n'est pas vide...
                else if (!_board.Pieces[index].IsEmptyPiece)
                {
                    // S'assurer que la pièce choisie correspond au jeton du joueur.
                    if ((_board.Pieces[index].IsCirclePiece && CurrentPlayer.PieceType != PieceType.Circle) ||
                        (_board.Pieces[index].IsCrossmarkPiece && CurrentPlayer.PieceType != PieceType.Crossmark))
                    {
                        return new ErrorResult("Vous devez choisir une de vos pièces!");
                    }
                }

                _selectedPieceIndex = index;
                toggleValidMoves(index);

                return new PieceSelectedResult(_selectedPieceIndex);
            }
        }

        public void RestartCurrentGame()
        {
            _isGameRunning = false;

            _isPlayer1Turn = true;
            _player1.IsInputRequired = false;
            _player2.IsInputRequired = false;
            _board.Reset();
        }

        public void PrepareNewGame()
        {
            _isGameRunning = false;

            _player1 = null;
            _player2 = null;
            _isPlayer1Turn = true;
            _board.Reset();
        }

        public void StartGame()
        {
            _isGameRunning = true;

            new Thread(_ => HandleTurns()).Start();
        }

        private Player CheckGameState()
        {
            bool isCurrentPlayerWin = _board.CheckPieceWin(CurrentPlayer.PieceType);
            bool isOtherWin = _board.CheckPieceWin(OtherPlayer.PieceType);

            Player winner = null;
            if (isCurrentPlayerWin && isOtherWin)
            {
                winner = OtherPlayer;
            }
            else if (isCurrentPlayerWin)
            {
                winner = CurrentPlayer;
            }
            else if (isOtherWin)
            {
                winner = OtherPlayer;
            }
            else
            {
                winner = null;
            }

            return winner;
        }

        private void HandleTurns()
        {
            while (IsGameRunning && CurrentPlayer.PlayTurn(_board) && CheckGameState() == null)
            {
                _isPlayer1Turn = !_isPlayer1Turn;

                // Ralentir les joueurs IA.
                if (!HasHumanPlayers())
                {
                    Thread.Sleep(1000);
                }
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
                _board.Pieces[left].IsValidMove = !_board.Pieces[left].IsValidMove;
            }
            if (right != selectedPieceIndex)
            {
                _board.Pieces[right].IsValidMove = !_board.Pieces[right].IsValidMove;
            }
            if (top != selectedPieceIndex)
            {
                _board.Pieces[top].IsValidMove = !_board.Pieces[top].IsValidMove;
            }
            if (bottom != selectedPieceIndex)
            {
                _board.Pieces[bottom].IsValidMove = !_board.Pieces[bottom].IsValidMove;
            }
        }

        private void TestWinMoveCrossmark()
        {
            _board.Pieces[0].PieceType = PieceType.Crossmark;
            _board.Pieces[5].PieceType = PieceType.Crossmark;
            _board.Pieces[10].PieceType = PieceType.Crossmark;
            _board.Pieces[20].PieceType = PieceType.Crossmark;
            _board.Pieces[2].PieceType = PieceType.Circle;
            _board.Pieces[3].PieceType = PieceType.Circle;
            _board.Pieces[4].PieceType = PieceType.Circle;
            _board.Pieces[9].PieceType = PieceType.Circle;
            _board.Pieces[14].PieceType = PieceType.Circle;

            _player2 = new MinMaxAiPlayer(2, "AI", PieceType.Crossmark);
            _player2.PlayTurn(_board);
        }

        private void TestWinMoveBlockSimplest()
        {
            _board.Pieces[10].PieceType = PieceType.Crossmark;
            _board.Pieces[15].PieceType = PieceType.Crossmark;
            _board.Pieces[20].PieceType = PieceType.Crossmark;
            _board.Pieces[4].PieceType = PieceType.Circle;
            _board.Pieces[9].PieceType = PieceType.Circle;
            _board.Pieces[14].PieceType = PieceType.Circle;
            _board.Pieces[19].PieceType = PieceType.Circle;

            _player2 = new MinMaxAiPlayer(2, "AI", PieceType.Crossmark);
            _player2.PlayTurn(_board);
        }
        private void TestWinMoveBlock()
        {
            _board.Pieces[0].PieceType = PieceType.Crossmark;
            _board.Pieces[5].PieceType = PieceType.Crossmark;
            _board.Pieces[10].PieceType = PieceType.Crossmark;
            _board.Pieces[1].PieceType = PieceType.Crossmark;
            _board.Pieces[20].PieceType = PieceType.Circle;
            _board.Pieces[4].PieceType = PieceType.Circle;
            _board.Pieces[9].PieceType = PieceType.Circle;
            _board.Pieces[14].PieceType = PieceType.Circle;
            _board.Pieces[24].PieceType = PieceType.Circle;

            _player2 = new MinMaxAiPlayer(2, "AI", PieceType.Crossmark);
            _player2.PlayTurn(_board);
        }
    }
}
