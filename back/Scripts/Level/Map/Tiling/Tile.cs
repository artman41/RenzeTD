using System;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Map.Tiling {
    public class Tile : MonoBehaviour {

        public SpriteRenderer Renderer;
        public Node TileNode;
        private Type _TileType = Type.Empty;
        public Type TileType {
            get { return _TileType; }
            set { _TileType = value; UpdateSprite(); }
        }

        void Start() {
            if (TileNode == null) {
                TileNode = GetComponent<Node>();
            }
            if (GetComponent<RectTransform>() == null) {
                gameObject.AddComponent<RectTransform>();
            }
            Renderer = GetComponent<SpriteRenderer>();
            if (Renderer == null) {
                Renderer = gameObject.AddComponent<SpriteRenderer>();
                var x = DateTime.Now.Ticks.ToString();
                name = $"{name} [seed:{x.Substring(x.Length - UnityEngine.Random.Range(5, 7))}]";
            }
            UpdateSprite();
            //transform.position = new Vector3(float.Parse(name.Substring(5, 1)), 0f, 0f);
        }

        void UpdateSprite() {
            Renderer.sprite = TileResource.GetSprite(TileType);
        }
        
        public enum Type {
            UpDown, UpLeft, UpRight, UpTJunc,
            DownLeft,DownRight, DownTJunc,
            LeftRight,
            Turret, Empty
        }
    }

    public static class TileResource {

        public static Sprite GetSprite(Tile.Type t) {
            var start = "Tiles/Tile_";
            switch (t) {
                    case Tile.Type.UpDown:
                        return Resources.Load<Sprite>(start + "Up-Down");
                    case Tile.Type.UpLeft:
                        return Resources.Load<Sprite>(start + "Up-Left");
                    case Tile.Type.UpRight:
                        return Resources.Load<Sprite>(start + "Up-Right");
                    case Tile.Type.UpTJunc:
                        return Resources.Load<Sprite>(start + "Up-TJunc");
                    case Tile.Type.DownLeft:
                        return Resources.Load<Sprite>(start + "Down-Left");
                    case Tile.Type.DownRight:
                        return Resources.Load<Sprite>(start + "Down-Right");
                    case Tile.Type.DownTJunc:
                        return Resources.Load<Sprite>(start + "Down-TJunc");
                    case Tile.Type.LeftRight:
                        return Resources.Load<Sprite>(start + "Left-Right");
                    case Tile.Type.Turret:
                        return Resources.Load<Sprite>(start + "Turret");
                    case Tile.Type.Empty:
                        return Resources.Load<Sprite>(start + "Empty");
                    default:
                        throw new Exception("Unhandled Tile Type");
            }
        }
        
    }
}