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
    /// <summary>
    /// The main button class for the application
    /// </summary>
    public class Button
    {
        /// <summary>
        /// The current state of the mouse
        /// </summary>
        private MouseState _currentMouseState;

        /// <summary>
        /// The previous state of the mouse
        /// </summary>
        private MouseState _previousMouseState;

        /// <summary>
        /// The text we want in the button to appear
        /// </summary>
        private string _message;

        /// <summary>
        /// The position of the button
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// The font of the text in the button
        /// </summary>
        private SpriteFont _font;

        /// <summary>
        /// The rectangle representing the button
        /// </summary>
        private Rectangle _buttonRect;

        /// <summary>
        /// The color of the button if a hover appears
        /// </summary>
        private Color _backgroundShade;

        /// <summary>
        /// The shade of the button
        /// </summary>
        private Color _shade;

        /// <summary>
        /// The color of the button
        /// </summary>
        private Color _color;

        /// <summary>
        /// Boolean for checking if the button has been pressed
        /// </summary>
        private bool _pressed;

        /// <summary>
        /// The constructor of the button
        /// </summary>
        /// <param name="font">The font we want to apply to the text in the button</param>
        /// <param name="message">The text we want the button to have</param>
        /// <param name="position">The position of the button</param>
        /// <param name="color">The color of the button</param>
        /// <param name="backgroundShade">The hover color of the button</param>
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

        /// <summary>
        /// The constructor of the button
        /// </summary>
        /// <param name="font">The font we want to apply to the text in the button</param>
        /// <param name="message">The text we want the button to have</param>
        /// <param name="position">The position of the button</param>
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


        /// <summary>
        /// The update method for the button
        /// </summary>
        /// <param name="gameTime">The time elapsed</param>
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

        /// <summary>
        /// The draw method of the button
        /// </summary>
        /// <param name="spriteBatch">The spritebatch of the draw method where to draw</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D rectangleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rectangleTexture.SetData(new[] { Color.White });
            spriteBatch.Draw(rectangleTexture, _buttonRect, _shade);

            Vector2 textPosition = _position + new Vector2((_buttonRect.Width - _font.MeasureString(_message).X) / 2, (_buttonRect.Height - _font.MeasureString(_message).Y) / 2);
            spriteBatch.DrawString(_font, _message, textPosition, Color.Black);
        }

        /// <summary>
        /// Checks if the button has been pressed
        /// </summary>
        /// <returns>True if the button has been pressed</returns>
        public bool ButtonPressed()
        {
            return _pressed;
        }
    }
}
