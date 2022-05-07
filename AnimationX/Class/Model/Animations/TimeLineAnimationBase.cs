using System;
using System.Windows;
using AnimationX.Class.Model.EasingFunctions;
using AnimationX.Interface;

namespace AnimationX.Class.Model.Animations;

public abstract class TimeLineAnimationBase : ITimeLineAnimation, IComputableAnimation
{
    public static int DesiredFrameRate { get; set; } = 60;

    public bool IsPausing { get; private protected set; }
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

    public bool RepeatForever { get; set; }

    public DependencyObject? AnimateObject { get; set; }
    public DependencyProperty? AnimateProperty { get; set; }

    public double SpeedRatio { get; set; } = 1d;
    public Duration Duration { get; set; } = new(TimeSpan.FromSeconds(0.5));

    public virtual IEasingFunction EasingFunction { get; set; } = new LinearEase();

    public abstract event EventHandler? Started;
    public abstract event EventHandler? Ended;

    public abstract void Begin();
    public abstract void Stop();

    public abstract void Pause();
    public abstract void Resume();

    private protected abstract void OnStart(object sender, EventArgs e);
    private protected abstract void OnEnd(object sender, EventArgs e);
}