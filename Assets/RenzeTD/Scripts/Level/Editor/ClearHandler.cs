using UnityEngine;
using UnityEngine.UI;

namespace RenzeTD.Scripts.Level.Editor {
    public class SaveHandler : MonoBehaviour{

        public void SaveMap() {
            var input = FindObjectOfType<InputField>();
            if (input.text != string.Empty) {
                FindObjectOfType<MapData>().SaveMap(input.text);
            }
        }
        
    }
}