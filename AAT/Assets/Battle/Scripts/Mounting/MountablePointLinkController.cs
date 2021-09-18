using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountablePointLinkController : MonoBehaviour
{
    [SerializeField] private List<BaseMountableController> _mountablePoints;
    public List<BaseMountableController> MountablePoints => _mountablePoints;

    private MountablePointLinkController _parentLink = null;
    private ILinkable linkable;

    private void Awake()
    {
        linkable = GetComponent<ILinkable>();
        if (linkable != null) linkable.OnJoin += JoinLinks;
    }

    public void SetParentLink(MountablePointLinkController link)
    {
        _parentLink = link;
    }

    private void JoinLinks(MountablePointLinkController otherLink, bool insert)
    {
        if (insert)
            _mountablePoints.InsertRange(0, otherLink.MountablePoints);
        else
            _mountablePoints.AddRange(otherLink.MountablePoints);
        otherLink.SetParentLink(this);
        otherLink.enabled = false;
    }

    public void BeginPreviewDisplayLink(BaseMountableController mountable, GameObject visuals, int unitAmount, bool up)
    {
        if (_parentLink != null)
        {
            _parentLink.BeginPreviewDisplayLink(mountable, visuals, unitAmount, up);
            return;
        }
        int index = _mountablePoints.IndexOf(mountable);
        if (index == 0) SendThrough(visuals, unitAmount, true);
        else if (index == _mountablePoints.Count - 1) SendThrough(visuals, unitAmount, false);
        else SendAltering(index, visuals, unitAmount, up);
    }

    private void SendAltering(int index, GameObject visuals, int unitAmount, bool up)
    {
        bool upSend = up;
        int upIndex = index + 1;
        int downIndex = index - 1;

        int mountCount = _mountablePoints.Count;
        for (int i = 0; i < unitAmount; i++)
        {
            if (upSend && upIndex < mountCount)
            {
                _mountablePoints[upIndex].CallDisplayPreview(visuals, 1);
                upIndex++;
                if (downIndex >= 0) upSend = false;
            } 
            else if (downIndex >= 0)
            {
                _mountablePoints[downIndex].CallDisplayPreview(visuals, 1);
                downIndex--;
                if (upIndex < mountCount) upSend = true;
            }
            else
            {
                Debug.Log("Link is full");
            }
        }
    }

    private void SendThrough(GameObject visuals, int unitAmount, bool up)
    {
        if (up)
        {
            for (int i = 1; i < unitAmount; i++)
            {
                if (i >= _mountablePoints.Count) 
                {
                    Debug.Log("Link is full");
                    break;
                }
                _mountablePoints[i].CallDisplayPreview(visuals, 1);
            }
        }
        else
        {
            for (int i = _mountablePoints.Count - 1; i < unitAmount; i--)
            {
                if (i < 0)
                {
                    Debug.Log("Link is full");
                    break;
                }
                _mountablePoints[i].CallDisplayPreview(visuals, 1);
            }
        }
    }
}
