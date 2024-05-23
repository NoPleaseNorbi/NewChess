using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NewChess
{
    /// <summary>
    /// The representation of a chess board
    /// </summary>
    public class Board
    {
        /// <summary>
        /// The 2D array for storing the pieces positions
        /// </summary>
        public IPiece[,] board;
        /// <summary>
        /// Boolean for who's turn it is
        /// </summary>
        private bool whitesTurn;
        /// <summary>
        /// The The constructor of the Board class
        /// </summary>
        public Board()
        {
            board = new IPiece[8, 8];
            whitesTurn = true;

            // Set up starting positions for black pieces

            board[0, 0] = new Rook(false);
            board[0, 1] = new Knight(false);
            board[0, 2] = new Bishop(false);
            board[0, 3] = new Queen(false);
            board[0, 4] = new King(false);
            board[0, 5] = new Bishop(false);
            board[0, 6] = new Knight(false);
            board[0, 7] = new Rook(false);
            for (int col = 0; col < 8; col++)
            {
                board[1, col] = new Pawn(false);
            }

            // Set up starting positions for white pieces
            board[7, 0] = new Rook(true);
            board[7, 1] = new Knight(true);
            board[7, 2] = new Bishop(true);
            board[7, 3] = new Queen(true);
            board[7, 4] = new King(true);
            board[7, 5] = new Bishop(true);
            board[7, 6] = new Knight(true);
            board[7, 7] = new Rook(true);
            for (int col = 0; col < 8; col++)
            {
                board[6, col] = new Pawn(true);
            }
        }

        /// <summary>
        /// Gets the turn of the game
        /// </summary>
        /// <returns>True if white's turn</returns>
        public bool GetTurn()
        {
            return whitesTurn;
        }

        /// <summary>
        /// Changes the boolean to mitigate who's turn it is
        /// </summary>
        public void NextTurn()
        {
            whitesTurn = !whitesTurn;
        }

        /// <summary>
        /// Gets the piece from the grid
        /// </summary>
        /// <param name="row">The row of the desired piece</param>
        /// <param name="col">The column of the desired piece</param>
        /// <returns>The object holding the piece stored in the table at the provided indexes</returns>
        public IPiece GetPiece(int row, int col)
        {
            return board[row, col];
        }

        /// <summary>
        /// Checks if a piece at an index is white
        /// </summary>
        /// <param name="row">The row of the desired piece</param>
        /// <param name="col">The column of the desired piece</param>
        /// <returns>True if the piece is defined and is white</returns>
        public bool IsWhite(int row, int col)
        {
            var piece = board[row, col];
            return piece != null && piece.isWhite;
        }

        /// <summary>
        /// Removes a piece at a desired index
        /// </summary>
        /// <param name="row">The row of the desired piece</param>
        /// <param name="col">The column of the desired piece</param>
        public void RemovePiece(int row, int col)
        {
            board[row, col] = null;
        }

        /// <summary>
        /// Stores a piece in the table
        /// </summary>
        /// <param name="row">The row of the desired piece</param>
        /// <param name="col">The column of the desired piece</param>
        /// <param name="piece">The piece wanted to store at the procided index</param>
        public void SetPiece(int row, int col, IPiece piece)
        {
            // If the piece is pawn and it reached the backrank, set it to a queen
            if (row == 0 && piece.isWhite && piece is Pawn)
            {
                board[row, col] = new Queen(true);
            }
            else if (row == 7 && !piece.isWhite && piece is Pawn)
            {
                board[row, col] = new Queen(false);
            }
            else
            {
                board[row, col] = piece;
            }
        }

        /// <summary>
        /// Checks if the position is valid
        /// </summary>
        /// <param name="position">Position on the chessboard</param>
        /// <returns>True if the position is valid</returns>
        public bool IsValidPosition(Vector2 position)
        {
            return position.X >= 0 && position.X < 8 && position.Y >= 0 && position.Y < 8;
        }

        /// <summary>
        /// Makes so that the king has moved
        /// </summary>
        /// <param name="row">The row of the desired piece</param>
        /// <param name="col">The column of the desired piece</param>
        public void KingMove(int row, int col)
        {
            if (board[row, col] != null && board[row, col] is King)
            {
                board[row, col].HasMoved = true;
            }
        }

        /// <summary>
        /// Finds the position of the king
        /// </summary>
        /// <param name="isWhite">The color of the piece</param>
        /// <returns>The position of the king</returns>
        /// <exception cref="InvalidOperationException">If the king is not found</exception>
        public Vector2 FindKingPosition(bool isWhite)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    IPiece piece = board[row, col];
                    if (piece is King && piece.isWhite == isWhite)
                    {
                        return new Vector2(row, col);
                    }
                }
            }
            throw new InvalidOperationException("King not found on the board.");
        }

        /// <summary>
        /// Checks if a player is in check
        /// </summary>
        /// <param name="isWhite">The color of the pieces</param>
        /// <returns>If the player is in check</returns>
        public bool IsInCheck(bool isWhite)
        {
            try
            {
                Vector2 kingPosition = FindKingPosition(isWhite);

                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        IPiece piece = board[row, col];
                        if (piece != null && piece.isWhite != isWhite)
                        {
                            List<Vector2> validMoves = piece.GetValidMoves(new Vector2(row, col), this, false, false);
                            if (validMoves.Contains(kingPosition))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks for a move if it blocks a check
        /// </summary>
        /// <param name="startPos">The starting position of the piece</param>
        /// <param name="endPos">The ending position of the piece</param>
        /// <returns>True if the move blocks a check</returns>
        public bool MoveBlocksCheck(Vector2 startPos, Vector2 endPos)
        {
            IPiece piece = GetPiece((int)startPos.X, (int)startPos.Y);
            RemovePiece((int)startPos.X, (int)startPos.Y);

            // Simulate the move
            IPiece removedPiece = GetPiece((int)endPos.X, (int)endPos.Y);
            SetPiece((int)endPos.X, (int)endPos.Y, piece);

            bool stillInCheck = IsInCheck(piece.isWhite);

            SetPiece((int)startPos.X, (int)startPos.Y, piece);

            RemovePiece((int)endPos.X, (int)endPos.Y);

            if (removedPiece != null)
            {
                SetPiece((int)endPos.X, (int)endPos.Y, removedPiece);
            }

            return !stillInCheck;
        }

        /// <summary>
        /// Checks if the piece is pinned
        /// </summary>
        /// <param name="startPos">The starting position of the piece</param>
        /// <param name="endPos">The ending position of the piece</param>
        /// <returns>True if the move doesn't cause check</returns>
        public bool MoveDoesntCauseCheck(Vector2 startPos, Vector2 endPos)
        {
            IPiece piece = GetPiece((int)startPos.X, (int)startPos.Y);
            RemovePiece((int)startPos.X, (int)startPos.Y);

            IPiece removedPiece = GetPiece((int)endPos.X, (int)endPos.Y);
            SetPiece((int)endPos.X, (int)endPos.Y, piece);

            bool kingInCheck = IsInCheck(piece.isWhite);

            SetPiece((int)startPos.X, (int)startPos.Y, piece);
            RemovePiece((int)endPos.X, (int)endPos.Y);

            if (removedPiece != null)
            {
                SetPiece((int)endPos.X, (int)endPos.Y, removedPiece);
            }

            return !kingInCheck;
        }

        /// <summary>
        /// Checks for a checkmate
        /// </summary>
        /// <param name="isWhiteTurn">Boolean for who's turn it is</param>
        /// <returns>If the desired color won</returns>
        public bool IsCheckmate(bool isWhiteTurn)
        {
            try
            {
                Vector2 kingPosition = FindKingPosition(isWhiteTurn);

                if (!IsInCheck(isWhiteTurn)) 
                {
                    return false;
                }

                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        IPiece piece = GetPiece(row, col);
                        if (piece != null && piece.isWhite == isWhiteTurn)
                        {
                            if (piece.GetValidMoves(new Vector2(row, col), this).Count > 0)
                                return false;
                        }
                    }
                }

                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks for a stalemate
        /// </summary>
        /// <param name="isWhiteTurn">Boolean for who's turn it is</param>
        /// <returns>True if it is a stalemate</returns>
        public bool IsStalemate(bool isWhiteTurn)
        {
            if (IsInCheck(isWhiteTurn)) 
            {
                return false;
            }

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    IPiece piece = GetPiece(row, col);
                    if (piece != null && piece.isWhite == isWhiteTurn)
                    {
                        if (piece.GetValidMoves(new Vector2(row, col), this).Count > 0)
                            return false;
                    }
                }
            }

            return true;
        }
    }
}
