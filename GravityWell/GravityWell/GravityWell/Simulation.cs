using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace GravityWell
{
	public class Simulation
	{
        ContentManager content;
        FileManager fileManager;
        public List<Entity> entities;
        List<List<string>> attributes;
        List<List<string>> contents;

        int iterations;
		//public List<Entity> entities;
		//GameTime gameTime;
		float secondsPast;
		float gravityFactor;
        public bool isEnd, isGameOver;

        public void LoadContent(ContentManager Content, string mapID)
        {
            //this.content = new ContentManager(Content.ServiceProvider, "Content");
            fileManager = new FileManager();

            entities = new List<Entity>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();

            gravityFactor = 1000000f;
            iterations = 1;
            isEnd = false;
            isGameOver = false;

            fileManager.LoadContent("Load/Maps/" + mapID + ".cme", attributes, contents, "Entities");

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "StartEntity":
                            for (int k = 0; k < contents[i].Count; k++)
                            {
                                string[] split = contents[i][k].Split(',');
                                entities.Add(new Entity(split[0], split[1], new Vector2(float.Parse(split[2]), float.Parse(split[3])),
                                    new Vector2(float.Parse(split[4]), float.Parse(split[5])),
                                    new Vector2(float.Parse(split[6]), float.Parse(split[7]))));
                            }
                            break;
                        case "EndEntity":
                            break;
                        case "StartWall":
                            for (int k = 0; k < contents[i].Count; k++)
                            {
                                string[] split = contents[i][k].Split(',');
                                entities.Add(new Entity(split[0], split[1], new Vector2(float.Parse(split[2]), float.Parse(split[3])),
                                    new Vector2(float.Parse(split[4]), float.Parse(split[5])),
                                    new Vector2(float.Parse(split[6]), float.Parse(split[7])),
                                    new Vector2(float.Parse(split[8]), float.Parse(split[9])), true));
                            }
                            break;
                        case "EndWall":
                            break;
                        case "StartWell":
                            for (int k = 0; k < contents[i].Count; k++)
                            {
                                string[] split = contents[i][k].Split(',');
                                entities.Add(new Entity(split[0], split[1], new Vector2(float.Parse(split[2]), float.Parse(split[3])),
                                    new Vector2(float.Parse(split[4]), float.Parse(split[5])),
                                    new Vector2(float.Parse(split[6]), float.Parse(split[7])),
                                    new Vector2(float.Parse(split[8]), float.Parse(split[9])), false));
                            }
                            break;
                        case "EndWell":
                            break;
                        case "StartFinish":
                            for (int k = 0; k < contents[i].Count; k++)
                            {
                                string[] split = contents[i][k].Split(',');
                                entities.Add(new Entity(split[0], split[1], new Vector2(float.Parse(split[2]), float.Parse(split[3])),
                                    new Vector2(float.Parse(split[4]), float.Parse(split[5])),
                                    new Vector2(float.Parse(split[6]), float.Parse(split[7])),
                                    new Vector2(float.Parse(split[8]), float.Parse(split[9])), false));
                            }
                            break;
                        case "EndFinish":
                            break;
                    }
                }
            }
        }

		public Simulation ()
		{
		}

        public void UnloadContent()
        {
            //this.content.Unload();
        }

		public void Update(GameTime gameTime, InputManager input)
		{
            secondsPast = (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;

            iterations++;

            if (isEnd)
            {
                /*
                foreach (Entity end in entities)
                {
                    if (end.type == "end")
                    {
                        foreach (Entity player in entities)
                        {
                            if (player.type == "player")
                            {
                                player.pos = end.pos;
                                player.vel = Vector2.Zero;
                                player.accel = Vector2.Zero;
                                player.rotation += 0.5f;
                                endTime++;
                                if (endTime > 5)
                                {
                                }
                            }
                        }
                        break;
                    }
                }
                 * */
            }
            else
            {
                this.updateEntityPositions();
                this.updateEntityVelocities();
                this.updateEntityAccelerations(input);
                this.testForCollisions();
                this.testForEnd();
            }
            //this.logEntities();

            //if (iterations%100 == 99)
                //iterations *= 1;
		}

		void updateEntityPositions ()
		{
			foreach (Entity entity in entities) {

				entity.pos.X += entity.vel.X * secondsPast;
				entity.pos.Y += entity.vel.Y * secondsPast;

			}
		}

		void updateEntityVelocities ()
		{

			foreach (Entity entity in entities) {
                
                entity.vel.X += entity.accel.X * secondsPast;
                entity.vel.Y += entity.accel.Y * secondsPast;

                if (Vector2.Distance(entity.vel, Vector2.Zero) != 0f)
                    entity.rotation += 5 * 1/Vector2.Distance(entity.vel, Vector2.Zero);
			}

		}

		void updateEntityAccelerations (InputManager input)
		{

			float netXAccel = 0f;
			float netYAccel = 0f;

            float minDistance = 100000000000f;

			foreach (Entity entity in entities) {

				if (entity.type == "player") {

					foreach (Entity otherEntity  in entities) {

						if (otherEntity.type == "well") {

                            float distance = Vector2.Distance(entity.pos, otherEntity.pos);
                            Vector2 distanceVector = Vector2.Subtract(otherEntity.pos, entity.pos);
							float angle = angleBetweenTwoEntities(distanceVector);
                            float accel;

                            if (input.KeyDown(Keys.Space))
                            {
                                accel = gravityFactor * 5f / (distance * distance);
                                if (accel < 15f)
                                {
                                    accel = 15f;
                                }
                            }
                            else
                            {
                                accel = gravityFactor / (distance * distance);
                            }

							float xAccel = accel * (float)(Math.Cos(angle));
							float yAccel = accel * (float)(Math.Sin(angle));

							netXAccel += xAccel;
							netYAccel += yAccel;

                            if (distance < minDistance)
                                minDistance = distance;

                            
						}

					}

                    if (minDistance > 1200 || minDistance < 10)
                        isGameOver = true;

                    if (input.KeyDown(Keys.Up)) { netYAccel -= 5f; }
                    if (input.KeyDown(Keys.Down)) { netYAccel += 5f; }
                    if (input.KeyDown(Keys.Left)) { netXAccel -= 5f; }
                    if (input.KeyDown(Keys.Right)) { netXAccel += 5f; }

					entity.accel.X = netXAccel;
					entity.accel.Y = netYAccel;

					break;
					
				}

			}


		}

		void testForCollisions ()
        {

            foreach (Entity entity in entities)
            {

                if (entity.type == "player")
                {

                    foreach (Entity wall in entities)
                    {

                        if (wall.type == "wall")
                        {

                            float distanceAlongWall = Vector2.Dot(entity.pos - wall.pos, wall.length);
                            Vector2 scaledWall = Vector2.Normalize(wall.length);
                            scaledWall *= distanceAlongWall;

                            if (distanceAlongWall > 0 && distanceAlongWall < Vector2.Distance(wall.length, Vector2.Zero))
                            {
                                float distanceToWall = Vector2.Distance(entity.pos, scaledWall + wall.pos);

                                if (distanceToWall < 11f)
                                {
                                    entity.vel = Vector2.Reflect(entity.vel, Vector2.Normalize(wall.length));
                                }
                            }

                            
                        }

                    }

                    break;

                }

            }

        }

        void testForEnd()
        {
            foreach (Entity entity in entities)
            {

                if (entity.type == "player")
                {

                    foreach (Entity end in entities)
                    {

                        if (end.type == "end")
                        {

                            float distance = Vector2.Distance(entity.pos, end.pos);

                            if (distance < 80)
                            {
                                isEnd = true;
                            }
                        }

                    }

                    break;

                }

            }
        }

		void logEntities ()
		{
			foreach (Entity entity in entities) {

				Console.WriteLine ("name: {0} position: ({1}, {2}) velocity: ({3}, {4}) accel: ({5}, {6})", entity.name, entity.pos.X, entity.pos.Y, entity.vel.X, entity.vel.Y, entity.accel.X, entity.accel.Y);

			}
		}

		void logEntitiesForExcel ()
		{
			foreach (Entity entity in entities) {

				if (entity.type == "player") {
					//Console.WriteLine ("{0} {1}", entity.pos.X, entity.pos.Y);
				}

			}
		}

		float angleBetweenTwoEntities (Vector2 vector) {

			return (float)(Math.Atan2(vector.Y, vector.X));

		}

		float distanceBetweenTwoEntities (Entity entity1, Entity entity2) {

			float distance = Vector2.Distance(entity1.pos, entity2.pos);

			return distance;

		}

	}
}

