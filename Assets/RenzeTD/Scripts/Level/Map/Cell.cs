using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NUnit.Framework;
using RenzeTD.Scripts.Misc;
using UnityEngine;
using UnityEngine.WSA;

namespace RenzeTD.Scripts.Level.Map {
    [RequireComponent(typeof(SpriteRenderer))]
    [DataContract]
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
        [DataMember]
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
            tag = "Tile";
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
            switch (t) {
                case Cell.Type.UpDown:
                    return Resources.Load<Sprite>(Settings.Level.TileLocation + "Up-Down");
                case Cell.Type.UpLeft:
                    return Resources.Load<Sprite>(Settings.Level.TileLocation + "Up-Left");
                case Cell.Type.UpRight:
                    return Resources.Load<Sprite>(Settings.Level.TileLocation + "Up-Right");
                case Cell.Type.UpTJunc:
                    return Resources.Load<Sprite>(Settings.Level.TileLocation + "Up-TJunc");
                case Cell.Type.DownLeft:
                    return Resources.Load<Sprite>(Settings.Level.TileLocation + "Down-Left");
                case Cell.Type.DownRight:
                    return Resources.Load<Sprite>(Settings.Level.TileLocation + "Down-Right");
                case Cell.Type.DownTJunc:
                    return Resources.Load<Sprite>(Settings.Level.TileLocation + "Down-TJunc");
                case Cell.Type.LeftRight:
                    return Resources.Load<Sprite>(Settings.Level.TileLocation + "Left-Right");
                case Cell.Type.Turret:
                    return Resources.Load<Sprite>(Settings.Level.TileLocation + "Turret");
                case Cell.Type.Empty:
                    return Resources.Load<Sprite>(Settings.Level.TileLocation + "Empty");
                default:
                    throw new Exception("Unhandled Tile Type");
            }
        }
    }
    
    [DataContract]
    [Serializable]
    public class CellHolder {
        [Serializable]
        [DataContract]
        public class CellArray {
            public List<GameObject> Objects;
            [DataMember]
            public List<Cell> Cells;
            public bool Folded;
            
            public CellArray(int x) {
                Objects = new List<GameObject>();
                Cells = new List<Cell>();
            }
        }

        [DataMember]
        public CellArray[] Holder;
        [SerializeField]
        private int items = 0;
        
        public CellHolder(int x, int y) {
            Holder = new CellArray[x];
            for (int i = 0; i < y; i++) {
                Holder[i] = new CellArray(y);
            }
        }
    }
}