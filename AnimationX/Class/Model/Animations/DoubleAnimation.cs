namespace AnimationX.Class.Model.Animations;

public class DoubleAnimation : AnimationBase<double>
{
    public override void ComputeNextFrame()
    {
        var progress = EasingFunction.Ease(CurrentFrameTime);
        var frameVal = From + (To - From) * progress;

        CurrentComputedFrame = frameVal ?? 0;
        CurrentFrameTime += StepAmount;
        CurrentFrame++;
    }
}