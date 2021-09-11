using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class MountableController : MonoBehaviour
{
    [SerializeField] private MountableUnitData mountData;
    public MountableUnitData MountData => mountData;

    [SerializeField] private List<Transform> mountablePoints;
    public List<Transform> MountablePoints => mountablePoints;

    [SerializeField] private GameObject mountableVisualsPrefab;
    [SerializeField] private Vector3 visualsOffset;

    private UnitController unitController;

    public event Action<MountableController> OnMountableHover = delegate { };
    public event Action<MountableController> OnMountableHoverStop = delegate { };

    private bool _visualsDisplayed;
    private bool _previewDisplayed;

    private void Awake()
    {
        unitController = GetComponent<UnitController>();
        unitController.OnHover += Hover;
        unitController.OnHoverStop += StopHover;
    }

    private void Start()
    {
        MountManager.Instance.AddMountable(this);
        mountableVisualsPrefab = Instantiate(mountableVisualsPrefab, gameObject.transform);
        mountableVisualsPrefab.transform.position += visualsOffset;
        mountableVisualsPrefab.SetActive(false);
    }

    private void Hover()
    {
        OnMountableHover.Invoke(this);
    }

    private void StopHover()
    {
        OnMountableHoverStop.Invoke(this);
    }

    public void DisplayVisuals()
    {
        if (_visualsDisplayed) return;
        _visualsDisplayed = true;
        mountableVisualsPrefab.SetActive(true);
    }

    public void RemoveVisuals()
    {
        if (!_visualsDisplayed) return;
        _visualsDisplayed = false;
        mountableVisualsPrefab.SetActive(false);
    }

    public virtual void DisplayPreview(GameObject transportableUnitPreview, int unitAmount)
    {
        if (_previewDisplayed) return;
        _previewDisplayed = true;
        for (int i = 0; i < unitAmount; i++)
        {
            if (i >= mountablePoints.Count)
            {
                Debug.Log("Not enough mountable points for every unit");
                continue;
            }
            Instantiate(transportableUnitPreview, mountablePoints[i]);
        }
    }

    public void RemovePreview()
    {
        if (!_previewDisplayed) return;
        _previewDisplayed = false;
        foreach (var point in mountablePoints)
        {
            for (int i = 0; i < point.childCount; i++) { 
                var child = point.GetChild(i);
                if (child.GetComponent<UnitController>() == null)
                    Destroy(child.gameObject);
            }
        }
    }
}
