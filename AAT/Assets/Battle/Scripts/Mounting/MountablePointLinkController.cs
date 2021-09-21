using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountablePointLinkController : MonoBehaviour
{
    [SerializeField] private List<BaseMountableController> _mountablePoints;
    public List<BaseMountableController> MountablePoints => _mountablePoints;

    private MountablePointLinkController _parentLink = null;
    private List<MountablePointLinkController> _childLinks = new List<MountablePointLinkController>();
    private ILinkable linkable;
    private bool _active = true;

    private void Awake()
    {
        linkable = GetComponent<ILinkable>();
        if (linkable != null) linkable.OnJoin += JoinLinks;
    }

    public void SetChildLink(MountablePointLinkController link, bool insert)
    {
        if (_parentLink != null) 
        {
            _parentLink.SetChildLink(link, insert);
        }
        int newIndex = _mountablePoints.Count - 1;
        _childLinks.Add(link);
        if (insert)
            _mountablePoints.InsertRange(0, link.MountablePoints);
        else
            _mountablePoints.AddRange(link.MountablePoints);
        foreach (var mountable in link.MountablePoints)
        {
            mountable.ResetLink(this);
        }
        link.Deactivate();
    }

    private void Deactivate()
    {
        _mountablePoints.Clear();
        _active = false;
    }

    private void JoinLinks(MountablePointLinkController otherLink, bool insert)
    {
        print(insert? "insertting" : "adding");
        otherLink.SetChildLink(this, insert);
        _parentLink = otherLink;
    }

    public void BeginPreviewDisplayLink(BaseMountableController mountable, PoolingObject visuals, int unitAmount, bool up)
    {
        if (!_active) return;
        int index = _mountablePoints.IndexOf(mountable);
        print(index);
        if (index == 0) SendThrough(visuals, unitAmount, true);
        else if (index == _mountablePoints.Count - 1) SendThrough(visuals, unitAmount, false);
        else SendAltering(index, visuals, unitAmount, up);
    }

    private void SendAltering(int index, PoolingObject visuals, int unitAmount, bool up)
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

    private void SendThrough(PoolingObject visuals, int unitAmount, bool up)
    {
        if (up)
        {
            for (int i = 1; i < unitAmount + 1; i++)
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
            for (int i = _mountablePoints.Count - 2; i > _mountablePoints.Count - (unitAmount + 2); i--)
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
