using UnityEngine;
using UnityEngine.UI;

namespace RenzeTD.Scripts.Level.LevelEditor {
    public class ClearHandler : MonoBehaviour {

        /// <summary>
        /// Triggers ClearMap in the MapData object
        /// </summary>
        public void ClearMap() {
            FindObjectOfType<MapData>().ClearMap();
        }
        
    }
}