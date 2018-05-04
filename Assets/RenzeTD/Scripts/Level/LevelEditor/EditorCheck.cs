using RenzeTD.Scripts.Data;
using UnityEngine;

namespace RenzeTD.Scripts.Level.LevelEditor {
    public class EditorCheck : MonoBehaviour {
        /// <summary>
        /// A value inside the inspector which says whether the gameobject the script is attached to
        ///      is the Edit Mode Button container
        /// </summary>
        public bool IsEditButtons;
        
        private void Start() {
            if (IsEditButtons) { //If the current script is executing on the Edit Mode Button container
                gameObject.SetActive(FindObjectOfType<PreservedData>().InEditMode);
            } else { //If the current script is executing on the Play Mode Button container
                gameObject.SetActive(!FindObjectOfType<PreservedData>().InEditMode);
            }
        }
    }
}