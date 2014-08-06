using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Collections.Generic;

public class NotificationTextDisplay : Singleton<NotificationTextDisplay>
{
    private List<Notification> messages = new List<Notification>();
    //Matches on special message attributes with a delimiter, Ex: f123|message goes here
    private Regex _sfxText = new Regex(@"^\w(\d*)\|([\w|\W]+)"); 

    private AudioClip[] _fartClips;
    private AudioSource _fartSource;
    void Start()
    {
        _fartClips = Resources.LoadAll("Sounds/Fart").Cast<AudioClip>().ToArray();
        _fartSource = gameObject.AddComponent<AudioSource>();
    }
    

    public void ShowNotification(string message)
    {
        Match match = _sfxText.Match(message);

        if (match.Success)
        {
            PeerTracker.Instance.AnimateGhost(GhostPlayer.GhostAnim.Fart, match.Groups[1].Value);
            PlaySound(message.Substring(0, 1));
            message = match.Groups[2].Value;
        }

        var notification = new Notification(message);
        var yPos = Screen.height * (2f/3f);
        yPos -= (Screen.height * (1f / 3f)) / Random.Range(1f, 12f);
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

    void PlaySound(string soundString)
    {
        if (soundString == "f")
        {
            _fartSource.PlayOneShot(_fartClips[Random.Range(0, _fartClips.Length)]);
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