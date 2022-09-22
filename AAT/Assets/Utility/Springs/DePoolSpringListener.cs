using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DePoolSpringListener : SpringListener, ISerializationCallbackReceiver
{
    [SerializeField] private PoolingObject poolObj;
    [SerializeField] private SpringController spring;
    
    public override void HandleSpringValue(float amount, float target)
    {
        if (amount < .999f) return;
        
        spring.Reset();
        poolObj.Deactivate();
    }

    public void OnBeforeSerialize()
    {
        useSetValue = true;
    }

    public void OnAfterDeserialize()
    {
        useSetValue = true;
    }
}
