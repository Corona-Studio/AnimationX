using System;

namespace AnimationX.Class.Model.EasingFunctions;

public class CircleEase : EasingBase
{
    protected override double EaseCore(double time)
    {
        time = Math.Max(0.0, Math.Min(1.0, time));

        return 1.0 - Math.Sqrt(1.0 - Math.Pow(time, 2));
    }
}