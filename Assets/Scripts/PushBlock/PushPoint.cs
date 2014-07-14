using UnityEngine;
using System.Collections;

public class PushPoint : PushableBase
{
    public override Transform GetParent()
    {
        return transform.parent;
    }
    public override Vector3 GetOrientation()
    {
        return transform.eulerAngles;
    }

    public override Vector3 GetPosition()
    {
        return transform.position;
    }
}
