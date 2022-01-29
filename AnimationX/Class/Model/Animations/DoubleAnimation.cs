using System;
using System.Diagnostics;
using System.Threading;

namespace AnimationX.Class.Model.Animations;

public class DoubleAnimation : AnimationBase<double>
{
    private protected override void ComputeAnimation(CancelRequest cancelReq, Action<double> frameCall)
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
                    Debug.WriteLine("动画取消");
                    return;
                }

                var frameVal = From + (To - From) * EasingFunction.Ease(currentTime);
                frameCall(frameVal ?? 0);

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