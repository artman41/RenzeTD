using RenzeTD.Scripts.Level;
using RenzeTD.Scripts.Level.Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Editor {
    [CustomPropertyDrawer(typeof(CellHolder))]
    public class CellHolderDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            var items = property.FindPropertyRelative("items");
            items.intValue = 0;
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            position.x += 20f;
            var newPos = position;
            
            newPos.y += 18f;
            
            var Rows = property.FindPropertyRelative("Holder");

            for (int i = 0; i < Rows.arraySize; i++) {
                var Row = Rows.GetArrayElementAtIndex(i).FindPropertyRelative("Cells");
                newPos.height = 15;
                newPos.width = 200;
                var f = Rows.GetArrayElementAtIndex(i).FindPropertyRelative("Folded");
                f.boolValue = EditorGUI.Foldout(newPos, f.boolValue, $"Row {i}");
                items.intValue++;
                
                if (f.boolValue) {
                    for (int j = 0; j < Row.arraySize; j++) {
                        if (j % 1 == 0) {
                            newPos.x = position.x;
                            newPos.y += newPos.height + 5;
                        }
                        EditorGUI.LabelField(newPos, $"{j}");
                        newPos.x += 15;
                        EditorGUI.PropertyField(newPos, Row.GetArrayElementAtIndex(j), GUIContent.none);
                        newPos.x += newPos.width + 20;
                        
                        items.intValue++;
                    }

                    newPos.x = position.x;
                    newPos.y += 20;
                } else {
                    newPos.y += 15;
                }
            }
         EditorGUI.EndProperty();   
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) + (property.FindPropertyRelative("items").intValue * 20f);
        }
    }
}