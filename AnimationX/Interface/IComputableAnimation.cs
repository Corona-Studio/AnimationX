namespace AnimationX.Interface;

public interface IComputableAnimation
{
    double CurrentFrameTime { get; }
    bool IsFinishedInvoked { get; }
    bool IsFinished { get; }
    bool IsPausing { get; }
    void ComputeNextFrame();

    void InvokeOnStart();
    void InvokeOnEnd();
}