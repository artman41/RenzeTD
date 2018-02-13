using System;
using System.Linq;
using RenzeTD.Scripts.Data;
using RenzeTD.Scripts.Selection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RenzeTD.Scripts.Menu {
    public class Buttons : MonoBehaviour {

        public NavigationType _NavigationType;

        private PreservedData pd;
        
        public enum NavigationType {
            MapSelect,
            Options,
            Level,
            Menu,
            Leaderboard
        }

        void Start() {
            pd = FindObjectOfType<PreservedData>();
        }

        public void ChangeScene(NavigationType nt) {
            _NavigationType = nt;
            ChangeScene();
        }
        
        public void ChangeScene() {
            string sceneName;
            switch (_NavigationType) {
                case NavigationType.MapSelect:
                    sceneName = "Selection";
                    break;
                case NavigationType.Options:
                    sceneName = "Options";
                    break;
                case NavigationType.Level:
                    sceneName = "Level";
                    break;
                case NavigationType.Menu:
                    sceneName = "Menu";
                    break;
                case NavigationType.Leaderboard:
                    sceneName = "Leaderboard";
                    break;
                default:
                    throw new Exception();
            }

            foreach (var s in SceneManager.GetAllScenes()) {
                Debug.Log(s.name);
            }
            SceneManager.LoadScene(sceneName);
        }

        public void MapShortcut() {
            pd.SelectedMap = FindObjectOfType<PreservedData>().AvailableMaps
                .Single(o => o.Name == name);
            ChangeScene(NavigationType.Level);
        }
        
    }
}