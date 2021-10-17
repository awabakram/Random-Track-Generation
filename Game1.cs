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
        Texture2D dot;

        Vector2 gameBorderTL = new Vector2(320, 0); //Top Left Corner of game section
        Vector2 gameBorderBR = new Vector2(1920, 1080); //Bottom Right Corner of game section

        TrackGenerator Generator;

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
            dot = loadRectangle(10, 10, Color.Red);

            Generator = new TrackGenerator(gameBorderTL, gameBorderBR, dot);

        }

        Texture2D loadRectangle(int width, int height, Color color)
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

            Generator.Draw(_spriteBatch);



            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
