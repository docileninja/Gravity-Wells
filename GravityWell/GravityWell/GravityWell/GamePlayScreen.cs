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
        int mapNum, endTime;
        double seconds;
        string tips;
        SpriteFont font;

        public override void LoadContent(ContentManager Content, InputManager input)
        {
            this.content = new ContentManager(Content.ServiceProvider, "Content");
            mapNum = 1;
            String mapID = "Map" + mapNum.ToString();
            simulation = new Simulation();
            simulation.LoadContent(content, mapID);
            renderer = new Rendering();
            renderer.LoadContent(content);
            base.LoadContent(content, input);
            tips = "Hold space to increase gravity.";

            if (font == null)
                font = this.content.Load<SpriteFont>("Font1");
            //player = new Player();
            //map = new Map();
            //map.LoadContent(content, map, "Map1");
            //player.LoadContent(content, input);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            renderer.UnloadContent();
            //player.UnloadContent();
            //map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            simulation.Update(gameTime, inputManager);
            seconds += gameTime.ElapsedGameTime.Milliseconds;
            if (simulation.isEnd)
            {
                foreach (Entity end in simulation.entities)
                {
                    if (end.type == "end")
                    {
                        foreach (Entity player in simulation.entities)
                        {
                            if (player.type == "player")
                            {
                                player.pos = end.pos;
                                player.vel = Vector2.Zero;
                                player.accel = Vector2.Zero;
                                player.rotation += 0.5f + endTime * 0.0003f;
                                endTime += gameTime.ElapsedGameTime.Milliseconds;
                                if (endTime > 2000)
                                {
                                    simulation.UnloadContent();
                                    simulation = new Simulation();
                                    mapNum++;
                                    if (mapNum > 1)
                                        tips = "";
                                    endTime = 0;
                                    if (mapNum > 3)
                                        ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
                                    else
                                        simulation.LoadContent(content, "Map" + mapNum.ToString());

                                }
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            if (simulation.isGameOver == true)
            {
                tips = "Don't get too far away.";
                simulation.UnloadContent();
                simulation = new Simulation();
                simulation.LoadContent(content, "Map" + mapNum.ToString());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach(Entity entity in simulation.entities)
            {
                if (entity.type == "player")
                {
                    renderer.Draw(simulation.entities, spriteBatch, entity.pos);
                    break;
                }
            }

            spriteBatch.DrawString(font, tips, new Vector2(0f, 650f), Color.White);
            //
            //map.Draw(spriteBatch);
            //player.Draw(spriteBatch);
        }
    }
}
