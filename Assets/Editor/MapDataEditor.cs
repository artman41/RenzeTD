using RenzeTD.Scripts.Level;
using RenzeTD.Scripts.Level.Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Editor {
    [CustomEditor(typeof(MapData))]
    public class MapDataEditor : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            MapData md = (MapData) target;
            //rows, columns, grid offset, array

            md.Rows = EditorGUILayout.IntField("Rows", md.Rows);
            md.Columns = EditorGUILayout.IntField("Columns", md.Columns);

            md.GridOffset = EditorGUILayout.Vector2Field("Grid Offset", md.GridOffset);
            
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CellHolder"));
            serializedObject.ApplyModifiedProperties();

            md.name = EditorGUILayout.TextField("Map Name", md.name);

            if (GUILayout.Button("Save Map")) {
                md.SaveMap(name);
            }

        }

    }
}