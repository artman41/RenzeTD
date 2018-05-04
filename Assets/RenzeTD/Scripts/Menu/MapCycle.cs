using System;
using System.CodeDom;
using RenzeTD.Scripts.Data;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Image = UnityEngine.UI.Image;

namespace RenzeTD.Scripts.Menu {
    public class MapCycle : MonoBehaviour {
        /// <summary>
        /// the index of the currently shown map
        /// </summary>
        private int currentMap = 0;
        /// <summary>
        /// Time since the shown map was last changed
        /// </summary>
        private TimeSpan lastUpdate;

        /// <summary>
        /// Reference to the local isntance of PreservedData
        /// </summary>
        private PreservedData pd;
        
        /// <summary>
        /// pointer that retrieves the Map in AvailableMaps at index currentMap
        /// </summary>
        public Map CurrentMap => pd.AvailableMaps[currentMap];

        /// <summary>
        /// The delay in seconds till the map is changed
        /// </summary>
        [Tooltip("Delay in seconds before the change to the next map")]
        public int Delay = 10;
        
        /// <summary>
        /// A time span of the delay to be used when getting the difference between times
        /// </summary>
        TimeSpan _Delay => new TimeSpan(0, 0, Delay);

        /// <summary>
        /// the image to be shown in the object
        /// </summary>
        private Image Image;
        
        void Start() {
            Image = GetComponent<Image>(); //retrieve the image component
            lastUpdate = DateTime.Now.TimeOfDay.Subtract(_Delay); //sets the last update to the current time subtracted the delay, so the current shown map is changed instantly
            pd = FindObjectOfType<PreservedData>(); //points the local pd variable to PreservedData
        }
        
        void Update() {
            var t = DateTime.Now.TimeOfDay; //gets the current Time Of Day, as a timespan
            if (t.Subtract(lastUpdate) >= _Delay) { //checks to see if the difference between current time and last update is greater than or equal to delay
                lastUpdate = t; //sets last updata to the current time
                var m = pd.AvailableMaps[currentMap++]; //gets the map at currentMap and increases currentMap by 1
                name = m.Name; //sets name to name of map
                Image.sprite = m.CoverSprite; //sets image to Map sprite
            }

            if (currentMap == pd.AvailableMaps.Count) { //if current map == number of maps
                currentMap = 0; //current map = index 0
            }
        }
        
    }
}