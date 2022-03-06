using System;
using System.Windows;

namespace AnimationX.Interface;

public interface ITimeLineAnimation
{
    DependencyObject AnimateObject { get; set; }
    DependencyProperty AnimateProperty { get; set; }

    bool RepeatForever { get; set; }
    double SpeedRatio { get; set; }
    IEasingFunction EasingFunction { get; set; }

    Duration Duration { get; set; }

    event EventHandler Started;
    event EventHandler Ended;

    void Begin();
}