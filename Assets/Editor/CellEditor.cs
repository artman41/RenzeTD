using RenzeTD.Scripts.Level.Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace Editor {
    [CustomEditor(typeof(Cell))]
    public class CellEditor : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            Cell cell = (Cell) target; //casts the target object as a Cell

            //creates a field in the inspector that modifies the local CellScale variable
            cell.CellScale = EditorGUILayout.Vector2Field("Cell Scale", cell.CellScale);
            EditorGUILayout.BeginHorizontal(); //begins a horizontal grouping
            GUILayout.Label(cell.CellSprite.texture); //displays the sprite sprite of the image to the left
            //creates a field in the inspector that modifies the CellType variable, positioned to the right
            cell.CellType = (Cell.Type)EditorGUILayout.EnumPopup("Cell Type", cell.CellType);
            EditorGUILayout.EndHorizontal(); //ends the horizontal grouping
        }
    }
}