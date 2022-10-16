using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Unit Data/Art")]
public class UnitArtData : ScriptableObject
{
    [SerializeField] private List<StylizedTextImage> unitIcon;
    public List<StylizedTextImage> UnitIcon => unitIcon;
    [SerializeField] private UnitStatIcons unitStatIcons;
    public UnitStatIcons UnitStatIcons => unitStatIcons;
    [SerializeField] private GameObject unitPreview;
    public GameObject UnitPreview => unitPreview;
    [SerializeField] private List<Mesh> skins;
    public List<Mesh> Skins => skins;
}
