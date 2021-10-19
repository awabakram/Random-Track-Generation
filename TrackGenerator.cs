using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Random_Track_Generation
{
    class TrackGenerator
    {
        Random rand = new Random();
        int numberOfPoints;
        TrackPoint[] trackPoints;
        //Texture2D dot;
        SpriteFont font;

        public TrackGenerator(Vector2 gameBorderTL, Vector2 gameBorderBR, SpriteFont newfont)
        {
            //dot = texture;
            font = newfont;

            InitialisePoints(gameBorderTL, gameBorderBR);
        }

        void InitialisePoints(Vector2 gameBorderTL, Vector2 gameBorderBR)
        {
            //Decide how many points/turns you want in the track - decided randomly
            numberOfPoints = rand.Next(3, 16);
            trackPoints = new TrackPoint[numberOfPoints];

            //generate the random points
            for (int i = 0; i < trackPoints.Length; i++)
            {
                float tempX = rand.Next(Convert.ToInt32(gameBorderTL.X + 20), Convert.ToInt32(gameBorderBR.X - 20));
                float tempY = rand.Next(Convert.ToInt32(gameBorderTL.Y + 20), Convert.ToInt32(gameBorderBR.Y - 20));

                trackPoints[i] = new TrackPoint(tempX, tempY);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < trackPoints.Length; i++)
            {
                //spriteBatch.DrawCircle(trackPoints[i], 10f, 2, Color.Red);
                //spriteBatch.DrawLine(trackPoints[i], trackPoints[i + 1], Color.Red, 5f);
                //spriteBatch.DrawPoint(trackPoints[i], Color.Red, 5);
                //spriteBatch.DrawString(font, $"{i}", trackPoints[i], Color.Red);
            }
        }


    }
}
