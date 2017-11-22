using System;
using UnityEngine;
using UnityEngine.WSA;

namespace RenzeTD.Scripts.Level.Map {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Cell : MonoBehaviour {
        private Sprite _cellSprite;
        
        public Sprite CellSprite {
            get { return _cellSprite; }
            set { _cellSprite = value;
                Renderer.sprite = value;
            }
        }
        public Vector2 CellSize => Parent.CellSize;
        public Vector2 CellScale;
        Type _CellType = Type.Empty;
        public Type CellType {
            get { return _CellType; }
            set { this._CellType = value;
                this.name = this.name.Split("::".ToCharArray())[0] + $":: {value}";
                UpdateSprite();
            }
        }
        private MapData Parent => transform.parent.GetComponent<MapData>();

        private SpriteRenderer Renderer => GetComponent<SpriteRenderer>();
        
        private void Start() {
            UpdateSprite();
        }

        void UpdateSprite() {
            CellSprite = CellResource.GetSprite(CellType);
            //Scale Cell
            CellScale.x = Parent.CellSize.x / CellSprite.bounds.size.x;
            CellScale.y = Parent.CellSize.y / CellSprite.bounds.size.y;

            transform.localScale = CellScale;
        }
        
        public enum Type {
            Empty,
            UpDown, UpLeft, UpRight, UpTJunc,
            DownLeft,DownRight, DownTJunc,
            LeftRight,
            Turret
        }
    }

    public static class CellResource {
        public static Sprite GetSprite(Cell.Type t) {
            var start = "Game/Tiles/Tile_";
            switch (t) {
                case Cell.Type.UpDown:
                    return Resources.Load<Sprite>(start + "Up-Down");
                case Cell.Type.UpLeft:
                    return Resources.Load<Sprite>(start + "Up-Left");
                case Cell.Type.UpRight:
                    return Resources.Load<Sprite>(start + "Up-Right");
                case Cell.Type.UpTJunc:
                    return Resources.Load<Sprite>(start + "Up-TJunc");
                case Cell.Type.DownLeft:
                    return Resources.Load<Sprite>(start + "Down-Left");
                case Cell.Type.DownRight:
                    return Resources.Load<Sprite>(start + "Down-Right");
                case Cell.Type.DownTJunc:
                    return Resources.Load<Sprite>(start + "Down-TJunc");
                case Cell.Type.LeftRight:
                    return Resources.Load<Sprite>(start + "Left-Right");
                case Cell.Type.Turret:
                    return Resources.Load<Sprite>(start + "Turret");
                case Cell.Type.Empty:
                    return Resources.Load<Sprite>(start + "Empty");
                default:
                    throw new Exception("Unhandled Tile Type");
            }
        }
    }
    
    [Serializable]
    public class CellHolder {
        [Serializable]
        public class CellArray {
            public GameObject[] Cells;
            public bool Folded;
            
            public CellArray(int x) {
                Cells = new GameObject[x];
            }
        }

        public CellArray[] Holder;
        [SerializeField]
        private int items = 0;
        
        public CellHolder(int x) {
            Holder = new CellArray[x];
            for (int i = 0; i < x; i++) {
                Holder[i] = new CellArray(x);
            }
        }
    }
}