using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GravityWell
{
    class GamePlayScreen : GameScreen
    {
        //Player player;
        //Map map;
        Entity entity;

        public override void LoadContent(ContentManager content, InputManager input)
        {
            entity = new Entity();
            base.LoadContent(content, input);
            entity.LoadContent(content);
            //player = new Player();
            //map = new Map();
            //map.LoadContent(content, map, "Map1");
            //player.LoadContent(content, input);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            entity.UnloadContent();
            //player.UnloadContent();
            //map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            entity.Update(gameTime);
            //player.Update(gameTime, inputManager, map.collision, map.layer);
            //map.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            entity.Draw(spriteBatch);
            //map.Draw(spriteBatch);
            //player.Draw(spriteBatch);
        }
    }
}
