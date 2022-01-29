namespace AnimationX.Class.Model.Animations;

public class DoubleAnimation : AnimationBase<double>
{
    public override void ComputeNextFrame()
    {
        var frameVal = From + (To - From) * EasingFunction.Ease(CurrentFrameTime);
        CurrentFrame = frameVal ?? 0;
        CurrentFrameTime += StepAmount;
    }
}