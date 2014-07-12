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
        private List<string> _scriptsToLoad =new List<string>()
        {
            {"firstLevel"},
            {"coolSign"}
        };


        List<DialogScriptBag> _scripts = new List<DialogScriptBag>(); 

        void Start()
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
                var scriptBag = JsonConvert.DeserializeObject<DialogScriptBag>(script.text);
                if (scriptBag != null)
                {
                    _scripts.Add(scriptBag);
                }
            }
        }

        //TODO: Use a map to back this so it's not dumb and stupid
        public string GetScript(string scriptId, string textId)
        {
            foreach (var dialogScriptBag in _scripts)
            {
                if (dialogScriptBag.ScriptId == scriptId)
                {
                    foreach (var dialogScript in dialogScriptBag.Content)
                    {
                        if (dialogScript.Id == textId)
                        {
                            return dialogScript.Text;
                        }
                    }
                }
            }
            Debug.LogError(String.Format("Script {0}/{1} not found in scripts", scriptId, textId));
            return null;
        }
    }
}
