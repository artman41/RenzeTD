using RenzeTD.Scripts.Level;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(Node))]
    public class NodeEditor : UnityEditor.Editor {
        
        public override void OnInspectorGUI() {
            Node node = (Node) target;

            EditorGUI.BeginDisabledGroup(true);
            node.ParentNode = EditorGUILayout.ObjectField("Parent Node", node.ParentNode, typeof(Node), true) as Node;
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.LabelField("Child Nodes");
            serializedObject.Update();
            SerializedProperty tps = serializedObject.FindProperty("ChildNodes");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(tps, true);
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
            node.Flag = (Flag)EditorGUILayout.EnumPopup("Node Flag", node.Flag);
        }
    }
}