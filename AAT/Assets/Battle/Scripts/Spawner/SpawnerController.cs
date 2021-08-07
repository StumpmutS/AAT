using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;

    private EntityController _entity;
    private List<EntityController> activeEntities;
    
    private void Setup(EntityController entity)
    {
        _entity = entity;
    }






}
