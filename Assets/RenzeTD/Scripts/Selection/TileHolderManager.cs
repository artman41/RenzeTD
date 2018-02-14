using RenzeTD.Scripts.Data;
using UnityEngine;

namespace RenzeTD.Scripts.Selection {
    public class TileHolderManager : MonoBehaviour {
        private PreservedData pd;

        public GameObject TileHolderPrefab;
        
        void Start() {
            pd = FindObjectOfType<PreservedData>();
            foreach (var map in pd.AvailableMaps) {
                var go = Instantiate(TileHolderPrefab);
                go.GetComponent<TileHolder>().Map = map;
                go.transform.SetParent(transform);
            }
        }

    }
}