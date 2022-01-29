﻿using System;

namespace AnimationX.Class.Model.EasingFunctions;

public class QuarticEase : EasingBase
{
    protected override double EaseCore(double time)
    {
        return Math.Pow(time, 4);
    }
}