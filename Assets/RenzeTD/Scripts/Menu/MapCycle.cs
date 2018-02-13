using System;
using RenzeTD.Scripts.Data;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Image = UnityEngine.UI.Image;

namespace RenzeTD.Scripts.Menu {
    public class MapCycle : MonoBehaviour{
        private int currentMap = 0;
        private TimeSpan lastUpdate;

        private PreservedData pd;

        [Tooltip("Delay in seconds before the change to the next map")]
        public int Delay = 10;
        
        TimeSpan _Delay => new TimeSpan(0, 0, Delay);

        private Image Image;
        
        void Start() {
            Image = GetComponent<Image>();
            lastUpdate = DateTime.Now.TimeOfDay.Subtract(_Delay);
            pd = FindObjectOfType<PreservedData>();
        }
        
        void Update() {
            var t = DateTime.Now.TimeOfDay;
            if (t.Subtract(lastUpdate) >= _Delay) {
                lastUpdate = t;
                var m = pd.AvailableMaps[currentMap++];
                name = m.Name;
                Image.sprite = m.CoverSprite;
            }

            if (currentMap == pd.AvailableMaps.Count) {
                currentMap = 0;
            }
        }
        
    }
}