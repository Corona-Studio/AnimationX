using System;
using System.Windows;

namespace AnimationX.Interface;

public interface IAnimation<T> where T : struct
{
    DependencyObject AnimateObject { get; }
    DependencyProperty AnimateProperty { get; }

    event EventHandler Started;
    event EventHandler Ended;

    T? From { get; }
    T? To { get; }
    IEasingFunction EasingFunction { get; }
    
    TimeSpan Duration { get; }

    bool IsRunning { get; }

    void Begin();
}