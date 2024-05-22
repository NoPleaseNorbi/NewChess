using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewChess
{
    internal class Menu
    {
        public enum MenuState { 
            Main, 
            OneOnOne, 
            VsAI,
            Exit
        }
        private MenuState currentMenuState = MenuState.Main;

        private SpriteFont _font;
        private GraphicsDeviceManager _graphics;
        private List<Button> mainMenuButtons = new List<Button>();
        Button oneOnOneButton;
        Button versusAIButton;
        Button exitButton;
        public Menu(SpriteFont font, GraphicsDeviceManager graphics)
        {
            _font = font;
            _graphics = graphics;
            oneOnOneButton = new Button(font, "Player vs Player", new Vector2(CalculateMiddleOfWindowHorizontally("Player vs Player"), 400), new Color(166, 123, 91), new Color(254, 216, 177));
            versusAIButton = new Button(font, "Player vs AI", new Vector2(CalculateMiddleOfWindowHorizontally("Player vs AI"), 500), new Color(166, 123, 91), new Color(254, 216, 177));
            exitButton = new Button(font, "Exit", new Vector2(CalculateMiddleOfWindowHorizontally("Exit"), 600), new Color(166, 123, 91), new Color(254, 216, 177));
            mainMenuButtons.Add(oneOnOneButton);
            mainMenuButtons.Add(versusAIButton);
            mainMenuButtons.Add(exitButton);
        }

        public int CalculateMiddleOfWindowHorizontally(string text) 
        {
            int windowWidth = _graphics.PreferredBackBufferWidth;
            int buttonWidth = (int)_font.MeasureString(text).X + 10;

            return (windowWidth - buttonWidth) / 2;
        }
        public void Update(GameTime gameTime)
        {
            foreach (Button button in mainMenuButtons)
            {
                button.Update(gameTime);
            }
            if (oneOnOneButton.ButtonPressed())
            {
                currentMenuState = MenuState.OneOnOne;
            }
            else if (versusAIButton.ButtonPressed()) 
            {
                currentMenuState = MenuState.VsAI;
            }
            else if (exitButton.ButtonPressed()) 
            {
                currentMenuState = MenuState.Exit;
            }
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.DrawString(_font, "Welcome to NewChess!", new Vector2(CalculateMiddleOfWindowHorizontally("Welcome to NewChess!")), Color.Black);
            foreach (Button button in mainMenuButtons) 
            {
                button.Draw(spriteBatch);
            }
        }

        public MenuState GetMenuState() 
        {
            return currentMenuState;
        }


    }
    
}
