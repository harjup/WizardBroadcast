using System.Collections;
using Assets.Scripts.Managers;
using UnityEditor.VersionControl;
using UnityEngine;
using System.Collections.Generic;

public class NotificationTextDisplay : Singleton<NotificationTextDisplay>
{
    private List<Notification> messages = new List<Notification>();
    public void ShowNotification(string message)
    {
        var notification = new Notification(message);
        var yPos = Screen.height * (2f/3f);
        yPos -= (Screen.height * (1f / 3f)) / Random.Range(1, 12);
        notification.Position = new Vector2(Screen.width + 16, yPos);
        messages.Add(notification);
    }

    //Ugh whatever this guy can handle his own gui stuff
    void OnGUI()
    {
        var workingMessageList = messages.ToArray();
        foreach (var notification in workingMessageList)
        {
            GUI.Label(new Rect(notification.Position.x, notification.Position.y, notification.PixelLength, 32), notification.Content, GuiManager.Instance.textBoxStyle);
            notification.Position.x -= 96 * Time.deltaTime;

            if (notification.Position.x < -notification.PixelLength*1.1f)
            {
                messages.Remove(notification);
            }
        }
    }

}


class Notification
{
    public Notification(string content)
    {
        Content = content;
    }

    public string Content;
    public int PixelLength
    {
        get { return 14 * Content.Length; }
    }
    public Vector2 Position;
}