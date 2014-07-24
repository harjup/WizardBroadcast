﻿using Assets.Common;
using Assets.Scripts.GameState;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using HutongGames.PlayMaker.Actions;
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

    private bool commentMode = false;
    private bool commentingAllowed = true;
    void OnGUI()
    {
        if (commentMode)
        {
            InputManager.Instance.PlayerEnteringComment = true;
            GuiManager.Instance.DrawCommentGui = true;
            if (GuiManager.Instance.PostComment())
            {
                ConfirmButtonPressed();
                commentMode = false;
                StartCoroutine(CommentCooldown());
            }
            else if (GuiManager.Instance.CancelPressed())
            {
                commentMode = false;
            }
        }
        else if (commentingAllowed && GUI.Button(new Rect(16, 32 + 16, 96, 32), "Comment") )
        {
            GuiManager.Instance.DrawCommentGui = false;
            commentMode = true;
        }
        else
        {
            InputManager.Instance.PlayerEnteringComment = false;
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

    IEnumerator CommentCooldown()
    {
        commentingAllowed = false;
        yield return new WaitForSeconds(60f);
        commentingAllowed = true;
    }
}