using System.Collections.Generic;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Collections;

public class PeerTracker : Singleton<PeerTracker>
{
    private Dictionary<string, GhostPlayer> ghostPlayers = new Dictionary<string, GhostPlayer>()
    {
    };


    public void UpdateGhostPositions(IEnumerable<GhostPosition> ghosts)
    {
        var ghostsToUpdate = new Dictionary<string, GhostPlayer>(ghostPlayers);

        foreach (var ghost in ghosts)
        {
            //Ignore the player's own ghost
            if (ghost.name == SessionStateStore.PlayerId)
                continue;

            //If the ghost does not exist create it
            if (!ghostPlayers.ContainsKey(ghost.name))
            {
                var newGhost = Instantiate(
                    //TODO: Put this prefab path in a dictionary somewhere
                    Resources.Load("Prefabs/GhostPlayer", typeof (GameObject)),
                    Vector3.zero,
                    new Quaternion()) as GameObject;

                newGhost.name = "Ghost_" + ghost.name;

                var ghostPlayerInstance = newGhost.GetComponent<GhostPlayer>();

                ghostPlayerInstance.Initialize(
                    ghost.name,
                    ghost.position.ParseToVector3());

                ghostPlayers.Add(ghost.name, ghostPlayerInstance);
            }
            //If they do, update them
            else
            {
                //I was getting a null reference around here maybe this will help
                if (ghostPlayers[ghost.name] != null && ghost.position != null)
                {
                    ghostPlayers[ghost.name].UpdatePosition(ghost.position.ParseToVector3());
                }

                ghostsToUpdate.Remove(ghost.name);
            }
        }
        //Destroy any ghosts that were not updated
        foreach (var ghostPlayer in ghostsToUpdate)
        {
            ghostPlayers.Remove(ghostPlayer.Key);
            ghostPlayer.Value.Remove();
        }

    }

    
}
