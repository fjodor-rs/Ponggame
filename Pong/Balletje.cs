using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
	class Balletje
	{
		Texture2D texture;
		Vector2 position;
		Vector2 speed;
		private bool alive;

		readonly Random rand = new Random();


		public void LoadContent(ContentManager content)
		{
		}

		public Balletje(Vector2 position, Texture2D texture)
		{
			this.position = position;
			this.texture = texture;
			BalReset();
		}

		public Vector2 Speed { get { return speed; } }

		public int Width { get { return texture.Width; } }

		public int Height { get { return texture.Height; } }

		public Vector2 Position { get { return position; } }

		public bool Alive { get { return alive; } set { alive = value; } }

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, position, Color.White);
		}

		public void BalDelete()
		{
		}

		//De bal versnellen
		public void Bounce()
		{
			speed.X *= -1;
			speed.Y *= 1.08f;
			speed.X *= 1.08f;
		}

		public void BalLimit()
		{
			speed.Y *= -1;
		}

		public void BalReset()
		{
			position = new Vector2(800 / 2, 480 / 2);
			var angle = rand.NextDouble() * MathHelper.TwoPi;
			if (angle > MathHelper.Pi * 0.25 && angle < MathHelper.Pi * 0.75)
			{
				angle += 0.5 * MathHelper.Pi;
			}
			else if (angle > MathHelper.Pi * 1.25 && angle < MathHelper.Pi * 1.75)
			{
				angle += 0.5 * MathHelper.Pi;
			}
			speed = new Vector2((float)Math.Cos(angle) * 5, (float)Math.Sin(angle) * 5);
			alive = true;
		}

		public void Update()
		{
			position += speed;
		}
	}
}
