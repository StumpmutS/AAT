using UnityEngine;

public interface IMovement
{
    public void SetDestination(Vector3 destination);
    public void Move();
}