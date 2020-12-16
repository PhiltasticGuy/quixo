using System.Collections.Generic;

namespace Quixo.Core.Players.AI.MinMax
{
    public abstract class Node
    {
        public int Depth { get; set; }
        public int Value { get; set; }
        public PieceType PlayerPieceType { get; set; }
        public PieceType OpponentPieceType => (PlayerPieceType == PieceType.Circle ? PieceType.Crossmark : PieceType.Circle);
        public Move Move { get; set; }
        public List<Node> Children { get; set; }

        public Node(Move move, PieceType playerPieceType, int depth)
        {
            Depth = depth;
            PlayerPieceType = playerPieceType;
            Move = move;
            Children = new List<Node>();
        }

        public abstract PieceType GetCurrentMovePieceType();
        public abstract int CompareValues(int v1, int v2);
        public abstract Node CreateChild(Move move, int depth);
        public abstract Move PickBestMoveFromChildren();

        public int Evaluate(QuixoBoard board)
        {
            var winner = board.GetWinner();
            if (winner == PlayerPieceType)
            {
                return int.MaxValue;
            }
            else if (winner == OpponentPieceType)
            {
                return int.MinValue;
            }

            int valueMax = EvaluateRows(board, PlayerPieceType) + 
                EvaluateColumns(board, PlayerPieceType) + 
                EvaluateDiagonals(board, PlayerPieceType);

            int valueMin = EvaluateRows(board, OpponentPieceType) +
                EvaluateColumns(board, OpponentPieceType) +
                EvaluateDiagonals(board, OpponentPieceType);

            return valueMax - valueMin;
        }

        private int EvaluateRows(QuixoBoard board, PieceType pieceType)
        {
            var value = 0;
            int count = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int index = i * 5 + j;
                    if (board.Pieces[index].PieceType == pieceType)
                    {
                        count++;
                    }
                }

                value += count;
                count = 0;
            }

            return value;
        }

        private int EvaluateColumns(QuixoBoard board, PieceType pieceType)
        {
            var value = 0;
            int count = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int index = j * 5 + i;
                    if (board.Pieces[index].PieceType == pieceType)
                    {
                        count++;
                    }
                }

                value += count;
                count = 0;
            }

            return value;
        }

        private int EvaluateDiagonals(QuixoBoard board, PieceType pieceType)
        {
            var value = 0;

            int count = 0;
            for (int i = 0; i < 5; i++)
            {
                if (board.Pieces[i * 5 + i].PieceType == pieceType)
                {
                    count++;
                }
            }

            value += count;
            count = 0;

            for (int i = 0; i < 5; i++)
            {
                if (board.Pieces[(i + 1) * 4].PieceType == pieceType)
                {
                    count++;
                }
            }

            value += count;

            return value;
        }
    }
}
