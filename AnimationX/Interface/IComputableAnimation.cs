namespace AnimationX.Interface;

public interface IComputableAnimation
{
    double CurrentFrameTime { get; }
    long CurrentFrame { get; }
    long TotalFrameCount { get; }
    bool IsFinishedInvoked { get; }
    bool IsFinished { get; }
    void ComputeNextFrame();
    void UpdateFrame();

    void InvokeOnStart();
    void InvokeOnEnd();
}