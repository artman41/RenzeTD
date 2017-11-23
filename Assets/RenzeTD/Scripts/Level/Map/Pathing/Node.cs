using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Map.Pathing {
    [RequireComponent(typeof(Cell))]
    public class Node : MonoBehaviour {
        public List<Node> ConnectedNodes { get; set; } = new List<Node>();
        public float Folded;
        public bool isStart;
        public bool isEnd;
        public int Value = -1;

        public int[] CellLoc {
            get {
                var n = name.Split(" :: ".ToCharArray())[0].Replace("[", "").Replace("]", "").Split(',');
                return new [] {int.Parse(n[0]), int.Parse(n[1])};
            }
        }

        private void Start() {
            if (GetComponent<Cell>().CellType == Cell.Type.Empty || GetComponent<Cell>().CellType == Cell.Type.Turret) {
                return;
            }
            List<GameObject> surrounded = new List<GameObject>();
            foreach (Transform t in transform.parent) {
                var loc = t.GetComponent<Node>().CellLoc;
                if ((CellLoc[0] - 1 <= loc[0] && loc[0] <= CellLoc[0] + 1) && (CellLoc[1] - 1 <= loc[1] && loc[1] <= CellLoc[1] + 1) && t.gameObject != gameObject && t.GetComponent<Cell>().CellType!=Cell.Type.Empty && t.GetComponent<Cell>().CellType!=Cell.Type.Turret) {
                    surrounded.Add(t.gameObject);
                }
            }
            var posLoc = new List<int[]>();
            var cell = GetComponent<Cell>();
                switch (cell.CellType) {
                    case Cell.Type.DownLeft:
                        posLoc.AddRange(new[] {new[] {-1, 0}, new[] {0, -1}});
                        break;
                    case Cell.Type.DownRight:
                        posLoc.AddRange(new[] {new[] {1, 0}, new[] {0, -1}});
                        break;
                    case Cell.Type.DownTJunc:
                        posLoc.AddRange(new[] {new[] {-1, 0}, new[] {1, 0}, new[] {0, -1}});
                        break;
                    case Cell.Type.LeftRight:
                        posLoc.AddRange(new[] {new[] {-1, 0}, new[] {1, 0}});
                        break;
                    case Cell.Type.UpDown:
                        posLoc.AddRange(new[] {new[] {0, -1}, new[] {0, 1}});
                        break;
                    case Cell.Type.UpLeft:
                        posLoc.AddRange(new[] {new[] {-1, 0}, new[] {0, 1}});
                        break;
                    case Cell.Type.UpRight:
                        posLoc.AddRange(new[] {new[] {1, 0}, new[] {0, 1}});
                        break;
                    case Cell.Type.UpTJunc:
                        posLoc.AddRange(new[] {new[] {-1, 0}, new[] {1, 0}, new int[] {0, 1}});
                        break;
                }
            /*
            {
                var x = o.GetComponent<Node>();
                return ((xMin + CellLoc[0]) <= x.CellLoc[0] && x.CellLoc[0] <= (xMax + CellLoc[0])) && ((yMin + CellLoc[1]) <= x.CellLoc[1] && x.CellLoc[1] <= (yMax + CellLoc[1]));
            }
            */

            var pos = posLoc.ToArray();
            var conCells = surrounded.Where(o => {
                var n = o.GetComponent<Node>();
                bool con = false;
                for (int i = 0; i < pos.Length; i++) {
                    if (con) {
                        break;
                    }
                    var dLoc = pos[i];
                    con = n.CellLoc[0] == (CellLoc[0] + dLoc[0]) && n.CellLoc[1] == (CellLoc[1] + dLoc[1]);
                }
                return con;
            });
            
            foreach (var c in conCells) {
                ConnectedNodes.Add(c.GetComponent<Node>());
            }

            var md = transform.parent.GetComponent<MapData>();
            switch (md.StartsFrom) {
                case MapData.Side.Top:
                    if (new[] {Cell.Type.UpDown, Cell.Type.UpLeft, Cell.Type.UpRight, Cell.Type.UpTJunc}.Contains(cell.CellType) && CellLoc[1] == md.Rows-1) {
                        isStart = true;
                    }
                    break;
                case MapData.Side.Right:
                    if (new[] {Cell.Type.DownRight, Cell.Type.LeftRight, Cell.Type.UpRight, Cell.Type.DownTJunc, Cell.Type.UpTJunc}.Contains(cell.CellType) && CellLoc[0] == md.Columns-1) {
                        isStart = true;
                    }
                    break;
                case MapData.Side.Bottom:
                    if (new[] {Cell.Type.DownLeft, Cell.Type.DownRight, Cell.Type.DownTJunc, Cell.Type.UpDown}.Contains(cell.CellType) && CellLoc[1]==0) {
                        isStart = true;
                    }
                    break;
                case MapData.Side.Left:
                    if (new[] {Cell.Type.DownLeft, Cell.Type.LeftRight, Cell.Type.UpLeft, Cell.Type.DownTJunc, Cell.Type.UpTJunc}.Contains(cell.CellType) && CellLoc[0] == 0) {
                        isStart = true;
                    }
                    break;
            }
            switch (md.EndsOn) {
                case MapData.Side.Top:
                    if (new[] {Cell.Type.UpDown, Cell.Type.UpLeft, Cell.Type.UpRight, Cell.Type.UpTJunc}.Contains(cell.CellType) && CellLoc[1] == md.Rows-1) {
                        isEnd = true;
                    }
                    break;
                case MapData.Side.Right:
                    if (new[] {Cell.Type.DownRight, Cell.Type.LeftRight, Cell.Type.UpRight, Cell.Type.DownTJunc, Cell.Type.UpTJunc}.Contains(cell.CellType) && CellLoc[0] == md.Columns-1) {
                        isEnd = true;
                    }
                    break;
                case MapData.Side.Bottom:
                    if (new[] {Cell.Type.DownLeft, Cell.Type.DownRight, Cell.Type.DownTJunc, Cell.Type.UpDown}.Contains(cell.CellType) && CellLoc[1]==0) {
                        isEnd = true;
                    }
                    break;
                case MapData.Side.Left:
                    if (new[] {Cell.Type.DownLeft, Cell.Type.LeftRight, Cell.Type.UpLeft, Cell.Type.DownTJunc, Cell.Type.UpTJunc}.Contains(cell.CellType) && CellLoc[0] == 0) {
                        isEnd = true;
                    }
                    break;
            }

            if (isStart) {
                Value = 0;
            }
        }

        void Update() {
            if (isEnd && Value<0) {
                transform.parent.GetComponent<MapData>().InitNodes();
            }
        }

        public void SetValue() {
            if (ConnectedNodes.Count != 0) {
                Value = ConnectedNodes.Max(o => o.Value) + 1;
            }
            var unset = ConnectedNodes.Where(o => o.Value < 0).ToArray();
            for (int i = 0; i < unset.Length; i++) {
                unset[i].SetValue();
            }
        }

        public List<Node> Pathing(Node StartNode, Node EndNode, List<Node> Visited) {
            if (Visited.Contains(StartNode)) {
                return null;
            } else {
                Visited.Add(StartNode);
                if (StartNode == EndNode) {
                    var tempList = new List<Node> {StartNode};
                    return tempList;
                } else {
                    var tempList = StartNode.ConnectedNodes.Where(o => !Visited.Contains(o)).OrderBy(o => o.Value).ToList();
                    foreach (var node in tempList) {
                        var tempList2 = Pathing(node, EndNode, Visited);
                        if (tempList2 != null) {
                            tempList2.Add(node);
                            return tempList2;
                        }
                    }
                    return null;
                }
            }
        }
    }
}