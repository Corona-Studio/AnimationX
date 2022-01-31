using System;
using System.Windows;

namespace AnimationX.Interface;

public interface ITimeLineAnimation
{
    DependencyObject AnimateObject { get; }
    DependencyProperty AnimateProperty { get; }

    event EventHandler Started;
    event EventHandler Ended;

    double SpeedRatio { get; }
    IEasingFunction EasingFunction { get; }

    Duration Duration { get; }

    void Begin();
}