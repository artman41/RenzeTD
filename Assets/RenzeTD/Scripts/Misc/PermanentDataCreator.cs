using RenzeTD.Scripts.Data;
using UnityEngine;

namespace RenzeTD.Scripts.Misc {
    public class PermanentDataCreator : MonoBehaviour
    {

        /// <summary>
        /// Contains a prefab of PreservedData to be instantiated
        /// </summary>
        public GameObject Prefab;
        
        void Start()
        {
            if (GameObject.FindObjectOfType<PreservedData>() == null) //if PreservedData does not exist
            {
                Instantiate(Prefab); //create an instance of Preserved Data
            }
            //This prevents a bug where PreservedData may be duplicated if placed in a scene and the scene is reloaded
        }
        
    }
}