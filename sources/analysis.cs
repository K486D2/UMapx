﻿// UMapx.NET framework
// Digital Signal Processing Library.
// 
// Copyright © UMapx.NET, 2015-2019
// Asiryan Valeriy
// Moscow, Russia
// Version 4.0.0

using System;
using UMapx.Decomposition;

namespace UMapx.Core
{
    // **************************************************************************
    //                            UMAPX.NET FRAMEWORK
    //                                 UMAPX.CORE
    // **************************************************************************
    // Designed by Asiryan Valeriy (c), 2015-2019
    // Moscow, Russia.
    // **************************************************************************

    #region Nonlinear solution
    /// <summary>
    /// Определяет класс, реализующий решение нелинейного уравнения.
    /// <remarks>
    /// Данный класс представляет собой решение задачи нахождения корня нелинейного уравнения вида F(x) = 0.
    /// </remarks>
    /// </summary>
    public class Nonlinear
    {
        #region Private data
        private Nonlinear.Method method;
        private double eps;
        #endregion

        #region Class components
        /// <summary>
        /// Инициализирует класс, реализующий решение нелинейного уравнения.
        /// </summary>
        /// <param name="eps">Погрешность [0, 1]</param>
        /// <param name="method">Метод решения нелинейного уравнения</param>
        public Nonlinear(double eps = 1e-8, Nonlinear.Method method = Method.Secant)
        {
            this.method = method;
            this.Eps = eps;
        }
        /// <summary>
        /// Получает или задает метод решения нелинейного уравнения.
        /// </summary>
        public Nonlinear.Method MethodType
        {
            get
            {
                return this.method;
            }
            set
            {
                this.method = value;
            }
        }
        /// <summary>
        /// Получает или задает значение погрешности [0, 1].
        /// </summary>
        public double Eps
        {
            get
            {
                return this.eps;
            }
            set
            {
                this.eps = Maths.Double(value);
            }
        }
        /// <summary>
        /// Получает значения корня нелинейного уравнения.
        /// </summary>
        /// <param name="function">Делегат непрерывной функции</param>
        /// <param name="a">Начало отрезка</param>
        /// <param name="b">Конец отрезка</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public double Compute(IDouble function, double a, double b)
        {
            // chose method of nonlinear
            switch (method)
            {
                case Method.Chord:
                    return Nonlinear.chord(function, a, b, this.eps);
                case Method.FalsePosition:
                    return Nonlinear.falpo(function, a, b, this.eps);
                case Method.Secant:
                    return Nonlinear.secan(function, a, b, this.eps);

                default:
                    return Nonlinear.bisec(function, a, b, this.eps);
            }
        }
        /// <summary>
        /// Получает значения корня нелинейного уравнения.
        /// </summary>
        /// <param name="function">Делегат непрерывной функции</param>
        /// <param name="a">Начало отрезка</param>
        /// <param name="b">Конец отрезка</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public Complex Compute(IComplex function, Complex a, Complex b)
        {
            // chose method of nonlinear
            switch (method)
            {
                case Method.Chord:
                    return Nonlinear.chord(function, a, b, this.eps);
                case Method.FalsePosition:
                    return Nonlinear.falpo(function, a, b, this.eps);

                default:
                    return Nonlinear.secan(function, a, b, this.eps);
            }
        }
        #endregion

        #region Private voids
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        private static double bisec(IDouble f, double a, double b, double eps = 1e-8)
        {
            double x1 = a; double x2 = b;
            double fb = f(b);
            double midpt;
            int n = 0;

            while (Math.Abs(x2 - x1) > eps && n < short.MaxValue)
            {
                midpt = 0.5 * (x1 + x2);

                if (fb * f(midpt) > 0)
                    x2 = midpt;
                else
                    x1 = midpt;
                n++;
            }
            return x2 - (x2 - x1) * f(x2) / (f(x2) - f(x1));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        private static double secan(IDouble f, double a, double b, double eps = 1e-8)
        {
            double x1 = a;
            double x2 = b;
            double fb = f(b);
            double mpoint;
            int n = 0;

            while (Math.Abs(f(x2)) > eps && n < short.MaxValue)
            {
                mpoint = x2 - (x2 - x1) * fb / (fb - f(x1));
                x1 = x2;
                x2 = mpoint;
                fb = f(x2);
                n++;
            }
            return x2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        private static double falpo(IDouble f, double a, double b, double eps = 1e-8)
        {
            double x1 = a;
            double x2 = b;
            double fb = f(b);
            int n = 0;

            while (Math.Abs(x2 - x1) > eps && n < short.MaxValue)
            {
                double xpoint = x2 - (x2 - x1) * f(x2) / (f(x2) - f(x1));
                if (fb * f(xpoint) > 0)
                    x2 = xpoint;
                else
                    x1 = xpoint;
                if (Math.Abs(f(xpoint)) < eps)
                    break;
                n++;
            }
            return x2 - (x2 - x1) * f(x2) / (f(x2) - f(x1));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        private static double chord(IDouble f, double a, double b, double eps = 1e-8)
        {
            int n = 0;
            double x0 = (b - a) / 2.0;
            double x;

            while (Math.Abs(f(x0) / b) > eps && n < short.MaxValue)
            {
                x = x0;
                x0 = x - (f(x) * (a - x)) / (f(a) - f(x));
                n++;
            }
            return x0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        private static Complex chord(IComplex f, Complex a, Complex b, double eps = 1e-8)
        {
            int n = 0;
            Complex x0 = (b - a) / 2.0;
            Complex x;

            while (Maths.Abs(f(x0) / b) > eps && n < short.MaxValue)
            {
                x = x0;
                x0 = x - (f(x) * (a - x)) / (f(a) - f(x));
                n++;
            }
            return x0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        private static Complex secan(IComplex f, Complex a, Complex b, double eps = 1e-8)
        {
            Complex x1 = a;
            Complex x2 = b;
            Complex fb = f(b);
            Complex mpoint;
            int n = 0;

            while (Maths.Abs(f(x2)) > eps && n < short.MaxValue)
            {
                mpoint = x2 - (x2 - x1) * fb / (fb - f(x1));
                x1 = x2;
                x2 = mpoint;
                fb = f(x2);
                n++;
            }
            return x2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        private static Complex falpo(IComplex f, Complex a, Complex b, double eps = 1e-8)
        {
            Complex x1 = a;
            Complex x2 = b;
            Complex fb = f(b);
            int n = 0;

            while (Maths.Abs(x2 - x1) > eps && n < short.MaxValue)
            {
                Complex xpoint = x2 - (x2 - x1) * f(x2) / (f(x2) - f(x1));
                Complex fxpoint = f(xpoint);
                double s = fb.Real * fxpoint.Real;

                // sign
                if (s > 0)
                    x2 = xpoint;
                else
                    x1 = xpoint;

                if (Maths.Abs(fxpoint) < eps)
                    break;
                n++;
            }
            return x2 - (x2 - x1) * f(x2) / (f(x2) - f(x1));
        }
        #endregion

        #region Enums
        /// <summary>
        /// Метод решения нелинейного уравнения.
        /// </summary>
        public enum Method
        {
            #region Methods
            /// <summary>
            /// Метод половинного деления.
            /// </summary>
            Bisection,
            /// <summary>
            /// Метод Ньютона.
            /// </summary>
            Chord,
            /// <summary>
            /// Метод секансов.
            /// </summary>
            Secant,
            /// <summary>
            /// Метод ложной точки.
            /// </summary>
            FalsePosition,
            #endregion
        }
        #endregion
    }
    #endregion

    #region Optimization methods
    /// <summary>
    /// Определяет класс, реализующий поиск экстремума.
    /// <remarks>
    /// Данный класс представляет собой решение задачи нахождения точек максимума и минимума функции F(x).
    /// </remarks>
    /// </summary>
    public class Optimization
    {
        #region Private data
        /// <summary>
        /// Погрешность вычислений.
        /// </summary>
        private double eps;
        #endregion

        #region Class components
        /// <summary>
        /// Инициализирует класс, реализующий поиск экстремума.
        /// </summary>
        /// <param name="eps">Погрешность [0, 1]</param>
        public Optimization(double eps = 1e-8)
        {
            this.Eps = eps;
        }
        /// <summary>
        /// Получает или задает значение погрешности [0, 1].
        /// </summary>
        public double Eps
        {
            get
            {
                return this.eps;
            }
            set
            {
                this.eps = Maths.Double(value);
            }
        }
        /// <summary>
        /// Возвращает соответствующий минимум функции на отреке.
        /// </summary>
        /// <param name="function">Делегат непрерывной функции</param>
        /// <param name="a">Начало отрезка</param>
        /// <param name="b">Конец отрезка</param>
        /// <param name="max">Искать максимум или минимум</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public double Compute(IDouble function, double a, double b, bool max = false)
        {
            // max or min
            return (max) ? goldenMax(function, a, b, this.eps) : goldenMin(function, a, b, this.eps);
        }
        #endregion

        #region Private voids
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        private static double goldenMin(IDouble f, double a, double b, double eps = 1e-8)
        {
            double x1, x2;

            for (int i = 0; i < short.MaxValue; i++)
            {
                x1 = b - (b - a) / Maths.Phi;
                x2 = a + (b - a) / Maths.Phi;

                if (f(x1) > f(x2))
                    a = x1;
                else
                    b = x2;
                if (Math.Abs(b - a) < eps)
                    break;
            }
            return (a + b) / 2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        private static double goldenMax(IDouble f, double a, double b, double eps = 1e-8)
        {
            double x1, x2;

            for (int i = 0; i < short.MaxValue; i++)
            {
                x1 = b - (b - a) / Maths.Phi;
                x2 = a + (b - a) / Maths.Phi;

                if (f(x1) < f(x2))
                    a = x1;
                else
                    b = x2;
                if (Math.Abs(b - a) < eps)
                    break;
            }
            return (a + b) / 2;
        }
        #endregion
    }
    #endregion

    #region Integral solution
    /// <summary>
    /// Определяет класс, реализующий численное интегрирование.
    /// <remarks>
    /// Данный класс представляет собой решение задачи нахождения значение интеграла функции F(x) в пределах значений a и b.
    /// </remarks>
    /// </summary>
    public class Integration
    {
        #region Private data
        private Integration.Method method;
        #endregion

        #region Class components
        /// <summary>
        /// Инициализирует класс, реализующий численное интегрирование.
        /// </summary>
        /// <param name="method">Метод интегрирования</param>
        public Integration(Integration.Method method = Method.Rectangle)
        {
            this.method = method;
        }
        /// <summary>
        /// Получает или задает метод интегрирования.
        /// </summary>
        public Integration.Method MethodType
        {
            get
            {
                return this.method;
            }
            set
            {
                this.method = value;
            }
        }
        /// <summary>
        /// Возвращает значение интеграла функции.
        /// </summary>
        /// <param name="function">Делегат непрерывной функции</param>
        /// <param name="a">Нижний предел</param>
        /// <param name="b">Верхний предел</param>
        /// <param name="n">Количество разбиений</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public double Compute(IDouble function, double a, double b, int n)
        {
            // chose method of integration
            switch (method)
            {
                case Method.Midpoint:
                    return Integration.midp(function, a, b, n);

                case Method.Trapezoidal:
                    return Integration.trap(function, a, b, n);

                case Method.Simpson:
                    return Integration.simp(function, a, b, n);

                case Method.Romberg:
                    return Integration.romb(function, a, b, n);

                default:
                    return Integration.rect(function, a, b, n);
            }
        }
        /// <summary>
        /// Возвращает значение интеграла функции.
        /// </summary>
        /// <param name="y">Вектор значений функции</param>
        /// <param name="a">Нижний предел</param>
        /// <param name="b">Верхний предел</param>
        /// <param name="n">Количество разбиений</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public double Compute(double[] y, double a, double b, int n)
        {
            // chose method of integration
            switch (method)
            {
                case Method.Midpoint:
                    return Integration.midp(y, a, b, n);

                case Method.Trapezoidal:
                    return Integration.trap(y, a, b, n);

                case Method.Simpson:
                    return Integration.simp(y, a, b, n);

                default:
                    return Integration.rect(y, a, b, n);
            }
        }
        /// <summary>
        /// Возвращает значение интеграла функции.
        /// </summary>
        /// <param name="function">Делегат непрерывной функции</param>
        /// <param name="a">Нижний предел</param>
        /// <param name="b">Верхний предел</param>
        /// <param name="n">Количество разбиений</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public Complex Compute(IComplex function, Complex a, Complex b, int n)
        {
            // chose method of integration
            switch (method)
            {
                case Method.Midpoint:
                    return Integration.midp(function, a, b, n);

                case Method.Trapezoidal:
                    return Integration.trap(function, a, b, n);

                case Method.Simpson:
                    return Integration.simp(function, a, b, n);

                case Method.Romberg:
                    return Integration.romb(function, a, b, n);

                default:
                    return Integration.rect(function, a, b, n);
            }
        }
        /// <summary>
        /// Возвращает значение интеграла функции.
        /// </summary>
        /// <param name="y">Вектор значений функции</param>
        /// <param name="a">Нижний предел</param>
        /// <param name="b">Верхний предел</param>
        /// <param name="n">Количество разбиений</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public Complex Compute(Complex[] y, Complex a, Complex b, int n)
        {
            // chose method of integration
            switch (method)
            {
                case Method.Midpoint:
                    return Integration.midp(y, a, b, n);

                case Method.Trapezoidal:
                    return Integration.trap(y, a, b, n);

                case Method.Simpson:
                    return Integration.simp(y, a, b, n);

                default:
                    return Integration.rect(y, a, b, n);
            }
        }
        #endregion

        #region Private voids
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double rect(IDouble f, double a, double b, int n)
        {
            double sum = 0.0;
            double h = (b - a) / n;
            for (int i = 0; i < n; i++)
            {
                sum += h * f(a + i * h);
            }
            return sum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double rect(double[] y, double a, double b, int n)
        {
            double sum = 0.0;
            double h = (b - a) / n;
            for (int i = 0; i < n; i++)
            {
                sum += h * y[i];
            }
            return sum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double midp(IDouble f, double a, double b, int n)
        {
            // Midpoint
            double sum = 0.0;
            double h = (b - a) / n;
            for (int i = 0; i < n; i++)
            {
                sum += h * f(a + (i + 0.5) * h);
            }
            return sum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double midp(double[] y, double a, double b, int n)
        {
            double sum = 0.0;
            double h = (b - a) / (n - 1);
            for (int i = 0; i < (n - 1); i++)
            {
                sum += h * 0.5 * (y[i] + y[i + 1]);
            }
            return sum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double trap(IDouble f, double a, double b, int n)
        {
            double sum = 0.0;
            double h = (b - a) / n;
            for (int i = 0; i < n; i++)
            {
                sum += 0.5 * h * (f(a + i * h) + f(a + (i + 1) * h));
            }
            return sum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double trap(double[] y, double a, double b, int n)
        {
            double sum = 0.0;
            double h = (b - a) / (n - 1);
            for (int i = 0; i < (n - 1); i++)
            {
                sum += 0.5 * h * (y[i] + y[i + 1]);
            }
            return sum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double simp(IDouble f, double a, double b, int n)
        {
            if (n < 3) return double.NaN; //Need at least 3 points
            double sum = 0.0;
            double h = (b - a) / n;
            if (n % 2 != 0)
            {
                for (int i = 0; i < n - 1; i += 2)
                {
                    sum += h * (f(a + i * h) + 4 * f(a + (i + 1) * h) + f(a + (i + 2) * h)) / 3;
                }
            }
            else
            {
                sum = 3 * h * (f(a) + 3 * f(a + h) + 3 * f(a + 2 * h) + f(a + 3 * h)) / 8;
                for (int i = 3; i < n - 1; i += 2)
                {
                    sum += h * (f(a + i * h) + 4 * f(a + (i + 1) * h) + f(a + (i + 2) * h)) / 3;
                }
            }
            return sum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double simp(double[] y, double a, double b, int n)
        {
            double h = (b - a) / n;
            //Need at least 3 points
            if (n < 3 || h == 0) return double.NaN;
            double sum = 0.0;
            if (n % 2 != 0)
            {
                for (int i = 0; i < n - 1; i += 2)
                {
                    sum += h * (y[i] + 4 * y[i + 1] + y[i + 2]) / 3;
                }
            }
            else
            {
                sum = 3 * h * (y[0] + 3 * y[1] + 3 * y[2] + y[3]) / 8;
                for (int i = 3; i < n - 1; i += 2)
                {
                    sum += h * (y[i] + 4 * y[i + 1] + y[i + 2]) / 3;
                }
            }
            return sum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="iterations"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        private static double romb(IDouble f, double a, double b, int iterations, double eps = 1e-8)
        {
            int n = 2;
            double h = b - a;
            double sum = 0.0;
            int j = 0;
            double[,] R = new double[iterations, iterations];
            R[1, 1] = h * (f(a) + f(b)) / 2.0;
            h = h / 2;
            R[2, 1] = R[1, 1] / 2 + h * f(a + h);
            R[2, 2] = (4 * R[2, 1] - R[1, 1]) / 3;
            for (j = 3; j <= iterations; j++)
            {
                n = 2 * n;
                h = h / 2;
                sum = 0.0;
                for (int k = 1; k <= n; k += 2)
                {
                    sum += f(a + k * h);
                }
                R[j, 1] = R[j - 1, 1] / 2 + h * sum;
                double factor = 4.0;
                for (int k = 2; k <= j; k++)
                {
                    R[j, k] = (factor * R[j, k - 1] - R[j - 1, k - 1]) / (factor - 1);
                    factor = factor * 4.0;
                }
                if (Math.Abs(R[j, j] - R[j, j - 1]) < eps * Math.Abs(R[j, j]))
                {
                    sum = R[j, j];
                    return sum;
                }
            }
            sum = R[n, n];
            return sum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Complex rect(IComplex f, Complex a, Complex b, int n)
        {
            Complex sum = 0.0;
            Complex h = (b - a) / n;
            for (int i = 0; i < n; i++)
            {
                sum += h * f(a + i * h);
            }
            return sum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Complex rect(Complex[] y, Complex a, Complex b, int n)
        {
            Complex sum = 0.0;
            Complex h = (b - a) / n;
            for (int i = 0; i < n; i++)
            {
                sum += h * y[i];
            }
            return sum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Complex midp(IComplex f, Complex a, Complex b, int n)
        {
            // Midpoint
            Complex sum = 0.0;
            Complex h = (b - a) / n;
            for (int i = 0; i < n; i++)
            {
                sum += h * f(a + (i + 0.5) * h);
            }
            return sum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Complex midp(Complex[] y, Complex a, Complex b, int n)
        {
            Complex sum = 0.0;
            Complex h = (b - a) / (n - 1);
            for (int i = 0; i < (n - 1); i++)
            {
                sum += h * 0.5 * (y[i] + y[i + 1]);
            }
            return sum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Complex trap(IComplex f, Complex a, Complex b, int n)
        {
            Complex sum = 0.0;
            Complex h = (b - a) / n;
            for (int i = 0; i < n; i++)
            {
                sum += 0.5 * h * (f(a + i * h) + f(a + (i + 1) * h));
            }
            return sum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Complex trap(Complex[] y, Complex a, Complex b, int n)
        {
            Complex sum = 0.0;
            Complex h = (b - a) / (n - 1);
            for (int i = 0; i < (n - 1); i++)
            {
                sum += 0.5 * h * (y[i] + y[i + 1]);
            }
            return sum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Complex simp(IComplex f, Complex a, Complex b, int n)
        {
            if (n < 3) return double.NaN; //Need at least 3 points
            Complex sum = 0.0;
            Complex h = (b - a) / n;
            if (n % 2 != 0)
            {
                for (int i = 0; i < n - 1; i += 2)
                {
                    sum += h * (f(a + i * h) + 4 * f(a + (i + 1) * h) + f(a + (i + 2) * h)) / 3;
                }
            }
            else
            {
                sum = 3 * h * (f(a) + 3 * f(a + h) + 3 * f(a + 2 * h) + f(a + 3 * h)) / 8;
                for (int i = 3; i < n - 1; i += 2)
                {
                    sum += h * (f(a + i * h) + 4 * f(a + (i + 1) * h) + f(a + (i + 2) * h)) / 3;
                }
            }
            return sum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static Complex simp(Complex[] y, Complex a, Complex b, int n)
        {
            Complex h = (b - a) / n;
            //Need at least 3 points
            if (n < 3 || h == 0) return double.NaN;
            Complex sum = 0.0;
            if (n % 2 != 0)
            {
                for (int i = 0; i < n - 1; i += 2)
                {
                    sum += h * (y[i] + 4 * y[i + 1] + y[i + 2]) / 3;
                }
            }
            else
            {
                sum = 3 * h * (y[0] + 3 * y[1] + 3 * y[2] + y[3]) / 8;
                for (int i = 3; i < n - 1; i += 2)
                {
                    sum += h * (y[i] + 4 * y[i + 1] + y[i + 2]) / 3;
                }
            }
            return sum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="iterations"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        private static Complex romb(IComplex f, Complex a, Complex b, int iterations, double eps = 1e-8)
        {
            int n = 2;
            Complex h = b - a;
            Complex sum = 0.0;
            int j = 0;
            Complex[,] R = new Complex[iterations, iterations];
            R[1, 1] = h * (f(a) + f(b)) / 2.0;
            h = h / 2;
            R[2, 1] = R[1, 1] / 2 + h * f(a + h);
            R[2, 2] = (4 * R[2, 1] - R[1, 1]) / 3;
            for (j = 3; j <= iterations; j++)
            {
                n = 2 * n;
                h = h / 2;
                sum = 0.0;
                for (int k = 1; k <= n; k += 2)
                {
                    sum += f(a + k * h);
                }
                R[j, 1] = R[j - 1, 1] / 2 + h * sum;
                double factor = 4.0;
                for (int k = 2; k <= j; k++)
                {
                    R[j, k] = (factor * R[j, k - 1] - R[j - 1, k - 1]) / (factor - 1);
                    factor = factor * 4.0;
                }
                if (Maths.Abs(R[j, j] - R[j, j - 1]) < eps * Maths.Abs(R[j, j]))
                {
                    sum = R[j, j];
                    return sum;
                }
            }
            sum = R[n, n];
            return sum;
        }
        #endregion

        #region Enums
        /// <summary>
        /// Метод интегрирования.
        /// </summary>
        public enum Method
        {
            #region Methods
            /// <summary>
            /// Метод прямоугольников.
            /// </summary>
            Rectangle,
            /// <summary>
            /// Метод средней точки.
            /// </summary>
            Midpoint,
            /// <summary>
            /// Метод трапеций.
            /// </summary>
            Trapezoidal,
            /// <summary>
            /// Метод Симпсона.
            /// </summary>
            Simpson,
            /// <summary>
            /// Метод Ромберга.
            /// </summary>
            Romberg,
            #endregion
        }
        #endregion
    }
    #endregion

    #region Numeric differentiation
    /// <summary>
    /// Определяет класс, реализующий численное дифференцирование.
    /// </summary>
    public class Differentation
    {
        #region Private data
        private int points;
        #endregion

        #region Components
        /// <summary>
        /// Инициализирует класс, реализующий численное дифференцирование.
        /// </summary>
        /// <param name="points">Количество точек интерполяции</param>
        public Differentation(int points)
        {
            this.Points = points;
        }
        /// <summary>
        /// Получает или задает количество точек интерполяции. 
        /// </summary>
        public int Points
        {
            get
            {
                return this.points;
            }
            set
            {
                if (value < 0)
                    throw new Exception("Неверное значение аргумента");

                this.points = value;
            }
        }
        /// <summary>
        /// Возвращает значение производной функции.
        /// </summary>
        /// <param name="function">Делегат непрерывной функции</param>
        /// <param name="x">Значение аргумента</param>
        /// <param name="h">Значение шага</param>
        /// <param name="order">Порядок производной</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public double Compute(IDouble function, double x, double h, int order)
        {
            // exception
            if (order > this.points)
                throw new Exception("Порядок производной не может быть больше количества точек интерполяции");
            if (order < 0)
                throw new Exception("Порядок производной не может меньше 0");

            // Create the interpolation points
            int length = this.points + 1;
            double[,] coefficients = Differentation.GetCoefficients(length);
            double sum = 0.0;

            // do job
            for (int i = 0, center = 0; i < length; i++)
            {
                sum += coefficients[order, i] * function(x + center * h);
                center++;
            }

            // result
            return sum / Math.Pow(h, order);
        }
        /// <summary>
        /// Возвращает значение производной функции.
        /// </summary>
        /// <param name="y">Массив значений функции</param>
        /// <param name="index">Индекс аргумента</param>
        /// <param name="h">Значение шага</param>
        /// <param name="order">Порядок производной</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public double Compute(double[] y, int index, double h, int order)
        {
            // exception
            if (order > this.points)
                throw new Exception("Порядок производной не может быть больше количества точек интерполяции");
            if (order < 0)
                throw new Exception("Порядок производной не может меньше 0");

            // Create the interpolation points
            int length = this.points + 1;
            double[,] coefficients = Differentation.GetCoefficients(length);
            double sum = 0.0;

            // do job
            for (int i = 0, center = 0; i < length; i++)
            {
                sum += coefficients[order, i] * y[index + i];
                center++;
            }

            // result
            return sum / Math.Pow(h, order);
        }
        /// <summary>
        /// Возвращает значение производной функции.
        /// </summary>
        /// <param name="function">Делегат непрерывной функции</param>
        /// <param name="x">Значение аргумента</param>
        /// <param name="h">Значение шага</param>
        /// <param name="order">Порядок производной</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public Complex Compute(IComplex function, Complex x, Complex h, int order)
        {
            // exception
            if (order > this.points)
                throw new Exception("Порядок производной не может быть больше количества точек интерполяции");
            if (order < 0)
                throw new Exception("Порядок производной не может меньше 0");

            // Create the interpolation points
            int length = this.points + 1;
            double[,] coefficients = Differentation.GetCoefficients(length);
            Complex sum = 0.0;

            // do job
            for (int i = 0, center = 0; i < length; i++)
            {
                sum += coefficients[order, i] * function(x + center * h);
                center++;
            }

            // result
            return sum / Maths.Pow(h, order);
        }
        /// <summary>
        /// Возвращает значение производной функции.
        /// </summary>
        /// <param name="y">Массив значений функции</param>
        /// <param name="index">Индекс аргумента</param>
        /// <param name="h">Значение шага</param>
        /// <param name="order">Порядок производной</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public Complex Compute(Complex[] y, int index, double h, int order)
        {
            // exception
            if (order > this.points)
                throw new Exception("Порядок производной не может быть больше количества точек интерполяции");
            if (order < 0)
                throw new Exception("Порядок производной не может меньше 0");

            // Create the interpolation points
            int length = this.points + 1;
            double[,] coefficients = Differentation.GetCoefficients(length);
            Complex sum = 0.0;

            // do job
            for (int i = 0, center = 0; i < length; i++)
            {
                sum += coefficients[order, i] * y[index + i];
                center++;
            }

            // result
            return sum / Math.Pow(h, order);
        }
        #endregion

        #region Static voids
        /// <summary>
        /// Возвращает матрицу коэффициентов интерполяции.
        /// </summary>
        /// <param name="points">Количество точек</param>
        /// <returns>Матрица</returns>
        public static double[,] GetCoefficients(int points)
        {
            // Compute difference coefficient table
            double fac = Special.Factorial(points);
            double[,] deltas = new double[points, points];
            double h, delta;
            int j, k;

            // do job
            for (j = 0; j < points; j++)
            {
                h = 1.0;
                delta = j;

                for (k = 0; k < points; k++)
                {
                    deltas[j, k] = h / Special.Factorial(k);
                    h *= delta;
                }
            }

            // matrix invert
            deltas = Matrice.Invert(deltas);

            //// rounding
            //for (j = 0; j < points; j++)
            //    for (k = 0; k < points; k++)
            //        deltas[j, k] = (Math.Round(deltas[j, k] * fac, MidpointRounding.AwayFromZero)) / fac;

            return deltas;
        }
        #endregion
    }
    #endregion

    #region Differential equation solution
    /// <summary>
    /// Определяет класс, реализующий решение дифференциального уравнения.
    /// <remarks>
    /// Данный класс представляет собой решение задачи Коши для обыкновенного дифференциального уравнения y' = F(x, y).
    /// </remarks>
    /// </summary>
    public class Differential
    {
        #region Private data
        private Differential.Method method;
        #endregion

        #region Diferentiation components
        /// <summary>
        /// Инициализирует класс, реализующий решение дифференциального уравнения.
        /// </summary>
        /// <param name="method">Метод дифференцирования</param>
        public Differential(Differential.Method method = Method.RungeKutta4)
        {
            this.method = method;
        }
        /// <summary>
        /// Получает или задает метод дифференцирования.
        /// </summary>
        public Differential.Method MethodType
        {
            get
            {
                return this.method;
            }
            set
            {
                this.method = value;
            }
        }
        /// <summary>
        /// Возвращает значение дифференциального уравнения.
        /// </summary>
        /// <param name="function">Делегат непрерывной функции, зависящей от двух переменных</param>
        /// <param name="x">Массив значений аргумент</param>
        /// <param name="y0">Значение</param>
        /// <returns>Массив значений функции</returns>
        public double[] Compute(IDoubleMesh function, double[] x, double y0)
        {
            // chose method of differentiation
            switch (method)
            {
                case Method.Euler:
                    return Differential.euler(function, x, y0);

                case Method.Fehlberg:
                    return Differential.fehlberg(function, x, y0);

                case Method.RungeKutta4:
                    return Differential.rungeKutta4(function, x, y0);

                default:
                    return Differential.rungeKutta2(function, x, y0);
            }
        }
        /// <summary>
        /// Возвращает значение дифференциального уравнения.
        /// </summary>
        /// <param name="function">Делегат непрерывной функции, зависящей от двух переменных</param>
        /// <param name="x">Массив значений аргумент</param>
        /// <param name="y0">Значение</param>
        /// <returns>Массив значений функции</returns>
        public Complex[] Compute(IComplexMesh function, Complex[] x, Complex y0)
        {
            // chose method of differentiation
            switch (method)
            {
                case Method.Euler:
                    return Differential.euler(function, x, y0);

                case Method.Fehlberg:
                    return Differential.fehlberg(function, x, y0);

                case Method.RungeKutta4:
                    return Differential.rungeKutta4(function, x, y0);

                default:
                    return Differential.rungeKutta2(function, x, y0);
            }
        }
        #endregion

        #region Recompute voids
        /// <summary>
        /// Возвращает значение дифференциального уравнения, вычисленное по методу Адамса-Башфорта.
        /// </summary>
        /// <param name="function">Делегат непрерывной функции, зависящей от двух переменных</param>
        /// <param name="x">Массив значений аргумент</param>
        /// <param name="y0">Значение</param>
        /// <param name="order">Порядок метода</param>
        /// <returns>Массив значений функции</returns>
        public double[] Compute(IDoubleMesh function, double[] x, double y0, int order = 2)
        {
            int n = x.Length - 1;

            // if order more than 1
            // Adams-Bashfort method
            if (order > 1 && order < n)
            {
                // params
                int i, j, k = order + 1;
                double[] y = new double[n];
                double[] r = new double[k];
                double[] c = Differential.GetCoefficients(order);
                double h, t, sum;

                // compute first points by order
                for (i = 0; i < k; i++)
                    r[i] = x[i];

                // classic differential
                r = this.Compute(function, r, y0);

                for (i = 0; i < order; i++)
                    y[i] = r[i];

                // Adams-Bashforth method
                // for order
                for (i = order; i < n; i++)
                {
                    sum = y[i - 1];

                    for (j = 0; j < order; j++)
                    {
                        t = x[i - j];
                        h = t - x[i - j - 1];
                        sum += h * c[j] * function(t, y[i - j - 1]);
                    }

                    y[i] = sum;
                }

                return y;
            }

            // classic differential
            return this.Compute(function, x, y0);
        }
        /// <summary>
        /// Возвращает значение дифференциального уравнения, вычисленное по методу Адамса-Башфорта.
        /// </summary>
        /// <param name="function">Делегат непрерывной функции, зависящей от двух переменных</param>
        /// <param name="x">Массив значений аргумент</param>
        /// <param name="y0">Значение</param>
        /// <param name="order">Порядок метода</param>
        /// <returns>Массив значений функции</returns>
        public Complex[] Compute(IComplexMesh function, Complex[] x, Complex y0, int order = 2)
        {
            int n = x.Length - 1;

            // if order more than 1
            // Adams-Bashfort method
            if (order > 1 && order < n)
            {
                // params
                int i, j, k = order + 1;
                Complex[] y = new Complex[n];
                Complex[] r = new Complex[k];
                double[] c = Differential.GetCoefficients(order);
                Complex h, t, sum;

                // compute first points by order
                for (i = 0; i < k; i++)
                    r[i] = x[i];

                // classic differential
                r = this.Compute(function, r, y0);

                for (i = 0; i < order; i++)
                    y[i] = r[i];

                // Adams-Bashforth method
                // for order
                for (i = order; i < n; i++)
                {
                    sum = y[i - 1];

                    for (j = 0; j < order; j++)
                    {
                        t = x[i - j];
                        h = t - x[i - j - 1];
                        sum += h * c[j] * function(t, y[i - j - 1]);
                    }

                    y[i] = sum;
                }

                return y;
            }

            // classic differential
            return this.Compute(function, x, y0);
        }
        #endregion

        #region Adams-Bashforth
        /// <summary>
        /// Возвращает массив значений коэффициентов для формулы Адамса-Башфорта.
        /// </summary>
        /// <param name="order">Порядок</param>
        /// <returns>Одномерный массив</returns>
        public static double[] GetCoefficients(int order)
        {
            double[,] A = new double[order, order];
            double[] c = new double[order];
            int i, j;

            for (i = 0; i < order; i++)
            {
                for (j = 0; j < order; j++)
                {
                    A[i, j] = Math.Pow(j, i);
                }
                c[i] = Math.Pow(-1, i) / (i + 1);
            }

            return A.Solve(c);
            //return c.Dot(A.Invert());
        }
        #endregion

        #region Runge-Kutta
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x"></param>
        /// <param name="y0"></param>
        /// <returns></returns>
        private static double[] euler(IDoubleMesh f, double[] x, double y0)
        {
            int n = x.Length - 1;
            double xnew, ynew = y0, h;
            double[] result = new double[n];

            for (int i = 0; i < n; i++)
            {
                h = x[i + 1] - x[i];
                xnew = x[i];
                ynew = ynew + f(xnew, ynew) * h;
                result[i] = ynew;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x"></param>
        /// <param name="y0"></param>
        /// <returns></returns>
        private static double[] rungeKutta2(IDoubleMesh f, double[] x, double y0)
        {
            int n = x.Length - 1;
            double xnew, ynew = y0, h, k1, k2;
            double[] result = new double[n];

            for (int i = 0; i < n; i++)
            {
                h = x[i + 1] - x[i];
                xnew = x[i];
                k1 = h * f(xnew, ynew);
                k2 = h * f(xnew + 0.5 * h, ynew + 0.5 * k1);
                ynew = ynew + k2;
                xnew = xnew + h;
                result[i] = ynew;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x"></param>
        /// <param name="y0"></param>
        /// <returns></returns>
        private static double[] rungeKutta4(IDoubleMesh f, double[] x, double y0)
        {
            int n = x.Length - 1;
            double xnew, ynew = y0, h, k1, k2, k3, k4;
            double[] result = new double[n];

            for (int i = 0; i < n; i++)
            {
                h = x[i + 1] - x[i];
                xnew = x[i];
                k1 = h * f(xnew, ynew);
                k2 = h * f(xnew + 0.5 * h, ynew + 0.5 * k1);
                k3 = h * f(xnew + 0.5 * h, ynew + 0.5 * k2);
                k4 = h * f(xnew + h, ynew + k3);
                ynew = ynew + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                xnew = xnew + h;
                result[i] = ynew;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x"></param>
        /// <param name="y0"></param>
        /// <returns></returns>
        private static double[] fehlberg(IDoubleMesh f, double[] x, double y0)
        {
            int n = x.Length - 1;
            double xnew, ynew = y0, h, k1, k2, k3, k4, k5, k6;
            double[] result = new double[n];

            for (int i = 0; i < n; i++)
            {
                h = x[i + 1] - x[i];
                xnew = x[i];
                k1 = h * f(xnew, ynew);
                k2 = h * f(xnew + 0.25 * h, ynew + 0.25 * k1);
                k3 = h * f(xnew + 3 * h / 8, ynew + 3 * k1 / 32 + 9 * k2 / 32);
                k4 = h * f(xnew + 12 * h / 13, ynew + 1932 * k1 / 2197 - 7200 * k2 / 2197 + 7296 * k3 / 2197);
                k5 = h * f(xnew + h, ynew + 439 * k1 / 216 - 8 * k2 + 3680 * k3 / 513 - 845 * k4 / 4104);
                k6 = h * f(xnew + 0.5 * h, ynew - 8 * k1 / 27 + 2 * k2 - 3544 * k3 / 2565 + 1859 * k4 / 4104 - 11 * k5 / 40);
                ynew = ynew + 25 * k1 / 216 + 1408 * k3 / 2565 + 2197 * k4 / 4104 - 0.2 * k5;
                xnew = xnew + h;
                result[i] = ynew;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x"></param>
        /// <param name="y0"></param>
        /// <returns></returns>
        private static Complex[] euler(IComplexMesh f, Complex[] x, Complex y0)
        {
            int n = x.Length - 1;
            Complex xnew, ynew = y0, h;
            Complex[] result = new Complex[n];

            for (int i = 0; i < n; i++)
            {
                h = x[i + 1] - x[i];
                xnew = x[i];
                ynew = ynew + f(xnew, ynew) * h;
                result[i] = ynew;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x"></param>
        /// <param name="y0"></param>
        /// <returns></returns>
        private static Complex[] rungeKutta2(IComplexMesh f, Complex[] x, Complex y0)
        {
            int n = x.Length - 1;
            Complex xnew, ynew = y0, h, k1, k2;
            Complex[] result = new Complex[n];

            for (int i = 0; i < n; i++)
            {
                h = x[i + 1] - x[i];
                xnew = x[i];
                k1 = h * f(xnew, ynew);
                k2 = h * f(xnew + 0.5 * h, ynew + 0.5 * k1);
                ynew = ynew + k2;
                xnew = xnew + h;
                result[i] = ynew;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x"></param>
        /// <param name="y0"></param>
        /// <returns></returns>
        private static Complex[] rungeKutta4(IComplexMesh f, Complex[] x, Complex y0)
        {
            int n = x.Length - 1;
            Complex xnew, ynew = y0, h, k1, k2, k3, k4;
            Complex[] result = new Complex[n];

            for (int i = 0; i < n; i++)
            {
                h = x[i + 1] - x[i];
                xnew = x[i];
                k1 = h * f(xnew, ynew);
                k2 = h * f(xnew + 0.5 * h, ynew + 0.5 * k1);
                k3 = h * f(xnew + 0.5 * h, ynew + 0.5 * k2);
                k4 = h * f(xnew + h, ynew + k3);
                ynew = ynew + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                xnew = xnew + h;
                result[i] = ynew;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x"></param>
        /// <param name="y0"></param>
        /// <returns></returns>
        private static Complex[] fehlberg(IComplexMesh f, Complex[] x, Complex y0)
        {
            int n = x.Length - 1;
            Complex xnew, ynew = y0, h, k1, k2, k3, k4, k5, k6;
            Complex[] result = new Complex[n];

            for (int i = 0; i < n; i++)
            {
                h = x[i + 1] - x[i];
                xnew = x[i];
                k1 = h * f(xnew, ynew);
                k2 = h * f(xnew + 0.25 * h, ynew + 0.25 * k1);
                k3 = h * f(xnew + 3 * h / 8, ynew + 3 * k1 / 32 + 9 * k2 / 32);
                k4 = h * f(xnew + 12 * h / 13, ynew + 1932 * k1 / 2197 - 7200 * k2 / 2197 + 7296 * k3 / 2197);
                k5 = h * f(xnew + h, ynew + 439 * k1 / 216 - 8 * k2 + 3680 * k3 / 513 - 845 * k4 / 4104);
                k6 = h * f(xnew + 0.5 * h, ynew - 8 * k1 / 27 + 2 * k2 - 3544 * k3 / 2565 + 1859 * k4 / 4104 - 11 * k5 / 40);
                ynew = ynew + 25 * k1 / 216 + 1408 * k3 / 2565 + 2197 * k4 / 4104 - 0.2 * k5;
                xnew = xnew + h;
                result[i] = ynew;
            }
            return result;
        }
        #endregion

        #region Enums
        /// <summary>
        /// Метод дифференцирования.
        /// </summary>
        public enum Method
        {
            #region Methods
            /// <summary>
            /// Метод Эйлера.
            /// </summary>
            Euler,
            /// <summary>
            /// Метод Рунге-Кутты второго порядка.
            /// </summary>
            RungeKutta2,
            /// <summary>
            /// Метод Рунге-Кутты четвертого порядка.
            /// </summary>
            RungeKutta4,
            /// <summary>
            /// Метод Фелберга.
            /// </summary>
            Fehlberg,
            #endregion
        }
        #endregion
    }
    #endregion

    #region Interpolation methods
    /// <summary>
    /// Определяет класс, реализующий интерполяцию.
    /// <remarks>
    /// Данный класс представляет собой решение задачи нахождения промежуточного значение функции F(x).
    /// </remarks>
    /// </summary>
    public class Interpolation
    {
        #region Private data
        private Interpolation.Method method;
        #endregion

        #region Class components
        /// <summary>
        /// Инициализирует класс, реализующий интерполяцию.
        /// </summary>
        /// <param name="method">Метод интерполяции</param>
        public Interpolation(Interpolation.Method method = Method.Lagrange)
        {
            this.method = method;
        }
        /// <summary>
        /// Получает или задает метод интерполяции.
        /// </summary>
        public Interpolation.Method MethodType
        {
            get
            {
                return this.method;
            }
            set
            {
                this.method = value;
            }
        }
        /// <summary>
        /// Возвращает значение функции в точке.
        /// <remarks>
        /// В данном случае используется только билинейная интерполяция.
        /// </remarks>
        /// </summary>
        /// <param name="x">Массив значений первого аргумента</param>
        /// <param name="y">Массив значений второго аргумента</param>
        /// <param name="z">Матрица значений функции</param>
        /// <param name="xl">Значение первого аргумента для вычисления</param>
        /// <param name="yl">Значение второго аргумента для вычисления</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public double Compute(double[] x, double[] y, double[,] z, double xl, double yl)
        {
            return bilinear(x, y, z, xl, yl);
        }
        /// <summary>
        /// Возвращает значение функции в точке.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="y">Массив значений функции</param>
        /// <param name="xl">Значение аргумента для вычисления</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public double Compute(double[] x, double[] y, double xl)
        {
            // chose method of interpolation
            switch (method)
            {
                case Method.Lagrange:
                    return Interpolation.lagra(x, y, xl);

                case Method.Newton:
                    return Interpolation.newto(x, y, xl);

                case Method.Barycentric:
                    return Interpolation.baryc(x, y, xl);

                default:
                    return Interpolation.linear(x, y, xl);
            }
        }
        /// <summary>
        /// Возвращает значение функции в точке.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="y">Массив значений функции</param>
        /// <param name="xl">Значение аргумента для вычисления</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public Complex Compute(Complex[] x, Complex[] y, Complex xl)
        {
            // chose method of interpolation
            switch (method)
            {
                case Method.Newton:
                    return Interpolation.newto(x, y, xl);

                case Method.Barycentric:
                    return Interpolation.baryc(x, y, xl);

                default:
                    return Interpolation.lagra(x, y, xl);
            }
        }
        #endregion

        #region Private voids
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="xl"></param>
        /// <returns></returns>
        private static double linear(double[] x, double[] y, double xl)
        {
            double yval = 0.0;
            int length = x.Length - 1;

            for (int i = 0; i < length; i++)
            {
                if (xl >= x[i] && xl < x[i + 1])
                {
                    yval = y[i] + (xl - x[i]) * (y[i + 1] - y[i]) / (x[i + 1] - x[i]);
                }
            }
            return yval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        /// <returns></returns>
        private static double bilinear(double[] x, double[] y, double[,] z, double xval, double yval)
        {
            double zval = 0.0;
            int xlength = x.Length - 1;
            int ylength = y.Length - 1;

            for (int i = 0; i < xlength; i++)
            {
                for (int j = 0; j < ylength; j++)
                {
                    if (xval >= x[i] && xval < x[i + 1] && yval >= y[j] && yval < y[j + 1])
                    {
                        zval = z[i, j] * (x[i + 1] - xval) * (y[j + 1] - yval) / (x[i + 1] - x[i]) / (y[j + 1] - y[j]) +
                        z[i + 1, j] * (xval - x[i]) * (y[j + 1] - yval) / (x[i + 1] - x[i]) / (y[j + 1] - y[j]) +
                        z[i, j + 1] * (x[i + 1] - xval) * (yval - y[j]) / (x[i + 1] - x[i]) / (y[j + 1] - y[j]) +
                        z[i + 1, j + 1] * (xval - x[i]) * (yval - y[j]) / (x[i + 1] - x[i]) / (y[j + 1] - y[j]);
                    }
                }
            }
            return zval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="xval"></param>
        /// <returns></returns>
        private static double lagra(double[] x, double[] y, double xval)
        {
            double yval = 0.0;
            double Products = y[0];
            int length = x.Length;
            int i, j;

            for (i = 0; i < length; i++)
            {
                Products = y[i];
                for (j = 0; j < length; j++)
                {
                    if (i != j)
                    {
                        Products *= (xval - x[j]) / (x[i] - x[j]);
                    }
                }
                yval += Products;
            }
            return yval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="xval"></param>
        /// <returns></returns>
        private static double newto(double[] x, double[] y, double xval)
        {
            double yval;
            int length = x.Length;
            double[] tarray = new double[length];
            int i, j;

            for (i = 0; i < length; i++)
            {
                tarray[i] = y[i];
            }
            for (i = 0; i < length - 1; i++)
            {
                for (j = length - 1; j > i; j--)
                {
                    tarray[j] = (tarray[j - 1] - tarray[j]) / (x[j - 1 - i] - x[j]);
                }
            }
            yval = tarray[length - 1];
            for (i = length - 2; i >= 0; i--)
            {
                yval = tarray[i] + (xval - x[i]) * yval;
            }
            return yval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="xval"></param>
        /// <returns></returns>
        private static double baryc(double[] x, double[] y, double xval)
        {
            double product;
            double deltaX;
            double bc1 = 0;
            double bc2 = 0;
            int length = x.Length;
            double[] weights = new double[length];
            int i, j;

            for (i = 0; i < length; i++)
            {
                product = 1;
                for (j = 0; j < length; j++)
                {
                    if (i != j)
                    {
                        product *= (x[i] - x[j]);
                        weights[i] = 1.0 / product;
                    }
                }
            }

            for (i = 0; i < length; i++)
            {
                deltaX = weights[i] / (xval - x[i]);
                bc1 += y[i] * deltaX;
                bc2 += deltaX;
            }
            return bc1 / bc2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="xval"></param>
        /// <returns></returns>
        private static Complex lagra(Complex[] x, Complex[] y, Complex xval)
        {
            Complex yval = 0.0;
            Complex Products = y[0];
            int length = x.Length;
            int i, j;

            for (i = 0; i < length; i++)
            {
                Products = y[i];
                for (j = 0; j < length; j++)
                {
                    if (i != j)
                    {
                        Products *= (xval - x[j]) / (x[i] - x[j]);
                    }
                }
                yval += Products;
            }
            return yval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="xval"></param>
        /// <returns></returns>
        private static Complex newto(Complex[] x, Complex[] y, Complex xval)
        {
            Complex yval;
            int length = x.Length;
            Complex[] tarray = new Complex[length];
            int i, j;

            for (i = 0; i < length; i++)
            {
                tarray[i] = y[i];
            }
            for (i = 0; i < length - 1; i++)
            {
                for (j = length - 1; j > i; j--)
                {
                    tarray[j] = (tarray[j - 1] - tarray[j]) / (x[j - 1 - i] - x[j]);
                }
            }
            yval = tarray[length - 1];
            for (i = length - 2; i >= 0; i--)
            {
                yval = tarray[i] + (xval - x[i]) * yval;
            }
            return yval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="xval"></param>
        /// <returns></returns>
        private static Complex baryc(Complex[] x, Complex[] y, Complex xval)
        {
            Complex product;
            Complex deltaX;
            Complex bc1 = 0;
            Complex bc2 = 0;
            int length = x.Length;
            Complex[] weights = new Complex[length];
            int i, j;

            for (i = 0; i < length; i++)
            {
                product = 1;
                for (j = 0; j < length; j++)
                {
                    if (i != j)
                    {
                        product *= (x[i] - x[j]);
                        weights[i] = 1.0 / product;
                    }
                }
            }

            for (i = 0; i < length; i++)
            {
                deltaX = weights[i] / (xval - x[i]);
                bc1 += y[i] * deltaX;
                bc2 += deltaX;
            }
            return bc1 / bc2;
        }
        #endregion

        #region Enums
        /// <summary>
        /// Метод интерполяции.
        /// </summary>
        public enum Method
        {
            #region Methods
            /// <summary>
            /// Линейный метод.
            /// </summary>
            Linear,
            /// <summary>
            /// Метод Лагранжа.
            /// </summary>
            Lagrange,
            /// <summary>
            /// Метод Ньютона.
            /// </summary>
            Newton,
            /// <summary>
            /// Барицентрический метод.
            /// </summary>
            Barycentric,
            #endregion
        }
        #endregion
    }
    #endregion

    #region Approximation methods
    /// <summary>
    /// Определяет класс аппроксимации методом наименьших квадратов.
    /// <remarks>
    /// Данный класс представляет собой решение задачи нахождения функции A(x) ≈ F(x), где F(x) - исходная функция.
    /// Более подробную информацию можно найти на сайте:
    /// http://simenergy.ru/math-analysis/digital-processing/85-ordinary_least_squares
    /// </remarks>
    /// </summary>
    public class Approximation
    {
        #region Private data
        private Approximation.Method method;
        private int power;
        #endregion

        #region Approximation components
        /// <summary>
        /// Инициализирует класс аппроксимации методом наименьших квадратов.
        /// </summary>
        /// <param name="power">Степень полинома</param>
        /// <param name="method">Метод аппроксимации</param>
        public Approximation(int power = 1, Approximation.Method method = Approximation.Method.Polynomial)
        {
            this.Power = power;
            this.method = method;
        }
        /// <summary>
        /// Получает или задает степень полинома.
        /// </summary>
        public int Power
        {
            get
            {
                return this.power;
            }
            set
            {
                if (value < 1)
                    throw new Exception("Неверное значение аргмуента");

                this.power = value;
            }
        }
        /// <summary>
        /// Получает или задает метод аппроксимации.
        /// </summary>
        public Approximation.Method MethodType
        {
            get
            {
                return this.method;
            }
            set
            {
                this.method = value;
            }
        }
        #endregion

        #region Public voids
        /// <summary>
        /// Возвращает значение аппроксимации.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="y">Массив значений функции</param>
        public double[] Compute(double[] x, double[] y)
        {
            double[] cf = null;
            double error = 0;
            string equation = null;

            // chose method of approximation
            switch (method)
            {
                case Method.Polynomial:
                    return Approximation.poly(x, y, power, ref cf, ref error, ref equation);

                case Method.Logarithmic:
                    return Approximation.logc(x, y, power, ref cf, ref error, ref equation);

                case Method.Exponential:
                    return Approximation.expn(x, y, power, ref cf, ref error, ref equation);

                default:
                    return Approximation.powr(x, y, power, ref cf, ref error, ref equation);
            }
        }
        /// <summary>
        /// Возвращает значение аппроксимации.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="y">Массив значений функции</param>
        /// <param name="cf">Коэффициенты аппроксимации</param>
        public double[] Compute(double[] x, double[] y, ref double[] cf)
        {
            double error = 0;
            string equation = null;

            // chose method of approximation
            switch (method)
            {
                case Method.Polynomial:
                    return Approximation.poly(x, y, power, ref cf, ref error, ref equation);

                case Method.Logarithmic:
                    return Approximation.logc(x, y, power, ref cf, ref error, ref equation);

                case Method.Exponential:
                    return Approximation.expn(x, y, power, ref cf, ref error, ref equation);

                default:
                    return Approximation.powr(x, y, power, ref cf, ref error, ref equation);
            }
        }
        /// <summary>
        /// Возвращает значение аппроксимации.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="y">Массив значений функции</param>
        /// <param name="cf">Коэффициенты аппроксимации</param>
        /// <param name="error">Погрешность аппроксимации</param>
        public double[] Compute(double[] x, double[] y, ref double[] cf, ref double error)
        {
            string equation = null;

            // chose method of approximation
            switch (method)
            {
                case Method.Polynomial:
                    return Approximation.poly(x, y, power, ref cf, ref error, ref equation);

                case Method.Logarithmic:
                    return Approximation.logc(x, y, power, ref cf, ref error, ref equation);

                case Method.Exponential:
                    return Approximation.expn(x, y, power, ref cf, ref error, ref equation);

                default:
                    return Approximation.powr(x, y, power, ref cf, ref error, ref equation);
            }
        }
        /// <summary>
        /// Возвращает значение аппроксимации.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="y">Массив значений функции</param>
        /// <param name="cf">Коэффициенты аппроксимации</param>
        /// <param name="error">Погрешность аппроксимации</param>
        /// <param name="equation">Уравнение аппроксимации</param>
        public double[] Compute(double[] x, double[] y, ref double[] cf, ref double error, ref string equation)
        {
            // chose method of approximation
            switch (method)
            {
                case Method.Polynomial:
                    return Approximation.poly(x, y, power, ref cf, ref error, ref equation);

                case Method.Logarithmic:
                    return Approximation.logc(x, y, power, ref cf, ref error, ref equation);

                case Method.Exponential:
                    return Approximation.expn(x, y, power, ref cf, ref error, ref equation);

                default:
                    return Approximation.powr(x, y, power, ref cf, ref error, ref equation);
            }
        }

        /// <summary>
        /// Возвращает значение аппроксимации.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="y">Массив значений функции</param>
        public Complex[] Compute(Complex[] x, Complex[] y)
        {
            Complex[] cf = null;
            Complex error = 0;
            string equation = null;

            // chose method of approximation
            switch (method)
            {
                case Method.Polynomial:
                    return Approximation.poly(x, y, power, ref cf, ref error, ref equation);

                case Method.Logarithmic:
                    return Approximation.logc(x, y, power, ref cf, ref error, ref equation);

                case Method.Exponential:
                    return Approximation.expn(x, y, power, ref cf, ref error, ref equation);

                default:
                    return Approximation.powr(x, y, power, ref cf, ref error, ref equation);
            }
        }
        /// <summary>
        /// Возвращает значение аппроксимации.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="y">Массив значений функции</param>
        /// <param name="cf">Коэффициенты аппроксимации</param>
        public Complex[] Compute(Complex[] x, Complex[] y, ref Complex[] cf)
        {
            Complex error = 0;
            string equation = null;

            // chose method of approximation
            switch (method)
            {
                case Method.Polynomial:
                    return Approximation.poly(x, y, power, ref cf, ref error, ref equation);

                case Method.Logarithmic:
                    return Approximation.logc(x, y, power, ref cf, ref error, ref equation);

                case Method.Exponential:
                    return Approximation.expn(x, y, power, ref cf, ref error, ref equation);

                default:
                    return Approximation.powr(x, y, power, ref cf, ref error, ref equation);
            }
        }
        /// <summary>
        /// Возвращает значение аппроксимации.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="y">Массив значений функции</param>
        /// <param name="cf">Коэффициенты аппроксимации</param>
        /// <param name="error">Погрешность аппроксимации</param>
        public Complex[] Compute(Complex[] x, Complex[] y, ref Complex[] cf, ref Complex error)
        {
            string equation = null;

            // chose method of approximation
            switch (method)
            {
                case Method.Polynomial:
                    return Approximation.poly(x, y, power, ref cf, ref error, ref equation);

                case Method.Logarithmic:
                    return Approximation.logc(x, y, power, ref cf, ref error, ref equation);

                case Method.Exponential:
                    return Approximation.expn(x, y, power, ref cf, ref error, ref equation);

                default:
                    return Approximation.powr(x, y, power, ref cf, ref error, ref equation);
            }
        }
        /// <summary>
        /// Возвращает значение аппроксимации.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="y">Массив значений функции</param>
        /// <param name="cf">Коэффициенты аппроксимации</param>
        /// <param name="error">Погрешность аппроксимации</param>
        /// <param name="equation">Уравнение аппроксимации</param>
        public Complex[] Compute(Complex[] x, Complex[] y, ref Complex[] cf, ref Complex error, ref string equation)
        {
            // chose method of approximation
            switch (method)
            {
                case Method.Polynomial:
                    return Approximation.poly(x, y, power, ref cf, ref error, ref equation);

                case Method.Logarithmic:
                    return Approximation.logc(x, y, power, ref cf, ref error, ref equation);

                case Method.Exponential:
                    return Approximation.expn(x, y, power, ref cf, ref error, ref equation);

                default:
                    return Approximation.powr(x, y, power, ref cf, ref error, ref equation);
            }
        }
        #endregion

        #region Private voids
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="power"></param>
        /// <param name="cf"></param>
        /// <param name="error"></param>
        /// <param name="equation"></param>
        /// <returns></returns>
        private static double[] poly(double[] x, double[] y, int power, ref double[] cf, ref double error, ref string equation)
        {
            // Options:
            int m = (power < 1) ? 2 : power + 1;
            cf = LeastSquaresOptions.Coefficients(x, y, m);
            double[] ya = LeastSquaresOptions.Polynomial(x, cf);
            error = LeastSquaresOptions.Error(ya, y);
            equation = LeastSquaresOptions.Equation(cf);
            return ya;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="power"></param>
        /// <param name="cf"></param>
        /// <param name="error"></param>
        /// <param name="equation"></param>
        /// <returns></returns>
        private static Complex[] poly(Complex[] x, Complex[] y, int power, ref Complex[] cf, ref Complex error, ref string equation)
        {
            // Options:
            int m = (power < 1) ? 2 : power + 1;
            cf = LeastSquaresOptions.Coefficients(x, y, m);
            Complex[] ya = LeastSquaresOptions.Polynomial(x, cf);
            error = LeastSquaresOptions.Error(ya, y);
            equation = LeastSquaresOptions.Equation(cf);
            return ya;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="power"></param>
        /// <param name="cf"></param>
        /// <param name="error"></param>
        /// <param name="equation"></param>
        /// <returns></returns>
        private static double[] logc(double[] x, double[] y, int power, ref double[] cf, ref double error, ref string equation)
        {
            // Options:
            int n = x.Length, i;
            int m = (power < 1) ? 2 : power + 1;
            double[] xa = new double[n];
            double[] ya = new double[n];

            // log-scale:
            for (i = 0; i < n; i++)
            {
                xa[i] = Maths.Log(x[i]);
            }

            // approximation:
            cf = LeastSquaresOptions.Coefficients(xa, y, m);
            ya = LeastSquaresOptions.Polynomial(xa, cf);
            error = LeastSquaresOptions.Error(ya, y);
            equation = LeastSquaresOptions.Equation(cf, " * LN(X)^");
            return ya;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="power"></param>
        /// <param name="cf"></param>
        /// <param name="error"></param>
        /// <param name="equation"></param>
        /// <returns></returns>
        private static Complex[] logc(Complex[] x, Complex[] y, int power, ref Complex[] cf, ref Complex error, ref string equation)
        {
            // Options:
            int n = x.Length, i;
            int m = (power < 1) ? 2 : power + 1;
            Complex[] xa = new Complex[n];
            Complex[] ya = new Complex[n];

            // log-scale:
            for (i = 0; i < n; i++)
            {
                xa[i] = Maths.Log(x[i]);
            }

            // approximation:
            cf = LeastSquaresOptions.Coefficients(xa, y, m);
            ya = LeastSquaresOptions.Polynomial(xa, cf);
            error = LeastSquaresOptions.Error(ya, y);
            equation = LeastSquaresOptions.Equation(cf, " * LN(X)^");
            return ya;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="power"></param>
        /// <param name="cf"></param>
        /// <param name="error"></param>
        /// <param name="equation"></param>
        /// <returns></returns>
        private static double[] expn(double[] x, double[] y, int power, ref double[] cf, ref double error, ref string equation)
        {
            // Options:
            int m = (power < 1) ? 2 : power + 1;
            int n = x.Length, i;
            double[] ya = new double[n];

            // log-scale:
            for (i = 0; i < n; i++)
            {
                ya[i] = Maths.Log(y[i], Math.E);
            }

            // approximation:
            cf = LeastSquaresOptions.Coefficients(x, ya, m);
            double[] p = LeastSquaresOptions.Polynomial(x, cf);

            // exponential-scale:
            for (i = 0; i < n; i++)
            {
                ya[i] = Maths.Pow(Math.E, p[i]);
            }

            error = LeastSquaresOptions.Error(ya, y);
            equation = "EXP" + '(' + LeastSquaresOptions.Equation(cf) + ')';
            return ya;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="power"></param>
        /// <param name="cf"></param>
        /// <param name="error"></param>
        /// <param name="equation"></param>
        /// <returns></returns>
        private static Complex[] expn(Complex[] x, Complex[] y, int power, ref Complex[] cf, ref Complex error, ref string equation)
        {
            // Options:
            int m = (power < 1) ? 2 : power + 1;
            int n = x.Length, i;
            Complex[] ya = new Complex[n];

            // log-scale:
            for (i = 0; i < n; i++)
            {
                ya[i] = Maths.Log(y[i], Math.E);
            }

            // approximation:
            cf = LeastSquaresOptions.Coefficients(x, ya, m);
            Complex[] p = LeastSquaresOptions.Polynomial(x, cf);

            // exponential-scale:
            for (i = 0; i < n; i++)
            {
                ya[i] = Maths.Pow(Math.E, p[i]);
            }

            error = LeastSquaresOptions.Error(ya, y);
            equation = "EXP" + '(' + LeastSquaresOptions.Equation(cf) + ')';
            return ya;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="power"></param>
        /// <param name="cf"></param>
        /// <param name="error"></param>
        /// <param name="equation"></param>
        /// <returns></returns>
        private static double[] powr(double[] x, double[] y, int power, ref double[] cf, ref double error, ref string equation)
        {
            // Options:
            int m = (power < 1) ? 2 : power + 1;
            int n = x.Length, i;
            double[] xa = new double[n];
            double[] ya = new double[n];

            // log-scale:
            for (i = 0; i < n; i++)
            {
                xa[i] = Maths.Log(x[i]);
                ya[i] = Maths.Log(y[i]);
            }

            // approximation:
            cf = LeastSquaresOptions.Coefficients(xa, ya, m);
            double[] p = LeastSquaresOptions.Polynomial(xa, cf);

            // exponential-scale:
            for (i = 0; i < n; i++)
            {
                ya[i] = Maths.Exp(p[i]);
            }

            error = LeastSquaresOptions.Error(ya, y);
            equation = "EXP" + '(' + LeastSquaresOptions.Equation(cf, " * LN(X)^") + ')';
            return ya;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="power"></param>
        /// <param name="cf"></param>
        /// <param name="error"></param>
        /// <param name="equation"></param>
        /// <returns></returns>
        private static Complex[] powr(Complex[] x, Complex[] y, int power, ref Complex[] cf, ref Complex error, ref string equation)
        {
            // Options:
            int m = (power < 1) ? 2 : power + 1;
            int n = x.Length, i;
            Complex[] xa = new Complex[n];
            Complex[] ya = new Complex[n];

            // log-scale:
            for (i = 0; i < n; i++)
            {
                xa[i] = Maths.Log(x[i]);
                ya[i] = Maths.Log(y[i]);
            }

            // approximation:
            cf = LeastSquaresOptions.Coefficients(xa, ya, m);
            Complex[] p = LeastSquaresOptions.Polynomial(xa, cf);

            // exponential-scale:
            for (i = 0; i < n; i++)
            {
                ya[i] = Maths.Exp(p[i]);
            }

            error = LeastSquaresOptions.Error(ya, y);
            equation = "EXP" + '(' + LeastSquaresOptions.Equation(cf, " * LN(X)^") + ')';
            return ya;
        }
        #endregion

        #region Enums
        /// <summary>
        /// Метод аппроксимации.
        /// </summary>
        public enum Method
        {
            #region Methods
            /// <summary>
            /// Полиномиальная аппроксимация.
            /// </summary>
            Polynomial,
            /// <summary>
            /// Логарифимическая аппроксимация.
            /// </summary>
            Logarithmic,
            /// <summary>
            /// Экспоненциальная аппроксимация.
            /// </summary>
            Exponential,
            /// <summary>
            /// Степенная аппроксимация.
            /// </summary>
            Power,
            #endregion
        }
        #endregion
    }
    /// <summary>
    /// Определяет класс, реализующий метод наименьших квадратов.
    /// </summary>
    internal static class LeastSquaresOptions
    {
        #region double components
        /// <summary>
        /// Возвращает значение полиномиала.
        /// </summary>
        /// <param name="x">Аргумент</param>
        /// <param name="c">Коэффициенты аппроксимации</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public static double Polynomial(double x, double[] c)
        {
            int n = c.Length, i;
            double p = 1, s = 0;

            for (i = 0; i < n; i++, p *= x)
            {
                s += c[i] * p;
            }
            return s;
        }
        /// <summary>
        /// Возвращает массив значений полиномиала.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="c">Коэффициенты аппроксимации</param>
        /// <returns>Одномерный массив</returns>
        public static double[] Polynomial(double[] x, double[] c)
        {
            int n = x.Length, i;
            double[] y = new double[n];

            for (i = 0; i < n; i++)
            {
                y[i] = LeastSquaresOptions.Polynomial(x[i], c);
            }
            return y;
        }
        /// <summary>
        /// Возвращает коэффициенты аппроксимации.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="y">Массив значений функции</param>
        /// <param name="iterations">Количество итераций</param>
        public static double[] Coefficients(double[] x, double[] y, int iterations)
        {
            // Построение матрицы преобразования:
            int i, j;
            int n = x.Length;
            int m = iterations < 1 ? 1 : iterations;
            double[,] matrix = new double[m, m + 1];

            for (i = 0; i < m; i++)
            {
                for (j = 0; j < m; j++)
                {
                    matrix[i, j] = LeastSquaresOptions.SummaryPow(x, j + i);
                }
                matrix[i, m] = LeastSquaresOptions.SummaryPow(y, x, 1, i);
            }

            // Решение системы линейных уравнений:
            return Matrice.Solve(matrix);
        }
        /// <summary>
        /// Возвращает значение выражения: s += v(i) ^ pow.
        /// </summary>
        /// <param name="v">Одномерный массив</param>
        /// <param name="pow">Степень</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public static double SummaryPow(double[] v, double pow)
        {
            double sum = 0;
            int length = v.Length;

            for (int i = 0; i < length; i++)
            {
                sum += Math.Pow(v[i], pow);
            }
            return sum;
        }
        /// <summary>
        /// Возвращает значение выражения: s += {x(i) ^ powx} * {y(i) ^ powy}.
        /// </summary>
        /// <param name="x">Одномерный массив</param>
        /// <param name="y">Одномерный массив</param>
        /// <param name="powx">Степень</param>
        /// <param name="powy">Степень</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public static double SummaryPow(double[] x, double[] y, double powx, double powy)
        {
            double sum = 0;
            int length = x.Length;

            for (int i = 0; i < length; i++)
            {
                sum += Math.Pow(x[i], powx) * Math.Pow(y[i], powy);
            }
            return sum;
        }
        /// <summary>
        /// Возвращает погрешность аппроксимации функции.
        /// </summary>
        /// <param name="a">Аппроксимация</param>
        /// <param name="b">Функция</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public static double Error(double[] a, double[] b)
        {
            double vara = Matrice.Var(a);
            double varb = Matrice.Var(b);

            if (vara < varb)
            {
                return vara / varb;
            }
            return varb / vara;
        }
        /// <summary>
        /// Возвращает уравнение полинома, представленного в виде строки.
        /// </summary>
        /// <param name="p">Коэффициенты полинома</param>
        /// <returns>Текст как последовательность знаков Юникода</returns>
        public static string Equation(double[] p)
        {
            string equation = "";
            int length = p.Length;

            for (int i = 0; i < length; i++)
            {
                equation += (Convert.ToString(p[i]) +
                            (i == 0 ? "" : (" * X^" + Convert.ToString(i))) +
                            (i < length - 1 ? (p[i + 1] < 0 ? " " : " + ") : ""));
            }

            return equation;
        }
        /// <summary>
        /// Возвращает уравнение полинома, представленного в виде строки.
        /// </summary>
        /// <param name="p">Коэффициенты полинома</param>
        /// <param name="function">Функция</param>
        /// <returns>Текст как последовательность знаков Юникода</returns>
        public static string Equation(double[] p, string function)
        {
            string equation = "";
            int length = p.Length;

            for (int i = 0; i < length; i++)
            {
                equation += (Convert.ToString(p[i]) +
                            (i == 0 ? "" : (function + Convert.ToString(i))) +
                            (i < length - 1 ? (p[i + 1] < 0 ? " " : " + ") : ""));
            }

            return equation;
        }
        #endregion

        #region Complex components
        /// <summary>
        /// Возвращает значение полиномиала.
        /// </summary>
        /// <param name="x">Аргумент</param>
        /// <param name="c">Коэффициенты аппроксимации</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public static Complex Polynomial(Complex x, Complex[] c)
        {
            int n = c.Length, i;
            Complex p = 1, s = 0;

            for (i = 0; i < n; i++, p *= x)
            {
                s += c[i] * p;
            }
            return s;
        }
        /// <summary>
        /// Возвращает массив значений полиномиала.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="c">Коэффициенты аппроксимации</param>
        /// <returns>Одномерный массив</returns>
        public static Complex[] Polynomial(Complex[] x, Complex[] c)
        {
            int n = x.Length, i;
            Complex[] y = new Complex[n];

            for (i = 0; i < n; i++)
            {
                y[i] = LeastSquaresOptions.Polynomial(x[i], c);
            }
            return y;
        }
        /// <summary>
        /// Возвращает коэффициенты аппроксимации.
        /// </summary>
        /// <param name="x">Массив значений аргумента</param>
        /// <param name="y">Массив значений функции</param>
        /// <param name="iterations">Количество итераций</param>
        public static Complex[] Coefficients(Complex[] x, Complex[] y, int iterations)
        {
            // Построение матрицы преобразования:
            int i, j;
            int n = x.Length;
            int m = iterations < 1 ? 1 : iterations;
            Complex[,] matrix = new Complex[m, m + 1];

            for (i = 0; i < m; i++)
            {
                for (j = 0; j < m; j++)
                {
                    matrix[i, j] = LeastSquaresOptions.SummaryPow(x, j + i);
                }
                matrix[i, m] = LeastSquaresOptions.SummaryPow(y, x, 1, i);
            }

            // Решение системы линейных уравнений:
            return Matrice.Solve(matrix);
        }
        /// <summary>
        /// Возвращает значение выражения: s += v(i) ^ pow.
        /// </summary>
        /// <param name="v">Одномерный массив</param>
        /// <param name="pow">Степень</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public static Complex SummaryPow(Complex[] v, double pow)
        {
            Complex sum = 0;
            int length = v.Length;

            for (int i = 0; i < length; i++)
            {
                sum += Maths.Pow(v[i], pow);
            }
            return sum;
        }
        /// <summary>
        /// Возвращает значение выражения: s += {x(i) ^ powx} * {y(i) ^ powy}.
        /// </summary>
        /// <param name="x">Одномерный массив</param>
        /// <param name="y">Одномерный массив</param>
        /// <param name="powx">Степень</param>
        /// <param name="powy">Степень</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public static Complex SummaryPow(Complex[] x, Complex[] y, double powx, double powy)
        {
            Complex sum = 0;
            int length = x.Length;

            for (int i = 0; i < length; i++)
            {
                sum += Maths.Pow(x[i], powx) * Maths.Pow(y[i], powy);
            }
            return sum;
        }
        /// <summary>
        /// Возвращает погрешность аппроксимации функции.
        /// </summary>
        /// <param name="a">Аппроксимация</param>
        /// <param name="b">Функция</param>
        /// <returns>Число двойной точности с плавающей запятой</returns>
        public static Complex Error(Complex[] a, Complex[] b)
        {
            Complex vara = Matrice.Var(a);
            Complex varb = Matrice.Var(b);

            if (vara.Abs < varb.Abs)
            {
                return (vara / varb).Real;
            }
            return (varb / vara).Real;
        }
        /// <summary>
        /// Возвращает уравнение полинома, представленного в виде строки.
        /// </summary>
        /// <param name="p">Коэффициенты полинома</param>
        /// <returns>Текст как последовательность знаков Юникода</returns>
        public static string Equation(Complex[] p)
        {
            string equation = "";
            int length = p.Length;

            for (int i = 0; i < length; i++)
            {
                equation += ("(" + Convert.ToString(p[i]) + ")" +
                            (i == 0 ? "" : (" * X^" + Convert.ToString(i))) +
                            (i < length - 1 ? (p[i + 1].Abs < 0 ? " " : " + ") : ""));
            }

            return equation;
        }
        /// <summary>
        /// Возвращает уравнение полинома, представленного в виде строки.
        /// </summary>
        /// <param name="p">Коэффициенты полинома</param>
        /// <param name="function">Функция</param>
        /// <returns>Текст как последовательность знаков Юникода</returns>
        public static string Equation(Complex[] p, string function)
        {
            string equation = "";
            int length = p.Length;

            for (int i = 0; i < length; i++)
            {
                equation += ("(" + Convert.ToString(p[i]) + ")" +
                            (i == 0 ? "" : (function + Convert.ToString(i))) +
                            (i < length - 1 ? (p[i + 1].Abs < 0 ? " " : " + ") : ""));
            }

            return equation;
        }
        #endregion
    }
    #endregion

    #region Roots solution
    /// <summary>
    /// Определяет класс решения уравнений с использованием спектрального разложения матрицы.
    /// <remarks>
    /// Более подробную информацию можно найти на сайте:
    /// https://www.mathworks.com/help/matlab/ref/roots.html
    /// </remarks>
    /// </summary>
    public class Roots
    {
        #region Private data
        /// <summary>
        /// Спектральное разложение матрицы.
        /// </summary>
        private EVD eig;
        /// <summary>
        /// Погрешность вычислений.
        /// </summary>
        private double eps;
        #endregion

        #region Class components
        /// <summary>
        /// Инициализирует класс решения уравнений с использованием спектрального разложения матрицы.
        /// </summary>
        /// <param name="eps">Погрешность [0, 1]</param>
        public Roots(double eps = 1e-16)
        {
            this.Eps = eps;
        }
        /// <summary>
        /// Получает или задает погрешность [0, 1].
        /// </summary>
        public double Eps
        {
            get
            {
                return this.eps;
            }
            set
            {
                this.eps = Maths.Double(value);
            }
        }
        /// <summary>
        /// Возвращает вектор-столбец, соответствующий численному решению полинома: p(1)*x^n + ... + p(n)*x + p(n+1) = 0.
        /// </summary>
        /// <param name="polynomial">Полином</param>
        /// <returns>Вектор-столбец</returns>
        public Complex[] Compute(double[] polynomial)
        {
            // MATLAB roots method
            // represented by Asiryan Valeriy, 2018.
            // properties of polynomial:
            int length = polynomial.Length;
            int i, index = -1;

            // finding non-zero element:
            for (i = 0; i < length; i++)
            {
                if (polynomial[i] != 0)
                {
                    index = i;
                    break;
                }
            }

            // return null array:
            if (index == -1)
            {
                return new Complex[0];
            }

            // get scaling factor:
            int m = length - index - 1;
            double scale = polynomial[index];
            double[] c = new double[m];

            // create new polynomial:
            for (i = 0; i < m; i++)
            {
                c[i] = polynomial[i + index + 1] / scale;
            }
            
            // Eigen-value decomposition for
            // companion matrix:
            eig = new EVD(Matrice.Companion(c), this.eps);

            // Complex result:
            return eig.D;
        }
        /// <summary>
        /// Возвращает вектор-столбец коэффициентов полинома: p(1)*x^n + ... + p(n)*x + p(n+1) = 0.
        /// </summary>
        /// <param name="roots">Корни полинома</param>
        /// <returns>Вектор-столбец</returns>
        public double[] Compute(Complex[] roots)
        {
            // MATLAB roots method
            // represented by Asiryan Valeriy, 2018.
            // properties of polynomial:
            int length = roots.Length, m = length + 1, j, i;

            // arrays:
            Complex[] v = new Complex[length];
            Complex[] p = new Complex[m];

            // point:
            p[0] = 1.0;

            // create new polynomial:
            for (j = 0; j < length; j++)
            {
                // right part:
                for (i = 0; i <= j; i++)
                {
                    v[i] = roots[j] * p[i];
                }
                // left part:
                for (i = 0; i <= j; i++)
                {
                    p[i + 1] -= v[i];
                }
            }

            // Real result:
            return p.Real();
        }
        #endregion
    }
    #endregion
}
