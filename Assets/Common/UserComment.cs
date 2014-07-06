using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// THIS CLASS IS COPIED FROM WIZARD CENTRAL SERVER SINCE UNITY WOULDN'T USE THE DLL I GENERATED
// CHANGING PROPERTIES IN HERE IS NOT A GOOD IDEA
namespace Assets.Common
{
    public class UserComment
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Mood { get; set; }
        public Nullable<int> SessionTime { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public string Location { get; set; }
        public string WorldPositon { get; set; }
    }
}
