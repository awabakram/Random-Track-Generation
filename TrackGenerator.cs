using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Random_Track_Generation
{
    class TrackGenerator
    {
        Random rand = new Random();
        int numberOfPoints;
        Vector2[] trackPoints;
        Texture2D dot;

        public TrackGenerator(Vector2 gameBorderTL, Vector2 gameBorderBR, Texture2D texture)
        {
            dot = texture;

            InitialisePoints(gameBorderTL, gameBorderBR);
        }

        void InitialisePoints(Vector2 gameBorderTL, Vector2 gameBorderBR)
        {
            //Decide how many points/turns you want in the track - decided randomly
            numberOfPoints = rand.Next(3, 16);
            trackPoints = new Vector2[numberOfPoints];

            //generate the random points
            for (int i = 0; i < trackPoints.Length; i++)
            {
                trackPoints[i].X = rand.Next(Convert.ToInt32(gameBorderTL.X + 20), Convert.ToInt32(gameBorderBR.X - 20));
                trackPoints[i].Y = rand.Next(Convert.ToInt32(gameBorderTL.Y + 20), Convert.ToInt32(gameBorderBR.Y - 20));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < trackPoints.Length; i++)
            {
                spriteBatch.Draw(dot, trackPoints[i], Color.Red);
            }
        }


    }
}
