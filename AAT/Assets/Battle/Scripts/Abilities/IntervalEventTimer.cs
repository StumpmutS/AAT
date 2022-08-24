using System;

public class IntervalEventTimer<T>
{
    private float _timeInterval;
    private float _eventTimeElapsed;
    private T _object;
    private Action<T> _callBack;
        
    public void Tick(float deltaTime)
    {
        _eventTimeElapsed += deltaTime;
        if (_eventTimeElapsed < _timeInterval) return;
            
        _callBack.Invoke(_object);
        _eventTimeElapsed = 0;
    }

    public IntervalEventTimer(float timeInterval, T obj, Action<T> callBack)
    {
        _timeInterval = timeInterval;
        _object = obj;
        _callBack = callBack;
    }
}