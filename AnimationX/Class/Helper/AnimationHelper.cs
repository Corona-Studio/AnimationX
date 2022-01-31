using AnimationX.Class.Model.Animations;
using AnimationX.Interface;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace AnimationX.Class.Helper;

internal static class AnimationHelper
{
    private static readonly double SleepTime;
    private static readonly Thread AnimationComputeThread;
    private static readonly ConcurrentDictionary<int, IComputableAnimation> AnimationList;

    static AnimationHelper()
    {
        SleepTime = 1d / TimeLineAnimationBase.DesiredFrameRate;
        AnimationList = new ConcurrentDictionary<int, IComputableAnimation>();
        AnimationComputeThread = new Thread(StartCompute)
        {
            IsBackground = true,
            Priority = ThreadPriority.AboveNormal
        };

        AnimationComputeThread.Start();
    }

    private static void StartCompute()
    {
        do
        {
            foreach (var (_, animation) in AnimationList)
            {
                if (animation.IsFinished)
                {
                    if (!animation.IsFinishedInvoked)
                    {
                        animation.InvokeOnEnd();
                    }

                    continue;
                }

                animation.ComputeNextFrame();
                animation.UpdateFrame();
            }

            Thread.Sleep(TimeSpan.FromSeconds(SleepTime));
        } while (true);
    }

    internal static void CommitAnimation(this IComputableAnimation ani)
    {
        AnimationList.AddOrUpdate(ani.GetHashCode(), ani, (_, _) => ani);
    }
}