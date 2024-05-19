using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NewChess
{
    public abstract class IPiece
    {
        public bool isWhite { get; set; }
        public bool HasMoved { get; set; }
        public abstract List<Vector2> GetValidMoves(Vector2 currentPosition, Board board);
    }
}
