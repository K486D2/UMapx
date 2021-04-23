﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using UMapx.Core;

namespace UMapx.Imaging
{
    /// <summary>
    /// Uses for editing and transforming images.
    /// </summary>
    public static class BitmapTransform
    {
        #region Rotate
        /// <summary>
        /// Rotates the bitmap by the specified angle. 
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Rotate(this Bitmap b, float angle)
        {
            return Rotate(b, new PointFloat(b.Width / 2.0f, b.Height / 2.0f), angle);
        }
        /// <summary>
        /// Rotates the bitmap by the specified angle. 
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Rotate(this Bitmap b, int x, int y, float angle)
        {
            return Rotate(b, new PointFloat(x, y), angle);
        }
        /// <summary>
        /// Rotates the bitmap by the specified angle. 
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <param name="point">Float point</param>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Rotate(this Bitmap b, PointFloat point, float angle)
        {
            Bitmap bmp = new Bitmap(b.Width, b.Height);
            bmp.SetResolution(b.HorizontalResolution, b.VerticalResolution);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.TranslateTransform(point.X, point.Y);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-point.X, -point.Y);
            graphics.DrawImage(b, new PointF(0, 0));
            graphics.Dispose();
            return bmp;
        }
        #endregion

        #region Flip
        /// <summary>
        /// Flips bitmap by X axis.
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <returns>Bitmap</returns>
        public static Bitmap FlipX(this Bitmap b)
        {
            var clone = (Bitmap)b.Clone();
            clone.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return clone;
        }
        /// <summary>
        /// Flips bitmap by Y axis.
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <returns>Bitmap</returns>
        public static Bitmap FlipY(this Bitmap b)
        {
            var clone = (Bitmap)b.Clone();
            clone.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return clone;
        }
        #endregion

        #region Crop
        /// <summary>
        /// Returns cropped image.
        /// </summary>
        /// <param name="image">Bitmap</param>
        /// <param name="rectangle">Rectangle</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Crop(this Bitmap image, Rectangle rectangle)
        {
            // image params
            int width = image.Width;
            int height = image.Height;

            // check section params
            int x = Range(rectangle.X, 0, width);
            int y = Range(rectangle.Y, 0, height);
            int w = Range(rectangle.Width, 0, width - x);
            int h = Range(rectangle.Height, 0, height - y);

            // fixes rectangle section
            var rectangle_fixed = new Rectangle(x, y, w, h);

            // crop image to rectangle section
            var bitmap = new Bitmap(rectangle_fixed.Width, rectangle_fixed.Height);
            var section = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            using var g = Graphics.FromImage(bitmap);
            g.DrawImage(image, section, rectangle_fixed, GraphicsUnit.Pixel);

            return bitmap;
        }
        /// <summary>
        /// Fixes value in range.
        /// </summary>
        /// <param name="x">Value</param>
        /// <param name="min">Min</param>
        /// <param name="max">Max</param>
        /// <returns>Value</returns>
        private static int Range(int x, int min, int max)
        {
            if (x < min)
            {
                return min;
            }
            else if (x > max)
            {
                return max;
            }
            return x;
        }
        #endregion

        #region Resize
        /// <summary>
        /// Resizes bitmap.
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Resize(this Bitmap b, int width, int height)
        {
            return new Bitmap(b, width, height);
        }
        /// <summary>
        /// Resizes bitmap.
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <param name="value">Scale value</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Resize(this Bitmap b, float value)
        {
            return Resize(b, (int)(b.Width * value), (int)(b.Height * value));
        }
        #endregion

        #region Shift
        /// <summary>
        /// Shifts bitmap by X axis.
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <param name="value">Shift value</param>
        /// <returns>Bitmap</returns>
        public static Bitmap ShiftX(this Bitmap b, int value)
        {
            Bitmap bmp = (Bitmap)b.Clone();
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.DrawImage(b, value, 0, b.Width, b.Height);
            graphics.DrawImage(b, value - b.Width, 0, b.Width, b.Height);
            graphics.Dispose();
            return bmp;
        }
        /// <summary>
        /// Shifts bitmap by Y axis.
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <param name="value">Shift value</param>
        /// <returns>Bitmap</returns>
        public static Bitmap ShiftY(this Bitmap b, int value)
        {
            Bitmap bmp = (Bitmap)b.Clone();
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.DrawImage(b, 0, value, b.Width, b.Height);
            graphics.DrawImage(b, 0, value - b.Height, b.Width, b.Height);
            graphics.Dispose();
            return bmp;
        }
        #endregion

        #region Transparecy
        /// <summary>
        /// Sets the transparency of the bitmap. 
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <param name="value">Transparency [0, 255]</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Transparency(this Bitmap b, int value)
        {
            int width = b.Width, height = b.Height;
            Bitmap bmp = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix cmx = new ColorMatrix();
            cmx.Matrix33 = value / 255.0f;
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(cmx);
            graphics.DrawImage(b, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, attributes);
            graphics.Dispose();
            attributes.Dispose();
            return bmp;
        }
        #endregion

        #region Merge
        /// <summary>
        /// Merges two bitmaps.
        /// </summary>s
        /// <param name="background">Background bitmap</param>
        /// <param name="foreground">Foreground bitmap</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Merge(this Bitmap background, Bitmap foreground)
        {
            var rectangle = new Rectangle(0, 0, foreground.Width, foreground.Height);
            return Merge(background, foreground, rectangle, 255);
        }
        /// <summary>
        /// Merges two bitmaps.
        /// </summary>s
        /// <param name="background">Background bitmap</param>
        /// <param name="foreground">Foreground bitmap</param>
        /// <param name="rectangle">Rectangle</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Merge(this Bitmap background, Bitmap foreground, Rectangle rectangle)
        {
            return Merge(background, foreground, rectangle, 255);
        }
        /// <summary>
        /// Merges two bitmaps.
        /// </summary>s
        /// <param name="background">Background bitmap</param>
        /// <param name="foreground">Foreground bitmap</param>
        /// <param name="transparency">Transparency value [0, 255]</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Merge(this Bitmap background, Bitmap foreground, int transparency)
        {
            var rectangle = new Rectangle(0, 0, foreground.Width, foreground.Height);
            return Merge(background, foreground, rectangle, transparency);
        }
        /// <summary>
        /// Merges two bitmaps.
        /// </summary>s
        /// <param name="background">Background bitmap</param>
        /// <param name="foreground">Foreground bitmap</param>
        /// <param name="rectangle">Rectangle</param>
        /// <param name="transparency">Transparency value [0, 255]</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Merge(this Bitmap background, Bitmap foreground, Rectangle rectangle, int transparency)
        {
            using var fb_tr = Transparency(foreground, transparency);
            using var fb_re = Resize(fb_tr, rectangle.Width, rectangle.Height);
            var b_out = (Bitmap)background.Clone();
            Graphics graphics = Graphics.FromImage(b_out);
            graphics.DrawImage(fb_re, rectangle);
            graphics.Dispose();
            return b_out;
        }
        #endregion
    }
}
