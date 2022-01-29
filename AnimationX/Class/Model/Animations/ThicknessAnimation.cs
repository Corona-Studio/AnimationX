using System;
using System.Threading;
using System.Windows;

namespace AnimationX.Class.Model.Animations;

public class ThicknessAnimation : AnimationBase<Thickness>
{
    private protected override void ComputeAnimation(CancelRequest cancelReq, Action<Thickness> frameCall)
    {
        var sleepTime = 1d / DesiredFrameRate;
        var stepAmount = 1 / Duration.TotalSeconds / DesiredFrameRate;
        var co = cancelReq;

        var aniThread = new Thread(() =>
        {
            var currentTime = 0d;

            do
            {
                if (co.IsCancelled)
                {
                    return;
                }

                var from = From!.Value;
                var to = To!.Value;
                var left = from.Left + (to.Left - from.Left) * EasingFunction.Ease(currentTime);
                var right = from.Right + (to.Right - from.Right) * EasingFunction.Ease(currentTime);
                var top = from.Top + (to.Top - from.Top) * EasingFunction.Ease(currentTime);
                var bottom = from.Bottom + (to.Bottom - from.Bottom) * EasingFunction.Ease(currentTime);

                var frameThickness = new Thickness(left, top, right, bottom);
                frameCall(frameThickness);

                currentTime += stepAmount;
                Thread.Sleep(TimeSpan.FromSeconds(sleepTime));
            }
            while (currentTime.CompareTo(1d) < 0);

            IsRunning = false;
            OnEnd(this, EventArgs.Empty);
        })
        {
            IsBackground = true
        };

        aniThread.Start();
    }
}