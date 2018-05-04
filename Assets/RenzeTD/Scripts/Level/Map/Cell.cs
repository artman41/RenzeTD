using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization;
using RenzeTD.Scripts.Misc;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Map {
    [RequireComponent(typeof(SpriteRenderer))]
    [DataContract]
    public class Cell : MonoBehaviour {
        /// <summary>
        /// The sprite of the cell
        /// </summary>
        private Sprite _cellSprite;
        
        /// <summary>
        /// The sprite of the cell
        /// </summary>
        public Sprite CellSprite {
            get { return _cellSprite; } //gets the cellSprite
            set { _cellSprite = value; //updates the private var when the property is set
                Renderer.sprite = value; //also updates the renderer sprite to the set value
            }
        }
        
        /// <summary>
        /// returns the CellSize defined in MapData
        /// </summary>
        public Vector2 CellSize => Parent.CellSize;
        /// <summary>
        /// Stores the scale of the cell in relation to cellSize/SpriteBounds
        /// </summary>
        public Vector2 CellScale;
        
        /// <summary>
        /// The current cell type
        /// </summary>
        [DataMember]
        Type _CellType = Type.Empty;
        public Type CellType {
            get { return _CellType; } //returns the local CellType variable
            set { _CellType = value; //updates the local CellType variable to value
                name = name.Split("::".ToCharArray())[0] + $":: {value}"; //updates the object name using value
                UpdateSprite(); //updates the sprite on CellType change
            }
        }

        /// <summary>
        /// Pointer, referring to MapData
        /// </summary>
        private MapData Parent => transform.parent.GetComponent<MapData>();

        /// <summary>
        /// Pointer referring to the SpriteRenderer component on the object
        /// </summary>
        private SpriteRenderer Renderer => GetComponent<SpriteRenderer>();
        
        private void Start() {
            tag = "Tile"; //Sets the tag of the object to Tile
            UpdateSprite(); //Updates the Sprite incase the sprite is incorrect
        }

        /// <summary>
        /// Updates the Sprite based on CellType
        /// Sets CellScale based on CellSprite
        /// Sets localScale to CellScale
        /// </summary>
        void UpdateSprite() {
            CellSprite = CellResource.GetSprite(CellType); //returns Sprite based on CellType
            CellScale.x = Parent.CellSize.x / CellSprite.bounds.size.x; //Sets x value of scale to a ratio of Size.x/Bounds.x
            CellScale.y = Parent.CellSize.y / CellSprite.bounds.size.y; //Sets y value of scale to a ratio of Size.y/Bounds.y

            transform.localScale = CellScale; //sets the local scale to the ratio of CellSize/bounds
        }

        /// <summary>
        /// Triggers when the mouse clicks the cell
        /// </summary>
        private void OnMouseDown() {
            FindObjectOfType<ClickHandler>().FunctionHandler(ClickHandler.ClickType.TURRETMENU, gameObject); //triggers the Function handler with the Type of click and the current object
        } //TODO: possible drawing of maps

        public enum Type {
            Empty,
            UpDown, UpLeft, UpRight, UpTJunc,
            DownLeft,DownRight, DownTJunc,
            LeftRight,
            Turret
        }
    }

    public static class CellResource {
        /// <summary>
        /// Returns the sprite based on Celltype
        /// </summary>
        /// <param name="t">CellType t</param>
        /// <returns>Sprite</returns>
        /// <exception cref="Exception">Throws an exception if the CellType is unknown</exception>
        public static Sprite GetSprite(Cell.Type t) {
            switch (t) {
                case Cell.Type.UpDown:
                    return Resources.Load<Sprite>("Game/Tiles/" + "Tile_Up-Down");
                case Cell.Type.UpLeft:
                    return Resources.Load<Sprite>("Game/Tiles/" + "Tile_Up-Left");
                case Cell.Type.UpRight:
                    return Resources.Load<Sprite>("Game/Tiles/" + "Tile_Up-Right");
                case Cell.Type.UpTJunc:
                    return Resources.Load<Sprite>("Game/Tiles/" + "Tile_Up-TJunc");
                case Cell.Type.DownLeft:
                    return Resources.Load<Sprite>("Game/Tiles/" + "Tile_Down-Left");
                case Cell.Type.DownRight:
                    return Resources.Load<Sprite>("Game/Tiles/" + "Tile_Down-Right");
                case Cell.Type.DownTJunc:
                    return Resources.Load<Sprite>("Game/Tiles/" + "Tile_Down-TJunc");
                case Cell.Type.LeftRight:
                    return Resources.Load<Sprite>("Game/Tiles/" + "Tile_Left-Right");
                case Cell.Type.Turret:
                    return Resources.Load<Sprite>("Game/Tiles/" + "Tile_Turret");
                case Cell.Type.Empty:
                    return Resources.Load<Sprite>("Game/Tiles/" + "Tile_Empty");
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

        /// <summary>
        /// The columns
        /// </summary>
        [DataMember]
        public CellArray[] Holder;
        
        /// <summary>
        /// an index of gui items used by the Cell Inspector Script
        /// </summary>
        [SerializeField]
        #pragma warning disable 0414
        private int items = 0;
        #pragma warning restore 0414
        
        /// <summary>
        /// An object to hold the 'columns' of cells.
        /// Actually used to properly iterate through the cells within the editor
        /// </summary>
        /// <param name="x">amount of columns</param>
        /// <param name="y">amount of rows</param>
        public CellHolder(int x, int y) {
            Holder = new CellArray[x]; //Creates an array of cell array, with a length of X (this array acts as the columns)
            for (int i = 0; i < y; i++) { //while i is less than y
                Holder[i] = new CellArray(y); //sets value of Holder at index i to a new instance of CellArray, with a count of value Y (this object acts as the rows)
            }
            // To get an idea of the format of Holder, it is formatted like this
            //   Columns  |      Rows
            //  Holder[i] |    CellArray
            //        [0] | [0] [1] [2] ... [y]
            //        [1] | [0] [1] [2] ... [y]
            //        ... | [0] [1] [2] ... [y]
            //        [x] | [0] [1] [2] ... [y]
        }
    }
}