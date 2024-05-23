using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NewChess
{
    /// <summary>
    /// The base abstract class for the pieces
    /// </summary>
    public abstract class IPiece
    {
        /// <summary>
        /// Boolean for the color of the piece
        /// </summary>
        public bool isWhite { get; set; }
        /// <summary>
        /// Boolean to check if the piece has moved. Neccessary for the king for example
        /// </summary>
        public bool HasMoved { get; set; }
        /// <summary>
        /// Gets the list of the valid moves for a piece
        /// </summary>
        /// <param name="currentPosition">The current position of the piece</param>
        /// <param name="board">The representation of the chessboard</param>
        /// <param name="checkingForCheck">If the function should check also for checks</param>
        /// <param name="checkingForPin">If the function should check for pinned pieces</param>
        /// <returns>The list of all the valid moves a piece has to offer</returns>
        public abstract List<Vector2> GetValidMoves(Vector2 currentPosition, Board board, bool checkingForCheck = true, bool checkingForPin = true);
    }
}
