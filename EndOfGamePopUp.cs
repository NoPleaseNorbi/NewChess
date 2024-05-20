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
    public class EndOfGamePopUp
    {
        private string _message;
        private SpriteBatch _spriteBatch;
        private Vector2 _position;
        private SpriteFont _font;
        private bool _isVisible = false;

        public EndOfGamePopUp(SpriteBatch spriteBatch, SpriteFont font, string message, Vector2 position)
        {
            _spriteBatch = spriteBatch;
            _message = message;
            _position = position;
            _font = font;
        }

        public void Show() => _isVisible = true;

        public void Hide() => _isVisible = false;

        public void Update(GameTime gameTime)
        {
            if (_isVisible && Keyboard.GetState().IsKeyDown(Keys.Enter))
                Hide();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isVisible)
            {
                _spriteBatch.DrawString(_font, _message, _position, Color.Red);
            }
        }
    }
}
