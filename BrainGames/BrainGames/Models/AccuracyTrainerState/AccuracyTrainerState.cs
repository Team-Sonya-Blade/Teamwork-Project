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
    using Models.AccuracyTrainerState;

    public class AccuracyTrainerState : State
    {

        private ClickableBox quit;
        private ScoreBox scoreBox;
        private ShootingRange shootingRange;

        public AccuracyTrainerState(Background background, GameStateManager gsm)
            : base(background, gsm)
        {
            this.InitializeObjects();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.shootingRange.Stage == AccuracyTrainerStateConstants.TotalStages)
            {
                this.QuitGame();
            }

            if (this.quit.CheckForClick())
            {
                this.QuitGame();
            }
        }

        private void InitializeObjects()
        {
            this.InitializeQuitButton();
            this.InitializeShootingRange();
            this.InitializeScoreBox();   
        }

        private void InitializeShootingRange()
        {
            this.shootingRange = new ShootingRange(
            Textures.GetTexture("MemoryMatrixGameBoard"),
            new Rectangle(
                MemoryMatrixConstants.GameBoardStartingX,
                MemoryMatrixConstants.GameBoardStartingY,
                MemoryMatrixConstants.GameBoardStartingWidth,
                MemoryMatrixConstants.GameBoardStartingHeight),
            this.StateManager.Difficulty
                );
            this.ListOfObjects.Add(this.shootingRange);
        }

        private void InitializeScoreBox()
        {
            var texture = Textures.GetTexture("MemoryMatrixUpperMenu");
            var rectangle = new Rectangle(
                MemoryMatrixConstants.UpperMenuStartingX,
                MemoryMatrixConstants.UpperMenuStartingY,
                MemoryMatrixConstants.UpperMenuStartingWidth,
                MemoryMatrixConstants.UpperMenuStartingHeight);

            this.scoreBox = new ScoreBox(texture, rectangle, this.shootingRange, this.StateManager.Game.SpriteFont);
            this.ListOfObjects.Add(this.scoreBox);
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

        private void QuitGame()
        {

            using (StreamWriter writer = new StreamWriter(GlobalConstants.HighScorePath, true)) // new StreamWriter(path, true) == constructor for append instead of overwrite
            {
                writer.WriteLine(this.shootingRange.Score);
            }

            Thread.Sleep(MemoryMatrixConstants.IntervalBeforeQuit);
            this.StateManager.States.Pop();
        }
    }
}