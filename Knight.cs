using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NewChess
{
    public class Knight : IPiece
    {
        public Knight(bool isWhite)
        {
            this.isWhite = isWhite;
        }
        public override List<Vector2> GetValidMoves(Vector2 currentPosition, Board board, bool checkingForCheck = true, bool checkingForPin = true) 
        {
            var validMoves = new List<Vector2>();
            var possibleOffsets = new[]
            {
                new Vector2(1, 2), 
                new Vector2(1, -2),
                new Vector2(-1, 2), 
                new Vector2(-1, -2),
                new Vector2(2, 1), 
                new Vector2(2, -1),
                new Vector2(-2, 1), 
                new Vector2(-2, -1)
            };

            foreach (var offset in possibleOffsets)
            {
                Vector2 newPosition = currentPosition + offset;
                if (board.IsValidPosition(newPosition) && (board.GetPiece((int)newPosition.X, (int)newPosition.Y) == null || board.IsWhite((int)newPosition.X, (int)newPosition.Y) != isWhite))
                {
                    validMoves.Add(newPosition);
                }
            }
            if (checkingForPin)
            {
                validMoves = validMoves.Where(move => board.MoveDoesntCauseCheck(currentPosition, move)).ToList();
            }
            if (!checkingForCheck)
            {
                return validMoves;
            }

            if (board.IsInCheck(isWhite))
            {
                validMoves = validMoves.Where(move => board.MoveBlocksCheck(currentPosition, move)).ToList();
            }

            return validMoves;
        }
    }
}
