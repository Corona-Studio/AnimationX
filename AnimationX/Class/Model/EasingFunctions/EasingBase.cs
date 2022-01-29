using System.Windows.Media.Animation;
using IEasingFunction = AnimationX.Interface.IEasingFunction;

namespace AnimationX.Class.Model.EasingFunctions;

public abstract class EasingBase : IEasingFunction
{
    public EasingMode EasingMode { get; set; }

    public double Ease(double normalizedTime)
    {
        return EasingMode switch
        {
            EasingMode.EaseIn => EaseCore(normalizedTime),
            EasingMode.EaseOut => 1.0 - EaseCore(1.0 - normalizedTime),
            _ => normalizedTime >= 0.5 ? (1.0 - EaseCore((1.0 - normalizedTime) * 2.0)) * 0.5 + 0.5 : EaseCore(normalizedTime * 2.0) * 0.5,
        };
    }

    protected abstract double EaseCore(double time);
}