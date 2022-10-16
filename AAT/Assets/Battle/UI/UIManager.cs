using System.Collections;
using System.Linq;
using Fusion;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<ESelectionType, StumpPanel> uiContainers;

    public static UIManager Instance { get; private set; }

    private ESelectionType _currentSelectionType;
    private SelectableController _currentSelected;
    private bool _selectionSet;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DeactivateAllPanels();
        uiContainers[ESelectionType.None].Activate(null);
    }

    public void ActivatePanel(SelectableController selectable)
    {
        var selectionType = selectable.SelectionType;
        if (_currentSelectionType == selectionType || _selectionSet) return;

        _currentSelected = selectable;
        _selectionSet = true;
        StartCoroutine(CoReset());
        DeactivateAllPanels();
        uiContainers[selectionType].Activate(selectable.gameObject);
    }

    public void DeactivatePanel(SelectableController selectableController)
    {
        if (_currentSelected != selectableController) return;
        var selectionType = SelectionManager.Instance.SelectedType;
        var newSelectable = SelectionManager.Instance.Selected[selectionType].FirstOrDefault();
        if (newSelectable != null)
        {
            ActivatePanel(newSelectable);
        }
        else
        {
            DeactivateAllPanels();
            uiContainers[ESelectionType.None].Activate(null);
        }
    }

    private void DeactivateAllPanels()
    {
        foreach (var panel in uiContainers.Values)
        {
            panel.Deactivate();
        }
    }

    private IEnumerator CoReset()
    {
        yield return new WaitForEndOfFrame();
        _selectionSet = false;
    }
}