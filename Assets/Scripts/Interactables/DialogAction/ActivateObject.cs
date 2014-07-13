using Assets.Scripts.Portals;
using UnityEngine;

public class ActivateObject : TextAction
{
    [SerializeField]
    private GameObject target;

    public override void Execute()
    {
        var components = target.GetComponents<MonoBehaviour>();
        foreach (var component in components)
        {
            var activatable = component as IActivatable;
            if (activatable == null) continue;

            activatable.Activate();
        }
    }
}

