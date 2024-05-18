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

        public Tuple<Pieces, bool>[,] board;
        private bool blacksTurn;
        public Board()
        {
            board = new Tuple<Pieces, bool>[8, 8];
            blacksTurn = false;
            // Set up starting positions for white pieces
            board[0, 0] = Tuple.Create(Pieces.Rook, true);
            board[0, 1] = Tuple.Create(Pieces.Knight, true);
            board[0, 2] = Tuple.Create(Pieces.Bishop, true);
            board[0, 3] = Tuple.Create(Pieces.Queen, true);
            board[0, 4] = Tuple.Create(Pieces.King, true);
            board[0, 5] = Tuple.Create(Pieces.Bishop, true);
            board[0, 6] = Tuple.Create(Pieces.Knight, true);
            board[0, 7] = Tuple.Create(Pieces.Rook, true);
            for (int col = 0; col < 8; col++)
            {
                board[1, col] = Tuple.Create(Pieces.Pawn, true);
            }

            // Set up starting positions for black pieces
            board[7, 0] = Tuple.Create(Pieces.Rook, false);
            board[7, 1] = Tuple.Create(Pieces.Knight, false);
            board[7, 2] = Tuple.Create(Pieces.Bishop, false);
            board[7, 3] = Tuple.Create(Pieces.Queen, false);
            board[7, 4] = Tuple.Create(Pieces.King, false);
            board[7, 5] = Tuple.Create(Pieces.Bishop, false);
            board[7, 6] = Tuple.Create(Pieces.Knight, false);
            board[7, 7] = Tuple.Create(Pieces.Rook, false);
            for (int col = 0; col < 8; col++)
            {
                board[6, col] = Tuple.Create(Pieces.Pawn, false);
            }
        }

        public bool GetTurn() {
            return blacksTurn;
        }
        public void NextTurn()
        {
            blacksTurn = !blacksTurn;
        }
        public Pieces? GetPiece(int row, int col)
        {
            return board[row, col]?.Item1;
        }

        public bool IsWhite(int row, int col)
        {
            return board[row, col]?.Item2 ?? false;
        }

        public void RemovePiece(int row, int col)
        {
            board[row, col] = null;
        }

        public void SetPiece(int row, int col, Pieces piece, bool isWhite)
        {
            board[row, col] = Tuple.Create(piece, isWhite);
        }

        public bool IsValidPosition(Vector2 position)
        {
            return position.X >= 0 && position.X < 8 && position.Y >= 0 && position.Y < 8;
        }
    }
}
