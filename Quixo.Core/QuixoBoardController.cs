using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core
{
    public class QuixoBoardController
    {
        public QuixoPiece[] Pieces { get; }

        public QuixoBoardController()
        {
            Pieces = new QuixoPiece[25];
            ResetGame();
        }

        public void ResetGame()
        {
            Array.Fill(Pieces, new QuixoPiece(PieceType.Empty));
        }

        public void SetCircles()
        {
            Array.Fill(Pieces, new QuixoPiece(PieceType.Circle));
        }

        public void SetCrossmarks()
        {
            Array.Fill(Pieces, new QuixoPiece(PieceType.Crossmark));
        }
    }
}
