using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    private Health[] healthThings;
    
    private void Start()
    {
        healthThings = FindObjectsOfType<Health>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var thing in healthThings)
            {
                thing.ModifyHealth(-5f);
            }
        }
    }
}
