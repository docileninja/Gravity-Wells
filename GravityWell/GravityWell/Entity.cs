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
        

        public Vector2 pos, vel, accel, length;
        public Vector2 center;
        public float rotation;
        public string textureName;
        public string name;
        public string type;
        public bool collided;
        public Vector2 scaleFactor;
        ContentManager content;

        public Entity()
        {
        }

        public Entity(string newType, string newTextureName, Vector2 newPos, Vector2 newVel, Vector2 newAccel)
        {
            pos = newPos;
            vel = newVel;
            accel = newAccel;
            rotation = 0f;
            type = newType;
            textureName = newTextureName;
        }

        public Entity(string newType, string newTextureName, Vector2 newPos, Vector2 newVel, Vector2 newAccel, Vector2 newLength, bool isWall)
        {
            pos = newPos;
            vel = newVel;
            accel = newAccel;
            rotation = 0f;
            type = newType;
            textureName = newTextureName;

            if (isWall)
            {
                length = newLength;
                rotation = (float)Math.Atan2(length.Y, length.X);
            }
            else
            {
                scaleFactor = newLength;
            }
        }

        public void LoadContent(ContentManager content)
        {
            
            this.content = new ContentManager(content.ServiceProvider, "Content");
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Texture2D drawTexture = content.Load<Texture2D>("MiscSprites/standingmario");
            //spriteBatch.Draw(drawTexture, new Vector2(0, 0), Color.White);
        }
    }
}
