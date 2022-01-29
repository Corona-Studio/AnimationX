using AnimationX.Interface;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Windows;
using AnimationX.Class.Model.EasingFunctions;

namespace AnimationX.Class.Model.Animations;

public abstract class AnimationBase<T> : IAnimation<T> where T : struct
{
    private readonly ConcurrentBag<CancelRequest> _cancelBag = new ();
    private readonly EventHandlerList _listEventDelegates = new();

    public DependencyObject? AnimateObject { get; set; }
    public DependencyProperty? AnimateProperty { get; set; }

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

    public virtual IEasingFunction EasingFunction { get; init; } = new LinearEase();

    public virtual T? From { get; set; }
    public virtual T? To { get; init; }
    public TimeSpan Duration { get; init; } = TimeSpan.FromSeconds(0.5);

    public virtual int DesiredFrameRate { get; init; } = 30;

    public bool IsRunning { get; private protected set; }

    public virtual async void Begin()
    {
        if (To == null)
            throw new ArgumentNullException(nameof(To));
        if (AnimateObject == null)
            throw new ArgumentNullException(nameof(AnimateObject));
        if (AnimateProperty == null)
            throw new ArgumentNullException(nameof(AnimateProperty));

        foreach (var co in _cancelBag)
        {
            co.Set();
        }

        _cancelBag.Clear();

        if (From == null)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                From = (T) AnimateObject!.GetValue(AnimateProperty);
            });
        }

        IsRunning = true;

        if (From.Equals(To))
        {
            IsRunning = false;
            OnEnd(this, EventArgs.Empty);

            return;
        }
        
        OnStart(this, EventArgs.Empty);

        var cancelObj = new CancelRequest();

        ComputeAnimation(cancelObj, UpdateCore);
        _cancelBag.Add(cancelObj);
    }

    private protected virtual void UpdateCore(T val)
    {
        if (AnimateObject == null || AnimateProperty == null) return;

        Application.Current.Dispatcher.Invoke(() =>
        {
            AnimateObject.SetValue(AnimateProperty, val);
        });
    }

    private protected abstract void ComputeAnimation(CancelRequest cancelReq, Action<T> frameCall);

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
}