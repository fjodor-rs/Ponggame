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
        float speedAngle;
		Game1 game;

		readonly Random rand = new Random();


		public void LoadContent(ContentManager content)
		{
		}

		public Balletje(Vector2 position, Texture2D texture, Game1 game)
		{
			this.position = position;
			this.texture = texture;
			this.game = game;

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

		//De nieuwe hoek van de bal berekenen
		public void Bounce(float balk)
		{
            float relpos, addAngle;
            int hitboxHeight = game.rood.Height + texture.Height;
            float balOrigin = position.Y + (texture.Height / 2);
            speed.X *= -1;
			speed.Y *= 1.08f;
			speed.X *= 1.08f;
            float x2 = speed.X * speed.X;
            float y2 = speed.Y * speed.Y;
            speedAngle = (float)Math.Atan2(speed.Y, speed.X);
            float speedSum = (float)Math.Sqrt(x2 + y2);
            int balkmidden = (int)balk - texture.Height + hitboxHeight / 2;

            //relatieve positie van de bal ten opzichte van het midden van de balk tot het uiteinde van de balk, op een schaal van 0 tot 1
            relpos = Math.Abs((balOrigin - balkmidden) / (hitboxHeight / 2));
            Console.WriteLine("relpos = " + relpos);
            addAngle = relpos * (30 * (MathHelper.Pi / 180));

            if (speed.X > 0 && balOrigin >= balkmidden) //onderin linker balk
            {
                speedAngle += addAngle; //afbuigen naar onder
            } 
            else if (speed.X < 0 && balOrigin >= balkmidden) //onderin rechter balk
            {
                speedAngle -= addAngle; //afbuigen naar onder
            } else if (speed.X < 0 && balOrigin < balkmidden) //bovenin linker balk
            {
                speedAngle += addAngle; //afbuigen naar boven
            } else if (speed.X > 0 && balOrigin < balkmidden) //bovenin rechter balk
            {
                speedAngle -= addAngle; //afbuigen naar boven
            }

            //Snelheidsvector updaten
            speed.Y = (float)Math.Sin(speedAngle) * speedSum;
            speed.X = (float)Math.Cos(speedAngle) * speedSum; 

        }
        
        //Regelt de collision tussen de bal, de randen van het scherm en de balken (spelers). Haalt ook levens er van af indien de bal gemist wordt.
		public void BalBounds()
		{
			if (position.Y + speed.Y < 0)
				BalLimit();

			if (position.Y + speed.Y > game.screenheight - texture.Height)
				BalLimit();

			if (position.X + speed.X <= game.lijnrood && position.X > game.lijnrood && position.Y > game.positierood.Y - texture.Height && position.Y < game.positierood.Y + game.rood.Height)
				Bounce(game.positierood.Y);

			if (position.X + speed.X + texture.Width >= game.lijnblauw && position.X + texture.Width < game.lijnblauw && position.Y > game.positieblauw.Y - texture.Height && position.Y < game.positieblauw.Y + game.blauw.Height)
				Bounce(game.positieblauw.Y);

			if (position.X < 0)
			{
				game.levenrood -= 1;
				BalReset();
			}
			if (position.X > game.screenwidth - texture.Width)
			{
				game.levenblauw -= 1;
				BalReset();
			}
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

            //Zorgt ervoor dat de bal stil staat als ie niet getekent wordt
            if (!alive && speed != Vector2.Zero)
                speed = Vector2.Zero;

		}
	}
}
