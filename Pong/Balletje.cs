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

		public static void LoadContent(ContentManager content)
		{
			balletjeTexture = content.Load<Texture2D>("bal");
		}

		public Balletje(Vector2 position)
		{
			this.position = position;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
            spriteBatch.Draw(balletjeTexture, position, Color.White);
		}

	}
}
