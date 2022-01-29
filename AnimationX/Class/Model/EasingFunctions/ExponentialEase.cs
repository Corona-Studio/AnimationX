using System;

namespace AnimationX.Class.Model.EasingFunctions;

public class ExponentialEase : EasingBase
{
    public double Exponent { get; set; } = 2;

    protected override double EaseCore(double time)
    {
        var exponent = Exponent;

        return exponent.CompareTo(0.0) == 0 ? time : (Math.Exp(exponent * time) - 1.0) / (Math.Exp(exponent) - 1.0);
    }
}