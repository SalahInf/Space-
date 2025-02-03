using System;
using System.Collections;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Tween scale functions
    /// Implementations based on http://theinstructionlimit.com/flash-style-tweeneasing-functions-in-c, which are based on http://www.robertpenner.com/easing/
    /// </remarks>
    public static class TweenScaleFunctions
    {
        private const float halfPi = Mathf.PI * 0.5f;

        /// <summary>
        /// A linear progress scale function.
        /// </summary>
        public static readonly Func<float, float> Linear = LinearFunc;
        private static float LinearFunc(float progress) { return progress; }

        /// <summary>
        /// A quadratic (x^2) progress scale function that eases in.
        /// </summary>
        public static readonly Func<float, float> QuadraticEaseIn = QuadraticEaseInFunc;
        private static float QuadraticEaseInFunc(float progress) { return EaseInPower(progress, 2); }

        /// <summary>
        /// A quadratic (x^2) progress scale function that eases out.
        /// </summary>
        public static readonly Func<float, float> QuadraticEaseOut = QuadraticEaseOutFunc;
        private static float QuadraticEaseOutFunc(float progress) { return EaseOutPower(progress, 2); }

        /// <summary>
        /// A quadratic (x^2) progress scale function that eases in and out.
        /// </summary>
        public static readonly Func<float, float> QuadraticEaseInOut = QuadraticEaseInOutFunc;
        private static float QuadraticEaseInOutFunc(float progress) { return EaseInOutPower(progress, 2); }

        /// <summary>
        /// A cubic (x^3) progress scale function that eases in.
        /// </summary>
        public static readonly Func<float, float> CubicEaseIn = CubicEaseInFunc;
        private static float CubicEaseInFunc(float progress) { return EaseInPower(progress, 3); }

        /// <summary>
        /// A cubic (x^3) progress scale function that eases out.
        /// </summary>
        public static readonly Func<float, float> CubicEaseOut = CubicEaseOutFunc;
        private static float CubicEaseOutFunc(float progress) { return EaseOutPower(progress, 3); }

        /// <summary>
        /// A cubic (x^3) progress scale function that eases in and out.
        /// </summary>
        public static readonly Func<float, float> CubicEaseInOut = CubicEaseInOutFunc;
        private static float CubicEaseInOutFunc(float progress) { return EaseInOutPower(progress, 3); }

        /// <summary>
        /// A quartic (x^4) progress scale function that eases in.
        /// </summary>
        public static readonly Func<float, float> QuarticEaseIn = QuarticEaseInFunc;
        private static float QuarticEaseInFunc(float progress) { return EaseInPower(progress, 4); }

        /// <summary>
        /// A quartic (x^4) progress scale function that eases out.
        /// </summary>
        public static readonly Func<float, float> QuarticEaseOut = QuarticEaseOutFunc;
        private static float QuarticEaseOutFunc(float progress) { return EaseOutPower(progress, 4); }

        /// <summary>
        /// A quartic (x^4) progress scale function that eases in and out.
        /// </summary>
        public static readonly Func<float, float> QuarticEaseInOut = QuarticEaseInOutFunc;
        private static float QuarticEaseInOutFunc(float progress) { return EaseInOutPower(progress, 4); }

        /// <summary>
        /// A quintic (x^5) progress scale function that eases in.
        /// </summary>
        public static readonly Func<float, float> QuinticEaseIn = QuinticEaseInFunc;
        private static float QuinticEaseInFunc(float progress) { return EaseInPower(progress, 5); }

        /// <summary>
        /// A quintic (x^5) progress scale function that eases out.
        /// </summary>
        public static readonly Func<float, float> QuinticEaseOut = QuinticEaseOutFunc;
        private static float QuinticEaseOutFunc(float progress) { return EaseOutPower(progress, 5); }

        /// <summary>
        /// A quintic (x^5) progress scale function that eases in and out.
        /// </summary>
        public static readonly Func<float, float> QuinticEaseInOut = QuinticEaseInOutFunc;
        private static float QuinticEaseInOutFunc(float progress) { return EaseInOutPower(progress, 5); }

        /// <summary>
        /// A sine progress scale function that eases in.
        /// </summary>
        public static readonly Func<float, float> SineEaseIn = SineEaseInFunc;
        private static float SineEaseInFunc(float progress) { return Mathf.Sin(progress * halfPi - halfPi) + 1; }

        /// <summary>
        /// A sine progress scale function that eases out.
        /// </summary>
        public static readonly Func<float, float> SineEaseOut = SineEaseOutFunc;
        private static float SineEaseOutFunc(float progress) { return Mathf.Sin(progress * halfPi); }

        /// <summary>
        /// A sine progress scale function that eases in and out.
        /// </summary>
        public static readonly Func<float, float> SineEaseInOut = SineEaseInOutFunc;
        private static float SineEaseInOutFunc(float progress) { return (Mathf.Sin(progress * Mathf.PI - halfPi) + 1) / 2; }

        public static Func<float, float> GetTweenScaleFunctions(TypeScale typeScale, TypeScaleMode scaleMode)
        {
            switch (typeScale)
            {
                case TypeScale.Linear:
                    return Linear;
                case TypeScale.QuadraticEase:
                    switch (scaleMode)
                    {
                        case TypeScaleMode.In:
                            return QuadraticEaseIn;
                        case TypeScaleMode.Out:
                            return QuadraticEaseOut;
                        case TypeScaleMode.InOut:
                            return QuadraticEaseInOut;
                    }
                    break;
                case TypeScale.CubicEase:
                    switch (scaleMode)
                    {
                        case TypeScaleMode.In:
                            return CubicEaseIn;
                        case TypeScaleMode.Out:
                            return CubicEaseOut;
                        case TypeScaleMode.InOut:
                            return CubicEaseInOut;
                    }
                    break;
                case TypeScale.QuarticEase:
                    switch (scaleMode)
                    {
                        case TypeScaleMode.In:
                            return QuarticEaseIn;
                        case TypeScaleMode.Out:
                            return QuarticEaseOut;
                        case TypeScaleMode.InOut:
                            return QuarticEaseInOut;
                    }
                    break;
                case TypeScale.QuinticEase:
                    switch (scaleMode)
                    {
                        case TypeScaleMode.In:
                            return QuinticEaseIn;
                        case TypeScaleMode.Out:
                            return QuinticEaseInOut;
                        case TypeScaleMode.InOut:
                            return QuinticEaseOut;
                    }
                    break;
                case TypeScale.SineEase:
                    switch (scaleMode)
                    {
                        case TypeScaleMode.In:
                            return SineEaseIn;
                        case TypeScaleMode.Out:
                            return SineEaseOut;
                        case TypeScaleMode.InOut:
                            return SineEaseInOut;
                    }
                    break;
                default:
                    return Linear;
            }
            return Linear;
        }

        public static Func<float, float> GetTweenScaleFunctions(TypeScaleFunctions typeScale)
        {
            switch (typeScale)
            {
                case TypeScaleFunctions.Linear:
                    return Linear;
                case TypeScaleFunctions.QuadraticEaseIn:
                    return QuadraticEaseIn;
                case TypeScaleFunctions.QuadraticEaseOut:
                    return QuadraticEaseOut;
                case TypeScaleFunctions.QuadraticEaseInOut:
                    return QuadraticEaseInOut;
                case TypeScaleFunctions.CubicEaseIn:
                    return CubicEaseIn;
                case TypeScaleFunctions.CubicEaseOut:
                    return CubicEaseOut;
                case TypeScaleFunctions.CubicEaseInOut:
                    return CubicEaseInOut;
                case TypeScaleFunctions.QuarticEaseIn:
                    return QuarticEaseIn;
                case TypeScaleFunctions.QuarticEaseOut:
                    return QuarticEaseOut;
                case TypeScaleFunctions.QuarticEaseInOut:
                    return QuarticEaseInOut;
                case TypeScaleFunctions.QuinticEaseIn:
                    return QuinticEaseIn;
                case TypeScaleFunctions.QuinticEaseOut:
                    return QuinticEaseOut;
                case TypeScaleFunctions.QuinticEaseInOut:
                    return QuinticEaseInOut;
                case TypeScaleFunctions.SineEaseIn:
                    return SineEaseIn;
                case TypeScaleFunctions.SineEaseOut:
                    return SineEaseOut;
                case TypeScaleFunctions.SineEaseInOut:
                    return SineEaseInOut;
                default:
                    return Linear;
            }
        }

        private static float EaseInPower(float progress, int power)
        {
            return Mathf.Pow(progress, power);
        }

        private static float EaseOutPower(float progress, int power)
        {
            int sign = power % 2 == 0 ? -1 : 1;
            return (sign * (Mathf.Pow(progress - 1, power) + sign));
        }

        private static float EaseInOutPower(float progress, int power)
        {
            progress *= 2.0f;
            if (progress < 1)
            {
                return Mathf.Pow(progress, power) / 2.0f;
            }
            else
            {
                int sign = power % 2 == 0 ? -1 : 1;
                return (sign / 2.0f * (Mathf.Pow(progress - 2, power) + sign * 2));
            }
        }

        public enum TypeScale
        {
            Linear,
            QuadraticEase,
            CubicEase,
            QuarticEase,
            QuinticEase,
            SineEase,
        }

        public enum TypeScaleMode
        {
            In,
            Out,
            InOut
        }

        public enum TypeScaleFunctions
        {
            Linear,
            QuadraticEaseIn,
            QuadraticEaseOut,
            QuadraticEaseInOut,
            CubicEaseIn,
            CubicEaseOut,
            CubicEaseInOut,
            QuarticEaseIn,
            QuarticEaseOut,
            QuarticEaseInOut,
            QuinticEaseIn,
            QuinticEaseOut,
            QuinticEaseInOut,
            SineEaseIn,
            SineEaseOut,
            SineEaseInOut
        }
    }
}