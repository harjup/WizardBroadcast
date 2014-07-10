using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class UserProgressStore : Singleton<UserProgressStore>
    {
        Dictionary<string, int> _treasureTotals = new Dictionary<string, int>();

        public void AddTreasure()
        {
            
        }
    }
}
