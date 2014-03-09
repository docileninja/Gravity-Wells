using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GravityWell
{
    class TitleScreen : GameScreen
    {
        Texture2D backgroundImage;
        Texture2D playImage, titleImage;
        SpriteFont font;

        public override void LoadContent(ContentManager Content, InputManager input)
        {
            base.LoadContent(Content, input);
            backgroundImage = this.content.Load<Texture2D>("MiscSprites/TitleBackground");
            titleImage = this.content.Load<Texture2D>("MiscSprites/Title");
            if (font == null)
                font = this.content.Load<SpriteFont>("Font1");
        }

        public override void UnloadContent()
        {
            base.content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            if (inputManager.KeyReleased(Keys.Space))
            {
                ScreenManager.Instance.AddScreen(new GamePlayScreen(), inputManager);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, Vector2.Zero, Color.White);
            spriteBatch.Draw(titleImage, Vector2.Zero, Color.White);
            if (inputManager.KeyDown(Keys.Space))
            {
                playImage = content.Load<Texture2D>("MiscSprites/PlayGameSelected");
            }
            else
            {
                playImage = content.Load<Texture2D>("MiscSprites/PlayGame");
            }

            spriteBatch.Draw(playImage, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "Press space to play.", new Vector2(0f, 400f), Color.White);
        }
    }
}
