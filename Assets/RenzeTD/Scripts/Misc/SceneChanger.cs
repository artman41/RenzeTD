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

        /// <summary>
        /// Returns the name of the scene file based on the NavigationType
        /// </summary>
        /// <param name="nt">The type of navigation</param>
        /// <returns>the name of the scene</returns>
        /// <exception cref="Exception">throws an exception if the NavigationType has not been given a name</exception>
        public static string GetSceneName(NavigationType nt) {
            string sceneName; //initalizes the variable first to reduce repeat variables
            switch (nt) { //sets the value of sceneName based upon NavType
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
            return sceneName; //returns the found sceneName
        }

        /// <summary>
        /// Retrieves the NavigationType of a scene when given its name
        /// </summary>
        /// <param name="name">the name of the scene</param>
        /// <returns>equivalent NavigationType</returns>
        public static NavigationType GetSceneType(string name) {
            //attempts to parse the string as a member of the NavigationType enum
            return (NavigationType) Enum.Parse(typeof(NavigationType), name); 
        }

        /// <summary>
        /// Loads a scene based on the NavigationType
        /// </summary>
        /// <param name="nt">The NavigationType</param>
        public static void ChangeScene(NavigationType nt) {
            //sanity check so that EditMode does not persist past the Level scene
            if (nt != NavigationType.Level) GameObject.FindObjectOfType<PreservedData>().InEditMode = false; 
            //Debug code to get all available scenes
            
            /*for (int i = 0; i < SceneManager.sceneCount; i++) {
                Debug.Log($"Scene {i} :: {SceneManager.GetSceneAt(i).name}");
            }*/
            
            //loads the scene based on a value retrieved using the Navigation Type
            SceneManager.LoadScene(GetSceneName(nt));
        }
    }
}