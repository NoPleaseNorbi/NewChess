using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
    public class Board {
        public Tuple<Pieces, bool>[,] board;
        public Board() {
            board = new Tuple<Pieces, bool>[8, 8];

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
    }

    public class Game1 : Game
    {
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

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            const int boardOffSetX = 200;
            const int boardOffSetY = 50;
            const int squareSize = 50;
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
                        if (_board.board[row, col].Item1 == Pieces.Pawn) 
                        { 
                            if (_board.board[row, col].Item2 == true)
                            {
                                pieceTexture = whitePawnTexture;
                            }
                            else 
                            {
                                pieceTexture = blackPawnTexture;
                            }
                        }
                    }
                    if (pieceTexture != null) { 
                        int pieceX = squareRect.X + (squareSize - pieceTexture.Width) / 2;
                        int pieceY = squareRect.Y + (squareSize - pieceTexture.Height) / 2;
                        _spriteBatch.Draw(pieceTexture, new Vector2(pieceX, pieceY), Color.White);
                    }

                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}