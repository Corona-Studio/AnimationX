namespace AnimationX.Interface;

public interface IComputableAnimation
{
    double CurrentFrameTime { get; }
    bool IsFinishedInvoked { get; }
    bool IsFinished { get; }
    void ComputeNextFrame();

    void InvokeOnStart();
    void InvokeOnEnd();
}