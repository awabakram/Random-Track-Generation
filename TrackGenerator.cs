﻿using System;
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
        const int SpaceOfPointsFromEdge = 20;
        TrackPoint[] trackPoints;
        TrackPoint point0; //the point with the lowest Y value
        TrackPoint[] orderedTrackPoints;

        //Texture2D dot;
        SpriteFont font;


        public TrackGenerator(Vector2 gameBorderTL, Vector2 gameBorderBR, SpriteFont newfont)
        {
            //dot = texture;
            font = newfont;

            InitialisePoints(gameBorderTL, gameBorderBR);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < trackPoints.Length; i++)
            {
                //spriteBatch.DrawCircle(trackPoints[i].getPosition(), 10f, 2, Color.Red);
                //spriteBatch.DrawLine(trackPoints[i].getPosition(), trackPoints[i + 1], Color.Red, 5f);
                spriteBatch.DrawPoint(trackPoints[i].getPosition(), Color.Red, 5);
                //spriteBatch.DrawString(font, $"{i}", trackPoints[i].getPosition(), Color.Red);
            }

            spriteBatch.DrawPoint(point0.getPosition(), Color.Blue, 5);

            //Draws Lines between p0 and all other points
            for (int i = 0; i < trackPoints.Length; i++)
            {
                spriteBatch.DrawLine(point0.getPosition(), trackPoints[i].getPosition(), Color.Yellow, 5);
            }

            //Writes the polar angle for the points so i could check them
            for (int i = 0; i < trackPoints.Length; i++)
            {
                spriteBatch.DrawString(font, $"{trackPoints[i].getPolarAngle()}", trackPoints[i].getPosition(), Color.White);
            }

        }

        void InitialisePoints(Vector2 gameBorderTL, Vector2 gameBorderBR)
        {
            //Decide how many points/turns you want in the track - decided randomly
            numberOfPoints = rand.Next(3, 16);
            trackPoints = new TrackPoint[numberOfPoints];

            //generate the random points
            for (int i = 0; i < trackPoints.Length; i++)
            {
                float tempX = rand.Next(Convert.ToInt32(gameBorderTL.X + SpaceOfPointsFromEdge), Convert.ToInt32(gameBorderBR.X - SpaceOfPointsFromEdge));
                float tempY = rand.Next(Convert.ToInt32(gameBorderTL.Y + SpaceOfPointsFromEdge), Convert.ToInt32(gameBorderBR.Y - SpaceOfPointsFromEdge));

                trackPoints[i] = new TrackPoint(tempX, tempY);
            }

            //set point0
            point0 = findLowestPoint();

            //Find the polar angles
            for (int i = 0; i < trackPoints.Length; i++)
            {
                trackPoints[i].setPolarAngle(findPolarAngle(trackPoints[i]));
            }

            //Find Distances
            for (int i = 0; i < trackPoints.Length; i++)
            {
                trackPoints[i].setDistance(findDistance(trackPoints[i]));
            }


        }

        TrackPoint findLowestPoint()
        {
            TrackPoint lowestPoint = trackPoints[0];
            for (int i = 1; i < trackPoints.Length; i++)
            {
                if (trackPoints[i].getPosition().Y > lowestPoint.getPosition().Y)
                {
                    lowestPoint = trackPoints[i];
                }
                else if (trackPoints[i].getPosition().Y == lowestPoint.getPosition().Y)
                {
                    if (trackPoints[i].getPosition().X < lowestPoint.getPosition().X)
                    {
                        lowestPoint = trackPoints[i];
                    }
                }
            }

            return lowestPoint;
        }

        double findPolarAngle(TrackPoint point)
        {
            float yDifference = point0.getPosition().Y - point.getPosition().Y;
            float xDifference = point.getPosition().X - point0.getPosition().X;

            double angle = Math.Atan(yDifference / xDifference);

            if (xDifference < 0)
            {
                angle = angle + Math.PI;
            }

            return angle;
        }

        double findDistance(TrackPoint point)
        {
            float yDifference = point0.getPosition().Y - point.getPosition().Y;
            float xDifference = point.getPosition().X - point0.getPosition().X;

            return Math.Sqrt((xDifference * xDifference) + (yDifference * yDifference)); //from pythagoras' theorem a^2 + b^2 = c^2
        }

        void orderTrackpoints()
        {
            


        }

        TrackPoint[] merge(TrackPoint[] list1, TrackPoint[] list2)
        {
            TrackPoint[] merged = new TrackPoint[list1.Length + list2.Length];

            int index1 = 0;
            int index2 = 0;
            int indexMerged = 0;

            while (index1 < list1.Length && index2 < list2.Length)
            {
                if (list1[index1].getPolarAngle() < list2[index2].getPolarAngle())
                {
                    merged[indexMerged] = list1[index1];
                    index1++;
                }
                else if (list1[index1].getPolarAngle() == list2[index2].getPolarAngle())
                {
                    if (list1[index1].getDistance() < list2[index2].getDistance())
                    {
                        merged[indexMerged] = list1[index1];
                        index1++;
                    }
                    else
                    {
                        merged[indexMerged] = list2[index2];
                        index2++;
                    }
                }
                else
                {
                    merged[indexMerged] = list2[index2];
                    index2++;
                }
                indexMerged++;
            }

            while (index1 < list1.Length)
            {
                merged[indexMerged] = list1[index1];
                index1++;
                indexMerged++;
            }

            while (index2 < list2.Length)
            {
                merged[indexMerged] = list2[index2];
                index2++;
                indexMerged++;
            }

            return merged;
        }

    }
}
