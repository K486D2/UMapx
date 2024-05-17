﻿using System;
using System.Drawing;

namespace UMapx.Imaging
{
    /// <summary>
    /// Using for points operations.
    /// </summary>
    public static class Points
    {
        #region Operators

        /// <summary>
        /// Returns processed points.
        /// </summary>
        /// <param name="points">Points</param>
        /// <param name="point">Point</param>
        /// <returns>Points</returns>
        public static Point[] Add(this Point[] points, Point point)
        {
            var count = points.Length;
            var output = new Point[count];

            for (int i = 0; i < count; i++)
            {
                output[i] = new Point
                {
                    X = points[i].X + point.X,
                    Y = points[i].Y + point.Y
                };
            }

            return output;
        }

        /// <summary>
        /// Returns processed points.
        /// </summary>
        /// <param name="points">Points</param>
        /// <param name="point">Point</param>
        /// <returns>Points</returns>
        public static Point[] Sub(this Point[] points, Point point)
        {
            var count = points.Length;
            var output = new Point[count];

            for (int i = 0; i < count; i++)
            {
                output[i] = new Point
                {
                    X = points[i].X - point.X,
                    Y = points[i].Y - point.Y
                };
            }

            return output;
        }

        #endregion

        #region Special operators

        /// <summary>
        /// Rotates points by angle.
        /// </summary>
        /// <param name="points">Points</param>
        /// <param name="centerPoint">Center point</param>
        /// <param name="angle">Angle</param>
        /// <returns>Points</returns>
        public static Point[] Rotate(this Point[] points, Point centerPoint, float angle)
        {
            int length = points.Length;
            var output = new Point[length];

            for (int i = 0; i < length; i++)
            {
                output[i] = points[i].Rotate(centerPoint, angle);
            }

            return output;
        }

        /// <summary>
        /// Rotates point by angle.
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="centerPoint">The center point of rotation.</param>
        /// <param name="angleInDegrees">The rotation angle in degrees.</param>
        /// <returns>Rotated point</returns>
        public static Point Rotate(this Point pointToRotate, Point centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);

            return new Point
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }

        /// <summary>
        /// Returns rectangle from points.
        /// </summary>
        /// <param name="points">Points</param>
        /// <returns>Rectangle</returns>
        public static Rectangle GetRectangle(this Point[] points)
        {
            int length = points.Length;
            int xmin = int.MaxValue;
            int ymin = int.MaxValue;
            int xmax = int.MinValue;
            int ymax = int.MinValue;

            for (int i = 0; i < length; i++)
            {
                int x = points[i].X;
                int y = points[i].Y;

                if (x < xmin)
                    xmin = x;
                if (y < ymin)
                    ymin = y;
                if (x > xmax)
                    xmax = x;
                if (y > ymax)
                    ymax = y;
            }

            return new Rectangle(xmin, ymin, xmax - xmin, ymax - ymin);
        }

        /// <summary>
        /// Return angle of the three points.
        /// </summary>
        /// <param name="left">Left point</param>
        /// <param name="right">Right point</param>
        /// <param name="support">Supported point</param>
        /// <returns>Angle</returns>
        public static float GetAngle(this Point left, Point right, Point support)
        {
            double kk = left.Y > right.Y ? 1 : -1;

            double x1 = left.X - support.X;
            double y1 = left.Y - support.Y;

            double x2 = right.X - left.X;
            double y2 = right.Y - left.Y;

            double a = Math.Sqrt(x1 * x1 + y1 * y1);
            double b = Math.Sqrt(x2 * x2 + y2 * y2);
            double c = x1 * x2 + y1 * y2;

            double d = c.Div(a).Div(b);

            return (float)(kk * (180.0 - Math.Acos(d) * 57.3));
        }

        /// <summary>
        /// Returns div result of two variables.
        /// </summary>
        /// <param name="a">First</param>
        /// <param name="b">Second</param>
        /// <returns>Result</returns>
        private static double Div(this double a, double b)
        {
            if (a == 0 && b == 0)
            {
                return double.Epsilon;
            }

            return a / b;
        }

        /// <summary>
        /// Returns supported point.
        /// </summary>
        /// <param name="left">Left point</param>
        /// <param name="right">Right point</param>
        /// <returns>Point</returns>
        public static Point GetSupportedPoint(this Point left, Point right)
        {
            return new Point(right.X, left.Y);
        }

        /// <summary>
        /// Returns mean point.
        /// </summary>
        /// <param name="points">Points</param>
        /// <returns>Point</returns>
        public static Point GetMeanPoint(params Point[] points)
        {
            var point = new Point(0, 0);
            var length = points.Length;

            for (int i = 0; i < length; i++)
            {
                point.X += points[i].X;
                point.Y += points[i].Y;
            }

            point.X /= length;
            point.Y /= length;

            return point;
        }

        #endregion
    }
}
