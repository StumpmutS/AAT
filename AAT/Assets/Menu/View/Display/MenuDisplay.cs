using System;
using System.Collections;
using UnityEngine;

public class MenuDisplay : MonoBehaviour
{
    [SerializeField] private SpringController menuSpring;

    public void Activate()
    {
        menuSpring.SetTarget(0);
    }

    public void Deactivate()
    {
        menuSpring.SetTarget(-1);
    }
}