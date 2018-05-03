using RenzeTD.Scripts.Data;
using UnityEngine;

namespace RenzeTD.Scripts.Level.LevelEditor {
    public class EditorCheck : MonoBehaviour {
        public bool IsEditButtons;
        
        private void Start() {
            if (IsEditButtons) {
                gameObject.SetActive(FindObjectOfType<PreservedData>().InEditMode);
            } else {
                gameObject.SetActive(!FindObjectOfType<PreservedData>().InEditMode);
            }
        }
    }
}