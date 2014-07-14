using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Resources;
using System.Text;
using Assets.Common.JsonModel;
using Assets.Scripts.Managers;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Repository
{
    class DialogRepository : Singleton<DialogRepository>
    {
        //TODO: Load these with a less terrible method
        private List<string> _scriptsToLoad =new List<string>()
        {
            {"firstLevel"},
            {"secondLevel"},
            {"coolSign"},
            {"treeLover"}
        };


        List<DialogBag> _scripts = new List<DialogBag>(); 

        void Awake()
        {
            LoadLevelScripts();
        }

        public void LoadLevelScripts()
        {
            foreach (var fileName in _scriptsToLoad)
            {
                var script = Resources.Load("Text/" + fileName) as TextAsset;

                if (script == null)
                {
                    Debug.LogError("Expected script to not be null");
                    return;
                }
                Debug.Log("Loaded Script...");
                Debug.Log(script.text);
                var scriptBag = JsonConvert.DeserializeObject<List<DialogBag>>(script.text);
                if (scriptBag != null && scriptBag.Count != 0)
                {
                    _scripts.AddRange(scriptBag);
                }
                else
                {
                    Debug.LogError(fileName + " was not loaded correctly");
                }
            }
        }

        //TODO: Use a map to back this so it's not dumb and stupid
        public Dialog GetDialogBit(string scriptId, string textId)
        {
            foreach (var dialogBag in _scripts)
            {
                if (dialogBag.Id != scriptId) continue;
                foreach (var dialog in dialogBag.Content)
                {                    
                    if (dialog.Id == textId)
                    {
                        return dialog;
                    }
                }
            }
            Debug.LogError(String.Format("Script {0}/{1} not found in scripts", scriptId, textId));
            return null;
        }
    }
}
