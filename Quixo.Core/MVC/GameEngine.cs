using Quixo.Core.Players;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Quixo.Core.MVC
{
    public class GameEngine
    {
        private readonly QuixoBoard _board = new QuixoBoard();

        private Player _player1;
        private Player _player2;

        private bool _isGameReady;
        private bool _isGameRunning;
        private bool _isPlayer1Turn;
        private Player _winner;

        public QuixoBoard Board => _board;
        public Player Winner => (_winner != null ? _winner : CheckGameState());

        public bool IsGameReady => _isGameReady;
        public bool IsGameRunning => _isGameRunning;

        public bool HasHumanPlayers() => _player1?.IsHuman == true || _player2?.IsHuman == true;
        public Player CurrentPlayer => (_isPlayer1Turn ? _player1 : _player2);
        public Player OtherPlayer => (!_isPlayer1Turn ? _player1 : _player2);

        public GameEngine()
        {
            ClearExistingGame();
        }

        public void StartGame()
        {
            if (_player1 == null || _player2 == null)
            {
                throw new InvalidOperationException("Une partie ne peut pas être lancée sans avoir deux joueurs d'assignés!");
            }

            _isGameRunning = true;

            NextTurn();
        }

        public void RestartCurrentGame()
        {
            ResetGameData();

            _player1.IsInputRequired = false;
            _player2.IsInputRequired = false;

            _board.Reset();
        }

        public void ClearExistingGame()
        {
            ResetGameData();

            _player1 = null;
            _player2 = null;
        }

        public void InitializeNewGame(Player player1, Player player2)
        {
            ResetGameData();

            _player1 = player1;
            _player2 = player2;

            _isGameReady = true;
        }

        public void NextTurn()
        {
            if (HasHumanPlayers())
            {
                HandleTurns();
            }
            else
            {
                new Thread(_ => HandleTurns()).Start();
            }
        }

        private void ResetGameData()
        {
            _isGameReady = false;
            _isGameRunning = false;

            _winner = null;
            _isPlayer1Turn = true;

            _board.Reset();
        }

        private void HandleTurns()
        {
            while (_isGameRunning && CurrentPlayer.PlayTurn(_board) && Winner == null)
            {
                _isPlayer1Turn = !_isPlayer1Turn;

                // Ralentir les joueurs IA.
                if (!HasHumanPlayers())
                {
                    Thread.Sleep(800);
                }
            }
        }

        private Player CheckGameState()
        {
            bool isCurrentPlayerWin = _board.CheckPieceWin(CurrentPlayer.PieceType);
            bool isOtherWin = _board.CheckPieceWin(OtherPlayer.PieceType);

            if (isOtherWin)
            {
                _winner = OtherPlayer;
            }
            else if (isCurrentPlayerWin)
            {
                _winner = CurrentPlayer;
            }
            else
            {
                _winner = null;
            }

            // Bloquer l'interaction des joueurs avec la grille.
            if (_winner != null)
            {
                _player1.IsInputRequired = false;
                _player2.IsInputRequired = false;
            }

            return _winner;
        }
    }
}
