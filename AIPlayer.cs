using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NewChess
{
    public class AIPlayer
    {
        private bool _isWhite;

        public AIPlayer(bool isWhite)
        {
            _isWhite = isWhite;
        }

        public (Vector2? from, Vector2? to) GetBestMove(Board board, int depth = 3)
        {
            float alpha = float.MinValue;
            float beta = float.MaxValue;
            float bestValue = float.MinValue;
            Vector2? bestFrom = null, bestTo = null;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    IPiece piece = board.GetPiece(row, col);
                    if (piece != null && piece.isWhite == _isWhite)
                    {
                        foreach (Vector2 move in piece.GetValidMoves(new Vector2(row, col), board))
                        {
                            IPiece deletedPiece = board.GetPiece((int)move.X, (int)move.Y);
                            board.SetPiece((int)move.X, (int)move.Y, piece);
                            board.RemovePiece(row, col);

                            float value = Minimax(board, depth - 1, alpha, beta, false);
                            if (value > bestValue && board.IsValidPosition(new Vector2(row, col)))
                            {
                                bestValue = value;
                                bestFrom = new Vector2(row, col);
                                bestTo = move;
                            }
                            alpha = Math.Max(alpha, value);

                           
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

            return (bestFrom, bestTo);
        }

        private float Minimax(Board board, int depth, float alpha, float beta, bool maximizingPlayer)
        {
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

            if (maximizingPlayer)
            {
                float value = float.MinValue;
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        IPiece piece = board.GetPiece(row, col);
                        if (piece != null && piece.isWhite != _isWhite)  // Opponent's pieces
                        {
                            foreach (Vector2 move in piece.GetValidMoves(new Vector2(row, col), board))
                            {
                                IPiece deletedPiece = board.GetPiece((int)move.X, (int)move.Y);

                                board.SetPiece((int)move.X, (int)move.Y, piece);
                                board.RemovePiece(row, col);

                                value = Math.Max(value, Minimax(board, depth - 1, alpha, beta, false));
                                alpha = Math.Max(alpha, value);

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
            else
            {
                float value = float.MaxValue;
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        IPiece piece = board.GetPiece(row, col);
                        if (piece != null && piece.isWhite == _isWhite)  // AI's pieces
                        {
                            foreach (Vector2 move in piece.GetValidMoves(new Vector2(row, col), board))
                            {
                                IPiece deletedPiece = board.GetPiece((int)move.X, (int)move.Y);

                                board.SetPiece((int)move.X, (int)move.Y, piece);
                                board.RemovePiece(row, col);

                                value = Math.Min(value, Minimax(board, depth - 1, alpha, beta, true));
                                beta = Math.Min(beta, value);

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
        }

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
