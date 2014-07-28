using System;
using System.Linq;
using System.Security.Cryptography;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OffAirCardCycle : MonoBehaviour
{
    private List<TitleCard> _titleCards = new List<TitleCard>();
    private int _treasureTotal;

    // Use this for initialization
    void Start()
    {
        _titleCards = GetComponentsInChildren<TitleCard>().ToList();
        StartCoroutine(CycleCards());

        _treasureTotal = UserProgressStore.Instance.GetGrandTotal();
    }


    IEnumerator CycleCards()
    {
        while (true)
        {
            foreach (var currentCard in _titleCards)
            {
                foreach (var titleCard in _titleCards)
                {
                    titleCard.gameObject.SetActive(false);
                }
                currentCard.gameObject.SetActive(true);
                yield return new WaitForSeconds(5f);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private const float padding = 32f;
    private const float boxHeight = 64f;
    private float boxWidth = Screen.width / 2f - (padding * 2f);
    void OnGUI()
    {
        if (_treasureTotal != 0)
        {
            GUI.Box(new Rect(
                padding,
                Screen.height - (padding + boxHeight),
                boxWidth,
                boxHeight
            ),
            "You collected <b>" + _treasureTotal.ToString("D2") + "</b> treasures over the last airing! Wow!",
            GuiManager.Instance.textBoxStyle);
        }

        if (SessionStateStore.GetSceneState(Scene.Hub) == State.InActive)
        {
            GUI.Box(new Rect(
                Screen.width - (boxWidth + padding),
                Screen.height - (padding + boxHeight),
                boxWidth,
                boxHeight
            ),
            "Next airing in <b>" + Convert.ToInt32(ScheduleTracker.Instance.NextAirTime()).ToString("D2") + "</b> minutes",
            GuiManager.Instance.textBoxStyle);
        }

        
    }
}
