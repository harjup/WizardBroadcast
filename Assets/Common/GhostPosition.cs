using System;
using UnityEngine;
using System.Collections.Generic;

public class GhostPosition
{
    public string name;
    public string position;
    
     //TODO: Ensure this actually compiles, integrate with server side
        public GhostPosition(string _name, string _position)
        {
            name = _name;
            position = _position;
        }
        
        
        public string ConvertToString(){
        //TODO: Use the fact that ids will only be letters to remove delimiter between name and position
            return String.Format("{0},{1}|", name, position);
        }
        
        public static List<GhostPosition> FromGroupString(string ghostString){
            
            List<GhostPosition> ghostPositions = new List<GhostPosition>();

            string[] ghosts = ghostString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in ghosts)
            {
                ghostPositions.Add(FromString(s));
            }
            
            return ghostPositions;
        }

    public static GhostPosition FromString(string str){
            string[] ghostProperties = str.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in ghostProperties)
            {
                return new GhostPosition(ghostProperties[0],
                                        String.Format("{0},{1},{2}",
                                            ghostProperties[1], 
                                            ghostProperties[2], 
                                            ghostProperties[3]
                                            )
                );
            }
            return null;
        }
}

/*
 [{"name":"player1", "position": "(1,1,1)"},
 {"name":"player1", "position": "(1,1,1)"},
 {"name":"player1", "position": "(1,1,1)"}]
 */
