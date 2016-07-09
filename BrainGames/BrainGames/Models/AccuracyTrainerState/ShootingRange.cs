namespace BrainGames.Models.AccuracyTrainerState
{
    using System;
    using global::BrainGames.Utilities.CustomExceptions;
    using Interfaces;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Utilities.Constants;
    using Microsoft.Xna.Framework.Input;
    using Utilities.Textures;
    using Utilities.Enumerations;
    using System.Threading;
    class ShootingRange : GameObject, IClickable
    {
        private Target targetA;
        private Target targetB;
        private int score;
        private int hits;
        private int stage;
        private bool mouseIsPressed = false;
        private bool mousePressedHit = false;
        private DifficultyType difficulty;
        private Texture2D targetTexture;
        private int targetSize;
        private int stageTimer;

        public ShootingRange(Texture2D texture, Rectangle rectangle, DifficultyType difficulty)
            : base(texture, rectangle)
        {
            this.Score = AccuracyTrainerStateConstants.Score;
            this.Hits = AccuracyTrainerStateConstants.Hits;
            this.Stage = AccuracyTrainerStateConstants.Stage;
            this.stageTimer = AccuracyTrainerStateConstants.Score;
            this.difficulty = difficulty;
            this.InitializeTargets();
        }

        public int Hits
        {
            get
            {
                return this.hits;
            }
            private set
            {
                this.hits = value;
            }
        }

        public int Stage
        {
            get
            {
                return this.stage;
            }
            private set
            {
                this.stage = value;
            }
        }

        public int Score
        {
            get
            {
                return this.score;
            }
            private set
            {
                this.score = value;
            }
        }

        private void InitializeTargets()
        {
            if (difficulty == DifficultyType.Easy)
            {
                this.targetTexture = Textures.GetTexture("AccuracyEasyTarget");
                this.targetSize = AccuracyTrainerStateConstants.EasyTargetSize;
            }
            else if (difficulty == DifficultyType.Normal)
            {
                this.targetTexture = Textures.GetTexture("AccuracyMediumTarget");
                this.targetSize = AccuracyTrainerStateConstants.MediumTargetSize;
            }
            else
            {
                this.targetTexture = Textures.GetTexture("AccuracyHardTarget");
                this.targetSize = AccuracyTrainerStateConstants.HardTargetSize;
            }

            Rectangle rectangle = new Rectangle
                (
                AccuracyTrainerStateConstants.TargetMinX,
                AccuracyTrainerStateConstants.TargetMinY,
                this.targetSize, this.targetSize 
                );
            this.targetA = new Target(this.targetTexture, rectangle);
            rectangle.X = AccuracyTrainerStateConstants.TargetMaxX;
            rectangle.Y = AccuracyTrainerStateConstants.TargetMaxY;
            this.targetB = new Target(this.targetTexture, rectangle);
        }

        private void RandomizeTargetLocations()
        {
            var randomizer = new Random();
            int coordX = randomizer.Next(AccuracyTrainerStateConstants.TargetMinX, AccuracyTrainerStateConstants.TargetMaxX);
            int coordY = randomizer.Next(AccuracyTrainerStateConstants.TargetMinY, AccuracyTrainerStateConstants.TargetMaxY);
            var mover = targetA.Rectangle;
            mover.X = coordX;
            mover.Y = coordY;
            targetA.Rectangle = mover;
            int coordXtargetB = randomizer.Next(AccuracyTrainerStateConstants.TargetMinX, AccuracyTrainerStateConstants.TargetMaxX);
            int coordYtargetB = randomizer.Next(AccuracyTrainerStateConstants.TargetMinY, AccuracyTrainerStateConstants.TargetMaxY);

            while (coordYtargetB > (coordY - 60) && coordYtargetB < (coordY + 60) &&
                coordXtargetB > (coordX - 60) && coordXtargetB < (coordX + 60))
            {
                coordXtargetB = randomizer.Next(AccuracyTrainerStateConstants.TargetMinX, AccuracyTrainerStateConstants.TargetMaxX);
                coordYtargetB = randomizer.Next(AccuracyTrainerStateConstants.TargetMinY, AccuracyTrainerStateConstants.TargetMaxY);
            }
            mover.X = coordXtargetB;
            mover.Y = coordYtargetB;
            targetB.Rectangle = mover;
        }

        public override void Update(GameTime gameTime)
        {
            this.stageTimer++;
            this.targetA.Update(gameTime);
            this.targetB.Update(gameTime);

            if (this.stageTimer == AccuracyTrainerStateConstants.TargetTimeout || this.CheckForClick())
            {
                this.stageTimer = 0;
                this.Stage += 1;
                this.Score -= AccuracyTrainerStateConstants.MissPenalty * (int)this.difficulty;
                RandomizeTargetLocations();
                this.targetA.IsHit = false;
                this.targetA.Texture = this.targetTexture;
                this.targetB.IsHit = false;
                this.targetB.Texture = this.targetTexture;
            }

            if (this.targetA.IsHit && this.targetB.IsHit)
            {
                this.stageTimer = 0;
                this.Stage += 1;
                this.Score += AccuracyTrainerStateConstants.ComboPoints * (int)this.difficulty;
                RandomizeTargetLocations();
                this.targetA.IsHit = false;
                this.targetA.Texture = this.targetTexture;
                this.targetB.IsHit = false;
                this.targetB.Texture = this.targetTexture;
            }

            if (this.CheckForHit())
            {
                this.Score += AccuracyTrainerStateConstants.HitPoints * (int)this.difficulty;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Rectangle, Color.White);
            this.targetA.Draw(gameTime, spriteBatch);
            this.targetB.Draw(gameTime, spriteBatch);
        }

        public bool CheckForClick()
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);
            if (this.Rectangle.Contains(mousePosition) && !this.targetA.Rectangle.Contains(mousePosition) && !this.targetB.Rectangle.Contains(mousePosition))
            {
                if (mouseState.LeftButton == ButtonState.Pressed && !this.mousePressedHit)
                {
                    this.mouseIsPressed = true;
                }
                if (mouseIsPressed && mouseState.LeftButton == ButtonState.Released)
                {
                    this.mouseIsPressed = false;
                    return true;
                }
            }

            return false;
        }

        public bool CheckForHit()
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);
            if (this.Rectangle.Contains(mousePosition) && (this.targetA.Rectangle.Contains(mousePosition) || this.targetB.Rectangle.Contains(mousePosition)))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    this.mousePressedHit = true;
                }
            }
            if (mousePressedHit && mouseState.LeftButton == ButtonState.Released)
            {
                this.mousePressedHit = false;
                return true;
            }

            return false;
        }
    }
}
