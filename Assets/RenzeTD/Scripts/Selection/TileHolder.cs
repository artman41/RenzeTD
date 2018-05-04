using System;
using RenzeTD.Scripts.Data;
using RenzeTD.Scripts.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace RenzeTD.Scripts.Selection {
    public class TileHolder : MonoBehaviour {
        /// <summary>
        /// The Map file the object is representing
        /// </summary>
        public Map Map;
        /// <summary>
        /// A reference to the local PreservedData object
        /// </summary>
        private PreservedData pd;
        
        /// <summary>
        /// A reference to the child object which displays the map CoverImage
        /// </summary>
        private GameObject MapImage;
        
        void Start() {
            pd = FindObjectOfType<PreservedData>(); //points the variable pd to the local PreservedData instance
            MapImage = transform.Find("MapImage").gameObject; //points MapImage to the container for the map CoverImage
            if (Map == null) { //if the holder has no map, throw an error.
                throw new Exception();
            }

            name = Map.Name; //sets the name of the current object to the Map name
            //Debug.LogError(Map.Name);
        }

        void Update() {
            if (MapImage.GetComponent<Image>().sprite == null) { //if the CoverImage object is not displaying the map COverImage
                if (Map != null) { //if there is a Map file
                    MapImage.GetComponent<Image>().sprite = Map.CoverSprite; //sets the sprite of the CoverImage object to the Map CoverImage
                }
            }
        }

        /// <summary>
        /// Handles the mouse click on the object
        /// </summary>
        public void OnClick() {
            pd.SelectedMap = Map; //sets the selected map to the Map the current object represents
            SceneChanger.ChangeScene(SceneChanger.NavigationType.Level); //changes the scene to Level
        }
        
    }
}