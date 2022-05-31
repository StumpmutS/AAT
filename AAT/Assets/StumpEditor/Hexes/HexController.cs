using UnityEngine;

public class HexController : MonoBehaviour
{
    [SerializeField] private FactionMaterials factionMaterials;
    #pragma warning disable 108,114
    [SerializeField] private Renderer renderer;
    #pragma warning restore 108,114
    
    public void Setup(EFaction faction)
    {
        renderer.material = factionMaterials.FactionsByMaterial[faction];
    }
}
