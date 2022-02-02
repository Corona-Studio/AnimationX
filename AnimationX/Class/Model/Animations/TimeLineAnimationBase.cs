using AnimationX.Class.Model.EasingFunctions;
using AnimationX.Interface;
using System;
using System.Windows;

namespace AnimationX.Class.Model.Animations;

public abstract class TimeLineAnimationBase : ITimeLineAnimation, IComputableAnimation
{
    public static int DesiredFrameRate { get; set; } = 60;

    public double CurrentFrameTime { get; private protected set; }

    public bool IsFinishedInvoked { get; private protected set; }
    public bool IsFinished => CurrentFrameTime == 1d; // CurrentFrame == TotalFrameCount;
    public abstract void ComputeNextFrame();

    public void InvokeOnStart()
    {
        OnStart(this, EventArgs.Empty);
    }

    public void InvokeOnEnd()
    {
        OnEnd(this, EventArgs.Empty);
    }

    public DependencyObject? AnimateObject { get; set; }
    public DependencyProperty? AnimateProperty { get; set; }

    public double SpeedRatio { get; set; } = 1d;
    public Duration Duration { get; set; } = new(TimeSpan.FromSeconds(0.5));

    public virtual IEasingFunction EasingFunction { get; set; } = new LinearEase();

    public abstract void Begin();

    public abstract event EventHandler? Started;
    public abstract event EventHandler? Ended;

    private protected abstract void OnStart(object sender, EventArgs e);
    private protected abstract void OnEnd(object sender, EventArgs e);
}