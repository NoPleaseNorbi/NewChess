using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static System.Formats.Asn1.AsnWriter;

namespace NewChess
{

    public enum Pieces { 
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public class Game1 : Game
    {
        private Vector2? selectedPiecePosition;
        private Vector2? initialMousePosition;
        private Vector2? previousMousePosition;

        Texture2D whitePawnTexture;
        Texture2D whiteBishopTexture;
        Texture2D whiteKnightTexture;
        Texture2D whiteRookTexture;
        Texture2D whiteQueenTexture;
        Texture2D whiteKingTexture;

        Texture2D blackPawnTexture;
        Texture2D blackBishopTexture;
        Texture2D blackKnightTexture;
        Texture2D blackRookTexture;
        Texture2D blackQueenTexture;
        Texture2D blackKingTexture;

        Texture2D cellTexture;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Board _board;

        private Tuple<Pieces?, bool> _draggedPiece;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;       
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();
            _draggedPiece = null;
            _board = new Board();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            cellTexture = new Texture2D(GraphicsDevice, 1, 1);
            cellTexture.SetData(new Color[] { Color.White });

            whitePawnTexture = Content.Load<Texture2D>("white-pawn");
            whiteBishopTexture = Content.Load<Texture2D>("white-bishop");
            whiteKnightTexture = Content.Load<Texture2D>("white-knight");
            whiteRookTexture = Content.Load<Texture2D>("white-rook");
            whiteQueenTexture = Content.Load<Texture2D>("white-queen");
            whiteKingTexture = Content.Load<Texture2D>("white-king");

            blackPawnTexture = Content.Load<Texture2D>("black-pawn");
            blackBishopTexture = Content.Load<Texture2D>("black-bishop");
            blackKnightTexture = Content.Load<Texture2D>("black-knight");
            blackRookTexture = Content.Load<Texture2D>("black-rook");
            blackQueenTexture = Content.Load<Texture2D>("black-queen");
            blackKingTexture = Content.Load<Texture2D>("black-king");
        }

        private Vector2? GetBoardPosition(Point mousePosition)
        {
            const int boardOffSetX = 80;
            const int boardOffSetY = 50;
            const int squareSize = 80;

            int col = (mousePosition.X - boardOffSetX) / squareSize;
            int row = (mousePosition.Y - boardOffSetY) / squareSize;

            if (row >= 0 && row < 8 && col >= 0 && col < 8)
                return new Vector2(row, col);
            else
                return null;
        }
        protected override void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (selectedPiecePosition == null)
                {
                    // Try to select a piece
                    selectedPiecePosition = GetBoardPosition(mouse.Position);
                    initialMousePosition = new Vector2(mouse.Position.X, mouse.Position.Y);
                    if (selectedPiecePosition != null) { 
                        if (_board.GetPiece((int)selectedPiecePosition.Value.X, (int)selectedPiecePosition.Value.Y) != null) {
                            if (_board.GetTurn() == _board.IsWhite((int)selectedPiecePosition.Value.X, (int)selectedPiecePosition.Value.Y)) { 
                                _draggedPiece = Tuple.Create(_board.GetPiece((int)selectedPiecePosition.Value.X, (int)selectedPiecePosition.Value.Y), _board.IsWhite((int)selectedPiecePosition.Value.X, (int)selectedPiecePosition.Value.Y));
                                _board.RemovePiece((int)selectedPiecePosition.Value.X, (int)selectedPiecePosition.Value.Y);
                                _board.NextTurn();
                            }
                        }
                    }
                }
                else
                {
                    previousMousePosition = new Vector2(mouse.Position.X, mouse.Position.Y);
                }

            }
            else if (mouse.LeftButton == ButtonState.Released && selectedPiecePosition.HasValue)
            {
                // Drop the piece
                var dropPosition = GetBoardPosition(mouse.Position);
                if (dropPosition.HasValue)
                {
                    // Update the board
                    if (_draggedPiece != null) { 
                        _board.SetPiece((int)dropPosition.Value.X, (int)dropPosition.Value.Y, _draggedPiece.Item1.Value, _draggedPiece.Item2);
                    }
                }

                initialMousePosition = null;
                previousMousePosition = null;
                selectedPiecePosition = null;
                _draggedPiece = null;
            }
            base.Update(gameTime);
        }

        private Texture2D GetPieceTexture(Pieces? piece, bool isWhite)
        {
            if (!piece.HasValue) return null; // If there's no piece, return null

            if (isWhite)
            {
                switch (piece.Value)
                {
                    case Pieces.Pawn: return blackPawnTexture;
                    case Pieces.Rook: return blackRookTexture;
                    case Pieces.Knight: return blackKnightTexture;
                    case Pieces.Bishop: return blackBishopTexture;
                    case Pieces.Queen: return blackQueenTexture;
                    case Pieces.King: return blackKingTexture;
                }
            }
            else
            {
                switch (piece.Value)
                {
                    case Pieces.Pawn: return whitePawnTexture;
                    case Pieces.Rook: return whiteRookTexture;
                    case Pieces.Knight: return whiteKnightTexture;
                    case Pieces.Bishop: return whiteBishopTexture;
                    case Pieces.Queen: return whiteQueenTexture;
                    case Pieces.King: return whiteKingTexture;              
                }
            }

            return null;
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            const int boardOffSetX = 80;
            const int boardOffSetY = 50;
            const int squareSize = 80;
            Color lightSquareColor = Color.LightYellow;
            Color darkSquareColor = Color.SaddleBrown;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Color squareColor = (row + col) % 2 == 0 ? lightSquareColor : darkSquareColor;
                    Rectangle squareRect = new Rectangle(boardOffSetX + col * squareSize, boardOffSetY + row * squareSize, squareSize, squareSize);
                    _spriteBatch.Draw(cellTexture, squareRect, squareColor);

                    Texture2D pieceTexture = null;
                    if (_board.board[row, col] != null) 
                    {
                        pieceTexture = GetPieceTexture(_board.board[row, col].Item1, (_board.board[row, col].Item2));
                        if (pieceTexture != null)
                        {
                            float pieceX = squareRect.X + (squareSize - pieceTexture.Width) / 2;
                            float pieceY = squareRect.Y + (squareSize - pieceTexture.Height) / 2;
                            float scale = 0.7f;

                            pieceX += ((pieceTexture.Width * (1 - scale)) / 2);
                            pieceY += ((pieceTexture.Height * (1 - scale)) / 2);

                            _spriteBatch.Draw(pieceTexture, new Vector2(pieceX, pieceY), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                        }
                    }          

                }
            }

            if (selectedPiecePosition.HasValue && initialMousePosition.HasValue && previousMousePosition.HasValue)
            {
                Texture2D pieceTexture = null;
                if (_draggedPiece != null) { 
                    pieceTexture = GetPieceTexture(_draggedPiece.Item1.Value, _draggedPiece.Item2);
                    if (pieceTexture != null)
                    {
                        var mousePosition = Mouse.GetState().Position;
                        float scale = 0.7f;

                        // Center the piece under the mouse cursor
                        Vector2 piecePosition = new Vector2(
                            mousePosition.X - (pieceTexture.Width * scale) / 2,
                            mousePosition.Y - (pieceTexture.Height * scale) / 2
                        );

                        _spriteBatch.Draw(pieceTexture, piecePosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    }
                }
                
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}