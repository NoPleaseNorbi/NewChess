using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NewChess
{
    public class Button
    {
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private string _message;
        private Vector2 _position;
        private SpriteFont _font;
        private Rectangle _buttonRect;
        private Color _backgroundShade;
        private Color _shade;
        private Color _color;
        private bool _pressed;

        public Button(SpriteFont font, string message, Vector2 position, Color color, Color backgroundShade)
        {
            _backgroundShade = backgroundShade;
            _pressed = false;
            _color = color;
            _message = message;
            _position = position;
            _font = font;
            _buttonRect = new Rectangle((int)_position.X, (int)_position.Y, (int)_font.MeasureString(_message).X + 10, (int)_font.MeasureString(_message).Y + 10);
        }

        public Button(SpriteFont font, string message, Vector2 position)
        {
            _color = Color.White;
            _message = message;
            _pressed = false;
            _position = position;
            _font = font;
            _buttonRect = new Rectangle((int)_position.X, (int)_position.Y, (int)_font.MeasureString(_message).X + 10, (int)_font.MeasureString(_message).Y + 10);
            _backgroundShade = Color.White;
        }

        public void Update(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            Rectangle cursor = new(_currentMouseState.Position.X, _currentMouseState.Position.Y, 1, 1);

            if (cursor.Intersects(_buttonRect))
            {
                _shade = _backgroundShade;

                if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    _pressed = true;
                }
                else 
                {
                    _pressed = false;
                }
            }
            else 
            {
                _shade = _color;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D rectangleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rectangleTexture.SetData(new[] { Color.White });
            spriteBatch.Draw(rectangleTexture, _buttonRect, _shade);

            Vector2 textPosition = _position + new Vector2((_buttonRect.Width - _font.MeasureString(_message).X) / 2, (_buttonRect.Height - _font.MeasureString(_message).Y) / 2);
            spriteBatch.DrawString(_font, _message, textPosition, Color.Black);
        }

        public bool ButtonPressed() 
        {
            return _pressed;
        }
    }
}
