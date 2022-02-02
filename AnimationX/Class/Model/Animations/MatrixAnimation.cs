using System.Windows.Media;

namespace AnimationX.Class.Model.Animations;

public class MatrixAnimation : AnimationBase<Matrix>
{
    public MatrixAnimation()
    {
        base.From = new Matrix(1, 1, 1, 1, 0, 0);
    }

    public override void ComputeNextFrame()
    {
        base.ComputeNextFrame();

        var progress = EasingFunction.Ease(CurrentFrameTime);
        var from = From ?? default;
        var to = To ?? default;

        var currentMatrix = new Matrix
        {
            M11 = from.M11 + (to.M11 - from.M11) * progress,
            M12 = from.M12 + (to.M12 - from.M12) * progress,
            M21 = from.M21 + (to.M21 - from.M21) * progress,
            M22 = from.M22 + (to.M22 - from.M22) * progress,
            OffsetX = from.OffsetX + (to.OffsetX - from.OffsetX) * progress,
            OffsetY = from.OffsetY + (to.OffsetY - from.OffsetY) * progress
        };

        CurrentComputedFrame = currentMatrix;
    }
}