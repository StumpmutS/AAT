using System;
using System.Linq;
using Fusion;
using UnityEngine;
using Utility.Scripts;

public abstract class InteractableController : SimulationBehaviour, IStatePrefabGen
{
    [SerializeField] private DeathController deathController;
    [SerializeField] private EInteractableType interactableType;
    public EInteractableType InteractableType => interactableType;
    [SerializeField] private float interactRange;
    public float InteractRange => interactRange;
    [SerializeField] private TypeReference stateType;
    [SerializeField] private InteractionComponentState interactionComponentStatePrefab;

    private bool _previewDisplayed;

    public event Action<InteractableController> OnInteractableDestroyed = delegate { };

    protected virtual void Awake()
    {
        deathController.OnDeath += DestroyInteractable;
    }

    private void DestroyInteractable()
    {
        OnInteractableDestroyed.Invoke(this);
    }

    public abstract void RequestAffection(InteractionComponentState componentState);

    public InteractionComponentState RequestInteractionState() => interactionComponentStatePrefab;

    private void OnDestroy()
    {
        deathController.OnDeath -= DestroyInteractable;
    }

    [ContextMenu("GenerateStatePrefabs")]
    public void GenerateStatePrefabs()
    {
        if (!PrefabGen.TryGenerateStateFolder(GetInstanceID(), out var path)) return;
        if (!PrefabGen.GenerateStatePrefab(stateType.TargetType, path, "", out var prefab)) return;
        
        interactionComponentStatePrefab = prefab.GetComponent<InteractionComponentState>();
    }
}