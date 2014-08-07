using Assets.Scripts.Interactables;
using UnityEngine;
using System.Collections;

public class UnreadDialogSignal : MonoBehaviourBase
{

    private string currentDialogId = "";
    private Signpost _signpost;
    private GameObject signalPrefab;
    private GameObject signalInstance;

    void Start()
    {
        _signpost = GetComponent<Signpost>();
        signalPrefab = Resources.Load("Prefabs/UnreadSignal") as GameObject;
        CheckStatus();
    }

    void CheckStatus()
    {
        var newTextId = _signpost.GetCurrentTextBag().id;
        if (newTextId != currentDialogId 
            && signalInstance == null)
        {
            signalInstance =
                Instantiate(signalPrefab, transform.position.SetY(transform.position.y + transform.localScale.y*.8f), Quaternion.identity) as
                    GameObject;
            if (signalInstance != null) signalInstance.transform.parent = transform;
        }

        currentDialogId = newTextId;

        if (_signpost.ReadDialogMap.ContainsKey(currentDialogId)
            && _signpost.ReadDialogMap[currentDialogId])
        {
            Destroy(signalInstance);
            signalInstance = null;
        }

        
    }


    public void UpdateStatuses()
    {
        var signals = FindObjectsOfType<UnreadDialogSignal>();
        foreach (var newDialogSignal in signals)
        {
            newDialogSignal.CheckStatus();
        }
    }
}
