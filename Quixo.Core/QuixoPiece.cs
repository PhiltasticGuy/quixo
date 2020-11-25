using System;

namespace Quixo.Core
{
    public class QuixoPiece
    {
        private PieceType _pieceType;

        public bool IsEmptyPiece
        {
            get { return _pieceType == PieceType.Empty; }
        }

        public bool IsCirclePiece
        {
            get { return _pieceType == PieceType.Circle; }
        }

        public bool IsCrossmarkPiece
        {
            get { return _pieceType == PieceType.Crossmark; }
        }

        public QuixoPiece(PieceType type)
        {
            _pieceType = type;
        }

        public void SetPieceType(PieceType pieceType)
        {
            _pieceType = pieceType;
        }
    }
}
