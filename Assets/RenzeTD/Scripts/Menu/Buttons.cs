using System;
using System.Linq;
using RenzeTD.Scripts.Data;
using RenzeTD.Scripts.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RenzeTD.Scripts.Menu {
    public class Buttons : MonoBehaviour
    { 
        
        /// <summary>
        /// checks whether the gameobject the script is attached to is the button to create a map
        /// </summary>
        public bool isCreateButton;
        /// <summary>
        /// The type of scene to load
        /// </summary>
        public SceneChanger.NavigationType _NavigationType;

        /// <summary>
        /// local reference to the PreservedData Object
        /// </summary>
        private PreservedData pd;

        void Start() {
            pd = FindObjectOfType<PreservedData>(); //points pd to the PreservedData object
        }

        /// <summary>
        /// triggered by internal methods
        /// </summary>
        /// <param name="nt">the Navigation Type</param>
        public void ChangeScene(SceneChanger.NavigationType nt) {
            if (nt == SceneChanger.NavigationType.Level) Debug.LogError($"Selected Map: {pd.SelectedMap.Name}");
            SceneChanger.ChangeScene(nt); //changes the scene to the given Navigation Type
        }
        
        /// <summary>
        /// Triggered by buttons using the inspector
        /// </summary>
        public void ChangeScene()
        {
            if (isCreateButton) pd.InEditMode = true; //if the create button is clicked, set the EditMode to true
            ChangeScene(_NavigationType); //Changes the scene to the local Navigation Type
        }

        /// <summary>
        /// Loads a map when the MapCycle map is clicked
        /// </summary>
        public void MapShortcut() {
            Debug.LogError($"Current map:: {name}");
            Debug.Log(GetComponent<MapCycle>().CurrentMap.Name);
            pd.SelectedMap = GetComponent<MapCycle>().CurrentMap; //Sets the selected map to the currently shown map
            pd.InEditMode = false; //Sets the EditMode to false incase the game is still in edit mode
            ChangeScene(SceneChanger.NavigationType.Level); //Changes the scene to the level
        }
        
    }
}