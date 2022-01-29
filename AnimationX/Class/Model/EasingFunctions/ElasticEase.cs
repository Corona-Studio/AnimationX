using System;

namespace AnimationX.Class.Model.EasingFunctions;

public class ElasticEase : EasingBase
{
    public int Oscillations { get; set; } = 3;
    public double Springiness { get; set; } = 3;

    protected override double EaseCore(double time)
    {
        var num = Math.Max(0.0, Oscillations);
        var d = Math.Max(0.0, Springiness);

        return (d.CompareTo(0.0) != 0 ? (Math.Exp(d * time) - 1.0) / (Math.Exp(d) - 1.0) : time) * Math.Sin((2.0 * Math.PI * num + Math.PI / 2.0) * time);
    }
}