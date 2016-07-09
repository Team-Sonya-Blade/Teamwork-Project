namespace BrainGames.Models.AccuracyTrainerState
{
    using global::BrainGames.Interfaces;
    using global::BrainGames.Models.CustomExceptions;
    using global::BrainGames.Utilities.Constants;
    using global::BrainGames.Utilities.Textures;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Target : GameObject, IClickable
    {
        private bool isHit = false;
        private bool mouseIsPressed = false;

        public Target(Texture2D texture, Rectangle rectangle)
            : base(texture, rectangle)
        {
        }

        public bool IsHit
        {
            get
            {
                return this.isHit;
            }
            set
            {
                this.isHit = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.CheckForClick())
            {
                this.Texture = Textures.GetTexture("AccuracyHitTarget");
                this.IsHit = true;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Rectangle, Color.White);
        }

        public bool CheckForClick()
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);

            if (this.Rectangle.Contains(mousePosition))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    this.mouseIsPressed = true;
                }
            }
            if (mouseIsPressed && mouseState.LeftButton == ButtonState.Released)
            {
                this.mouseIsPressed = false;
                return true;
            }
 

            return false;
        }
    }
}