using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using AnimationX.Class.Model.Animations;
using AnimationX.Interface;

namespace AnimationX.Class.Helper;

public static class AnimationHelper
{
    private static readonly ReaderWriterLockSlim Lock = new(LockRecursionPolicy.NoRecursion);
    private static readonly List<IComputableAnimation> AnimationList;
    private static long _lastTick = DateTime.Now.Ticks;

    static AnimationHelper()
    {
        SleepTime = TimeSpan.FromSeconds(1d / TimeLineAnimationBase.DesiredFrameRate).TotalSeconds *
                    TimeSpan.TicksPerSecond;
        AnimationList = new List<IComputableAnimation>();

        CompositionTarget.Rendering += CompositionTargetOnRendering;
    }

    public static double SleepTime { get; }

    private static void CompositionTargetOnRendering(object? sender, EventArgs e)
    {
        var currentTick = DateTime.Now.Ticks;

        if (currentTick - _lastTick < SleepTime) return;

        _lastTick = currentTick;
        Compute();
    }

    private static void Compute()
    {
        Lock.TryEnterReadLock(100);
        var count = AnimationList.Count;
        Lock.ExitReadLock();

        if (count == 0) return;

        for (var i = count - 1; i >= 0; i--)
        {
            Lock.TryEnterReadLock(100);
            var animation = AnimationList[i];
            Lock.ExitReadLock();

            if (animation.IsPausing) continue;
            if (animation.IsFinished)
            {
                if (!animation.IsFinishedInvoked) animation.InvokeOnEnd();

                Lock.TryEnterWriteLock(100);
                AnimationList.Remove(animation);
                Lock.ExitWriteLock();

                continue;
            }

            animation.ComputeNextFrame();
        }
    }

    internal static void CommitAnimation(this IComputableAnimation ani)
    {
        Lock.TryEnterWriteLock(100);

        if (AnimationList.Contains(ani))
            AnimationList.Remove(ani);

        AnimationList.Add(ani);

        Lock.ExitWriteLock();
    }

    public static void BeginAnimation(this DependencyObject obj, DependencyProperty property, TimeLineAnimationBase ani)
    {
        ani.AnimateObject = obj;
        ani.AnimateProperty = property;

        ani.Begin();
    }

    public static void RemoveAnimation(this TimeLineAnimationBase ani)
    {
        Lock.TryEnterWriteLock(100);
        AnimationList.Remove(ani);
        Lock.ExitWriteLock();
    }
}