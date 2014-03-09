using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GravityWell
{
    public class Entity
    {
        Vector2 pos, vel, accel;
        Vector2 center;
        float theta;
        String textureName;
        ContentManager content;

        public void LoadContent(ContentManager content)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D drawTexture = content.Load<Texture2D>("MiscSprites/standingmario");
            spriteBatch.Draw(drawTexture, new Vector2(0, 0), Color.White);
        }
    }
}
