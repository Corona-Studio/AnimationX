namespace AnimationX.Class.Model.EasingFunctions;

public class LinearEase : EasingBase
{
    protected override double EaseCore(double time)
    {
        return time;
    }
}