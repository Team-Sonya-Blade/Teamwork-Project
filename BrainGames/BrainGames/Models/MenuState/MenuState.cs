namespace BrainGames.Models.MenuState
{
    using global::BrainGames.Models.MemoryMatrixState;
    using global::BrainGames.Utilities.Enumerations;

    using Microsoft.Xna.Framework;

    using Models.BaseModels.Boxes;

    using Utilities.Constants;
    using Utilities.Textures;

    public class MenuState : State
    {
        private RegularBox difficultyBox;
        private ClickableBox difficultyBoxEasy;
        private ClickableBox difficultyBoxNormal;
        private ClickableBox difficultyBoxHard;
        private ClickableBox highScoresBox;
        private ClickableBox exitBox;
        private RegularBox selectBox;
        private ClickableBox selectBoxAccuracyTrainer;
        private ClickableBox selectBoxMemoryMatrix;
        private ClickableBox selectBoxPinball;

        public MenuState(Background background, GameStateManager gsm)
            : base(background, gsm)
        {
            this.InitializeObjects();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.difficultyBoxEasy.CheckForClick())
            {
                this.difficultyBoxEasy.Texture = Textures.GetTexture("DifficultyEasySelected");
                this.difficultyBoxNormal.Texture = Textures.GetTexture("DifficultyNormal");
                this.difficultyBoxHard.Texture = Textures.GetTexture("DifficultyHard");
                this.StateManager.Difficulty = DifficultyType.Easy;
            }

            if (this.difficultyBoxNormal.CheckForClick())
            {
                this.difficultyBoxEasy.Texture = Textures.GetTexture("DifficultyEasy");
                this.difficultyBoxNormal.Texture = Textures.GetTexture("DifficultyNormalSelected");
                this.difficultyBoxHard.Texture = Textures.GetTexture("DifficultyHard");
                this.StateManager.Difficulty = DifficultyType.Normal;
            }

            if (this.difficultyBoxHard.CheckForClick())
            {
                this.difficultyBoxEasy.Texture = Textures.GetTexture("DifficultyEasy");
                this.difficultyBoxNormal.Texture = Textures.GetTexture("DifficultyNormal");
                this.difficultyBoxHard.Texture = Textures.GetTexture("DifficultyHardSelected");
                this.StateManager.Difficulty = DifficultyType.Hard;
            }

            if (this.selectBoxAccuracyTrainer.CheckForClick())
            {
                Background accuracyTrainerBackground = new Background(Textures.GetTexture("MemoryMatrixBackground"));
                this.StateManager.States.Push(new AccuracyTrainerState(accuracyTrainerBackground, this.StateManager));
            }

            if (this.selectBoxMemoryMatrix.CheckForClick())
            {
                Background memoryMatrixBackground = new Background(Textures.GetTexture("MemoryMatrixBackground"));
                this.StateManager.States.Push(new MemoryMatrixState(memoryMatrixBackground, this.StateManager));
            }

            if (this.selectBoxPinball.CheckForClick())
            {
                //this.StateManager.States.Push(new PinballState());
            }

            if (this.highScoresBox.CheckForClick())
            {
                Background accuracyTrainerBackground = new Background(Textures.GetTexture("MemoryMatrixBackground"));
                this.StateManager.States.Push(new HighScoreState(accuracyTrainerBackground, this.StateManager));
            }

            if (this.exitBox.CheckForClick())
            {
                this.StateManager.Game.Exit();
            }
        }

        private void InitializeObjects()
        {
            this.difficultyBox = new RegularBox(
                Textures.GetTexture("DifficultyBox"), 
                new Rectangle(
                    MenuStateConstants.DifficultyBoxStartingX, 
                    MenuStateConstants.DifficultyBoxStartingY,
                    MenuStateConstants.DifficultyBoxWidth,
                    MenuStateConstants.DifficultyBoxHeight));
            this.ListOfObjects.Add(this.difficultyBox);

            this.highScoresBox = new ClickableBox(
                Textures.GetTexture("HighScoresBox"),
                new Rectangle(
                    MenuStateConstants.HighScoreBoxStartingX,
                    MenuStateConstants.HighScoreBoxStartingY,
                    MenuStateConstants.HighScoreBoxWidth,
                    MenuStateConstants.HighScoreBoxHeight));
            this.ListOfObjects.Add(this.highScoresBox);

            this.exitBox = new ClickableBox(
                Textures.GetTexture("ExitBox"),
                new Rectangle(
                    MenuStateConstants.ExitBoxStartingX,
                    MenuStateConstants.ExitBoxStartingY,
                    MenuStateConstants.ExitBoxWidth,
                    MenuStateConstants.ExitBoxHeight));
            this.ListOfObjects.Add(this.exitBox);

            this.selectBox = new RegularBox(
                Textures.GetTexture("SelectBox"),
                new Rectangle(
                    MenuStateConstants.SelectBoxStartingX,
                    MenuStateConstants.SelectBoxStartingY,
                    MenuStateConstants.SelectBoxWidth,
                    MenuStateConstants.SelectBoxHeight));
            this.ListOfObjects.Add(this.selectBox);

            this.difficultyBoxEasy = new ClickableBox(
                Textures.GetTexture("DifficultyEasySelected"), // default difficulty will be easy
                new Rectangle(
                    MenuStateConstants.DifficultySelectBoxX,
                    MenuStateConstants.DifficultySelectBoxY1,
                    MenuStateConstants.DifficultySelectBoxWidth,
                    MenuStateConstants.DifficultySelectBoxHeight));
            this.ListOfObjects.Add(this.difficultyBoxEasy);

            this.difficultyBoxNormal = new ClickableBox(
                Textures.GetTexture("DifficultyNormal"), // default difficulty will be easy
                new Rectangle(
                    MenuStateConstants.DifficultySelectBoxX,
                    MenuStateConstants.DifficultySelectBoxY2,
                    MenuStateConstants.DifficultySelectBoxWidth,
                    MenuStateConstants.DifficultySelectBoxHeight));
            this.ListOfObjects.Add(this.difficultyBoxNormal);

            this.difficultyBoxHard = new ClickableBox(
                Textures.GetTexture("DifficultyHard"), // default difficulty will be easy
                new Rectangle(
                    MenuStateConstants.DifficultySelectBoxX,
                    MenuStateConstants.DifficultySelectBoxY3,
                    MenuStateConstants.DifficultySelectBoxWidth,
                    MenuStateConstants.DifficultySelectBoxHeight));
            this.ListOfObjects.Add(this.difficultyBoxHard);

            this.selectBoxAccuracyTrainer = new ClickableBox(
                Textures.GetTexture("SelectBoxAccuracy"),
                new Rectangle(
                    MenuStateConstants.GameSelectBoxX,
                    MenuStateConstants.GameSelectBoxY1,
                    MenuStateConstants.GameSelectBoxWidth,
                    MenuStateConstants.GameSelectBoxHeight));
            this.ListOfObjects.Add(this.selectBoxAccuracyTrainer);

            this.selectBoxMemoryMatrix = new ClickableBox(
                Textures.GetTexture("SelectBoxMemoryMatrix"),
                new Rectangle(
                    MenuStateConstants.GameSelectBoxX,
                    MenuStateConstants.GameSelectBoxY2,
                    MenuStateConstants.GameSelectBoxWidth,
                    MenuStateConstants.GameSelectBoxHeight));
            this.ListOfObjects.Add(this.selectBoxMemoryMatrix);

            this.selectBoxPinball = new ClickableBox(
                Textures.GetTexture("SelectBoxPinball"),
                new Rectangle(
                    MenuStateConstants.GameSelectBoxX,
                    MenuStateConstants.GameSelectBoxY3,
                    MenuStateConstants.GameSelectBoxWidth,
                    MenuStateConstants.GameSelectBoxHeight));
            this.ListOfObjects.Add(this.selectBoxPinball);
        }
    }
}
