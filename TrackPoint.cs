using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Random_Track_Generation
{
    class TrackPoint
    {
        Vector2 position;
        double polarAngle;

        public TrackPoint(Vector2 newPos)
        {
            position = newPos;
        }

        public TrackPoint(float newX, float newY)
        {
            position.X = newX;
            position.Y = newY;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public double getPolarAngle()
        {
            return polarAngle;
        }

        public void setPolarAngle(double angle)
        {
            polarAngle = angle;
        }
    }
}
