using System;

namespace AnimationX.Class.Model.EasingFunctions;

public class QuinticEase : EasingBase
{
    protected override double EaseCore(double time)
    {
        return Math.Pow(time, 5);
    }
}