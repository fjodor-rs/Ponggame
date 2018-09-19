using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Vector2 positiebal, positierood, positieblauw, snelheidbal;
		Texture2D bal, rood, blauw;
		private float speed = 10;
		private int levenrood = 3;
		private int levenblauw = 3;
		private int lijnrood;
		private int lijnblauw;
        enum GameState { init, running, gameOver };
        GameState gameState;
		readonly Random rand = new Random();



		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		
		protected override void Initialize()
        { 
            base.Initialize();
            gameState = GameState.init;
        }

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			bal = Content.Load<Texture2D>("bal");
			rood = Content.Load<Texture2D>("rodeSpeler");
			blauw = Content.Load<Texture2D>("blauweSpeler");

			lijnrood = 50 + rood.Width;
			lijnblauw = GraphicsDevice.Viewport.Width - 50;

            //balken klaarzetten
			positierood = new Vector2(50, (GraphicsDevice.Viewport.Height - rood.Height) / 2);
			positieblauw = new Vector2(lijnblauw, (GraphicsDevice.Viewport.Height - blauw.Height) / 2);

            //Bal in het midden zetten
            positiebal.X = (GraphicsDevice.Viewport.Width - bal.Width) / 2;
            positiebal.Y = (GraphicsDevice.Viewport.Height - bal.Height) / 2;
        }

		protected override void UnloadContent()
		{
		}

		protected void ScreenBounds(ref Vector2 positiebalk)
		{
			if (positiebalk.Y < 0)
				positiebalk.Y = 0;
			if (positiebalk.Y > GraphicsDevice.Viewport.Height - rood.Height)
				positiebalk.Y = GraphicsDevice.Viewport.Height - rood.Height;
		}

        //De bal versnellen
		protected void Bounce()
		{
			snelheidbal.X *= -1;
			snelheidbal.Y *= 1.08f;
			snelheidbal.X *= 1.08f;
		}

		protected void BalReset()
		{

			positiebal = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

			var angle = rand.NextDouble() * MathHelper.TwoPi;
			snelheidbal = new Vector2((float)Math.Cos(angle) * 4, (float)Math.Sin(angle) * 4);
		}

		protected void BalBounds()
		{
			if (positiebal.Y + snelheidbal.Y < 0)
				snelheidbal.Y *= -1;

			if (positiebal.Y + snelheidbal.Y > GraphicsDevice.Viewport.Height - bal.Height)
				snelheidbal.Y *= -1;

			if (positiebal.X + snelheidbal.X <= lijnrood && positiebal.X > lijnrood && positiebal.Y > positierood.Y - bal.Height && positiebal.Y < positierood.Y + rood.Height)
				Bounce();

			if (positiebal.X + snelheidbal.X + bal.Width >= lijnblauw && positiebal.X + bal.Width < lijnblauw && positiebal.Y > positieblauw.Y - bal.Height && positiebal.Y < positieblauw.Y + blauw.Height)
				Bounce();

			if (positiebal.Y + bal.Height + snelheidbal.Y == positierood.Y && positiebal.X + snelheidbal.X <= lijnrood && positiebal.X <= positierood.X)
				Bounce();

			if (positiebal.X < 0)
			{
				levenrood -= 1;
				BalReset();
			}
			if (positiebal.X > GraphicsDevice.Viewport.Width - bal.Width)
			{
				levenblauw -= 1;
				BalReset();
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
                BalReset();
            }

            if (gameState == GameState.running)
                HandleInput();

            ScreenBounds(ref positierood); //Houdt de rode balk in het scherm
            ScreenBounds(ref positieblauw); //Houdt de blauwe balk in het scherm



			positiebal += snelheidbal; //Verplaatst de bal

			BalBounds(); //Checkt voor collision

			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);
			spriteBatch.Begin();
			spriteBatch.Draw(rood, positierood, Color.White);
			spriteBatch.Draw(blauw, positieblauw, Color.White);
			spriteBatch.Draw(bal, positiebal, Color.White);

			base.Draw(gameTime);
			spriteBatch.End();
		}
	}
}
