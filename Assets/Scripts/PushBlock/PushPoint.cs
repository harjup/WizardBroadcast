using UnityEngine;
using System.Collections;

public class PushPoint : PushableBase
{
    public bool _enabled = true;

    public override PushBlock GetPushBlock()
    {
        return transform.parent.GetComponentInChildren<PushBlock>();
    }

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

    public override bool IsEnabled()
    {
        return _enabled;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BlockStopper>() != null)
        {
            _enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BlockStopper>() != null)
        {
            _enabled = true;
        }
    }
}
