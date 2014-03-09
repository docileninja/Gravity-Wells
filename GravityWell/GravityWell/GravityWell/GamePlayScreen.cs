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
        Rendering renderer;
        Simulation simulation;
        //Player player;
        //Map map;
        Entity entity;
        int mapNum;
        ContentManager content;

        public override void LoadContent(ContentManager Content, InputManager input)
        {
            this.content = new ContentManager(Content.ServiceProvider, "Content");
            mapNum = 2;
            String mapID = "Map" + mapNum.ToString();
            simulation = new Simulation();
            simulation.LoadContent(content, input, mapID);
            renderer = new Rendering();
            renderer.LoadContent(content);
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
            renderer.UnloadContent();
            //player.UnloadContent();
            //map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            entity.Update(gameTime);
            simulation.Update(gameTime);
            
            //player.Update(gameTime, inputManager, map.collision, map.layer);
            //map.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            entity.Draw(spriteBatch);
            renderer.Draw(simulation.entities, spriteBatch);
            
            //
            //map.Draw(spriteBatch);
            //player.Draw(spriteBatch);
        }
    }
}
