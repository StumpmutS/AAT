using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Actions/Visuals/Pool Object")]
public class PoolObjectGameAction : VisualGameAction
{
    [SerializeField] private PoolingObject poolingObject;
    [SerializeField] private bool useTransformAsParent;
    
    public override void PerformAction(GameActionInfo info)
    {
            var targetTransform = GetTransform(info.TransformChain);
            var pooledObject = PoolingManager.Instance.CreatePoolingObject(poolingObject);
            pooledObject.transform.position = targetTransform.position;
            pooledObject.transform.rotation = targetTransform.rotation;
            if (useTransformAsParent) pooledObject.transform.parent = targetTransform;
            
            pooledObject.StartCoroutine(CoDeactivatePooledObject(pooledObject));
    }

    private IEnumerator CoDeactivatePooledObject(PoolingObject pooledObject)
    {
        yield return new WaitForSeconds(Duration);
        pooledObject.Deactivate();
    }
}