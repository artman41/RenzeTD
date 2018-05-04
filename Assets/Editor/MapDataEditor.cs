using System;
using System.Linq;
using RenzeTD.Scripts.Level;
using RenzeTD.Scripts.Level.Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Editor {
    [CustomEditor(typeof(MapData))]
    public class MapDataEditor : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            MapData md = (MapData) target; //casts the target object to MapData
            //rows, columns, grid offset, array

            //sets the value of Rows in target to the value in an IntField with the default value of Rows
            md.Rows = EditorGUILayout.IntField("Rows", md.Rows);
            //sets the value of Columns in target to the value in an IntField with the default value of Columns
            md.Columns = EditorGUILayout.IntField("Columns", md.Columns);
            //sets the value of GridOffset in target to the value in an IntField with the default value of GridOffset
            md.GridOffset = EditorGUILayout.Vector2Field("Grid Offset", md.GridOffset);
            
            //gets any changes to the object
            serializedObject.Update();
            //attempts to render the default editor for the CellHolder property in the inspector
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CellHolder"));
            //applies any changes to the object
            serializedObject.ApplyModifiedProperties();

            //Sets the name of target to the value of a text field with a default value of target.name
            md.name = EditorGUILayout.TextField("Map Name", md.name);
            
            var start = (MapData.Side)EditorGUILayout.EnumPopup("Enemy Start", md.StartsFrom); //allows a variable to be set to a value in the Side enum
            if (md.EndsOn != start) { //if the side to end on isn't the same as the side chosen above
                md.StartsFrom = start; //the side to start on is set to the variable above
            }
            //update the object's values
            serializedObject.Update(); 
            //render the default editor for the StartNode in the inspector
            EditorGUILayout.PropertyField(serializedObject.FindProperty("StartNode"));
            //render the default editor for the EndNode in the inspector
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EndNode"));
            //apply any changes
            serializedObject.ApplyModifiedProperties();
            
            var end = (MapData.Side) EditorGUILayout.EnumPopup("Enemy End", md.EndsOn); //allows a variable to be set to a value in the Side enum
            if (md.StartsFrom != end) {//if the side to start on isn't the same as the side chosen above
                md.EndsOn = end;//the side to end on is set to the variable above
            }
            
            //creates a button
            if (GUILayout.Button("Save Map")) { //if the button is clicked
                md.SaveMap(md.name); //saves the map with the name of the current instance
            }

        }

    }
}