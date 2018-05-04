using System.Linq;
using RenzeTD.Scripts.Level.Map.Pathing;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(Node))]
    public class NodeEditor : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            Node node = (Node) target; //casts the target object to a Node
            
            EditorGUI.BeginDisabledGroup(true); //prevents a user from modifying values in the field
            if (node.isStart) { //if the node is the start node
                EditorGUILayout.TextField("Starting Node"); //adds a 'title' text field stating it is the starting node
            } else if (node.isEnd) { //if the node is the end node
                EditorGUILayout.TextField("Ending Node"); //adds a 'title' text field stating it is the ending node
            }
            EditorGUILayout.TextField($"Node Value: {node.Value}"); //creates a text field showing the current value
            EditorGUI.EndDisabledGroup(); //end the group of disabled fields

            EditorGUILayout.PrefixLabel("Connected Nodes"); //adds a label to the next obejct

            if (node.ConnectedNodes.Count == 0) { //if there are no connected nodes
                EditorGUI.BeginDisabledGroup(true); //begin another disabled group
                EditorGUILayout.TextField("Empty"); //add a text field stating 'Empty'
                EditorGUI.EndDisabledGroup(); //end the disabled group
            } else { //if there are connected nodes
                EditorGUILayout.BeginFadeGroup(node.Folded); //begin a Fade group, using the node.folded value as a reference
                EditorGUI.BeginDisabledGroup(true); //begins a disabled group of fields
                var sorted = node.ConnectedNodes.OrderBy(o => o.Value).ToList(); //gets a list of the connected nodes in a list of lowest -> greatest
                for (int i = 0; i < sorted.Count; i++) { //for each item in the list
                    string text = //sets the text variable to 1 of 2 possible values
                        i == 0 && //if the first index and
                        !node.isStart ? //is not the start node
                            "Previous Node" : //then return the string 'Previous Node'
                            "Next Node"; //else return the string 'Next Node'
                    //renders the default editor for the node, with a label that has the value of 'text'
                    EditorGUILayout.ObjectField(text, sorted[i], typeof(Node), false);
                }
                EditorGUI.EndDisabledGroup(); //ends the group of disabled fields
                EditorGUILayout.EndFadeGroup(); //ends the fade group
            }
        }
    }
}