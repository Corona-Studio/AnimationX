using System;
using System.Windows;

namespace AnimationX.Interface;

public interface IAnimation<T> where T : struct
{
    DependencyObject AnimateObject { get; set; }
    DependencyProperty AnimateProperty { get; set; }

    event EventHandler Started;
    event EventHandler Ended;

    T? From { get; }
    T? To { get; }
    IEasingFunction EasingFunction { get; }
    
    TimeSpan Duration { get; }
    int DesiredFrameRate { get; }

    bool IsRunning { get; }

    void Begin();
}