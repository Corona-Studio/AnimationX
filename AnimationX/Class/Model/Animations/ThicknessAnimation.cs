using System.Diagnostics;
using System.Windows;

namespace AnimationX.Class.Model.Animations;

public class ThicknessAnimation : AnimationBase<Thickness>
{
    public override void ComputeNextFrame()
    {
        var progress = EasingFunction.Ease(CurrentFrameTime);
        var from = From ?? default;
        var to = To!.Value;

        var left = from.Left + (to.Left - from.Left) * progress;
        var right = from.Right + (to.Right - from.Right) * progress;
        var top = from.Top + (to.Top - from.Top) * progress;
        var bottom = from.Bottom + (to.Bottom - from.Bottom) * progress;

        var frameThickness = new Thickness(left, top, right, bottom);

        CurrentComputedFrame = frameThickness;

        base.ComputeNextFrame();
    }
}