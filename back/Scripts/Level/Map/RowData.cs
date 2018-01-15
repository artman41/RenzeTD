using RenzeTD.Scripts.Level.Map.Tiling;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Map {
    public class RowData : MonoBehaviour{
        
        public Tile[] Tiles { get; set; } = new Tile[9];

        public void FillRow() {
            for (int i = 0; i < Tiles.Length; i++) {
                var g = new GameObject();
                g.name = $"Tile {i}";
                g.transform.parent = transform;
                Tiles[i] = g.gameObject.AddComponent<Tile>();
                //g.transform.position = new Vector3(Tiles[i].Renderer.size.x * i, g.transform.position.y, g.transform.position.z);
            }
        }
    }
}