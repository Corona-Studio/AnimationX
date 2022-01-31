using System;
using System.Windows;

namespace AnimationX.Interface;

public interface IAnimation<T> where T : struct
{
    T? From { get; set; }
    T? To { get; set; }

    bool IsRunning { get; }
}