using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace NewChess
{
    internal class King : IPiece
    {
        public King(bool isWhite)
        {
            this.isWhite = isWhite;
        }
        public override List<Vector2> GetValidMoves(Vector2 currentPosition, Board board) 
        {
            var validMoves = new List<Vector2>();
            var possibleOffsets = new[]
            {
                new Vector2(1, 0), new Vector2(-1, 0),
                new Vector2(0, 1), new Vector2(0, -1),
                new Vector2(1, 1), new Vector2(1, -1),
                new Vector2(-1, 1), new Vector2(-1, -1)
            };

            foreach (var offset in possibleOffsets)
            {
                Vector2 newPosition = currentPosition + offset;
                if (board.IsValidPosition(newPosition) && (board.GetPiece((int)newPosition.X, (int)newPosition.Y) == null || board.IsWhite((int)newPosition.X, (int)newPosition.Y) == isWhite))
                {
                    validMoves.Add(newPosition);
                }
            }
            return validMoves;
        }
    }
}
