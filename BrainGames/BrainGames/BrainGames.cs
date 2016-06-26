namespace BrainGames
{
    using global::BrainGames.Models;
    using global::BrainGames.Models.MenuState;
    using global::BrainGames.Utilities.Textures;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using Utilities.Constants;

    public class BrainGames : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameStateManager stateManager;
        private SpriteFont spriteFont;

        public BrainGames()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferWidth = GlobalConstants.WindowWidth;
            this.graphics.PreferredBackBufferHeight = GlobalConstants.WindowHeight;
            this.IsMouseVisible = true;
        }

        public SpriteFont SpriteFont
        {
            get
            {
                return this.spriteFont;
            }
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            Textures.InitiateTextures(this);
            this.spriteFont = this.Content.Load<SpriteFont>("Fonts\\ArialFont");
            this.stateManager = new GameStateManager(this);
            Background startingBackground = new Background(Textures.GetTexture("MenuBackground")); // starting state is Menu State
            MenuState menuState = new MenuState(startingBackground, this.stateManager);
            this.stateManager.States.Push(menuState);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            this.stateManager.States.Peek().Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Black);

            this.spriteBatch.Begin();
            this.stateManager.States.Peek().Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
        }
    }
}
