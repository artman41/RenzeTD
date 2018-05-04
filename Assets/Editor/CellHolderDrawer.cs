using System.Threading;
using RenzeTD.Scripts.Level;
using RenzeTD.Scripts.Level.Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Editor {
    [CustomPropertyDrawer(typeof(CellHolder))]
    public class CellHolderDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property); //allows the object to be used by SerializedProperty
            var items = property.FindPropertyRelative("items"); //gets the variable items
            items.intValue = 0; //sets the value to 0
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label); //creates the default label
            
            position.x += 20f; //increases the default x pos by 20 units
            var newPos = position; //creates a new position using position as a base
            
            newPos.y += 18f; //increases the y of the newPosition by 18 units
            
            var Rows = property.FindPropertyRelative("Holder"); //gets the Rows array in the object

            for (int i = 0; i < Rows.arraySize; i++) { //iterates through the rows
                var Row = Rows.GetArrayElementAtIndex(i).FindPropertyRelative("Cells"); //gets the row at index i
                newPos.height = 15; //sets the height of the newPos to 15 units
                newPos.width = 200;//sets the width of the newPos to 200 units
                var f = Rows.GetArrayElementAtIndex(i).FindPropertyRelative("Folded"); //gets the folded variable of the current row
                //creates a foldout grouping where the folded variable is equal to its return value,
                //    where the current value of isFolded is the folded variable 
                f.boolValue = EditorGUI.Foldout(newPos, f.boolValue, $"Row {i}");
                items.intValue++; //increase the inspector item count by 1
                
                if (f.boolValue) { //if the foldout is meant to be showing items
                    for (int j = 0; j < Row.arraySize; j++) { //iterate through the cells in the row
                        newPos.x = position.x; //resets the xPos
                        newPos.y += newPos.height + 5; //sets the y to the height + 5 units
                        
                        EditorGUI.LabelField(newPos, $"{j}"); //creates a label with the value of the row number
                        newPos.x += 15; //increases the x by 15 units to indent the next object
                        EditorGUI.PropertyField(newPos, Row.GetArrayElementAtIndex(j), GUIContent.none); //displays the cell at index j in the inspector
                        newPos.x += newPos.width + 20; //increases the x to be the width plus a spacing of 20 units
                        
                        items.intValue++; //increase the amount of inspector items by 1
                    }

                    newPos.x = position.x; //reset the newPosition x value
                    newPos.y += 20; //increases the y by 20 units to space out the rows
                } else { //if the foldout should not be showing items
                    newPos.y += 15; //increases the y by 15 units to prevent overlapping with the bottom of the inspector
                }
            }
         EditorGUI.EndProperty(); //ends modification and accessing of the object
        }

        /// <summary>
        /// returns the total height of the component when in the inspector
        /// </summary>
        /// <param name="property">the current object</param>
        /// <param name="label">the label to be shown</param>
        /// <returns>the height as a float</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) + //gets the default height
                   (property.FindPropertyRelative("items").intValue * 20f); //increases it by the amount of items * 20
        }
    }
}