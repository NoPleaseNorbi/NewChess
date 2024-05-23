using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NewChess
{
    /// <summary>
    /// The derived class for the representation of the Pawn piece
    /// </summary>
    public class Pawn : IPiece
    {
        /// <summary>
        /// The constructor of the Pawn class
        /// </summary>
        /// <param name="isWhite">The color of the piece</param>
        public Pawn(bool isWhite)
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
            int direction = isWhite ? -1 : 1;
            int startingRow = isWhite ? 6 : 1; // Starting row for pawns

            // One square forward
            Vector2 oneSquareForward = currentPosition + new Vector2(direction, 0);
            if (board.IsValidPosition(oneSquareForward) && board.GetPiece((int)oneSquareForward.X, (int)oneSquareForward.Y) == null)
            {
                validMoves.Add(oneSquareForward);
            }

            // Two squares forward -- only if the pawn is in  its own colors starting position
            if ((int)currentPosition.X == startingRow)
            {
                Vector2 twoSquaresForward = currentPosition + new Vector2(2 * direction, 0);
                if (board.IsValidPosition(twoSquaresForward) && board.GetPiece((int)twoSquaresForward.X, (int)twoSquaresForward.Y) == null && board.GetPiece((int)oneSquareForward.X, (int)oneSquareForward.Y) == null)
                {
                    validMoves.Add(twoSquaresForward);
                }
            }

            // Capture diagonally -- only if opponent piece is present
            for (int i = -1; i <= 1; i += 2)
            {
                Vector2 diagonal = currentPosition + new Vector2(direction, i);
                if (board.IsValidPosition(diagonal) && board.GetPiece((int)diagonal.X, (int)diagonal.Y) != null && board.IsWhite((int)diagonal.X, (int)diagonal.Y) != isWhite)
                {
                    validMoves.Add(diagonal);
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
