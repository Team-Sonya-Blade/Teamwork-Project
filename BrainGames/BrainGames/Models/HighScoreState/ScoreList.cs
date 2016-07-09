namespace BrainGames.Models.AccuracyTrainerState
{
    using System;
    using System.Collections.Generic;

    using global::BrainGames.Utilities.Constants;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.IO;
    using System.Linq;

    class ScoreList : GameObject
    {
        private SpriteFont font;
        private string[] scores;

        public ScoreList(Texture2D texture, Rectangle rectangle, SpriteFont font)
            : base(texture, rectangle)
        {
            this.font = font;
            InitializeScores();
        }

        private void InitializeScores()
        {
            this.scores = File.ReadAllLines(GlobalConstants.HighScorePath);
            this.scores = this.scores.OrderByDescending(x => int.Parse(x)).ToArray();
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Rectangle, Color.White);

            int lineCount;

            if (this.scores.Length < HighScoreStateConstants.rows)
            {
                lineCount = this.scores.Length;
            }
            else
            {
                lineCount = HighScoreStateConstants.rows;
            }

            for (int i = 0; i < lineCount; i++)
            {
                this.DrawLine(this.scores[i], spriteBatch, HighScoreStateConstants.ScoresStartingYpos + (i * HighScoreStateConstants.ScoresYinterval));
            }
        }

        private void DrawLine(string line, SpriteBatch spriteBatch, int yPos)
        {
            spriteBatch.DrawString(
                this.font,
                string.Format("Score: {0}", line),
                new Vector2(HighScoreStateConstants.ScoresXpos, yPos),
                Color.White,
                0,
                new Vector2(0, 0),
                new Vector2(HighScoreStateConstants.FontScale, HighScoreStateConstants.FontScale),
                SpriteEffects.None,
                0f);
        }
    }
}
