using AnimationX.Class.Model.EasingFunctions;
using AnimationX.Interface;
using System;
using System.ComponentModel;
using System.Windows;

namespace AnimationX.Class.Model.Animations;

public abstract class TimeLineAnimationBase : ITimeLineAnimation, IComputableAnimation
{
    public DependencyObject? AnimateObject { get; init; }
    public DependencyProperty? AnimateProperty { get; init; }

    public double CurrentFrameTime { get; private protected set; }
    public long CurrentFrame { get; private protected set; }
    public long TotalFrameCount { get; private protected set; }

    public bool IsFinishedInvoked { get; private protected set; }
    public bool IsFinished => CurrentFrame == TotalFrameCount;

    public double SpeedRatio { get; init; } = 1d;
    public Duration Duration { get; init; } = new(TimeSpan.FromSeconds(0.5));

    public virtual IEasingFunction EasingFunction { get; init; } = new LinearEase();

    public static int DesiredFrameRate { get; set; } = 60;

    public abstract void Begin();
    public abstract void ComputeNextFrame();
    public abstract void UpdateFrame();

    public abstract event EventHandler? Started;
    public abstract event EventHandler? Ended;

    private protected abstract void OnStart(object sender, EventArgs e);
    private protected abstract void OnEnd(object sender, EventArgs e);

    public void InvokeOnStart() => OnStart(this, EventArgs.Empty);
    public void InvokeOnEnd() => OnEnd(this, EventArgs.Empty);
}