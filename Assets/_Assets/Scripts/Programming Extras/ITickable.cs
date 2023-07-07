/// <summary>
/// An interface for non-monobehaviour classes that still wish to subscribe to Update
/// </summary>
public interface ITickable
{
    void Tick(float deltaTime);
}