using UnityEngine;

public class LayerManager : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    public LayerMask GroundLayer => groundLayer;

    public static LayerManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
