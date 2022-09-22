using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutUpdater : MonoBehaviour
{
    private void Update()
    {
        LayoutRebuilder.MarkLayoutForRebuild((RectTransform) transform);
    }
}
