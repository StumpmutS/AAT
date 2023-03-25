using Fusion;

public class NetworkedSingleton<T> : NetworkBehaviour where T : NetworkedSingleton<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = (T) this;
    }
}