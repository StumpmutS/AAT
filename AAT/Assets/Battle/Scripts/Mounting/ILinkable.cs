using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILinkable
{
    public event Action<MountablePointLinkController, bool> OnJoin;
}
