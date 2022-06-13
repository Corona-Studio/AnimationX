using AnimationX.Class.Model.Animations;
using AnimationX.Interface;
using System;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Media;

namespace AnimationX.Class.Helper;

public static class AnimationHelper
{
    private static readonly ConcurrentDictionary<int, IComputableAnimation> AnimationList;
    private static long _lastTick = DateTime.Now.Ticks;

    static AnimationHelper()
    {
        SleepTime = TimeSpan.FromSeconds(1d / TimeLineAnimationBase.DesiredFrameRate).TotalSeconds * TimeSpan.TicksPerSecond;
        AnimationList = new ConcurrentDictionary<int, IComputableAnimation>();
        
        CompositionTarget.Rendering += CompositionTargetOnRendering;
    }

    private static void CompositionTargetOnRendering(object? sender, EventArgs e)
    {
        var currentTick = DateTime.Now.Ticks;

        if (currentTick - _lastTick < SleepTime) return;

        _lastTick = currentTick;
        Compute();
    }

    public static double SleepTime { get; }

    private static void Compute()
    {
        foreach (var (_, animation) in AnimationList)
        {
            if (animation.IsPausing) continue;
            if (animation.IsFinished)
            {
                if (!animation.IsFinishedInvoked) animation.InvokeOnEnd();

                continue;
            }

            animation.ComputeNextFrame();
        }
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
    }

    public static bool RemoveAnimation(this TimeLineAnimationBase ani)
    {
        return AnimationList.TryRemove(ani.GetHashCode(), out _);
    }
}