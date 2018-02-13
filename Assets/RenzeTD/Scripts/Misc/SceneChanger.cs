using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RenzeTD.Scripts.Misc {
    public static class SceneChanger {
        
        public enum NavigationType {
            MapSelect,
            Options,
            Level,
            Menu,
            Leaderboard
        }

        public static void ChangeScene(NavigationType nt) {
            string sceneName;
            switch (nt) {
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
    }
}