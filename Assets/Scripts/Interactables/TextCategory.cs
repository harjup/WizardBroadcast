using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Interactables
{
    /// <summary>
    /// Literally just here so I can easily autogenerate ids by composition instead of
    /// manually renaming each one so it has the right id
    /// </summary>
    class TextCategory : MonoBehaviourBase
    {
        public string id;
        //public List<TextBag> TextBags = new List<TextBag>();
        public List<TextBag> GetTextbags()
        {
            return GetComponentsInChildren<TextBag>().ToList();
        }
    }
}
