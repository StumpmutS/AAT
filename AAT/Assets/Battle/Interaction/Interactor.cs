using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private List<EInteractableType> interactableTypes;
    public List<EInteractableType> InteractableTypes => interactableTypes;
}