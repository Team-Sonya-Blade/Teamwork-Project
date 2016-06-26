namespace BrainGames.Models.MemoryMatrixState
{
    using System.IO;
    using System.Linq;
    using System.Threading;

    using global::BrainGames.Models.BaseModels.Boxes;
    using global::BrainGames.Models.MemoryMatrixState.LevelMaps;
    using global::BrainGames.Utilities.Enumerations;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Utilities.Constants;
    using Utilities.Textures;

    public class MemoryMatrixState : State
    {
        private Level level;
        private float hideBoardTimer; // based on difficulty
        private float elapsedTime;
        private int currentScore = MemoryMatrixConstants.StartingScore;
        private int currentLevel = MemoryMatrixConstants.StartingLevel;
        private GameBoard gameBoard;
        private UpperMenu upperMenu;
        private ClickableBox quit;

        public MemoryMatrixState(Background background, GameStateManager gsm)
            : base(background, gsm)
        {
            this.SetDifficultyTimer();
            this.InitializeObjects();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            this.HideBoard(gameTime);

            if (this.gameBoard.BlocksLeft == 0)
            {
                if (this.gameBoard.LevelNumber != MemoryMatrixConstants.FinalLevel)
                {
                    this.ChangeLevel();
                }
                else
                {
                    this.QuitGame();
                }
            }

            if (this.quit.CheckForClick())
            {
                this.QuitGame();
            }
        }

        private void InitializeObjects()
        {
            this.InitializeLevel();
            this.InitializeBoard();
            this.InitializeUpperMenu();
            this.InitializeQuitButton();
        }

        // Initialize the game board
        private void InitializeBoard()
        {
            this.gameBoard = new GameBoard(
                Textures.GetTexture("MemoryMatrixGameBoard"),
                new Rectangle(
                    MemoryMatrixConstants.GameBoardStartingX,
                    MemoryMatrixConstants.GameBoardStartingY,
                    MemoryMatrixConstants.GameBoardStartingWidth,
                    MemoryMatrixConstants.GameBoardStartingHeight),
                this.currentScore,
                this.level.LevelNumber,
                this.level.NumberOfBlocks);
            this.ListOfObjects.Add(this.gameBoard);
            this.gameBoard.Board = new Block[MemoryMatrixConstants.Size, MemoryMatrixConstants.Size];
            for (int row = 0; row < MemoryMatrixConstants.Size; row++)
            {
                for (int col = 0; col < MemoryMatrixConstants.Size; col++)
                {
                    if (this.level.Map[row, col] == MemoryMatrixConstants.DefaultBlockCode)
                    {
                        Texture2D texture = Textures.GetTexture("DeffaultBlock");
                        Rectangle rectangle = this.GenerateRectangle(row, col);
                        int code = MemoryMatrixConstants.DefaultBlockCode;

                        this.gameBoard.Board[row, col] = new Block(texture, rectangle, code);
                    }

                    if (this.level.Map[row, col] == MemoryMatrixConstants.CorrectBlockCode)
                    {
                        Texture2D texture = Textures.GetTexture("CorrectBlock");
                        Rectangle rectangle = this.GenerateRectangle(row, col);
                        int code = MemoryMatrixConstants.CorrectBlockCode;

                        this.gameBoard.Board[row, col] = new Block(texture, rectangle, code);
                    }
                }
            }
        }

        private void InitializeUpperMenu()
        {
            var texture = Textures.GetTexture("MemoryMatrixUpperMenu");
            var rectangle = new Rectangle(
                MemoryMatrixConstants.UpperMenuStartingX, 
                MemoryMatrixConstants.UpperMenuStartingY,
                MemoryMatrixConstants.UpperMenuStartingWidth,
                MemoryMatrixConstants.UpperMenuStartingHeight);

            this.upperMenu = new UpperMenu(texture, rectangle, this.gameBoard, this.StateManager.Game.SpriteFont);
            this.ListOfObjects.Add(this.upperMenu);
        }

        private void InitializeLevel()
        {
            int levelNumber;
            int numberOfBlocks;
            int[,] map = new int[MemoryMatrixConstants.Size, MemoryMatrixConstants.Size];

            string roadToMap = string.Format("Content/LevelMaps/MemoryMatrix/Level{0}.txt", this.currentLevel);
            StreamReader reader = new StreamReader(roadToMap);
            using (reader)
            {
                levelNumber = int.Parse(reader.ReadLine());
                numberOfBlocks = int.Parse(reader.ReadLine());
                for (int i = 0; i < MemoryMatrixConstants.Size; i++)
                {
                    int[] arr = reader.ReadLine().Split().Select(x => int.Parse(x)).ToArray();
                    for (int j = 0; j < MemoryMatrixConstants.Size; j++)
                    {
                        map[i, j] = arr[j];
                    }
                }
            }

            this.level = new Level(levelNumber, numberOfBlocks, map);
        }

        private void InitializeQuitButton()
        {
            this.quit = new ClickableBox(
                Textures.GetTexture("MemoryMatrixQuit"),
                new Rectangle(
                    MemoryMatrixConstants.QuitStartingX,
                    MemoryMatrixConstants.QuitStartingY,
                    MemoryMatrixConstants.QuitStartingWidth,
                    MemoryMatrixConstants.QuitStartingHeight));
            this.ListOfObjects.Add(this.quit);
        }

        // Hides the content of the board after X seconds(according to difficulty). Executed only once at the beggining of the level
        private void HideBoard(GameTime gameTime)
        {
            if (!this.gameBoard.IsBoardHidden)
            {
                this.elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.hideBoardTimer -= this.elapsedTime;
                if (this.hideBoardTimer < 0)
                {
                    for (int row = 0; row < MemoryMatrixConstants.Size; row++)
                    {
                        for (int col = 0; col < MemoryMatrixConstants.Size; col++)
                        {
                            this.gameBoard.Board[row, col].Texture = Textures.GetTexture("DeffaultBlock");
                            this.gameBoard.Board[row, col].IsTurned = false;
                        }
                    }

                    this.gameBoard.IsBoardHidden = true;
                }
            }
        }

        // Generates positions for the blocks on the screen
        private Rectangle GenerateRectangle(int row, int col)
        {
            int startingX = (MemoryMatrixConstants.BlockStartingX + MemoryMatrixConstants.BlockPadding)
                            + (col * (MemoryMatrixConstants.BlockWidth + MemoryMatrixConstants.BlockPadding));
            int startingY = (MemoryMatrixConstants.BlockStartingY + MemoryMatrixConstants.BlockPadding)
                            + (row * (MemoryMatrixConstants.BlockHeight + MemoryMatrixConstants.BlockPadding));
            int width = MemoryMatrixConstants.BlockWidth;
            int height = MemoryMatrixConstants.BlockHeight;

            var rectangle = new Rectangle(startingX, startingY, width, height);

            return rectangle;
        }

        // Define the visible time in seconds of the blocks at the beggining of the level based on game difficulty!
        private void SetDifficultyTimer()
        {
            if (this.StateManager.Difficulty == DifficultyType.Easy)
            {
                this.hideBoardTimer = MemoryMatrixConstants.TimeForEasyDifficulty;
            }
            else if (this.StateManager.Difficulty == DifficultyType.Normal)
            {
                this.hideBoardTimer = MemoryMatrixConstants.TimeForNormalDifficulty;
            }
            else if (this.StateManager.Difficulty == DifficultyType.Hard)
            {
                this.hideBoardTimer = MemoryMatrixConstants.TimeForHardDifficulty;
            }
        }

        // Changes level after no blocks are left
        private void ChangeLevel()
        {
            this.currentScore += this.gameBoard.BoardScore;
            this.currentLevel++;
            // TO DO: check if all levels are passed
            this.InitializeLevel();
            this.InitializeBoard();
            this.InitializeUpperMenu();
            this.SetDifficultyTimer();
        }

        private void QuitGame()
        {
            Thread.Sleep(MemoryMatrixConstants.IntervalBeforeQuit);
            this.StateManager.States.Pop();
        }
    }
}
