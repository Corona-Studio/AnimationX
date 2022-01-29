using AnimationX.Class.Model.EasingFunctions;
using AnimationX.Interface;
using System;
using System.ComponentModel;
using System.Windows;
using AnimationX.Class.Helper;

namespace AnimationX.Class.Model.Animations;

public abstract class AnimationBase<T> : IComputableAnimation, IAnimation<T> where T : struct
{
    private readonly EventHandlerList _listEventDelegates = new();

    public DependencyObject? AnimateObject { get; init; }
    public DependencyProperty? AnimateProperty { get; init; }

    public event EventHandler? Started
    {
        add => _listEventDelegates.AddHandler(nameof(Started), value);
        remove => _listEventDelegates.RemoveHandler(nameof(Started), value);
    }

    public event EventHandler? Ended
    {
        add => _listEventDelegates.AddHandler(nameof(Ended), value);
        remove => _listEventDelegates.RemoveHandler(nameof(Ended), value);
    }

    public static int DesiredFrameRate { get; set; } = 60;

    public virtual IEasingFunction EasingFunction { get; init; } = new LinearEase();

    private protected double StepAmount { get; set; }
    private protected T CurrentFrame { get; set; }

    public bool IsFinished => CurrentFrameTime.Equals3DigitPrecision(1d);
    public double CurrentFrameTime { get; private protected set; }
    public virtual T? From { get; set; }
    public virtual T? To { get; init; }
    public TimeSpan Duration { get; init; } = TimeSpan.FromSeconds(0.5);

    public bool IsRunning { get; private protected set; }

    private async void ResetAnimation()
    {
        if (From == null)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                From = (T)AnimateObject!.GetValue(AnimateProperty);
            });
        }

        CurrentFrame = From!.Value;
        CurrentFrameTime = 0;
        StepAmount = 1 / Duration.TotalSeconds / DesiredFrameRate;
    }

    public virtual void Begin()
    {
        if (To == null)
            throw new ArgumentNullException(nameof(To));
        if (AnimateObject == null)
            throw new ArgumentNullException(nameof(AnimateObject));
        if (AnimateProperty == null)
            throw new ArgumentNullException(nameof(AnimateProperty));

        ResetAnimation();
        this.CommitAnimation();
    }

    public virtual void OnStart(object sender, EventArgs e)
    {
        var eventList = _listEventDelegates;
        var @event = (EventHandler)eventList[nameof(Started)]!;
        @event?.Invoke(sender, e);
    }

    public virtual void OnEnd(object sender, EventArgs e)
    {
        var eventList = _listEventDelegates;
        var @event = (EventHandler)eventList[nameof(Ended)]!;
        @event?.Invoke(sender, e);
    }

    public abstract void ComputeNextFrame();

    public virtual void UpdateFrame()
    {
        if (AnimateObject == null || AnimateProperty == null) return;

        Application.Current.Dispatcher?.Invoke(() =>
        {
            AnimateObject.SetValue(AnimateProperty, CurrentFrame);
        });
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AnimateObject, AnimateProperty, To, Duration);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not AnimationBase<T> ani) return false;

        return AnimateObject == ani.AnimateObject &&
               AnimateProperty == ani.AnimateProperty &&
               Duration == ani.Duration;
    }
}