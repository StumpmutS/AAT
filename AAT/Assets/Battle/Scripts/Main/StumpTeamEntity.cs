using UnityEngine;

public class StumpTeamEntity : MonoBehaviour
{
    [Tooltip("Team numbers start at 1, 0 is no team")] [SerializeField] private int teamNumber;

    public int GetTeam() => teamNumber;
}
