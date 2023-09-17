using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResources
{
    public ResourcesTypes ResourceType {get;}
    public Vector3 ResourcePosition {get;}
    public void Interact(int damage);
}
