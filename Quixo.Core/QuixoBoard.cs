using System;
using System.Collections.Generic;
using System.Linq;

namespace Quixo.Core
{
    public class QuixoBoard
    {
        private const int Width = 5;
        private readonly int[] _perimeterIndexes = {
             0,  1,  2,  3,  4,
             5,              9,
            10,             14,
            15,             19,
            20, 21, 22, 23, 24 };

        public QuixoPiece[] Pieces { get; }

        public QuixoBoard()
        {
            Pieces = new QuixoPiece[25];

            for (int i = 0; i < Pieces.Length; i++)
            {
                Pieces[i] = new QuixoPiece(PieceType.Empty);
            }
        }

        public QuixoBoard(QuixoPiece[] pieces)
        {
            Pieces = pieces;
        }

        public bool IsPerimeterPiece(int index)
        {
            return _perimeterIndexes.Contains(index);
        }

        public QuixoBoard DeepClone()
        {
            QuixoPiece[] pieces = new QuixoPiece[25];

            for (int i = 0; i < Pieces.Length; i++)
            {
                pieces[i] = new QuixoPiece(Pieces[i].PieceType);
            }

            return new QuixoBoard(pieces);
        }

        public List<Move> GetValidMoves(PieceType pieceType)
        {
            var moves = new List<Move>();

            foreach (var i in _perimeterIndexes)
            {
                if (Pieces[i].PieceType == PieceType.Empty || Pieces[i].PieceType == pieceType)
                {
                    if (i < Width)
                    {
                        moves.Add(new Move() { Index = i, Direction = MoveDirection.ShiftUp });

                        if (i % Width != 0)
                        {
                            moves.Add(new Move() { Index = i, Direction = MoveDirection.ShiftRight });
                        }
                        if ((i + 1) % Width != 0)
                        {
                            moves.Add(new Move() { Index = i, Direction = MoveDirection.ShiftLeft });
                        }
                    }
                    else if ((i + Width) >= Pieces.Length)
                    {
                        moves.Add(new Move() { Index = i, Direction = MoveDirection.ShiftDown });

                        if (i % Width != 0)
                        {
                            moves.Add(new Move() { Index = i, Direction = MoveDirection.ShiftRight });
                        }
                        if ((i + 1) % Width != 0)
                        {
                            moves.Add(new Move() { Index = i, Direction = MoveDirection.ShiftLeft });
                        }
                    }
                    else
                    {
                        moves.Add(new Move() { Index = i, Direction = MoveDirection.ShiftUp });
                        moves.Add(new Move() { Index = i, Direction = MoveDirection.ShiftDown });

                        if (i % Width == 0)
                        {
                            moves.Add(new Move() { Index = i, Direction = MoveDirection.ShiftLeft });
                        }
                        else if ((i + 1) % Width == 0)
                        {
                            moves.Add(new Move() { Index = i, Direction = MoveDirection.ShiftRight });
                        }
                    }
                }
            }

            return moves;
        }

        public void Play(Move move, PieceType pieceType)
        {
            if (Pieces[move.Index].PieceType != PieceType.Empty && Pieces[move.Index].PieceType != pieceType)
            {
                throw new InvalidOperationException();
            }

            int rowStart = move.Index / 5;
            int colStart = move.Index % 5;
            int indexEnd = -1;

            if (move.Direction == MoveDirection.ShiftUp)
            {
                for (int i = rowStart; i < 4; i++)
                {
                    Pieces[i * 5 + colStart].PieceType = Pieces[(i + 1) * 5 + colStart].PieceType;
                }

                indexEnd = 20 + colStart;
            }
            else if (move.Direction == MoveDirection.ShiftDown)
            {
                for (int i = rowStart; i > 0; i--)
                {
                    Pieces[i * 5 + colStart].PieceType = Pieces[(i - 1) * 5 + colStart].PieceType;
                }

                indexEnd = colStart;
            }
            else if (move.Direction == MoveDirection.ShiftLeft)
            {
                for (int i = colStart; i < 4; i++)
                {
                    Pieces[i + rowStart * 5].PieceType = Pieces[i + rowStart * 5 + 1].PieceType;
                }

                indexEnd = rowStart * 5 + 4;
            }
            else if (move.Direction == MoveDirection.ShiftRight)
            {
                for (int i = colStart; i > 0; i--)
                {
                    Pieces[i + rowStart * 5].PieceType = Pieces[i + rowStart * 5 - 1].PieceType;
                }

                indexEnd = rowStart * 5;
            }

            Pieces[indexEnd].PieceType = pieceType;
        }

        public bool CheckPieceWin(PieceType pieceType)
        {
            if ((Pieces[0].PieceType == pieceType && Pieces[1].PieceType == pieceType && Pieces[2].PieceType == pieceType && Pieces[3].PieceType == pieceType && Pieces[4].PieceType == pieceType) ||
                (Pieces[5].PieceType == pieceType && Pieces[6].PieceType == pieceType && Pieces[7].PieceType == pieceType && Pieces[8].PieceType == pieceType && Pieces[9].PieceType == pieceType) ||
                (Pieces[10].PieceType == pieceType && Pieces[11].PieceType == pieceType && Pieces[12].PieceType == pieceType && Pieces[13].PieceType == pieceType && Pieces[14].PieceType == pieceType) ||
                (Pieces[15].PieceType == pieceType && Pieces[16].PieceType == pieceType && Pieces[17].PieceType == pieceType && Pieces[18].PieceType == pieceType && Pieces[19].PieceType == pieceType) ||
                (Pieces[20].PieceType == pieceType && Pieces[21].PieceType == pieceType && Pieces[22].PieceType == pieceType && Pieces[23].PieceType == pieceType && Pieces[24].PieceType == pieceType))
            {
                return true;
            }

            if ((Pieces[0].PieceType == pieceType && Pieces[5].PieceType == pieceType && Pieces[10].PieceType == pieceType && Pieces[15].PieceType == pieceType && Pieces[20].PieceType == pieceType) ||
                (Pieces[1].PieceType == pieceType && Pieces[6].PieceType == pieceType && Pieces[11].PieceType == pieceType && Pieces[16].PieceType == pieceType && Pieces[21].PieceType == pieceType) ||
                (Pieces[2].PieceType == pieceType && Pieces[7].PieceType == pieceType && Pieces[12].PieceType == pieceType && Pieces[17].PieceType == pieceType && Pieces[22].PieceType == pieceType) ||
                (Pieces[3].PieceType == pieceType && Pieces[8].PieceType == pieceType && Pieces[13].PieceType == pieceType && Pieces[18].PieceType == pieceType && Pieces[23].PieceType == pieceType) ||
                (Pieces[4].PieceType == pieceType && Pieces[9].PieceType == pieceType && Pieces[14].PieceType == pieceType && Pieces[19].PieceType == pieceType && Pieces[24].PieceType == pieceType))
            {
                return true;
            }

            if ((Pieces[0].PieceType == pieceType && Pieces[6].PieceType == pieceType && Pieces[12].PieceType == pieceType && Pieces[18].PieceType == pieceType && Pieces[24].PieceType == pieceType) ||
                (Pieces[4].PieceType == pieceType && Pieces[8].PieceType == pieceType && Pieces[12].PieceType == pieceType && Pieces[16].PieceType == pieceType && Pieces[20].PieceType == pieceType))
            {
                return true;
            }

            return false;
        }

        public void Reset()
        {
            for (int i = 0; i < Pieces.Length; i++)
            {
                Pieces[i].PieceType = PieceType.Empty;
            }
        }

        public PieceType GetWinner()
        {
            if (CheckPieceWin(PieceType.Circle))
            {
                return PieceType.Circle;
            }
            else if (CheckPieceWin(PieceType.Crossmark))
            {
                return PieceType.Crossmark;
            }
            else
            {
                return PieceType.Empty;
            }
        }
    }
}
