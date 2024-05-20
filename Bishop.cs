﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace NewChess
{
    public class Bishop : IPiece
    {
        public Bishop(bool isWhite)
        {
            this.isWhite = isWhite;
        }

        public override List<Vector2> GetValidMoves(Vector2 currentPosition, Board board, bool checkingForCheck = true, bool checkingForPin = true)
        {
            var validMoves = new List<Vector2>();

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
