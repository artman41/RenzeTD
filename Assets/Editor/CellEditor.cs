using RenzeTD.Scripts.Level.Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace Editor {
    [CustomEditor(typeof(Cell))]
    public class CellEditor : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            Cell cell = (Cell) target;

            cell.CellScale = EditorGUILayout.Vector2Field("Cell Scale", cell.CellScale);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(cell.CellSprite.texture);
            cell.CellType = (Cell.Type)EditorGUILayout.EnumPopup("Cell Type", cell.CellType);
            EditorGUILayout.EndHorizontal();
        }
    }
}