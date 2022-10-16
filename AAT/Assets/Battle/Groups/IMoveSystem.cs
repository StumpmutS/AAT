using System;

public interface IMoveSystem
{
    public event Action OnPathFinished;
    
    public void Move(StumpTarget target);

    public void Follow(StumpTarget target);

    public void Stop();

    public void Enable();
    
    public void Disable();
}