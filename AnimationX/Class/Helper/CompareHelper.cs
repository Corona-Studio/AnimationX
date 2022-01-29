using System;

namespace AnimationX.Class.Helper;

public static class CompareHelper
{
    private const double _3 = 0.001;
    private const double _4 = 0.0001;

    public static bool Equals3DigitPrecision(this double left, double right)
    {
        return Math.Abs(left - right) < _3;
    }

    public static bool Equals4DigitPrecision(this double left, double right)
    {
        return Math.Abs(left - right) < _4;
    }
}