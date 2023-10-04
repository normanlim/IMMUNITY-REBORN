using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attack
{
    [field: SerializeField]
    public string AnimationName { get; private set; }

    [field: SerializeField]
    public float TransitionDuration { get; private set; }
}
