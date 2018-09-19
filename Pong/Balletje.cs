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
			speed = new Vector2((float)Math.Cos(angle) * 4, (float)Math.Sin(angle) * 4);
		}

		public void Update()
		{
			position += speed;
		}
	}
}
