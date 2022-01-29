using System;

namespace AnimationX.Class.Model.EasingFunctions;

public class BounceEase : EasingBase
{
    public int Bounces { get; set; } = 3;
    public double Bounciness { get; set; } = 2.0;

    protected override double EaseCore(double time)
    {
        var y1 = Math.Max(0.0, Bounces);
        var num1 = Bounciness;

        if (num1 < 1.0 || num1.CompareTo(1) == 0)
            num1 = 1001.0 / 1000.0;

        var num2 = Math.Pow(num1, y1);
        var num3 = 1.0 - num1;
        var num4 = (1.0 - num2) / num3 + num2 * 0.5;
        var y2 = Math.Floor(Math.Log(-(time * num4) * (1.0 - num1) + 1.0, num1));
        var y3 = y2 + 1.0;
        var num5 = (1.0 - Math.Pow(num1, y2)) / (num3 * num4);
        var num6 = (1.0 - Math.Pow(num1, y3)) / (num3 * num4);
        var num7 = (num5 + num6) * 0.5;
        var num8 = time - num7;
        var num9 = num7 - num5;

        return -Math.Pow(1.0 / num1, y1 - y2) / (num9 * num9) * (num8 - num9) * (num8 + num9);
    }
}