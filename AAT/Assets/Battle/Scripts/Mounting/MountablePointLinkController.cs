using System.Collections.Generic;
using UnityEngine;

public class MountablePointLinkController : MonoBehaviour
{
    [SerializeField] private List<BaseMountableController> mountablePoints;
    [SerializeField] private LinkPointController _linkPointStart, _linkPointEnd;

    private List<LinkPointController> _linkPoints = new();
    private bool _active = true;
    private bool _loop;

    private void Awake()
    {
        if (mountablePoints.Count < 1) return;
        _linkPointStart.Setup(this, true);
        _linkPointEnd.Setup(this, false);
        _linkPoints.Add(_linkPointStart);
        _linkPoints.Add(_linkPointEnd);
    }

    private void Deactivate()
    {
        _active = false;
        gameObject.SetActive(false);
    }

    public void CreateLinkDouble(LinkPointController linkPoint1, LinkPointController linkPoint2, List<BaseMountableController> newMounts)
    {
        mountablePoints.AddRange(newMounts);
        if (linkPoint1.Link == linkPoint2.Link)
        {
            SetupClose(linkPoint1, linkPoint2);
            return;
        }
        SetupDouble(linkPoint1, linkPoint2);
    }
    
    public void CreateLinkSingle(LinkPointController linkPointHas, LinkPointController linkPointEnd, List<BaseMountableController> newMounts, bool wallsFlipped = false)
    {
        mountablePoints.AddRange(newMounts);
        SetupSingle(linkPointHas, linkPointEnd, wallsFlipped);
    }

    public void CreateNewLink(LinkPointController linkPoint1, LinkPointController linkPoint2, List<BaseMountableController> newMounts)
    {
        mountablePoints.AddRange(newMounts);
        SetupNew(linkPoint1, linkPoint2);
    }

    private void SetupMounts()
    {
        foreach (var mountable in mountablePoints)
        {
            mountable.SetLink(this);
            mountable.OnInteractableDestroyed += HandleMountDestroyed;
        }
    }
    
    private void HandleMountDestroyed(InteractableController interactable)
    {
        mountablePoints.RemoveAt(mountablePoints.IndexOf(interactable as BaseMountableController));
        //TODO: Should link break?
    }

    #region SetupLinks
    private void SetupSingle(LinkPointController linkPointHas, LinkPointController linkPointEnd, bool wallsFlipped)
    {
        print("single");
        if (linkPointHas.Start)
        {print("has start");
            if (!wallsFlipped) mountablePoints.Reverse();
            mountablePoints.AddRange(linkPointHas.Link.mountablePoints);
            _linkPoints.Add(linkPointEnd);
            _linkPoints.AddRange(linkPointHas.Link._linkPoints);
        }
        else
        {print("has end");
            if (wallsFlipped) mountablePoints.Reverse();
            mountablePoints.InsertRange(0, linkPointHas.Link.mountablePoints);
            _linkPoints.AddRange(linkPointHas.Link._linkPoints);
            _linkPoints.Add(linkPointEnd);
        }

        linkPointEnd.Setup(this, linkPointHas.Start);
        linkPointHas.Link.Deactivate();
        foreach (var linkPoint in _linkPoints)
        {
            linkPoint.ResetLink(this);
        }
        SetupMounts();
    }

    private void SetupDouble(LinkPointController linkPointFrom, LinkPointController linkPointTo)
    {
        print("double");
        if (linkPointFrom.Start == linkPointTo.Start)
        {
            if (linkPointFrom.Start)
            {
                linkPointFrom.Link.mountablePoints.Reverse();
                linkPointFrom.Link._linkPoints.Reverse();
            }
            else
            {
                linkPointTo.Link.mountablePoints.Reverse();
                linkPointTo.Link._linkPoints.Reverse();
            }
        }
        mountablePoints.InsertRange(0, linkPointFrom.Link.mountablePoints);
        mountablePoints.AddRange(linkPointTo.Link.mountablePoints);
        _linkPoints.AddRange(linkPointFrom.Link._linkPoints);
        _linkPoints.AddRange(linkPointTo.Link._linkPoints);
        linkPointFrom.Link.Deactivate();
        linkPointTo.Link.Deactivate();
        foreach (var linkPoint in _linkPoints)
        {
            linkPoint.ResetLink(this);
        }
        SetupMounts();
    }

    private void SetupClose(LinkPointController linkPointFrom, LinkPointController linkPointTo)
    {
        print("close");
        _loop = true;
        if (linkPointFrom.Start)
        {
            mountablePoints.Reverse();
        }
        mountablePoints.AddRange(linkPointFrom.Link.mountablePoints);
        
        _linkPoints = linkPointFrom.Link._linkPoints;
        foreach (var linkPoint in _linkPoints)
        {
            linkPoint.ResetLink(this);
        }
        SetupMounts();
    }

    private void SetupNew(LinkPointController linkPointStart, LinkPointController linkPointEnd)
    {
        linkPointStart.Setup(this, true);
        linkPointEnd.Setup(this, false);
        _linkPoints.Add(linkPointStart);
        _linkPoints.Add(linkPointEnd);
        SetupMounts();
    }
    #endregion

    private List<BaseMountableController> _previewedMountables = new();

    public void BeginPreviewDisplayLink(BaseMountableController mountable, PoolingObject visuals, int unitAmount, bool up)
    {
        if (!_active || mountablePoints.Count <= 1) return;
        _previewedMountables.Clear();
        _previewedMountables.Add(mountable);
        
        int index = mountablePoints.IndexOf(mountable);
        if (_loop) SendLoop(index, visuals, unitAmount, up);
        else SendAltering(index, visuals, unitAmount, up);
    }

    #region Send
    private void SendLoop(int index, PoolingObject visuals, int unitAmount, bool up)
    {
        int mountCount = mountablePoints.Count;
        int upIndex = index + 1;
        if (upIndex == mountCount) upIndex = 0;
        int downIndex = index - 1;
        if (downIndex == -1) downIndex = mountCount - 1;

        for (int i = 0; i < unitAmount; i++)
        {
            if (upIndex == downIndex)
            {
                mountablePoints[upIndex].CallDisplayPreview(visuals, 1);
                _previewedMountables.Add(mountablePoints[upIndex]);
                Debug.Log("Link is full");
                return;
            }
            if (up)
            {
                mountablePoints[upIndex].CallDisplayPreview(visuals, 1);
                _previewedMountables.Add(mountablePoints[upIndex]);
                upIndex++;
                if (upIndex == mountCount) upIndex = 0;
                up = false;
            } 
            else
            {
                mountablePoints[downIndex].CallDisplayPreview(visuals, 1);
                _previewedMountables.Add(mountablePoints[downIndex]);
                downIndex--;
                if (downIndex == -1) downIndex = mountCount - 1;
                up = true;
            }
        }
    }
    
    private void SendAltering(int index, PoolingObject visuals, int unitAmount, bool up)
    {
        int mountCount = mountablePoints.Count;
        int upIndex = index + 1;
        int downIndex = index - 1;

        for (int i = 0; i < unitAmount; i++)
        {
            if (up && upIndex < mountCount)
            {
                mountablePoints[upIndex].CallDisplayPreview(visuals, 1);
                _previewedMountables.Add(mountablePoints[upIndex]);
                upIndex++;
                if (downIndex >= 0) up = false;
            } 
            else if (downIndex >= 0)
            {
                mountablePoints[downIndex].CallDisplayPreview(visuals, 1);
                _previewedMountables.Add(mountablePoints[downIndex]);
                downIndex--;
                if (upIndex < mountCount) up = true;
            }
            else
            {
                Debug.Log("Link is full");
                return;
            }
        }
    }
    #endregion

    public List<BaseMountableController> DetermineMountables(BaseMountableController mountable, int unitCount, bool up)
    {
        if (!_active || mountablePoints.Count <= 1) return new List<BaseMountableController>();
        int index = mountablePoints.IndexOf(mountable);
        var returnList = _loop ? DetermineLoop(index, unitCount, up) : DetermineAltering(index, unitCount, up);
        returnList.Add(mountable);
        return returnList;
    }

    private List<BaseMountableController> DetermineLoop(int index, int unitAmount, bool up)
    {
        List<BaseMountableController> mountables = new();
        int mountCount = mountablePoints.Count;
        int upIndex = index + 1;
        if (upIndex == mountCount) upIndex = 0;
        int downIndex = index - 1;
        if (downIndex == -1) downIndex = mountCount - 1;
        
        for (int i = 0; i < unitAmount; i++)
        {
            if (upIndex == downIndex)
            {
                mountables.Add(mountablePoints[upIndex]);
                Debug.Log("Link is full");
                return mountables;
            }
            if (up)
            {
                mountables.Add(mountablePoints[upIndex]);
                upIndex++;
                if (upIndex == mountCount) upIndex = 0;
                up = false;
            } 
            else
            {
                mountables.Add(mountablePoints[downIndex]);
                downIndex--;
                if (downIndex == -1) downIndex = mountCount - 1;
                up = true;
            }
        }

        return mountables;
    }

    private List<BaseMountableController> DetermineAltering(int index, int unitAmount, bool up)
    {
        List<BaseMountableController> mountables = new();
        int mountCount = mountablePoints.Count;
        int upIndex = index;
        int downIndex = index;
        if (up) upIndex += 1;
        else downIndex -= 1;//TODO: test and also put in determineloop
        
        for (int i = 0; i < unitAmount; i++)
        {
            if (up && upIndex < mountCount)
            {
                mountables.Add(mountablePoints[upIndex]);
                upIndex++;
                if (downIndex >= 0) up = false;
            } 
            else if (downIndex >= 0)
            {
                mountables.Add(mountablePoints[downIndex]);
                downIndex--;
                if (upIndex < mountCount) up = true;
            }
            else
            {
                Debug.Log("Link is full");
                return mountables;
            }
        }

        return mountables;
    }
}
