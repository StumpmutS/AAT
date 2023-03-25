using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassiveBrain : Brain<PassiveTransitionBlackboard>
{
    [SerializeField] private PassiveTransition defaultTransition;
    [SerializeField] private List<PassiveTransition> transitions;

    protected override Transition<PassiveTransitionBlackboard> GetDefaultTransition() => defaultTransition;

    protected override List<Transition<PassiveTransitionBlackboard>> GetTransitions() => transitions.Select(t => (Transition<PassiveTransitionBlackboard>) t).ToList();

    protected override PassiveTransitionBlackboard InitBlackboard() => new PassiveTransitionBlackboard();
}