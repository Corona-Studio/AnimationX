using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using AnimationX.Class.Model.Animations;
using AnimationX.Interface;

namespace AnimationX.Class.Helper;

public static class AnimationHelper
{
    private static readonly double SleepTime;
    private static readonly ConcurrentDictionary<int, IComputableAnimation> AnimationList;

    static AnimationHelper()
    {
        SleepTime = 1d / TimeLineAnimationBase.DesiredFrameRate;
        AnimationList = new ConcurrentDictionary<int, IComputableAnimation>();
        
        var thread = new Thread(StartCompute)
        {
            IsBackground = true
        };
        thread.Start();
    }

    private static void StartCompute()
    {
        do
        {
            foreach (var (_, animation) in AnimationList)
            {
                if (animation.IsFinished)
                {
                    if (!animation.IsFinishedInvoked) animation.InvokeOnEnd();

                    continue;
                }

                animation.ComputeNextFrame();
            }

            Thread.Sleep(TimeSpan.FromSeconds(SleepTime));
        } while (true);
    }

    internal static void CommitAnimation(this IComputableAnimation ani)
    {
        AnimationList.AddOrUpdate(ani.GetHashCode(), ani, (_, _) => ani);
    }

    public static void BeginAnimation(this DependencyObject obj, DependencyProperty property, TimeLineAnimationBase ani)
    {
        ani.AnimateObject = obj;
        ani.AnimateProperty = property;

        ani.Begin();
        //ani.CommitAnimation(HashCode.Combine(obj, property));
    }

    public static bool RemoveAnimation(this TimeLineAnimationBase ani)
    {
        return AnimationList.TryRemove(ani.GetHashCode(), out _);
    }
}