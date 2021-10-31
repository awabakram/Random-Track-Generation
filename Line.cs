using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Random_Track_Generation
{
    class Line
    {
        float gradient;
        float y_intercept;
        bool fixedLength;
        Vector2 point1;
        Vector2 point2;

        public Line(Vector2 p1, Vector2 p2, bool fxdlength)
        {
            gradient = (p2.Y - p1.Y) / (p2.X - p1.X);
            y_intercept = (gradient * -p1.X) + p1.Y;

            fixedLength = fxdlength;

            if (fixedLength)
            {
                point1 = p1;
                point2 = p2;
            }
        }

        public Line(float m, Vector2 p1)
        {
            gradient = m;
            y_intercept = (gradient * -p1.X) + p1.Y;
        }

        public Line(float m, float yInt)
        {
            gradient = m;
            y_intercept = yInt;
        }

        public float getGradient()
        {
            return gradient;
        }

        public float getYIntercept()
        {
            return y_intercept;
        }

        public Vector2 getPoint1()
        {
            return point1;
        }

        public Vector2 getPoint2()
        {
            return point2;
        }

        public float findYValue(float X)
        {
            return gradient * X + y_intercept;
        }

        public float findXValue(float Y)
        {
            return (Y - y_intercept) / gradient;
        }
    }
}

