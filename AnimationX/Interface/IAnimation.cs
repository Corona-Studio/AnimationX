namespace AnimationX.Interface;

public interface IAnimation<T> where T : struct
{
    T? From { get; set; }
    T? To { get; set; }
}