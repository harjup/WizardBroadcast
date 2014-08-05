using System.Linq;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FartMessenger : MonoBehaviourBase
{
    private bool _fartEnabled = false;

    void Start()
    {
        StartCoroutine(FartCoolDown(30f));
    }

    void OnGUI()
    {
        if (_fartEnabled && GUI.Button(new Rect(16, (16 + 32 * 2 + 8), 96, 32), "Fart"))
        {
            var message = "f|" + GuiManager.Instance.PlayerNameInput + " " + GenerateFartMessage();
            SignalrEndpoint.Instance.Broadcast(message);
            NotificationTextDisplay.Instance.ShowNotification(message); // Show the player their own message
            StartCoroutine(FartCoolDown(60f));
        }
    }

    IEnumerator FartCoolDown(float time)
    {
        _fartEnabled = false;
        yield return new WaitForSeconds(time);
        _fartEnabled = true;
    }


    string GenerateFartMessage()
    {
        var messages = new List<string>()
        {
            "passed gas",
            "cut the cheese",
            "found a barking spider",
            "loudly coughed",
            "broke wind",
            "is having stomach pains",
            "is stinking it up",
            "flatulated"
        };
        return messages[Random.Range(0, messages.Count)];
    }
}
