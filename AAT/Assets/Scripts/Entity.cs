using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        GetComponent<IHealth>().ModifyHealth(-2f);
    }
}
