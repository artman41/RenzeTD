using RenzeTD.Scripts.Level;
using RenzeTD.Scripts.Level.Map;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(MapData))]
    public class MapDataEditor : UnityEditor.Editor{

        public override void OnInspectorGUI() {
            MapData md = (MapData) target;
            //rows, columns, grid offset, array
            
            /*md.Rows = EditorGUILayout.IntField("Rows", md.Rows);
            md.Columns = EditorGUILayout.IntField("Columns", md.Columns);

            md.GridOffset = EditorGUILayout.Vector2Field("Grid Offset", md.GridOffset);

            EditorGUILayout.BeginVertical("Cell Holder");
            for (int i = 0; i < md.CellHolder.Holder.Length; i++) {
                EditorGUILayout.BeginVertical($"Row {i}");
                for (int j = 0; j < md.CellHolder.Holder[i].Cells.Count; j++) {
                    var x = md.CellHolder.Holder[i].Cells[j];
                    EditorGUILayout.BeginVertical($"Cell [{i}, {j}]");
                    x = EditorGUILayout.)
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();*/
            DrawDefaultInspector();
            name = EditorGUILayout.TextField("Map Name", name);
            if (GUI.Button(EditorGUILayout.GetControlRect(true, 20f), "Save Map")) {
                md.SaveMap(name);
            }
        }
        
    }
}