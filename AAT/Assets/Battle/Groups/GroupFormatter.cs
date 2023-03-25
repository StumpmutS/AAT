using UnityEngine;

public class GroupFormatter : MonoBehaviour
{
    [SerializeField] private GroupFormations groupFormations;

    public Vector3 GetPosition(int memberCount, int memberIndex, Vector3 targetPosition) //TODO: Vector3 direction
    {
        return groupFormations.Formations[memberCount][memberIndex] + targetPosition;
    }
}