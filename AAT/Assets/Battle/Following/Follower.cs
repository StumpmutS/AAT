using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private List<EFollowType> followTypes;

    public bool CanFollow(StumpTarget target)
    {
        if (target.Hit.TryGetComponent<Followable>(out var followable))
        {
            return followable.FollowTypes.Any(ft => followTypes.Contains(ft));
        }

        return false;
    }
}