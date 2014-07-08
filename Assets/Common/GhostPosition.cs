using UnityEngine;
using System.Collections;

public class GhostPosition
{
    public string name;
    public string position;
    
    /*TODO: Ensure this actually compiles and intergrates correctly
        public GhostPosition(string _name, string _position)
        {
            name = _name;
            position = _position;
        }
        
        
        public static string ConvertToString(){
        //TODO: Use the fact that ids will only be letters to remove delimiter between name and position
            return String.Format("{0},{1}|", name, position);
        }
        
        public static List<GhostPostiion> FromGroupString(string ghostString){
            
            List<GhostPosition> ghostPositions = new List<GhostPosition>();
            
            string[] ghosts = ghostString.split('|');
            foreach (string s in ghosts)
            {
                ghostPositions.Add(FromString(s));
            }
            
            return ghostPositions;
        }
        
        private static GhostPosition FromString(string str){
            string[] ghostProperties = str.split(',');
            foreach (string s in ghostProperties)
            {
                return new GhostPositon(ghostProperties[0],
                                        String.Format("{0},{1},{2}" 
                                            ghostProperties[1], 
                                            ghostProperties[2], 
                                            ghostProperties[3]
                                            )
                );
            }
        }
        */
    
}

/*
 [{"name":"player1", "position": "(1,1,1)"},
 {"name":"player1", "position": "(1,1,1)"},
 {"name":"player1", "position": "(1,1,1)"}]
 */
