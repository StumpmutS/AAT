using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MountablePointLinkController))]
public class LinkablePlaceableWallController : PlaceableWallController, ILinkable
{
    public event Action<MountablePointLinkController, bool> OnJoin;

    public override void Setup(int connectedWallIndex, List<WallJointController> wallJoints = null)
    {
        base.Setup(connectedWallIndex, wallJoints);
        JoinLinks(connectedWallIndex);
    }

    private void JoinLinks(int index)
    {
        for (int i = 0; i < _wallJoints.Count; i++)
        {
            var otherWall = _wallJoints[i].OtherWall(this);
            if (otherWall != null)
            {
                var otherLink = otherWall.GetComponent<MountablePointLinkController>();
                if (otherLink != null)
                    switch (index)
                    {
                        case 0:
                            OnJoin.Invoke(otherLink, true);
                            break;
                        case 1:
                            OnJoin.Invoke(otherLink, false);
                            break;
                        case 2: OnJoin.Invoke(otherLink, i == 0? false : true);
                            break;
                    }
            }
        }
    }
}
