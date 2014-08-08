using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameState;
using Assets.Scripts.Interactables;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class UserProgressStore : Singleton<UserProgressStore>
    {

        //Maps each scene to a map of TreasureTypes and amounts
        private Dictionary<Scene, Dictionary<TreasureType, int>> _treasureTotals;
        public Dictionary<Scene, int> LevelTotalScore;

        private Scene currentScene;

        void Start()
        {
            currentScene = SceneMap.GetSceneFromStringName(Application.loadedLevelName);
            _drawTreasureCount = (currentScene == Scene.Level1
                                    || currentScene == Scene.Level2
                                    || currentScene == Scene.Level3);
            if (_treasureTotals == null)
            {
                Init();
            }
        }
        void OnLevelWasLoaded(int level)
        {
            currentScene = SceneMap.GetSceneFromStringName(Application.loadedLevelName);
            _drawTreasureCount = (currentScene == Scene.Level1 
                                    || currentScene == Scene.Level2 
                                    || currentScene == Scene.Level3);
            if (_treasureTotals == null)
            {
                Init();
                LevelTotalScore[currentScene] = 0;
            }
        }

       

        public void Init()
        {
            _treasureTotals =
                new Dictionary<Scene, Dictionary<TreasureType, int>>()
                {
                    {Scene.Hub,     new  Dictionary<TreasureType, int>()},
                    {Scene.Level1,  new  Dictionary<TreasureType, int>()},
                    {Scene.Level2,  new  Dictionary<TreasureType, int>()},
                    {Scene.Level3,  new  Dictionary<TreasureType, int>()},
                    {Scene.Level4,  new  Dictionary<TreasureType, int>()}
                };

            LevelTotalScore = new Dictionary<Scene, int>()
            {
                    {Scene.Level1, 0},
                    {Scene.Level2, 0},
                    {Scene.Level3, 0},
                    {Scene.Level4, 0},
                    {Scene.Hub, 0},
                    {Scene.Start, 0}
            };
        }

        private bool _drawTreasureCount = false;
        void OnGUI()
        {
            if (_drawTreasureCount)
            {
                var treasureDictionary = GetTreasureTotals(SceneMap.GetSceneFromStringName(Application.loadedLevelName));
                var count = 0;
                foreach (var i in treasureDictionary)
                {
                    count += i.Value;
                }
                UnityEngine.GUI.Label(new Rect((Screen.width / 2f - 25f), 16f, 100f, 25f), count.ToString("D2") + " / " + LevelTotalScore[currentScene].ToString("D2") + " Gems");
            }
        }

        public void AddTreasure(TreasureType treasure)
        {
            var currentScene = SceneMap.GetSceneFromStringName(Application.loadedLevelName);

            Debug.Log(String.Format("Adding {0} to {1}", treasure, currentScene));

            if (currentScene == Scene.Undefined)
            {
                return;
            }
            if (!_treasureTotals.ContainsKey(currentScene))
            {
                _treasureTotals.Add(currentScene, new  Dictionary<TreasureType, int>());
            }

            if (!_treasureTotals[currentScene].ContainsKey(treasure))
            {
                _treasureTotals[currentScene].Add(treasure, 1);
                return;
            }

            _treasureTotals[currentScene][treasure] += 1;
        }

        public Dictionary<TreasureType, int> GetTreasureTotals(Scene scene = Scene.Undefined)
        {
            if (scene == Scene.Undefined)
            {
                //return everything smooshed into a single dictionary. Maybe if can just be another grand total dictionary
            }
            return _treasureTotals[scene];
        }

        public int TreasureTotal(Scene scene = Scene.Undefined)
        {
            var treasureDictionary = GetTreasureTotals(SceneMap.GetSceneFromStringName(Application.loadedLevelName));

            int count = 0;
            if (treasureDictionary.ContainsKey(TreasureType.Little)) count += treasureDictionary[TreasureType.Little];
            if (treasureDictionary.ContainsKey(TreasureType.Medium)) count += treasureDictionary[TreasureType.Medium] * 5;
            if (treasureDictionary.ContainsKey(TreasureType.Large)) count += treasureDictionary[TreasureType.Large] * 10;
            return count;
        }


        public int GetGrandTotal()
        {
            if (_treasureTotals == null) return 0;

            int sum = 0;
            foreach (var levelTotal in _treasureTotals)
            {
                int sum1 = 0;
                foreach (var treasureTotal in levelTotal.Value)
                {
                    int multiplier;
                    //TODO: WHOOOPS!!!!! The point values are duplicated in Treasure.cs!!!
                    switch (treasureTotal.Key)
                    {
                        case TreasureType.Little:
                            multiplier = 1;
                            break;
                        case TreasureType.Medium:
                            multiplier = 5;
                            break;
                        case TreasureType.Large:
                            multiplier = 10;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    sum1 += treasureTotal.Value * multiplier;
                }
                sum += sum1;
            }
            return sum;
        }


        private List<KeyValuePair<int, string>> playerScores = new List<KeyValuePair<int, string>>();
        private IEnumerator _bestScoreRoutine;
        public void GetSubmittedScore(string scoreMsg)
        {
            int score = 0;
            var worked = Int32.TryParse(scoreMsg.Split('|')[1], out score);
            if (!worked) return;

            string name = scoreMsg.Split('|')[0];

            playerScores.Add(new KeyValuePair<int, string>(score, name));

            if (_bestScoreRoutine == null)
            {
                _bestScoreRoutine = DisplayBestScore();
                StartCoroutine(_bestScoreRoutine);
            }
        }

        private string _scoreMessage = null;
        IEnumerator DisplayBestScore()
        {
            yield return new WaitForSeconds(5f);
            var bestScore = playerScores.OrderByDescending(s => s.Key).First();
            _scoreMessage = bestScore.Value 
                                + " collected " 
                                + bestScore.Key
                                + " treasures in the last level! That was the most! Wow!!!!! Everyone please shower "
                                + bestScore.Value 
                                + " with love and affection!!!";

            StartCoroutine(PassiveTextboxDisplay.Instance.DisplayText(_scoreMessage, "Score Wizard", () =>
            {
                _scoreMessage = null;
                _bestScoreRoutine = null;
                playerScores.Clear();
            }));
        }
    }
}
