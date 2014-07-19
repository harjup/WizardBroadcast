using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//While this is a singleton, it needs to be manually placed for each scene
//so it won't go in the client bootstrapper
public class CheckpointStore : Singleton<CheckpointStore>
{
    public SpawnMarker ActiveSpawnMarker { get; set; }
}
