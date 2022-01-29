namespace AnimationX.Class.Model;

public class CancelRequest
{
    public bool IsCancelled { get; private set; }
    public void Set() => IsCancelled = true;
}