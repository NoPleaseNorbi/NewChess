using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NewChess
{
    public class Pawn : IPiece
    {
        public Pawn(bool isWhite)
        {
            this.isWhite = isWhite;
        }

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

            // Two squares forward (if on starting position)
            if ((int)currentPosition.X == startingRow)
            {
                Vector2 twoSquaresForward = currentPosition + new Vector2(2 * direction, 0);
                if (board.IsValidPosition(twoSquaresForward) && board.GetPiece((int)twoSquaresForward.X, (int)twoSquaresForward.Y) == null && board.GetPiece((int)oneSquareForward.X, (int)oneSquareForward.Y) == null) validMoves.Add(twoSquaresForward);
            }

            // Capture diagonally (if opponent piece is present)
            for (int i = -1; i <= 1; i += 2)
            {
                Vector2 diagonal = currentPosition + new Vector2(direction, i);
                if (board.IsValidPosition(diagonal) && board.GetPiece((int)diagonal.X, (int)diagonal.Y) != null && board.IsWhite((int)diagonal.X, (int)diagonal.Y) != isWhite) validMoves.Add(diagonal);
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
