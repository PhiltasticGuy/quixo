using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.Players
{
    public class RandomAiPlayer : ComputerPlayer
    {
        public RandomAiPlayer(int id, string name, PieceType pieceType)
            : base(id, name, pieceType)
        {
        }

        public override bool PlayTurn(QuixoBoard board)
        {
            var moves = board.GetValidMoves(this.PieceType);

            var selectedMoveIndex = new Random().Next(moves.Count);
            var selectedMove = moves[selectedMoveIndex];
            board.Play(selectedMove, this.PieceType);

            Console.WriteLine($"{this.Name}: Piece #{selectedMove.Index + 1}, {selectedMove.Direction}");

            return true;
        }
    }
}
