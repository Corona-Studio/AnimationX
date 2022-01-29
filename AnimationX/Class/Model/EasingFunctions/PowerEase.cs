using System;

namespace AnimationX.Class.Model.EasingFunctions;

public class PowerEase : EasingBase
{
    public double Power { get; set; } = 2;

    protected override double EaseCore(double time)
    {
        var y = Math.Max(0.0, Power);

        return Math.Pow(time, y);
    }
}