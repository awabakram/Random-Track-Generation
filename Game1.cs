using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Text;

namespace Random_Track_Generation
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D greenRectangle;
        //Texture2D dot;

        Vector2 gameBorderTL = new Vector2(320, 0); //Top Left Corner of game section
        Vector2 gameBorderBR = new Vector2(1920, 1080); //Bottom Right Corner of game section

        Track Track_1;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Make the Green Rectangle for the background
            greenRectangle = loadRectangle(Convert.ToInt32(_graphics.PreferredBackBufferWidth - gameBorderTL.X), _graphics.PreferredBackBufferHeight, Color.Green);

            //Make a small red square as temporary markers for the points
            //dot = loadRectangle(10, 10, Color.Red);

            SpriteFont arial = Content.Load<SpriteFont>("Arial");

            //Set of test points
            //TrackPoint[] inputPoints = new TrackPoint[] { new TrackPoint(600, 900), new TrackPoint(900, 750), new TrackPoint(1000, 650), new TrackPoint(900, 500), new TrackPoint(550, 450), new TrackPoint(450, 500) };

            Track_1 = new Track(gameBorderTL, gameBorderBR, arial);

            //test for commit in college

            

        }

        Texture2D loadRectangle(int width, int height, Color color) //I Made this method before i added monogame extended
        {
            Texture2D texture = new Texture2D(GraphicsDevice, width, height);
            Color[] colourPixels = new Color[width * height];
            for (int i = 0; i < colourPixels.Length; i++)
            {
                colourPixels[i] = color;
            }
            texture.SetData<Color>(colourPixels);

            return texture;
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);
            _spriteBatch.Begin();

            _spriteBatch.Draw(greenRectangle, gameBorderTL, Color.White);

            Track_1.Draw(_spriteBatch);



            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
