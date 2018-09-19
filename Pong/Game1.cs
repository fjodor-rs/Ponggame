using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Pong
{
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Vector2 positierood, positieblauw;
		Texture2D rood, blauw;
		private Texture2D bal;
		private float speed = 10;
		private int levenrood = 3;
		private int levenblauw = 3;
		private int lijnrood;
		private int lijnblauw;
		private int screenheight;
		private int screenwidth;
        enum GameState { init, running, gameOver };
        GameState gameState;
		List<Balletje> balletjes = new List<Balletje>();



		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		
		protected override void Initialize()
        { 
            base.Initialize();
            gameState = GameState.init;
			screenheight = GraphicsDevice.Viewport.Height;
			screenwidth = GraphicsDevice.Viewport.Width;
			balletjes.Add(new Balletje(new Vector2(screenheight / 2, screenwidth / 2), bal));

			balletjes[0].BalReset();
        }

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			rood = Content.Load<Texture2D>("rodeSpeler");
			blauw = Content.Load<Texture2D>("blauweSpeler");
			bal = Content.Load<Texture2D>("bal");
			
			Console.WriteLine("Console");

			lijnrood = 50 + rood.Width;
			lijnblauw = GraphicsDevice.Viewport.Width - 50;

            //balken klaarzetten
			positierood = new Vector2(50, (GraphicsDevice.Viewport.Height - rood.Height) / 2);
			positieblauw = new Vector2(lijnblauw, (GraphicsDevice.Viewport.Height - blauw.Height) / 2);

        }

		protected override void UnloadContent()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="positiebalk"></param>
		protected void ScreenBounds(ref Vector2 positiebalk)
		{
			if (positiebalk.Y < 0)
				positiebalk.Y = 0;
			if (positiebalk.Y > GraphicsDevice.Viewport.Height - rood.Height)
				positiebalk.Y = GraphicsDevice.Viewport.Height - rood.Height;
		}

		/// <summary>
		/// 
		/// </summary>
		protected void BalBounds()
		{
			if (balletjes[0].Position.Y + balletjes[0].Speed.Y < 0)
				balletjes[0].BalLimit();

			if (balletjes[0].Position.Y + balletjes[0].Speed.Y > GraphicsDevice.Viewport.Height - balletjes[0].Height)
				balletjes[0].BalLimit();

			if (balletjes[0].Position.X + balletjes[0].Speed.X <= lijnrood && balletjes[0].Position.X > lijnrood && balletjes[0].Position.Y > positierood.Y - balletjes[0].Height && balletjes[0].Position.Y < positierood.Y + rood.Height)
				balletjes[0].Bounce();

			if (balletjes[0].Position.X + balletjes[0].Speed.X + balletjes[0].Width >= lijnblauw && balletjes[0].Position.X + balletjes[0].Width < lijnblauw && balletjes[0].Position.Y > positieblauw.Y - balletjes[0].Height && balletjes[0].Position.Y < positieblauw.Y + blauw.Height)
				balletjes[0].Bounce();

			if (balletjes[0].Position.X < 0)
			{
				levenrood -= 1;
				balletjes[0].BalReset();
			}
			if (balletjes[0].Position.X > GraphicsDevice.Viewport.Width - balletjes[0].Width)
			{
				levenblauw -= 1;
				balletjes[0].BalReset();
			}
		}

		protected void HandleInput()
        {
            //De input van speler rood
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                positierood += new Vector2(0, -speed);
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                positierood += new Vector2(0, speed);

            //De input van speler blauw
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                positieblauw += new Vector2(0, -speed);
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                positieblauw += new Vector2(0, speed);
        }
	
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

            if (gameState == GameState.init && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                gameState = GameState.running;
            }

            if (gameState == GameState.running)
                HandleInput();

            ScreenBounds(ref positierood); //Houdt de rode balk in het scherm
            ScreenBounds(ref positieblauw); //Houdt de blauwe balk in het scherm

			balletjes[0].Update();

			BalBounds(); //Checkt voor collision

			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);
			spriteBatch.Begin();
			spriteBatch.Draw(rood, positierood, Color.White);
			spriteBatch.Draw(blauw, positieblauw, Color.White);
			foreach (Balletje bal in balletjes)
			{
				bal.Draw(spriteBatch);
			}
			base.Draw(gameTime);
			spriteBatch.End();
		}
	}
}
