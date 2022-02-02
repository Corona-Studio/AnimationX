using System.Windows.Media;

namespace AnimationX.Class.Model.Animations;

public class ColorAnimation : AnimationBase<Color>
{
    public override void ComputeNextFrame()
    {
        base.ComputeNextFrame();

        var progress = EasingFunction.Ease(CurrentFrameTime);
        var from = From ?? default;
        var to = To!.Value;

        var frameVal = from + (to - from) * (float) progress;

        CurrentComputedFrame = frameVal;
    }
}