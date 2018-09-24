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
		public Vector2 positierood, positieblauw;
		public Texture2D rood, blauw;
		private Texture2D bal;
		private float speed = 10;
		public int levenrood = 3, levenblauw = 3;
        Vector2 poslevenrood, poslevenblauw;
		public int lijnrood, lijnblauw;
		public int screenheight, screenwidth;
        enum GameState { init, running, gameOver };
        GameState gameState;
		List<Balletje> balletjes = new List<Balletje>();
		SpriteFont font1;
        string message, message2;
        Vector2 messageSize, messageSize2;
        Vector2 roodStart, blauwStart;



		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		
		protected override void Initialize()
        { 
            base.Initialize();
            gameState = GameState.init;
            message = "Druk op spatie om te beginnen";
            messageSize = font1.MeasureString(message);
            message2 = "Druk op spatie om te herstarten";
            messageSize2 = font1.MeasureString(message2);

            screenheight = GraphicsDevice.Viewport.Height;
			screenwidth = GraphicsDevice.Viewport.Width;

			balletjes.Add(new Balletje(new Vector2(screenheight / 2, screenwidth / 2), bal, this));
            //positie van de ballen die de levens aan gaan duiden
            poslevenrood.Y = 16;
            poslevenblauw.Y = 16;
        }

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			rood = Content.Load<Texture2D>("rodeSpeler");
			blauw = Content.Load<Texture2D>("blauweSpeler");
            font1 = Content.Load<SpriteFont>("font1");
            
			bal = Content.Load<Texture2D>("bal");
			
			lijnrood = 50 + rood.Width;
			lijnblauw = GraphicsDevice.Viewport.Width - (50 + blauw.Width);

            //balken klaarzetten
            roodStart = new Vector2(50, (GraphicsDevice.Viewport.Height - rood.Height) / 2);
            blauwStart = new Vector2(lijnblauw, (GraphicsDevice.Viewport.Height - blauw.Height) / 2);
            positierood = roodStart;
			positieblauw = blauwStart;

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


        //protected 
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

            // start het spel
            if (gameState == GameState.init && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                gameState = GameState.running;
                balletjes[0].BalReset();
            }

            if (gameState == GameState.gameOver && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                levenblauw = 3;
                levenrood = 3;
                balletjes[0].BalReset();
                gameState = GameState.running;
                positierood = roodStart;
                positieblauw = blauwStart;
            }

                if (gameState == GameState.running)
                HandleInput();

            ScreenBounds(ref positierood); //Houdt de rode balk in het scherm
            ScreenBounds(ref positieblauw); //Houdt de blauwe balk in het scherm

			balletjes[0].Update();

			balletjes[0].BalBounds(); //Checkt voor collision

            //controleert de levens
            if (levenblauw == 0 && gameState == GameState.running)
            {
                gameState = GameState.gameOver;
                message = "Rood heeft gewonnen!";
                messageSize = font1.MeasureString(message);
            }

            if (levenrood == 0 && gameState == GameState.running)
            {
                gameState = GameState.gameOver;
                message = "Blauw heeft gewonnen!";
                messageSize = font1.MeasureString(message);
            }

			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);
			spriteBatch.Begin();

            for (var i = 0; i < levenrood; i++)
            {
                poslevenrood.X = lijnrood + 16 + i * bal.Width;
                spriteBatch.Draw(bal, poslevenrood, Color.White);
            }
            for (var i = 0; i < levenblauw; i++)
            {
                poslevenblauw.X = (lijnblauw - 16) - (i + 1) * bal.Width;
                spriteBatch.Draw(bal, poslevenblauw, Color.White);
            }

            //Start message aan het begin van het spel
            if (gameState == GameState.init)
            {
                spriteBatch.DrawString(font1, message, new Vector2((screenwidth - messageSize.X) / 2, (screenheight - messageSize.Y) / 2), Color.Black);
            }

            //message als iemand gewonnen heeft
            if (gameState == GameState.gameOver)
            {
                spriteBatch.DrawString(font1, message, new Vector2((screenwidth - messageSize.X) / 2, (screenheight - messageSize.Y) / 2), Color.Black);
                spriteBatch.DrawString(font1, message2, new Vector2((screenwidth - messageSize2.X) / 2, (screenheight - (messageSize2.Y * 2) - 20) / 2), Color.Black);
				balletjes[0].Alive = false;
			}

            spriteBatch.Draw(rood, positierood, Color.White);
			spriteBatch.Draw(blauw, positieblauw, Color.White);
			
			foreach (Balletje bal in balletjes)
				if(bal.Alive)
					{
						bal.Draw(spriteBatch);
					}
			base.Draw(gameTime);
			spriteBatch.End();
		}


	}
}
