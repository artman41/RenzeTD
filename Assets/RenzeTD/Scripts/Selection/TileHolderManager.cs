using RenzeTD.Scripts.Data;
using UnityEngine;

namespace RenzeTD.Scripts.Selection {
    public class TileHolderManager : MonoBehaviour {
        /// <summary>
        /// A reference to the local PreservedData object
        /// </summary>
        private PreservedData pd;
        /// <summary>
        /// A reference to a TileHolder prefab
        /// </summary>
        public GameObject TileHolderPrefab;
        
        void Start() {
            pd = FindObjectOfType<PreservedData>(); //points the variable pd to the local PreservedData instance
            foreach (var map in pd.AvailableMaps) { //for each available map
                var go = Instantiate(TileHolderPrefab); //create an instance of TileHolder
                go.GetComponent<TileHolder>().Map = map; //set the represented map to Map
                go.transform.SetParent(transform); //make the object a child of the Current Object
            }
        }

    }
}