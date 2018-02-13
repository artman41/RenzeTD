using System;
using System.Linq;
using RenzeTD.Scripts.Data;
using RenzeTD.Scripts.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RenzeTD.Scripts.Menu {
    public class Buttons : MonoBehaviour {

        public SceneChanger.NavigationType _NavigationType;

        private PreservedData pd;

        void Start() {
            pd = FindObjectOfType<PreservedData>();
        }

        public void ChangeScene(SceneChanger.NavigationType nt) {
            SceneChanger.ChangeScene(nt);
        }
        
        public void ChangeScene() {
            ChangeScene(_NavigationType);
        }

        public void MapShortcut() {
            pd.SelectedMap = FindObjectOfType<PreservedData>().AvailableMaps
                .Single(o => o.Name == name);
            ChangeScene(SceneChanger.NavigationType.Level);
        }
        
    }
}