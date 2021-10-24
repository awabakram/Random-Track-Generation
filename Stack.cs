using System;
using System.Collections.Generic;
using System.Text;

namespace Random_Track_Generation
{
    class Stack
    {
        List<TrackPoint> stack = new List<TrackPoint>();

        public Stack(TrackPoint[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                stack.Add(array[i]);
            }
        }
        
        public void push(TrackPoint addPoint)
        {
            stack.Add(addPoint);
        }

        public TrackPoint pop()
        {
            try
            {
                TrackPoint tempPoint = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                return tempPoint;
            }
            catch (ArgumentOutOfRangeException)
            {

                return null;
            }
        }

        public TrackPoint popSTL() //pop Second to Last
        {
            try
            {
                TrackPoint tempPoint = stack[stack.Count - 2];
                stack.RemoveAt(stack.Count - 2);
                return tempPoint;
            }
            catch (ArgumentOutOfRangeException)
            {

                return null;
            }
        }


        public List<TrackPoint> getStack()
        {
            return stack;
        }

        public TrackPoint getLastPoint()
        {
            try
            {
                return stack[stack.Count - 1];
            }
            catch (ArgumentOutOfRangeException)
            {

                return null;
            }
        }

        public TrackPoint getSTLPoint() //Get second to last point
        {
            try
            {
                return stack[stack.Count - 2];
            }
            catch (ArgumentOutOfRangeException)
            {

                return null;
            }
        }
    }
}
