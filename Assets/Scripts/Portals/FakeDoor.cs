using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GUI;
using Assets.Scripts.Interactables;
using Assets.Scripts.Portals;
using Assets.Scripts.Repository;
using UnityEngine;

class FakeDoor : ExaminableBase, IActivatable
{
    private string _scriptId = "FalseWall"; //All false walls share the same script id at the moment
    private List<TextBag> _textBags;
    private TextBag _currentTextBag;
    private bool isActive = false;
    void Start()
    {
        //_scriptId = gameObject.name;

        _textBags = GetComponentsInChildren<TextBag>().ToList();

        if (_textBags == null)
        {
            return;
        }

        for (var i = 0; i < _textBags.Count; i++)
        {
            var id = _textBags[i].id;
            //If the id is not explicitly specified, set it to a default value
            if (String.IsNullOrEmpty(id))
            {
                id = (i + 1).ToString("D2"); //Its place in the list with a padded zero. EX: "06", "15"
                _textBags[i].id = id;
            }

            var dialog = DialogRepository.Instance.GetDialogBit(_scriptId, id);

            _textBags[i].text = dialog.Text;
            _textBags[i].Name = dialog.Name;

            //Overwrite the given textbag's flag with the flag from the stored version if it exists
            if (dialog.Flag != null) { _textBags[i].flag = dialog.Flag; }
        }

        _currentTextBag = _textBags.First();
    }

    public override IEnumerator Examine(Action callback)
    {
        yield return StartCoroutine(TextboxDisplay.Instance.DisplayText(_currentTextBag.text, _currentTextBag.Name,() =>{}));
        if (isActive)
        {
            if (_examineCallback != null) _examineCallback();

            var parent = transform.parent.gameObject;

            iTween.MoveTo(parent, parent.transform.position.SetY(parent.transform.position.y - 5f), 1f);
            Destroy(gameObject, 2f);
        }
        callback();
    }

    private Action _examineCallback;
    public void Activate(Action callback)
    {
        isActive = true;
        //TODO: less bad etc etc
        _currentTextBag = _textBags[1];

        _examineCallback = callback;
    }
}

