namespace BrainGames.Models.MenuState
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class Box : GameObject
    {
        public Box(Texture2D texture, Rectangle rectangle)
            : base(texture, rectangle)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Rectangle, Color.White);
        }
    }
}
