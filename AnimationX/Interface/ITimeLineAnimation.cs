using System;
using System.Windows;

namespace AnimationX.Interface;

public interface ITimeLineAnimation
{
    DependencyObject AnimateObject { get; set; }
    DependencyProperty AnimateProperty { get; set; }

    event EventHandler Started;
    event EventHandler Ended;

    double SpeedRatio { get; set; }
    IEasingFunction EasingFunction { get; set; }

    Duration Duration { get; set; }

    void Begin();
}