using UnityEngine;

namespace RenzeTD.Scripts.Level.Map {
    public class Map : MonoBehaviour {

        public MapData Data;

        private void Start() {
            if (Data == null) {
                Data = new MapData(gameObject);
                if (!Data.Filled) {
                    Data.FillMap();
                }
            }
        }

    }
}