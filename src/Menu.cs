using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewChess
{
    /// <summary>
    /// The Menu class for the application
    /// </summary>
    internal class Menu
    {
        /// <summary>
        /// Enumeration states for the menu which determine which window the user sees
        /// </summary>
        public enum MenuState
        {
            Main,
            OneOnOne,
            VsAI,
            Exit
        }

        /// <summary>
        /// The state of the window the user sees
        /// </summary>
        private MenuState currentMenuState;

        /// <summary>
        /// The font inside the menu
        /// </summary>
        private SpriteFont _font;

        /// <summary>
        /// The graphics package for handling calculations
        /// </summary>
        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// The buttons of the main menu
        /// </summary>
        private List<Button> mainMenuButtons = new List<Button>();

        Button oneOnOneButton;
        Button versusAIButton;
        Button exitButton;

        /// <summary>
        /// The constructor of the menu
        /// </summary>
        /// <param name="font">The font of the menu</param>
        /// <param name="graphics">The graphics of the menu</param>
        public Menu(SpriteFont font, GraphicsDeviceManager graphics)
        {
            currentMenuState = MenuState.Main;
            _font = font;
            _graphics = graphics;
            oneOnOneButton = new Button(font, "Player vs Player", new Vector2(CalculateMiddleOfWindowHorizontally("Player vs Player"), 400), new Color(166, 123, 91), new Color(254, 216, 177));
            versusAIButton = new Button(font, "Player vs AI", new Vector2(CalculateMiddleOfWindowHorizontally("Player vs AI"), 500), new Color(166, 123, 91), new Color(254, 216, 177));
            exitButton = new Button(font, "Exit", new Vector2(CalculateMiddleOfWindowHorizontally("Exit"), 600), new Color(166, 123, 91), new Color(254, 216, 177));
            mainMenuButtons.Add(oneOnOneButton);
            mainMenuButtons.Add(versusAIButton);
            mainMenuButtons.Add(exitButton);
        }

        /// <summary>
        /// Calculates the middle of the window position
        /// </summary>
        /// <param name="text">The text we want to calculate the middle for</param>
        /// <returns>The integer value of the position of the middle of the position for the text to appear in the middle of the object</returns>
        public int CalculateMiddleOfWindowHorizontally(string text)
        {
            int windowWidth = _graphics.PreferredBackBufferWidth;
            int buttonWidth = (int)_font.MeasureString(text).X + 10;

            return (windowWidth - buttonWidth) / 2;
        }

        /// <summary>
        /// The update function for the menu class
        /// </summary>
        /// <param name="gameTime">The time of the application</param>
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

        /// <summary>
        /// The draw method for the menu
        /// </summary>
        /// <param name="spriteBatch">The spritebatch for where to draw</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "Welcome to NewChess!", new Vector2(CalculateMiddleOfWindowHorizontally("Welcome to NewChess!")), Color.Black);
            foreach (Button button in mainMenuButtons)
            {
                button.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Gets the current state of the menu
        /// </summary>
        /// <returns>The menustate</returns>
        public MenuState GetMenuState()
        {
            return currentMenuState;
        }

    }

}
