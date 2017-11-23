using System.Linq;
using RenzeTD.Scripts.Level.Map.Pathing;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(Node))]
    public class NodeEditor : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            Node node = (Node) target;
            
            EditorGUI.BeginDisabledGroup(true);
            if (node.isStart) {
                EditorGUILayout.TextField("Starting Node");
            } else if (node.isEnd) {
                EditorGUILayout.TextField("Ending Node");
            }
            EditorGUILayout.TextField($"Node Value: {node.Value}");
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PrefixLabel("Connected Nodes");

            if (node.ConnectedNodes.Count == 0) {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("Empty");
                EditorGUI.EndDisabledGroup();
            } else {
                EditorGUILayout.BeginFadeGroup(node.Folded);
                EditorGUI.BeginDisabledGroup(true);
                var sorted = node.ConnectedNodes.OrderBy(o => o.Value).ToList();
                for (int i = 0; i < sorted.Count; i++) {
                    string text = i == 0 && !node.isStart ? "Previous Node" : "Next Node";
                    EditorGUILayout.ObjectField(text, sorted[i], typeof(Node), false);
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndFadeGroup();
            }
        }
    }
}