using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using AnimationX.Class.Helper;
using AnimationX.Interface;

namespace AnimationX.Class.Model.Animations;

public abstract class AnimationBase<T> : TimeLineAnimationBase, IAnimation<T> where T : struct
{
    private readonly EventHandlerList _listEventDelegates = new();

    private T _currentComputedFrame;

    private bool _isFrameUpdated;

    private long _last = 0;

    private double StepAmount { get; set; }

    public T CurrentComputedFrame
    {
        get => _currentComputedFrame;
        set
        {
            _isFrameUpdated = false;
            _currentComputedFrame = value;
        }
    }

    public virtual T? From { get; set; }
    public virtual T? To { get; set; }

    private async void ResetAnimation()
    {
        await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
        {
            AnimateObject.SetValue(UIElement.CacheModeProperty, new BitmapCache());
        });

        if (From == null)
            await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                From = (T)AnimateObject!.GetValue(AnimateProperty!);
            });

        var totalSeconds = Duration switch
        {
            _ when Duration == Duration.Automatic => 0.75,
            _ when Duration == Duration.Forever => double.NaN,
            _ => Duration.TimeSpan.TotalSeconds
        };

        if (double.IsNaN(totalSeconds))
        {
            StepAmount = double.NaN;
            return;
        }

        /*
        BindingOperations.ClearBinding(AnimateObject!, AnimateProperty!);
        BindingOperations.SetBinding(AnimateObject!, AnimateProperty!, new Binding(nameof(CurrentComputedFrame))
        {
            Source = this,
            Mode = BindingMode.OneWay
        });
        */

        IsFinishedInvoked = false;
        CurrentComputedFrame = From ?? default;
        StepAmount = 1 / (totalSeconds / SpeedRatio * DesiredFrameRate);
        // CurrentFrame = 0;
        _isFrameUpdated = false;
        CurrentFrameTime = 0;

        // TotalFrameCount = (long) Math.Floor(1d / StepAmount) + 1;
    }

    private void CompositionTargetOnRendering(object? sender, EventArgs e)
    {
        if (e is not RenderingEventArgs renderingEventArgs) return;
        
        if (_last == renderingEventArgs.RenderingTime.Ticks) return;
        if (_isFrameUpdated) return;

        _last = renderingEventArgs.RenderingTime.Ticks;
        _isFrameUpdated = true;

        AnimateObject!.SetCurrentValue(AnimateProperty!, CurrentComputedFrame);
    }

    public override void Begin()
    {
        if (SpeedRatio is < 0 or double.NaN)
            throw new ArgumentOutOfRangeException(nameof(SpeedRatio));
        if (To == null)
            throw new ArgumentNullException(nameof(To));
        if (AnimateObject == null)
            throw new ArgumentNullException(nameof(AnimateObject));
        if (AnimateProperty == null)
            throw new ArgumentNullException(nameof(AnimateProperty));

        ResetAnimation();

        if (double.IsNaN(StepAmount)) return;

        OnStart(this, EventArgs.Empty);
        this.CommitAnimation();
    }

    public override void Stop()
    {
        this.RemoveAnimation();
    }

    public override void Pause()
    {
        IsPausing = true;
        CompositionTarget.Rendering -= CompositionTargetOnRendering;
    }

    public override void Resume()
    {
        IsPausing = false;
        CompositionTarget.Rendering += CompositionTargetOnRendering;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AnimateObject, AnimateProperty);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not AnimationBase<T> ani) return false;

        return AnimateObject == ani.AnimateObject &&
               AnimateProperty == ani.AnimateProperty &&
               Duration == ani.Duration;
    }

    public override void ComputeNextFrame()
    {
        var frameTime = CurrentFrameTime + StepAmount;
        CurrentFrameTime = Math.Min(1d, frameTime);

        if (RepeatForever && CurrentFrameTime == 1d)
            CurrentFrameTime = 0;
    }

    private protected override void OnStart(object sender, EventArgs e)
    {
        CompositionTarget.Rendering += CompositionTargetOnRendering;

        var eventList = _listEventDelegates;
        var @event = (EventHandler)eventList[nameof(Started)]!;
        @event?.Invoke(sender, e);
    }

    private protected override void OnEnd(object sender, EventArgs e)
    {
        var eventList = _listEventDelegates;
        var @event = (EventHandler)eventList[nameof(Ended)]!;
        @event?.Invoke(sender, e);

        IsFinishedInvoked = true;
        CompositionTarget.Rendering -= CompositionTargetOnRendering;
    }

    public override event EventHandler? Started
    {
        add => _listEventDelegates.AddHandler(nameof(Started), value);
        remove => _listEventDelegates.RemoveHandler(nameof(Started), value);
    }

    public override event EventHandler? Ended
    {
        add => _listEventDelegates.AddHandler(nameof(Ended), value);
        remove => _listEventDelegates.RemoveHandler(nameof(Ended), value);
    }
}