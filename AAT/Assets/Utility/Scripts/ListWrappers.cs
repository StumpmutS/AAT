using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility.Scripts
{
    [Serializable]
    public class ButtonListWrapper
    {
        public List<Button> List;
    }
    
    [Serializable]
    public class GameObjectListWrapper
    {
        public List<GameObject> List;
    }

    [Serializable]
    public class UpgradeDataListWrapper
    {
        public List<UpgradeData> UpgradeData;
    }
}
