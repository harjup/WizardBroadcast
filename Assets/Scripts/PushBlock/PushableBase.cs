using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;

public class PushableBase : MonoBehaviour {

    public virtual Vector3 GetOrientation()
    {
        Debug.LogError("Called a base method for Pushablebase");
        return Vector3.zero;
    }

    public virtual Vector3 GetPosition()
    {
        Debug.LogError("Called a base method for Pushablebase");
        return Vector3.zero;
    }

    public virtual Vector3 GetPushBlockPosition()
    {
        throw new System.NotImplementedException();
    }

    public virtual Transform GetParent()
    {
        throw new System.NotImplementedException();
    }
}
