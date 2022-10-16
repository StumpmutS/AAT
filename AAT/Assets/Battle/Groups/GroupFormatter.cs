using UnityEngine;

public class GroupFormatter : MonoBehaviour
{
    [SerializeField] private GroupFormations groupFormations;

    public Vector3 GetPosition(int groupNumber, int memberIndex, Vector3 targetPosition)
    {
        return groupFormations.Formations[groupNumber][memberIndex] + targetPosition;
    }
}