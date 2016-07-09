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

    public class HighScoreState : State
    {

        private ClickableBox quit;
        private ScoreList scoreList;

        public HighScoreState(Background background, GameStateManager gsm)
            : base(background, gsm)
        {
            this.InitializeObjects();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.quit.CheckForClick())
            {
                this.QuitGame();
            }
        }

        private void InitializeObjects()
        {
            this.InitializeQuitButton();
            this.InitializeScoreList();
        }

        private void InitializeScoreList()
        {
            this.scoreList = new ScoreList(
            Textures.GetTexture("HighScoreBackground"),
            new Rectangle(
                MemoryMatrixConstants.GameBoardStartingX,
                MemoryMatrixConstants.GameBoardStartingY,
                MemoryMatrixConstants.GameBoardStartingWidth,
                MemoryMatrixConstants.GameBoardStartingHeight),
            this.StateManager.Game.SpriteFont
           );
            this.ListOfObjects.Add(this.scoreList);
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
            Thread.Sleep(MemoryMatrixConstants.IntervalBeforeQuit);
            this.StateManager.States.Pop();
        }
    }
}
