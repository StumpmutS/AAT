using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIBrain : Brain<AiTransitionBlackboard>
{
    [SerializeField] private AiTransition defaultTransition;
    [SerializeField] private List<AiTransition> transitions;

    protected override Transition<AiTransitionBlackboard> GetDefaultTransition() => defaultTransition;

    protected override List<Transition<AiTransitionBlackboard>> GetTransitions() => transitions.Select(t => (Transition<AiTransitionBlackboard>) t).ToList();

    protected override AiTransitionBlackboard InitBlackboard() => new AiTransitionBlackboard();
}
