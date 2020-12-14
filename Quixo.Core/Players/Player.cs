using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.Players
{
    public abstract class Player
    {
        protected int _id;
        protected string _name;
        protected PieceType _pieceType;
        private bool _isInputRequired;

        public int Id => _id;

        public string Name => _name;

        public PieceType PieceType => _pieceType;
        
        public abstract bool IsHuman { get; }

        public bool IsInputRequired { get => _isInputRequired; set => _isInputRequired = value; }

        public abstract bool PlayTurn(QuixoBoard board);

        public Player(int id, string name, PieceType pieceType)
        {
            _id = id;
            _name = name;
            _pieceType = pieceType;
        }
    }
}
