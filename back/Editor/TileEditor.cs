using System;
using RenzeTD.Scripts.Level;
using RenzeTD.Scripts.Level.Map.Tiling;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(Tile))]
    public class TileEditor : UnityEditor.Editor{
        
        public override void OnInspectorGUI() {
            Tile tile = (Tile) target;

            tile.Renderer = EditorGUILayout.ObjectField("Sprite Renderer", tile.Renderer, typeof(SpriteRenderer), true) as SpriteRenderer;
            tile.TileNode = EditorGUILayout.ObjectField("Tile Node", tile.TileNode, typeof(Node), true) as Node;
            tile.TileType = (Tile.Type)EditorGUILayout.EnumPopup("Tile Type", tile.TileType);
        }
    }
}