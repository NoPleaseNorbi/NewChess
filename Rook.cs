using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace NewChess
{
    public class Rook : IPiece
    {
        public Rook(bool isWhite)
        {
            this.isWhite = isWhite;
        }

        public override List<Vector2> GetValidMoves(Vector2 currentPosition, Board board)
        {
            var validMoves = new List<Vector2>();

            var directions = new[] { 
                new Vector2(0, -1), 
                new Vector2(0, 1), 
                new Vector2(-1, 0), 
                new Vector2(1, 0) 
            };
            foreach (var direction in directions)
            {
                Vector2 newPosition = currentPosition + direction;
                while (board.IsValidPosition(newPosition))
                {
                    var piece = board.GetPiece((int)newPosition.X, (int)newPosition.Y);
                    if (piece == null)
                    {
                        validMoves.Add(newPosition);
                    }
                    else
                    {
                        if (board.IsWhite((int)newPosition.X, (int)newPosition.Y) != isWhite)
                        {
                            validMoves.Add(newPosition);
                        }
                        break;
                    }
                    newPosition += direction;
                }
            }
            return validMoves;
        }
    }
}
