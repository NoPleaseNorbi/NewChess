using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace NewChess
{
    /// <summary>
    /// The derived class for the representation of the King piece
    /// </summary>
    public class King : IPiece
    {
        /// <summary>
        /// The constructor of the King class
        /// </summary>
        /// <param name="isWhite">The color of the piece</param>
        public King(bool isWhite)
        {
            this.isWhite = isWhite;
            HasMoved = false;
        }
        /// <summary>
        /// Gets the list of the valid moves for a piece
        /// </summary>
        /// <param name="currentPosition">The current position of the piece</param>
        /// <param name="board">The representation of the chessboard</param>
        /// <param name="checkingForCheck">If the function should check also for checks</param>
        /// <param name="checkingForPin">If the function should check for pinned pieces</param>
        /// <returns>The list of all the valid moves a piece has to offer</returns>
        public override List<Vector2> GetValidMoves(Vector2 currentPosition, Board board, bool checkingForCheck = true, bool checkingForPin = true)
        {
            var validMoves = new List<Vector2>();
           
            // The possible moves of the king
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
            else
            {
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
            }

            return validMoves;
        }
        /// <summary>
        /// The function for checking if the king can castle
        /// </summary>
        /// <param name="currentPosition">The current position of the king</param>
        /// <param name="board">The representation of the chessboard</param>
        /// <param name="rookCol">The column where the rook is located</param>
        /// <returns>True if the king can castle with the determined rook position</returns>
        private bool CanCastle(Vector2 currentPosition, Board board, int rookCol)
        {
            var rook = board.board[!isWhite ? 0 : 7, rookCol];
            if (rook == null || rook is not Rook || rook.isWhite != isWhite)
            {
                return false;
            }

            int startCol = (int)currentPosition.Y + (rookCol == 7 ? 1 : -1);
            int endCol = rookCol - (rookCol == 7 ? 1 : 0);
            for (int col = startCol; col != endCol; col += rookCol == 7 ? 1 : -1)
            {
                if (board.GetPiece((int)currentPosition.X, col) != null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
