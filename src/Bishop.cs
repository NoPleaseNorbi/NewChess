using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace NewChess
{
    /// <summary>
    /// The derived class for the representation of the Bishop piece
    /// </summary>
    public class Bishop : IPiece
    {
        /// <summary>
        /// The constructor of the Bishop class
        /// </summary>
        /// <param name="isWhite">The color of the piece</param>
        public Bishop(bool isWhite)
        {
            this.isWhite = isWhite;
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

            // The directions where the bishop can move in one square
            var directions = new[] {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(1, -1),
                new Vector2(1, 1)
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
