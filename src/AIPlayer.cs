using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NewChess
{
    /// <summary>
    /// The main AI class for the minimax algorithm with alfa beta pruning
    /// </summary>
    public class AIPlayer
    {
        /// <summary>
        /// Boolean for the piece if is white
        /// </summary>
        private bool _isWhite;

        /// <summary>
        /// The constructor of the class
        /// </summary>
        /// <param name="isWhite">The boolean for the piece if it is white</param>
        public AIPlayer(bool isWhite)
        {
            _isWhite = isWhite;
        }

        /// <summary>
        /// Gets the best move using the minimax algorithm
        /// </summary>
        /// <param name="board">The representation of the chessboard</param>
        /// <param name="depth">The depth into which the minimax will search the best move</param>
        /// <returns>The position from and to where the best move was found</returns>
        public (Vector2? from, Vector2? to) GetBestMove(Board board, int depth = 3)
        {
            // The alpha and beta floats for the pruning
            float alpha = float.MinValue;
            float beta = float.MaxValue;

            // The best value for the deeper searches
            float bestValue = float.MinValue;
            Vector2? bestFrom = null, bestTo = null;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    // Get the piece in that position
                    IPiece piece = board.GetPiece(row, col);
                    if (piece != null && piece.isWhite == _isWhite)
                    {
                        foreach (Vector2 move in piece.GetValidMoves(new Vector2(row, col), board))
                        {
                            // If the move would delete a piece, save it
                            IPiece deletedPiece = board.GetPiece((int)move.X, (int)move.Y);

                            // Simulate the move
                            board.SetPiece((int)move.X, (int)move.Y, piece);
                            board.RemovePiece(row, col);

                            // After the move, call the minimax to go one depth deeper
                            float value = Minimax(board, depth - 1, alpha, beta, false);
                            if (value > bestValue && board.IsValidPosition(new Vector2(row, col)))
                            {
                                bestValue = value;
                                bestFrom = new Vector2(row, col);
                                bestTo = move;
                            }
                            alpha = Math.Max(alpha, value);

                            // Reset the position after the move simulation
                            board.SetPiece(row, col, piece);
                            board.RemovePiece((int)move.X, (int)move.Y);

                            if (deletedPiece != null)
                            {
                                // Restore the piece that was deleted
                                board.SetPiece((int)move.X, (int)move.Y, deletedPiece);
                            }

                            if (beta <= alpha)
                                break;
                        }
                    }
                }
            }

            return (bestFrom, bestTo);
        }

        /// <summary>
        /// The main minimax algorithm function
        /// </summary>
        /// <param name="board">The representation of the chess board</param>
        /// <param name="depth">The depth into what we are searching in the minimax algorithm</param>
        /// <param name="alpha">The alpha variable of the alfa beta pruning</param>
        /// <param name="beta">The beta variable of the alfa beta pruning</param>
        /// <param name="maximizingPlayer">Boolean for determining currently which player is searched trough</param>
        /// <returns></returns>
        private float Minimax(Board board, int depth, float alpha, float beta, bool maximizingPlayer)
        {

            // Check for checmkate and stalemate
            if (board.IsCheckmate(!_isWhite))
            {
                return maximizingPlayer ? float.MinValue : float.MaxValue;
            }
            if (board.IsCheckmate(_isWhite))
            {
                return maximizingPlayer ? float.MaxValue : float.MinValue;
            }
            if (board.IsStalemate(!_isWhite))
            {
                return 0;
            }
            if (depth == 0)
            {
                return EvaluateBoard(board, maximizingPlayer);
            }

            float value = maximizingPlayer ? float.MinValue : float.MaxValue;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    IPiece piece = board.GetPiece(row, col);
                    if (piece != null && piece.isWhite == maximizingPlayer)
                    {
                        foreach (Vector2 move in piece.GetValidMoves(new Vector2(row, col), board))
                        {
                            IPiece deletedPiece = board.GetPiece((int)move.X, (int)move.Y);

                            // Simulate the move
                            board.SetPiece((int)move.X, (int)move.Y, piece);
                            board.RemovePiece(row, col);

                            value = maximizingPlayer ? Math.Max(value, Minimax(board, depth - 1, alpha, beta, false)) : Math.Min(value, Minimax(board, depth - 1, alpha, beta, true));

                            // Update alpha/beta based on maximizingPlayer
                            if (maximizingPlayer)
                                alpha = Math.Max(alpha, value);
                            else
                                beta = Math.Min(beta, value);

                            // Reset the position 
                            board.SetPiece(row, col, piece);
                            board.RemovePiece((int)move.X, (int)move.Y);
                            if (deletedPiece != null) 
                            {
                                board.SetPiece((int)move.X, (int)move.Y, deletedPiece);
                            }

                            if (beta <= alpha)
                                break;
                        }
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// Evaluates the values on the board
        /// </summary>
        /// <param name="board">The representation of the board</param>
        /// <param name="maximizingPlayer">Boolean for determining currently which player is searched trough</param>
        /// <returns>The value of the board</returns>
        private float EvaluateBoard(Board board, bool maximizingPlayer)
        {
            float score = 0;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    IPiece piece = board.GetPiece(row, col);
                    if (piece != null)
                    {
                        score += (piece.isWhite == maximizingPlayer ? 1 : -1) * GetPieceValue(piece);
                    }
                }
            }
            return score;
        }

        /// <summary>
        /// Gets the value of the piece
        /// </summary>
        /// <param name="piece">The piece we want to evaluate</param>
        /// <returns>The value of the piece</returns>
        private float GetPieceValue(IPiece piece)
        {
            if (piece is King)
            {
                return 100000;
            }
            else if (piece is Queen)
            {
                return 9;
            }
            else if (piece is Rook)
            {
                return 5;
            }
            else if (piece is Knight || piece is Bishop)
            {
                return 3;
            }
            return 1;
        }
    }
}
