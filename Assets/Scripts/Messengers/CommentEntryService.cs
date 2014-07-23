using Assets.Common;
using Assets.Scripts.GameState;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class CommentEntryService : Singleton<CommentEntryService>
{
    private string playerName;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool commentMode = true;
    void OnGUI()
    {
        if (commentMode)
        {
            if (GuiManager.Instance.PostComment())
            {
                ConfirmButtonPressed();
                commentMode = false;
            }
            else if (GuiManager.Instance.CancelPressed())
            {
                commentMode = false;
            }
        }
        else if (GuiManager.Instance.CommentButtonPressed)
        {
            commentMode = true;
        }
    }

    void ConfirmButtonPressed()
    {
        var comment = new UserComment
        {
            Name = GuiManager.Instance.PlayerNameInput,
            Content = GuiManager.Instance.PlayerMessageInput,
            Mood = "Unknown",
            Location = Application.loadedLevelName,
            WorldPositon = FindObjectOfType<InfoPlayer>().transform.position.ToString()
        };

        CommentsManager.Instance.PostComment(comment);
    }
}
