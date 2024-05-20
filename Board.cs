using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NewChess
{
    public class Board
    {

        public IPiece[,] board;
        private bool whitesTurn;
        public Board()
        {
            board = new IPiece[8, 8];
            whitesTurn = true;
            // Set up starting positions for white pieces

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

            // Set up starting positions for black pieces
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

        public bool GetTurn() {
            return whitesTurn;
        }
        public void NextTurn()
        {
            whitesTurn = !whitesTurn;
        }
        public IPiece GetPiece(int row, int col)
        {
            return board[row, col];
        }

        public bool IsWhite(int row, int col)
        {
            var piece = board[row, col];
            return piece != null && piece.isWhite;
        }

        public void RemovePiece(int row, int col)
        {
            board[row, col] = null;
        }

        public void SetPiece(int row, int col, IPiece piece)
        {
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

        public bool IsValidPosition(Vector2 position)
        {
            return position.X >= 0 && position.X < 8 && position.Y >= 0 && position.Y < 8;
        }

        public void KingMove(int row, int col) 
        {
            if (board[row, col] != null && board[row, col] is King)
            {
                board[row, col].HasMoved = true;
            }
        }
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
        public bool IsInCheck(bool isWhite)
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

        public bool MoveBlocksCheck(Vector2 startPos, Vector2 endPos)
        {
            IPiece piece = GetPiece((int)startPos.X, (int)startPos.Y);
            RemovePiece((int)startPos.X, (int)startPos.Y);

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
    }
}
