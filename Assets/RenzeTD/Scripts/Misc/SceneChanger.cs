using RenzeTD.Scripts.Data;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RenzeTD.Scripts.Misc {
    public static class SceneChanger {
        
        public enum NavigationType {
            Selection,
            Options,
            Level,
            Menu,
            Leaderboard
        }

        public static string GetSceneName(NavigationType nt) {
            string sceneName;
            switch (nt) {
                case NavigationType.Selection:
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
            return sceneName;
        }

        public static NavigationType GetSceneType(string name) {
            return (NavigationType) Enum.Parse(typeof(NavigationType), name);
        }

        public static void ChangeScene(NavigationType nt) {
            if (nt != NavigationType.Level) GameObject.FindObjectOfType<PreservedData>().InEditMode = false;
            for (int i = 0; i < SceneManager.sceneCount; i++) {
                Debug.Log($"Scene {i} :: {SceneManager.GetSceneAt(i).name}");
            }
            SceneManager.LoadScene(GetSceneName(nt));
        }
    }
}