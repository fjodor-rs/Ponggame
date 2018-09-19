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
		static Texture2D balletjeTexture;
		Vector2 position;

		Vector2 speed;

		public static void LoadContent(ContentManager content)
		{
			balletjeTexture = content.Load<Texture2D>("bal");
		}

		public Balletje(Vector2 position)
		{
			this.position = position;
		}

		public Vector2 Position{ get { return position; } }

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(balletjeTexture, position, Color.White);
		}

		public void balDelete()
		{
		}
	//	protected void BalReset()
	//	{
	//		position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
		//	var angle = rand.NextDouble() * MathHelper.TwoPi;
		//	snelheidbal = new Vector2((float)Math.Cos(angle) * 4, (float)Math.Sin(angle) * 4);
		//}


	}
}
