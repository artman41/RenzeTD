using RenzeTD.Scripts.Data;
using UnityEngine;

namespace RenzeTD.Scripts.Level.LevelEditor {
    public class EditorCheck : MonoBehaviour{
        private void Start() {
            gameObject.SetActive(FindObjectOfType<PreservedData>().InEditMode);
        }
    }
}