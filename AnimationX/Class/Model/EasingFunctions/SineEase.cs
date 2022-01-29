using System;

namespace AnimationX.Class.Model.EasingFunctions;

public class SineEase : EasingBase
{
    protected override double EaseCore(double time)
    {
        return 1.0 - Math.Sin(Math.PI / 2.0 * (1.0 - time));
    }
}