namespace AnimationX.Interface;

public interface IComputableAnimation
{
    double CurrentFrameTime { get; }
    bool IsFinished { get; }
    void ComputeNextFrame();
    void UpdateFrame();
}