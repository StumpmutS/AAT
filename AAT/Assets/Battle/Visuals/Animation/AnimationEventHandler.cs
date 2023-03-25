using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour, IGameActionInfoGetter
{
    [SerializeField, Tooltip("Ability name: part: action")]
    private SerializableDictionary<string, SerializableDictionary<int, List<StumpGameAction>>> animationGameActions;
    
    private SectorReference _sectorReference;
    
    public void CallAnimationEvent(int part, string abilityName)
    {
        GameActionRunner.Instance.PerformActions(animationGameActions[abilityName][part], this);
    }

    public IEnumerable<GameActionInfo> GetInfo()
    {
        return new []
        {
            new GameActionInfo(_sectorReference.Object, _sectorReference.Sector, GetComponent<TransformContainer>().ToChain(),
                new StumpTarget(gameObject, transform.position))
        };
    }
}