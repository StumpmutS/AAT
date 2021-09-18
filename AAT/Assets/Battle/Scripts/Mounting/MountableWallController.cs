using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountableWallController : BaseMountableController
{
    [SerializeField] private BaseMountableData mountableData;

    public override BaseMountableDataInfo ReturnData() => mountableData.MountableInfo;
}
