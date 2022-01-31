using AnimationX.Class.Model.EasingFunctions;
using BackEase = System.Windows.Media.Animation.BackEase;
using IEasingFunction = AnimationX.Interface.IEasingFunction;

namespace AnimationX.Class.Helper;

public static class AnimationConvertHelper
{
    public static IEasingFunction ToEasing(this System.Windows.Media.Animation.IEasingFunction function)
    {
        return function switch
        {
            BackEase e => new Model.EasingFunctions.BackEase{Amplitude = e.Amplitude, EasingMode = e.EasingMode},
            BounceEase e => new BounceEase{Bounces = e.Bounces, Bounciness = e.Bounciness, EasingMode = e.EasingMode},
            CircleEase e => new CircleEase{EasingMode = e.EasingMode},
            CubicEase e => new CubicEase{ EasingMode = e.EasingMode },
            ElasticEase e => new ElasticEase{ EasingMode = e.EasingMode, Oscillations = e.Oscillations, Springiness = e.Springiness},
            ExponentialEase e => new ExponentialEase{ EasingMode = e.EasingMode, Exponent = e.Exponent},
            PowerEase e => new PowerEase{ EasingMode = e.EasingMode, Power = e.Power},
            QuadraticEase e => new QuadraticEase{ EasingMode = e.EasingMode },
            QuarticEase e => new QuarticEase{ EasingMode = e.EasingMode },
            QuinticEase e => new QuinticEase{ EasingMode = e.EasingMode },
            SineEase e => new SineEase{ EasingMode = e.EasingMode },
            EasingBase e => new LinearEase{ EasingMode = e.EasingMode },
            _ => new LinearEase()
        };
    }
}