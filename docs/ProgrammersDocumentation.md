# Main Classes Used in the Program

This is the programming documentation for the NewChess application written in C#. This documentation can be used alongside the Doxygen documentation, which can be generated using the instructions in the main README file.

## Location and Other Important Details

Every file of this program is located inside the src folder. The program was written in C# and uses the MonoGame library for window management and graphics rendering. This project is a chess clone, with a built-in AI, which utilizes the minimax algorithm with the extension of alpha-beta pruning. The users can choose to play against the AI, or to play against each other. The white players will always start, and the board is not changing. Currently the en-passant rule is not implemented as this chess program utilizes the 1880 pre-Milan tournament rules. 

## GameMode Enum

This enumeration defines the possible game modes

### Enum values

#### `PlayerVsPlayer`
- **Description:** Two human players compete against each other.

#### `PlayerVsAI`
- **Description:** A human player competes against the computer AI.


## NewChess Class
This is the main class of the NewChess application, deriving from the MonoGame Game class. It handles game initialization, content loading, updating, and drawing.

### Members

`selectedPiecePosition`: Stores the position of the currently selected piece.
`initialMousePosition`: Stores the initial mouse position during a drag operation.
`previousMousePosition`: Stores the previous mouse position.
`PawnTexture`, `BishopTexture`, etc.: Textures for the various chess pieces (white and black).
`cellTexture`: Texture for the chessboard cells.
`_graphics`: Graphics device manager for the game.
`_spriteBatch`: SpriteBatch for rendering sprites.
`_board`: The chessboard object.
`_menu`: The game menu.
`_restartButton`: Button to restart the game.
`_exitButton`: Button to exit the game.
`_draggedPiece`: The currently dragged piece.
`_avalaibleMoveSquares`: List of available moves for the dragged piece.
`_font`: Font for displaying text.
`textToShow`: Text to be displayed on the screen.
`_aiplayer`: The AI player object.
`_showMenu`: Flag to control menu visibility.
`_gameMode`: The current game mode.
`boardOffSetX`, `boardOffSetY`: Offsets for positioning the board on the screen.
`squareSize`: The size of each chessboard square.

### Methods

`Initialize()`: Initializes game variables.
`LoadContent()`: Loads textures and fonts.
`GetBoardPosition(Point mousePosition)`: Translates mouse coordinates to chessboard coordinates.
`Update(GameTime gameTime)`: The main update method of the game, called repeatedly during the game loop. It handles user input (mouse clicks and releases), piece selection and movement, castling, game state updates (checkmate, stalemate), AI turns (in Player vs. AI mode), and button interactions (restart, exit).
`Draw(GameTime gameTime)`: The main draw method of the game, called repeatedly during the game loop. It renders the chessboard with alternating square colors, draws the chess pieces in their current positions, highlights available moves for the selected piece, displays the dragged piece under the mouse cursor (during dragging), and renders buttons and game status text (e.g., "White wins!").
`GetPieceTexture(IPiece piece)`: This method retrieves the appropriate texture for a given chess piece (`IPiece`) based on its type (`Pawn`, `Rook`, `Knight`, etc.) and color (white or black). It uses a switch statement to determine the piece type and returns the corresponding texture. If the piece is null, it returns null.

## IPiece Abstract Class
This is the base abstract class for all chess pieces in the NewChess application. It defines common properties and methods that are inherited by specific piece implementations.

### Members
`isWhite`: A boolean property indicating whether the piece is white (true) or black (false).
`HasMoved`: A boolean property indicating whether the piece has been moved from its starting position.

### Methods
`GetValidMoves(Vector2 currentPosition, Board board, bool checkingForCheck = true, bool checkingForPin = true)`: This abstract method is responsible for calculating and returning a list of valid positions to which the piece can legally move, considering the current state of the chessboard. Each specific type of chess piece (derived from this abstract class) will implement this method differently based on its unique movement rules.

The classes derived from the `IPiece` abstract class all have the same constructor which looks like this:

`King/Queen/...(bool isWhite)`: Initializes a new instance of the specific piece class with the color of the piece as the `isWhite` boolean indicates.

I will note here, that I would rather not copy-paste the same classes 6 times (The `Pawn`, `Rook`, `Queen`, `King`, `Knight`, `Bishop`) as these classes all implement the abstract class's functions, as described previously. The only differences is the `King` class, because it implements the following function:

`CanCastle(Vector2 currentPosition, Board board, int rookCol)`: A private helper method used to determine whether castling is possible for the king given its current position and the state of the board. It checks if the relevant rook is in position and if there are no pieces obstructing the castling path.

## Menu Class
This class represents the main menu of the NewChess application, handling user interactions and transitions to different game modes.

### Members
`currentMenuState`: The current state of the menu (`Main`, `OneOnOne`, `VsAI`, `Exit`).
`_font`: The font used for text in the menu.
`_graphics`: The graphics device manager for window calculations.
`mainMenuButtons`: A list of buttons in the main menu.
`oneOnOneButton`: Button for starting a Player vs. Player game.
`versusAIButton`: Button for starting a Player vs. AI game.
`exitButton`: Button for exiting the game.

### Methods
`CalculateMiddleOfWindowHorizontally(string text)`: Calculates the horizontal position to center text in the window.
`Update(GameTime gameTime)`: Updates the menu state based on button presses.
`Draw(SpriteBatch spriteBatch)`: Draws the menu elements (text and buttons).
`GetMenuState()`: Returns the current state of the menu.

In the `Menu` class we have defined the following enumeration:

### Enum - MenuState
`Main`: The main menu screen.
`OneOnOne`: Player vs. Player mode.
`VsAI`: Player vs. AI mode.
`Exit`: Exit the game.

## AIPlayer Class
This class implements the AI player for the NewChess game using the Minimax algorithm with alpha-beta pruning.

### Members
`_isWhite`: Determines whether the AI player is playing as white (true) or black (false).

### Methods
`GetBestMove(Board board, int depth = 3)`: This method calculates the best possible move for the AI player using the Minimax algorithm with alpha-beta pruning. It takes the current Board state and a depth parameter (defaulting to 3) representing the maximum depth of the search tree. It returns a tuple `(Vector2? from, Vector2? to)`, representing the starting and ending positions of the best move found.

`Minimax(Board board, int depth, float alpha, float beta, bool maximizingPlayer)`: This is the recursive Minimax algorithm implementation. It explores possible moves up to the specified depth, evaluating board positions and maximizing (for the AI player) or minimizing (for the opponent) the score. It uses alpha-beta pruning to improve efficiency by eliminating branches that cannot lead to a better outcome. 

`EvaluateBoard(Board board, bool maximizingPlayer)`: This method evaluates the current board state from the perspective of the maximizing player. It assigns scores to pieces based on their type and position and returns the total score for the board.

`GetPieceValue(IPiece piece)`: This method returns a numerical value representing the relative importance of a chess piece (`King`, `Queen`, `Rook`, etc.) used in the evaluation of the board.

## Board Class
This class represents the chessboard and manages the positions of the chess pieces.

### Members
`board`: A 2D array that stores the chess pieces.
`whitesTurn`: A boolean indicating whether it's white's turn to move.

### Methods
`GetTurn()`: Returns a boolean indicating whether it's white's turn.
`NextTurn()`: Switches the turn to the other player.
`GetPiece(int row, int col)`: Returns the piece at the specified row and column.
`IsWhite(int row, int col)`: Checks if the piece at the specified row and column is white.
`RemovePiece(int row, int col)`: Removes the piece at the specified row and column.
`SetPiece(int row, int col, IPiece piece)`: Places the given piece at the specified row and column. If a pawn reaches the opposite side of the board, it promotes to a queen.
`IsValidPosition(Vector2 position)`: Checks if a given position is within the bounds of the chessboard.
`KingMove(int row, int col)`: Sets the HasMoved property of the king at the specified row and column to true.
`FindKingPosition(bool isWhite)`: Returns the position of the king of the specified color.
`IsInCheck(bool isWhite)`: Checks if the king of the specified color is in check.
`MoveBlocksCheck(Vector2 startPos, Vector2 endPos)`: Checks if a move from startPos to endPos would block a check for the player whose turn it is.
`MoveDoesntCauseCheck(Vector2 startPos, Vector2 endPos)`: Checks if a move from startPos to endPos would result in the player's own king being in check.
`IsCheckmate(bool isWhiteTurn)`: Checks if the player whose turn it is is in checkmate.
`IsStalemate(bool isWhiteTurn)`: Checks if the game is in a stalemate position.

## Button Class
This class represents a button in the NewChess user interface, handling mouse interactions and visual feedback.

### Members
`_currentMouseState`: Stores the current state of the mouse.
`_previousMouseState`: Stores the previous state of the mouse.
`_message`: The text displayed on the button.
`_position`: The position of the button on the screen.
`_font`: The font used for the button text.
`_buttonRect`: The rectangular area of the button.
`_backgroundShade`: The color of the button when the mouse hovers over it.
`_shade`: The current color of the button (either the base color or the hover color).
`_color`: The base color of the button.
`_pressed`: Indicates whether the button is currently being pressed.

### Constructors
`Button(SpriteFont font, string message, Vector2 position, Color color, Color backgroundShade)`: Initializes a button with the specified font, text, position, color, and hover color.
`Button(SpriteFont font, string message, Vector2 position)`: Initializes a button with the specified font, text, and position, using default colors (white).

### Methods
`Update(GameTime gameTime)`: Updates the button's state based on mouse interactions. Checks if the mouse is hovering over the button and changes its color accordingly. Also detects if the button has been pressed.
`Draw(SpriteBatch spriteBatch)`: Renders the button using the specified SpriteBatch.
`ButtonPressed()`: Returns true if the button is currently being pressed, false otherwise.