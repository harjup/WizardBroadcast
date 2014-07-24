using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

//Here's a specific volume for setting the fog via player position because I am lazy
//TODO: This is terrible and I hate it ugh these transitions are garbage
public class FogTrigger : MonoBehaviour
{
    public bool activate = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InfoPlayer>() != null)
        {
            if (activate)
            {
                FogMachine.Instance.SetFogForIndoors();
            }
            else
            {
                FogMachine.Instance.DisableFog();
            }
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InfoPlayer>() != null)
        {
            if (!activate)
            {
                FogMachine.Instance.SetFogForIndoors();
            }
            else
            {
                FogMachine.Instance.DisableFog();
            }
        }
    }
}
