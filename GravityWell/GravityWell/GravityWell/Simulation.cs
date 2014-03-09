using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


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

        public void LoadContent(ContentManager Content, InputManager input, string mapID)
        {
            //this.content = new ContentManager(Content.ServiceProvider, "Content");
            fileManager = new FileManager();

            entities = new List<Entity>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();

            gravityFactor = 10000000f;
            iterations = 1;

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
                                //loadTexture = content.Load<Texture2D>("BorderSets/" + split[5]);
                                entities.Add(new Entity(split[0], split[1], new Vector2(float.Parse(split[2]), float.Parse(split[3])),
                                    new Vector2(float.Parse(split[4]), float.Parse(split[5])),
                                    new Vector2(float.Parse(split[6]), float.Parse(split[7]))));
                            }
                            break;
                        case "EndEntity":
                            break;
                    }
                }
            }
        }

		public Simulation ()
		{
			//Console.WriteLine ("initing simulation");
            //entities = new List<Entity>();
            //entities.Add(new Entity("well1", "MiscSprites/marioblackhole", new Vector2(400f, 400f), Vector2.Zero, Vector2.Zero));
            //entities.Add(new Entity("well2", "MiscSprites/marioblackhole", new Vector2(1000f, 200f), Vector2.Zero, Vector2.Zero));
            //entities.Add(new Entity("player", "MiscSprites/Astro0", new Vector2(100f, 100f), new Vector2(250f, 0f), Vector2.Zero));
			//entities.Add (new Entity ("well2", 38f, 45f, 0f, 0f, 0f, 0f));
			//gravityFactor = 10000000f;
            //iterations = 1;
		}

		public void Update(GameTime gameTime)
		{
            //gameTime = new GameTime (fps, seconds);
            secondsPast = (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
            //Console.WriteLine("Seconds {0}", secondsPast);

            iterations++;
            //Console.WriteLine("");
            //Console.WriteLine("iteration: {0}", iterations);

            this.updateEntityPositions();
            this.updateEntityVelocities();
            this.updateEntityAccelerations();
            //this.logEntities();

            //if (iterations%100 == 99)
                //iterations *= 1;
		}

		void updateEntityPositions ()
		{
			foreach (Entity entity in entities) {

                //float xVelocity = Vector2.UnitX(entity.vel * (float)Math.Cos(entity.vel.Y);
                //float yVelocity = entity.vel.X * (float)Math.Sin(entity.vel.UnY);

				entity.pos.X += entity.vel.X * secondsPast;
				entity.pos.Y += entity.vel.Y * secondsPast;

			}
		}

		void updateEntityVelocities ()
		{

			foreach (Entity entity in entities) {
                /*
                float xVelocity = entity.vel.X * (float)Math.Cos(entity.vel.Y);
                float yVelocity = entity.vel.X * (float)Math.Sin(entity.vel.Y);

				float xAcceleration = entity.accel.X * (float)Math.Cos (entity.accel.Y);
                float yAcceleration = entity.accel.X * (float)Math.Sin(entity.accel.Y);

				float newXVelocity = xVelocity + xAcceleration;
				float newYVelocity = yVelocity + yAcceleration;

				entity.vel.X = (float)Math.Sqrt (newXVelocity * newXVelocity + newYVelocity * newYVelocity);
                entity.vel.Y = (float)Math.Atan2(newYVelocity, newXVelocity);
                */

                entity.vel.X += entity.accel.X * secondsPast;
                entity.vel.Y += entity.accel.Y * secondsPast;

                if (Vector2.Distance(entity.vel, Vector2.Zero) != 0f)
                    entity.rotation += 5 * 1/Vector2.Distance(entity.vel, Vector2.Zero);
                //Console.WriteLine("accel * {0} = ({1}, {2})",secondsPast, entity.accel.X*secondsPast, entity.accel.Y*secondsPast);
			}

		}

		void updateEntityAccelerations ()
		{

			float netXAccel = 0f;
			float netYAccel = 0f;

			foreach (Entity entity in entities) {

				if (entity.name == "player") {

					foreach (Entity otherEntity  in entities) {

						if (otherEntity.name != "player") {

                            float distance = Vector2.Distance(entity.pos, otherEntity.pos);
                            Vector2 distanceVector = Vector2.Subtract(otherEntity.pos, entity.pos);
							float angle = angleBetweenTwoEntities(distanceVector);

                            float accel = gravityFactor / (distance * distance);

							float xAccel = accel * (float)(Math.Cos(angle));
							float yAccel = accel * (float)(Math.Sin(angle));

							netXAccel += xAccel;
							netYAccel += yAccel;

						}

					}

					

					entity.accel.X = netXAccel;
					entity.accel.Y = netYAccel;

					break;
					
				}

			}


		}

		void testForCollisions ()
		{
			for (int i = 0; i < entities.Count; i++) {

				for (int j = 0; j < entities.Count; j++) {

					if (!(i == j)) {



					}

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

				if (entity.name == "player") {
					//Console.WriteLine ("{0} {1}", entity.pos.X, entity.pos.Y);
				}

			}
		}

		float angleBetweenTwoEntities (Vector2 vector) {

			return (float)(Math.Atan2(vector.Y, vector.X));

		}

		float distanceBetweenTwoEntities (Entity entity1, Entity entity2) {

			//float xDistance = entity1.pos.X - entity2.pos.X;
			//float yDistance = entity1.pos.Y - entity2.pos.Y;

			float distance = Vector2.Distance(entity1.pos, entity2.pos);
			//Console.WriteLine ("distance between {0} and {1}: {2}", entity1.name, entity2.name, distance);

			return distance;

		}

	}
}

