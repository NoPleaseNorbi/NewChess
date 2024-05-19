using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace NewChess
{
    public class King : IPiece
    {
        public King(bool isWhite)
        {
            this.isWhite = isWhite;
            this.HasMoved = false;
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
                if (board.IsValidPosition(newPosition) && (board.GetPiece((int)newPosition.X, (int)newPosition.Y) == null || board.IsWhite((int)newPosition.X, (int)newPosition.Y) != isWhite))
                {
                    validMoves.Add(newPosition);
                }
            }
            if (!HasMoved)
            {
                // Kingside castling
                if (CanCastle(currentPosition, board, 7)) // Check kingside rook
                {
                    validMoves.Add(currentPosition + new Vector2(0, 2));
                }

                // Queenside castling
                if (CanCastle(currentPosition, board, 0)) // Check queenside rook
                {
                    validMoves.Add(currentPosition + new Vector2(0, -2));
                }
            }
            return validMoves;
        }
        private bool CanCastle(Vector2 currentPosition, Board board, int rookCol)
        {
            // Check if the rook has moved
            var rook = board.board[(!isWhite ? 0 : 7), rookCol];
            if (rook == null || rook is not Rook || rook.isWhite != isWhite)
            {
                return false;
            }

            // Check if there are pieces between king and rook
            int startCol = (int)currentPosition.Y + (rookCol == 7 ? 1 : -1); // Kingside or queenside?
            int endCol = rookCol - (rookCol == 7 ? 1 : 0); // Rook's final position
            for (int col = startCol; col != endCol; col += (rookCol == 7 ? 1 : -1)) // Iterate towards the rook
            {
                if (board.GetPiece((int)currentPosition.X, col) != null)
                {
                    return false; // Piece in the way
                }
            }

            // Check if squares are under attack (you'll need to implement this based on your game rules)
            for (int col = (int)currentPosition.Y; col != endCol + (rookCol == 7 ? 1 : -1); col += (rookCol == 7 ? 1 : -1))
            {
                //
            }
            return true;
        }
    }
}
