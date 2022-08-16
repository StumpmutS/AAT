using System;
using UnityEngine;
using Utility.Scripts;

public class SpawnPlotButtonsController : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<EFaction, ButtonListWrapper> buttons;

    public event Action<UnitSpawnData> OnSpawnRequest = delegate { };
    
    public void DisplayByFaction(EFaction faction)
    {
        RemoveButtons();
        gameObject.SetActive(true);
        foreach (var button in buttons[faction].List)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void RequestSpawn(UnitSpawnData spawnData)
    {
        OnSpawnRequest.Invoke(spawnData);
    }

    private void RemoveButtons()
    {
        gameObject.SetActive(false);
        foreach (var kvp in buttons)
        {
            kvp.Value.List.ForEach(button => button.gameObject.SetActive(false));
        }
    }
}
