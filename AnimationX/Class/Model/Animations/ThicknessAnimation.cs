using System.Windows;

namespace AnimationX.Class.Model.Animations;

public class ThicknessAnimation : AnimationBase<Thickness>
{
    public override void ComputeNextFrame()
    {
        var from = From ?? default;
        var to = To!.Value;

        var left = from.Left + (to.Left - from.Left) * EasingFunction.Ease(CurrentFrameTime);
        var right = from.Right + (to.Right - from.Right) * EasingFunction.Ease(CurrentFrameTime);
        var top = from.Top + (to.Top - from.Top) * EasingFunction.Ease(CurrentFrameTime);
        var bottom = from.Bottom + (to.Bottom - from.Bottom) * EasingFunction.Ease(CurrentFrameTime);

        var frameThickness = new Thickness(left, top, right, bottom);
        CurrentFrame = frameThickness;
        CurrentFrameTime += StepAmount;
    }
}