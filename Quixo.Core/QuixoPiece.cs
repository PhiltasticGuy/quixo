namespace Quixo.Core
{
    public class QuixoPiece
    {
        public PieceType PieceType { get; set; }

        public bool IsSelected { get; set; }

        public bool IsValidMove { get; set; }

        public bool IsEmptyPiece
        {
            get { return PieceType == PieceType.Empty; }
        }

        public bool IsCirclePiece
        {
            get { return PieceType == PieceType.Circle; }
        }

        public bool IsCrossmarkPiece
        {
            get { return PieceType == PieceType.Crossmark; }
        }

        public QuixoPiece(PieceType type)
        {
            PieceType = type;
        }
    }
}
