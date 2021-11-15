using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Random_Track_Generation
{
    class Track
    {
        Random rand = new Random();

        //Attributes relating to the game border
        Vector2 gameBorderTL;
        Vector2 gameBorderBR;
        const int SpaceOfPointsFromEdge = 120;

        //Attributes relating to track points
        int numberOfPoints;
        TrackPoint[] trackPoints;
        TrackPoint point0; //the point with the lowest Y value
        TrackPoint[] orderedTrackPoints;
        TrackPoint[] convexHullPoints;
        TrackPoint[] convexHullPointsWithMidpoints;

        List<TrackPoint> finalPoints = new List<TrackPoint>();
        TrackPoint startPoint;
        TrackPoint[] startPointEdges;
        

        bool trackPossible = true;

        //Attributes relating to drawing
        //Texture2D dot;
        SpriteFont font;
        const float trackWidth = 100f;


        public Track(Vector2 newGameBorderTL, Vector2 newGameBorderBR, SpriteFont newfont)
        {
            //dot = texture;
            font = newfont;
            gameBorderTL = newGameBorderTL;
            gameBorderBR = newGameBorderBR;


            GenerateTrack();
        }

        public Track(Vector2 newGameBorderTL, Vector2 newGameBorderBR, SpriteFont newfont, TrackPoint[] cHull)
        {
            //dot = texture;
            font = newfont;
            gameBorderTL = newGameBorderTL;
            gameBorderBR = newGameBorderBR;

            convexHullPoints = cHull;
            findFinalTrackPoints();
            findStartPoint();
        }

        public void GenerateTrack()
        {
            InitialisePoints(gameBorderTL, gameBorderBR);
            orderTrackpoints();
            trackPossible = checkTrackPossible();

            if (trackPossible == false)
            {
                return;
            }

            grahamScan();
            findFinalTrackPoints();
            findStartPoint();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (trackPossible == false)
            {
                spriteBatch.DrawString(font, "Track Generation Failed", new Vector2((gameBorderTL.X + gameBorderBR.X) / 2, (gameBorderTL.Y + gameBorderBR.Y) / 2), Color.Black);
                return;
            }

            ////draw all points as red dots
            //for (int i = 0; i < trackPoints.Length; i++)
            //{
            //    spriteBatch.DrawPoint(trackPoints[i].getPosition(), Color.Red, 5);
            //}

            //spriteBatch.DrawPoint(point0.getPosition(), Color.Blue, 5);

            ////Writes the polar angle for the points so I could check them
            //for (int i = 0; i < trackPoints.Length; i++)
            //{
            //    spriteBatch.DrawString(font, $"{trackPoints[i].getPolarAngle()}", trackPoints[i].getPosition(), Color.White);
            //}

            ////Writes the polar angles in a list on the eft side of the window
            //string tempDisplaypoints = $"";
            //for (int i = 0; i < orderedTrackPoints.Length; i++)
            //{
            //    tempDisplaypoints += $"{orderedTrackPoints[i].getPolarAngle()} , \n";
            //}
            //spriteBatch.DrawString(font, tempDisplaypoints, new Vector2(10, 10), Color.Black);

            ////draw Convex Hull
            //for (int i = 0; i < convexHullPoints.Length - 1; i++)
            //{
            //    spriteBatch.DrawLine(convexHullPoints[i].getPosition(), convexHullPoints[i + 1].getPosition(), Color.Black, trackWidth);
            //}
            //spriteBatch.DrawLine(convexHullPoints[convexHullPoints.Length - 1].getPosition(), convexHullPoints[0].getPosition(), Color.Black, trackWidth);

            ////draw circles
            //for (int i = 0; i < convexHullPoints.Length; i++)
            //{
            //    spriteBatch.DrawCircle(convexHullPoints[i].getPosition(), trackWidth / 2, 32, Color.Black, trackWidth / 2);
            //}


            //Draw lines between teh points
            for (int i = 0; i < finalPoints.Count - 1; i++)
            {
                spriteBatch.DrawLine(finalPoints[i].getPosition(), finalPoints[i + 1].getPosition(), Color.Black, trackWidth);
            }
            spriteBatch.DrawLine(finalPoints[finalPoints.Count - 1].getPosition(), finalPoints[0].getPosition(), Color.Black, trackWidth);

            for (int i = 0; i < finalPoints.Count; i++)
            {
                spriteBatch.DrawCircle(finalPoints[i].getPosition(), trackWidth / 2, 32, Color.Black, trackWidth / 2);
            }

            spriteBatch.DrawLine(startPointEdges[0].getPosition(), startPointEdges[1].getPosition(), Color.White, 5f);


        }

        public bool getTrackPossible()
        {
            return trackPossible;
        }

        void InitialisePoints(Vector2 gameBorderTL, Vector2 gameBorderBR)
        {
            //Decide how many points you want to generate - decided randomly
            numberOfPoints = rand.Next(5, 25);
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
                trackPoints[i].setDistance(findDistance(point0,trackPoints[i]));
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

        double findDistance(TrackPoint p0, TrackPoint p1)
        {
            float yDifference = p0.getPosition().Y - p1.getPosition().Y;
            float xDifference = p1.getPosition().X - p0.getPosition().X;

            return Math.Sqrt((xDifference * xDifference) + (yDifference * yDifference)); //from pythagoras' theorem a^2 + b^2 = c^2
        }

        void orderTrackpoints()
        {
            List<TrackPoint> orderedTrackPointsList = new List<TrackPoint>();
            orderedTrackPointsList.Add(point0);

            TrackPoint[] sanitisedTrackPoints = removePoint(trackPoints, point0); //return an array of trackPoints with point0 removed from it

            TrackPoint[] tempSortedPoints = mergeSort(sanitisedTrackPoints);
            tempSortedPoints = removePointsWithSamePolarAngle(tempSortedPoints);

            for (int i = 0; i < tempSortedPoints.Length; i++)
            {
                orderedTrackPointsList.Add(tempSortedPoints[i]);
            }

            orderedTrackPoints = orderedTrackPointsList.ToArray();
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

        TrackPoint[] mergeSort(TrackPoint[] items)
        {
            TrackPoint[] left_half;
            TrackPoint[] right_half;

            //Base case for recursion
            if (items.Length < 2)
            {
                return items;
            }

            int midpoint = items.Length / 2;

            //Do the left half
            left_half = new TrackPoint[midpoint];
            for (int i = 0; i < midpoint; i++)
            {
                left_half[i] = items[i];
            }

            //figure out how big the right half should be
            if (items.Length % 2 == 0)
            {
                right_half = new TrackPoint[midpoint];
            }
            else
            {
                right_half = new TrackPoint[midpoint + 1];
            }

            //fill in hte right half
            int rightIndex = 0;
            for (int i = midpoint; i < items.Length; i++)
            {
                right_half[rightIndex] = items[i];
                rightIndex++;
            }

            //recursion bit
            left_half = mergeSort(left_half);
            right_half = mergeSort(right_half);

            items = merge(left_half, right_half);

            return items;
        }

        TrackPoint[] removePoint(TrackPoint[] inputPoints, TrackPoint pointToBeRemoved)
        {
            TrackPoint[] points = new TrackPoint[inputPoints.Length - 1];
            
            int pointIndex = 0;
            
            for (int i = 0; i < inputPoints.Length; i++)
            {
                if (trackPoints[i] != pointToBeRemoved)
                {
                    points[pointIndex] = inputPoints[i];
                    pointIndex++;
                }
            }

            //i tried this incase it thought every point in trackPoints == point0 and was thus returning null
            if (points == null)
            {
                return new TrackPoint[] { new TrackPoint(-1, -1), new TrackPoint(-2, -2) };
            }

            return points;
        }

        TrackPoint[] removePointsWithSamePolarAngle(TrackPoint[] inputSortedPoints)
        {
            TrackPoint[] currentSortedPoints = inputSortedPoints;
            int roundingValue = 5; //checks them rounded to 5 d.p.

            for (int i = 0; i < inputSortedPoints.Length - 1; i++)
            {
                if (Math.Round(inputSortedPoints[i].getPolarAngle(), roundingValue) == Math.Round(inputSortedPoints[i + 1].getPolarAngle(), roundingValue))
                {
                    if (inputSortedPoints[i].getDistance() > inputSortedPoints[i + 1].getDistance())
                    {
                        currentSortedPoints = removePoint(currentSortedPoints, inputSortedPoints[i + 1]);
                    }
                    else
                    {
                        currentSortedPoints = removePoint(currentSortedPoints, inputSortedPoints[i]);
                    }
                }
            }

            return currentSortedPoints;
        }

        bool checkTrackPossible()
        {
            if (orderedTrackPoints.Length < 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        void grahamScan()
        {
            Stack pointsStack = new Stack(new TrackPoint[] {orderedTrackPoints[0], orderedTrackPoints[1], orderedTrackPoints[2]});

            for (int i = 3; i < orderedTrackPoints.Length; i++)
            {
                while (checkLeft(pointsStack.getSTLPoint(), pointsStack.getLastPoint(), orderedTrackPoints[i]) == false)
                {
                    pointsStack.pop();
                }
                pointsStack.push(orderedTrackPoints[i]);
            }

            convexHullPoints = pointsStack.getStack().ToArray();

        }

        bool checkLeft(TrackPoint A, TrackPoint B, TrackPoint C) //Checks if the next point is left or not
        {
            double result = ((B.getPosition().X - A.getPosition().X) * (A.getPosition().Y - C.getPosition().Y)) - ((A.getPosition().Y - B.getPosition().Y) * (C.getPosition().X - A.getPosition().X)); // Cross-product of lines AB and AC

            if (result < 0)
            {
                return false;
            }

            return true;
        }

        void findFinalTrackPoints()
        {

            for (int i = 0; i < convexHullPoints.Length - 1; i++)
            {
                //finalPoints.Add(tempInputPoints[i]);

                TrackPoint[] tempPoints = findBezierCurve(convexHullPoints[i], findCurvePoint(convexHullPoints[i], convexHullPoints[i + 1]), convexHullPoints[i + 1]);

                for (int j = 0; j < tempPoints.Length; j++)
                {
                    finalPoints.Add(tempPoints[j]);
                }
            }

        }

        TrackPoint findCurvePoint(TrackPoint point1, TrackPoint point2)
        {
            TrackPoint randomPoint = findRandomPointAlongLine(point1, point2);
            Line line = new Line(point1.getPosition(), point2.getPosition(), true);

            float perpGradient = -1 / line.getGradient();
            Line perpLine = new Line(perpGradient, randomPoint.getPosition());

            float xOffset;
            float curvePointX;
            float curvePointY;

            do
            {
                xOffset = rand.Next(-20, 20);

                curvePointX = randomPoint.getPosition().X + xOffset;
                curvePointY = perpLine.findYValue(curvePointX); 
            } while (curvePointY < gameBorderTL.Y || curvePointY > gameBorderBR.Y || curvePointX < gameBorderTL.X || curvePointX > gameBorderBR.X);

            return new TrackPoint(curvePointX, curvePointY);
        }

        TrackPoint findMidpoint(TrackPoint point1, TrackPoint point2)
        {
            return new TrackPoint(new Vector2((point1.getPosition().X + point2.getPosition().X) / 2, (point1.getPosition().Y + point2.getPosition().Y) / 2));
        }

        TrackPoint findRandomPointAlongLine(TrackPoint point1, TrackPoint point2)
        {
            float X;
            float threshold = 0.1f; //between 0 and 1
            Line line = new Line(point1.getPosition(), point2.getPosition(), true);
            float xDifference = point2.getPosition().X - point1.getPosition().X;
            
            float minX = xDifference * threshold + point1.getPosition().X;
            float maxX = xDifference * (1 - threshold) + point1.getPosition().X;

            if (minX < maxX)
            {
                X = rand.Next(Convert.ToInt32(minX), Convert.ToInt32(maxX));
            }
            else if (minX > maxX)
            {
                X = rand.Next(Convert.ToInt32(maxX), Convert.ToInt32(minX));
            }
            else
            {
                X = findMidpoint(point1, point2).getPosition().X;
            }
            
            float Y = line.findYValue(X);

            return new TrackPoint(X, Y);
        }

        TrackPoint[] findBezierCurve(TrackPoint p0, TrackPoint p1, TrackPoint p2)
        {
            List<Line> curveLines = new List<Line>();
            List<Vector2> POIs = new List<Vector2>();

            float tIncrement = 0.01f;

            for (float t = tIncrement; t < 1; t += tIncrement)
            {
                float pA_X = ((p1.getPosition().X - p0.getPosition().X) * t) + p0.getPosition().X;
                float pA_Y = ((p1.getPosition().Y - p0.getPosition().Y) * t) + p0.getPosition().Y;
                TrackPoint pA = new TrackPoint(new Vector2(pA_X, pA_Y));

                float pB_X = ((p2.getPosition().X - p1.getPosition().X) * t) + p1.getPosition().X;
                float pB_Y = ((p2.getPosition().Y - p1.getPosition().Y) * t) + p1.getPosition().Y;
                TrackPoint pB = new TrackPoint(new Vector2(pB_X, pB_Y));

                curveLines.Add(new Line(pA.getPosition(), pB.getPosition(), true));
            }

            for (int i = 0; i < curveLines.Count - 1; i++)
            {
                POIs.Add(findPOI(curveLines[i], curveLines[i + 1]));
            }

            TrackPoint[] returnArray = new TrackPoint[POIs.Count];
            for (int i = 0; i < returnArray.Length; i++)
            {
                returnArray[i] = new TrackPoint(POIs[i]);
            }

            return returnArray;

        }

        Vector2 findPOI(Line l1, Line l2)
        {
            float tempY = l2.getYIntercept() - l1.getYIntercept();
            float tempM = l1.getGradient() - l2.getGradient();
            float X = tempY / tempM;
            float Y = l1.findYValue(X);

            return new Vector2(X, Y);
        }

        void  findStartPoint()
        {
            startPoint = findRandomPointAlongLine(finalPoints[finalPoints.Count - 1], finalPoints[0]);
            Line lastLine = new Line(finalPoints[finalPoints.Count - 1].getPosition(), finalPoints[0].getPosition(), true);

            float perpendicularGradient = (-1) / lastLine.getGradient();
            Line perpendicularLine = new Line(perpendicularGradient, startPoint.getPosition());

            Vector2 edge1 = perpendicularLine.findPointAtDistance(startPoint.getPosition(), trackWidth / 2, true);
            Vector2 edge2 = perpendicularLine.findPointAtDistance(startPoint.getPosition(), trackWidth / 2, false);

            startPointEdges = new TrackPoint[] { new TrackPoint(edge1), new TrackPoint(edge2) };

        }
    }
}
