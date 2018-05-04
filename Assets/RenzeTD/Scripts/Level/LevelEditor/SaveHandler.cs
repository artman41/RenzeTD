using UnityEngine;
using UnityEngine.UI;

namespace RenzeTD.Scripts.Level.LevelEditor {
    public class SaveHandler : MonoBehaviour{

        /// <summary>
        /// Saves the map to the Map Directory
        /// </summary>
        public void SaveMap() {
            var input = FindObjectOfType<InputField>(); //gets the input field for the map name
            if (input.text != string.Empty) { //if the field contains text
                FindObjectOfType<MapData>().SaveMap(input.text); //Save the map with the name contained in the input field
            }
        }
        
    }
}