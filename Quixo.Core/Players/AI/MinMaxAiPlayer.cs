using Quixo.Core.Players.AI.MinMax;
using System;

namespace Quixo.Core.Players.AI
{
    public class MinMaxAiPlayer : ComputerPlayer
    {
        protected readonly MinMaxStrategy _minMaxStrategy;

        public MinMaxAiPlayer(int id, string name, PieceType pieceType, MinMaxStrategy minMaxStrategy)
            : base(id, name, pieceType)
        {
            _minMaxStrategy = minMaxStrategy;
        }

        public override bool PlayTurn(QuixoBoard board)
        {
            Move bestMove;
            if (board.IsStillEmpty())
            {
                // Nous rendons le jeu un peu plus intéressant en faisant placer
                // une pièce aléatoire pour les IA si la grille est encore vide.
                var moves = board.GetValidMoves(this.PieceType);
                var selectedMoveIndex = new Random().Next(moves.Count);
                bestMove = moves[selectedMoveIndex];
            }
            else
            {
                Node root = new MaxNode(null, PieceType, 0);
                _minMaxStrategy.MinMax(board, root);
                bestMove = root.PickBestMoveFromChildren();
            }

            board.Play(bestMove, this.PieceType);

            Console.WriteLine($"{this.Name}: Piece #{bestMove.Index + 1}, {bestMove.Direction}");

            return true;
        }
    }
}
