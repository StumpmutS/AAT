using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DataContainerController : MonoBehaviour
{
    [SerializeField] private StumpService service;
    [SerializeField] private InteractableDataContainer mainDataDisplay;
    [SerializeField] private List<DataFilter> dataFilters;

    public UnityEvent<object> OnDataSelected;

    public virtual async void ResetView()
    {
        var data = await service.RequestData();
        foreach (var filter in dataFilters) data = filter.FilterData(data);
        mainDataDisplay.DisplayData(data, HandleDataCallback);
    }

    public virtual void DisableView()
    {
        mainDataDisplay.RemoveDisplay();
        OnDataSelected.Invoke(null);
    }

    private void HandleDataCallback(object data)
    {
        OnDataSelected.Invoke(data);
    }
}