using UnityEngine;
using UnityEngine.UI;

namespace RenzeTD.Scripts.Level.LevelEditor {
    public class ClearHandler : MonoBehaviour {

        public void ClearMap() {
            FindObjectOfType<MapData>().ClearMap();
        }
        
    }
}