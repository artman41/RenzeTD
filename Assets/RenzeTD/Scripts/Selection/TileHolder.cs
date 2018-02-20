using System;
using RenzeTD.Scripts.Data;
using RenzeTD.Scripts.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace RenzeTD.Scripts.Selection {
    public class TileHolder : MonoBehaviour {
        public Map Map;
        private PreservedData pd;
        
        private GameObject MapImage;
        
        void Start() {
            pd = FindObjectOfType<PreservedData>();
            MapImage = transform.Find("MapImage").gameObject;
            if (Map == null) {
                throw new Exception();
            }

            name = Map.Name;
        }

        void Update() {
            if (MapImage.GetComponent<Image>().sprite == null) {
                if (Map != null) {
                    MapImage.GetComponent<Image>().sprite = Map.CoverSprite;
                }
            }
        }

        public void OnClick() {
            pd.SelectedMap = Map;
            SceneChanger.ChangeScene(SceneChanger.NavigationType.Level);
        }
        
    }
}