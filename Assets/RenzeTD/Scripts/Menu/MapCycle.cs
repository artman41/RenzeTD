using System;
using RenzeTD.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace RenzeTD.Scripts.Menu {
    public class MapCycle : MonoBehaviour{
        private int currentMap = 0;
        private TimeSpan lastUpdate;

        private PreservedData pd;

        public TimeSpan Delay = new TimeSpan(0, 0, 10);

        private Image Image;
        
        void Start() {
            Image = GetComponent<Image>();
            lastUpdate = DateTime.Now.TimeOfDay.Subtract(Delay);
            pd = FindObjectOfType<PreservedData>();
        }
        
        void Update() {
            var t = DateTime.Now.TimeOfDay;
            if (t.Subtract(lastUpdate) >= Delay) {
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