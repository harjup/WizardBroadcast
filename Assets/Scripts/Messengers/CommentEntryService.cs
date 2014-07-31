using Assets.Common;
using Assets.Scripts.GameState;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class CommentEntryService : Singleton<CommentEntryService>
{
    private string playerName;
    private bool commentMode = false;
    private bool commentingAllowed = true;

    void OnGUI()
    {
        if (Application.loadedLevelName == SceneMap.GetScene(Scene.Start)) return;

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
                SoundManager.Instance.Play(SoundManager.SoundEffect.BeepNo);
                commentMode = false;
            }
        }
        else if (commentingAllowed && GUI.Button(new Rect(16, 32 + 16, 96, 32), "Comment") )
        {
            SoundManager.Instance.Play(SoundManager.SoundEffect.BeepShort);
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
        SoundManager.Instance.Play(SoundManager.SoundEffect.BeepYes);

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
