﻿using System;
using UMapx.Core;

namespace UMapx.Window
{
    /// <summary>
    /// Defines the Blackman window function.
    /// </summary>
    [Serializable]
    public class Blackman : WindowBase
    {
        #region Window components
        /// <summary>
        /// Initializes the Blackman window function.
        /// </summary>
        /// <param name="frameSize">Window size</param>
        public Blackman(int frameSize)
        {
            this.FrameSize = frameSize;
        }
        /// <summary>
        /// Returns the value of a window function.
        /// </summary>
        /// <param name="x">Argument</param>
        /// <param name="frameSize">Window size</param>
        /// <returns>Double precision floating point number</returns>
        public override double Function(double x, int frameSize)
        {
            return 0.42 - 0.5 * Cosine.cosinefunc(2 * x, frameSize) + 0.08 * Cosine.cosinefunc(4 * x, frameSize);
        }
        /// <summary>
        /// Returns the window function.
        /// </summary>
        /// <param name="frameSize">Window size</param>
        /// <returns>Array</returns>
        public override double[] GetWindow(int frameSize)
        {
            double t = frameSize - 1;
            double[] x = Matrice.Compute(0, t, 1);
            return this.Function(x, frameSize);
        }
        #endregion
    }
}