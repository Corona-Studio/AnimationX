using System;

namespace AnimationX.Class.Model.EasingFunctions;

public class BackEase : EasingBase
{
    public double Amplitude { get; set; }

    protected override double EaseCore(double time)
    {
        var num = Math.Max(0.0, Amplitude);
        return Math.Pow(time, 3.0) - time * num * Math.Sin(Math.PI * time);
    }
}