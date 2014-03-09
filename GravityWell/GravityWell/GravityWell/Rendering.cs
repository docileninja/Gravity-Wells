using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GravityWell
{
    public class Rendering
    {
        Texture2D drawTexture;
        ContentManager content;


        public void LoadContent(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public void Draw(List<Entity> ents, SpriteBatch spriteBatch)
        {
            drawTexture = content.Load<Texture2D>("MiscSprites/bg1");
            spriteBatch.Draw(drawTexture, Vector2.Zero, null, Color.MidnightBlue, 0f, Vector2.Zero, new Vector2((float)1280 / drawTexture.Width, (float)720 / drawTexture.Height), SpriteEffects.None, 1f);
            for (int i = 0; i < ents.Count; i++)
            {
                drawTexture = content.Load<Texture2D>(ents[i].textureName);
                spriteBatch.Draw(drawTexture, ents[i].pos, null, Color.White, ents[i].rotation, new Vector2(drawTexture.Width/2, drawTexture.Height/2), new Vector2(1f, 1f), SpriteEffects.None, 0f);
            }
            
        }
    }
}
