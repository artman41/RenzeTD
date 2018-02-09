using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Map.Pathing {
    [RequireComponent(typeof(Cell))]
    public class Node : MonoBehaviour {
        public List<Node> ConnectedNodes { get; set; } = new List<Node>();
        public float Folded;
        public bool isStart;
        public bool isEnd;
        public int Value = -1;
        public Cell.Type CellType;

        private int[] CellLoc {
            get {
                var n = name.Split(" :: ".ToCharArray())[0].Replace("[", "").Replace("]", "").Split(',');
                return new[] {int.Parse(n[0]), int.Parse(n[1])};
            }
        }

        int[][] PossibleLocations() {
            return PossibleLocations(transform.GetComponent<Cell>().CellType);
        }
        
        int[][] PossibleLocations(Cell.Type t) {
            var posLoc = new List<int[]>();
            switch (t) {
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
                    posLoc.AddRange(new[] {new[] {-1, 0}, new[] {1, 0}, new[] {0, 1}});
                    break;
            }

            return posLoc.ToArray();
        }

        private void Start() {
            CellType = GetComponent<Cell>().CellType;
            if (CellType == Cell.Type.Empty || CellType == Cell.Type.Turret) {
                return;
            }

            List<GameObject> surrounded = new List<GameObject>();
            foreach (Transform t in transform.parent) {
                var loc = t.GetComponent<Node>().CellLoc;
                if ((CellLoc[0] - 1 <= loc[0] && loc[0] <= CellLoc[0] + 1) &&
                    (CellLoc[1] - 1 <= loc[1] && loc[1] <= CellLoc[1] + 1) && t.gameObject != gameObject &&
                    t.GetComponent<Cell>().CellType != Cell.Type.Empty &&
                    t.GetComponent<Cell>().CellType != Cell.Type.Turret) {
                    surrounded.Add(t.gameObject);
                }
            }

            
            var pos = PossibleLocations();
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

            var cell = GetComponent<Cell>();
            var md = transform.parent.GetComponent<MapData>();
            switch (md.StartsFrom) {
                case MapData.Side.Top:
                    if (new[] {Cell.Type.UpDown, Cell.Type.UpLeft, Cell.Type.UpRight, Cell.Type.UpTJunc}.Contains(
                        cell.CellType) && CellLoc[1] == md.Rows - 1) {
                        isStart = true;
                    }

                    break;
                case MapData.Side.Right:
                    if (new[] {
                        Cell.Type.DownRight, Cell.Type.LeftRight, Cell.Type.UpRight, Cell.Type.DownTJunc,
                        Cell.Type.UpTJunc
                    }.Contains(cell.CellType) && CellLoc[0] == md.Columns - 1) {
                        isStart = true;
                    }

                    break;
                case MapData.Side.Bottom:
                    if (new[] {Cell.Type.DownLeft, Cell.Type.DownRight, Cell.Type.DownTJunc, Cell.Type.UpDown}.Contains(
                        cell.CellType) && CellLoc[1] == 0) {
                        isStart = true;
                    }

                    break;
                case MapData.Side.Left:
                    if (new[] {
                        Cell.Type.DownLeft, Cell.Type.LeftRight, Cell.Type.UpLeft, Cell.Type.DownTJunc,
                        Cell.Type.UpTJunc
                    }.Contains(cell.CellType) && CellLoc[0] == 0) {
                        isStart = true;
                    }

                    break;
            }

            switch (md.EndsOn) {
                case MapData.Side.Top:
                    if (new[] {Cell.Type.UpDown, Cell.Type.UpLeft, Cell.Type.UpRight, Cell.Type.UpTJunc}.Contains(
                        cell.CellType) && CellLoc[1] == md.Rows - 1) {
                        isEnd = true;
                    }

                    break;
                case MapData.Side.Right:
                    if (new[] {
                        Cell.Type.DownRight, Cell.Type.LeftRight, Cell.Type.UpRight, Cell.Type.DownTJunc,
                        Cell.Type.UpTJunc
                    }.Contains(cell.CellType) && CellLoc[0] == md.Columns - 1) {
                        isEnd = true;
                    }

                    break;
                case MapData.Side.Bottom:
                    if (new[] {Cell.Type.DownLeft, Cell.Type.DownRight, Cell.Type.DownTJunc, Cell.Type.UpDown}.Contains(
                        cell.CellType) && CellLoc[1] == 0) {
                        isEnd = true;
                    }

                    break;
                case MapData.Side.Left:
                    if (new[] {
                        Cell.Type.DownLeft, Cell.Type.LeftRight, Cell.Type.UpLeft, Cell.Type.DownTJunc,
                        Cell.Type.UpTJunc
                    }.Contains(cell.CellType) && CellLoc[0] == 0) {
                        isEnd = true;
                    }

                    break;
            }

            if (isStart) {
                Value = 0;
            }
        }

        void Update() {
            if (isEnd && Value < 0) {
                transform.parent.GetComponent<MapData>().InitNodes();
            }
        }

        public void SetValue() {
            var prev = ConnectedNodes.OrderByDescending(o => o.Value).First(o => isStart || o.Value != -1 && o.Value > -1);
            
            if (ConnectedNodes.Count != 0) {
                Value = prev != null ? prev.Value + 1 : 0;
            };
            var unset = ConnectedNodes.Where(o=> o != prev).Where(o => o.Value < 0 || (o.CellType == Cell.Type.DownTJunc && CellLoc != new []{o.CellLoc[0] + o.PossibleLocations()[2][0], o.CellLoc[1] + o.PossibleLocations()[2][1]})).ToArray(); //Done so on any loops past a T-Junc it will reset the value to a greater one
            
            if (unset.Length > 1) {
                if (GetComponent<Cell>().CellType != Cell.Type.DownTJunc) {
                    //BRANCH
                   // Parallel.ForEach(unset, (n) => { n.SetValue(); });
                    //TODO: FIX PARALLEL
                } else {
                    //Debug.Log($"[{CellLoc[0]+ PossibleLocations()[2][0]},{CellLoc[1]+ PossibleLocations()[2][1]}]");
                    var next = ConnectedNodes.First(o => o.name.Contains($"[{CellLoc[0]+ PossibleLocations()[2][0]},{CellLoc[1]+ PossibleLocations()[2][1]}]"));
                    try {
                        next?.SetValue();
                    } catch (IndexOutOfRangeException e) {
                        //Debug.Log(e);
                    }
                }
            } else {
                var next = unset.Length != 0 ? unset[0] : null;
                
                try {
                    next?.SetValue();
                } catch (IndexOutOfRangeException e) {
                    //Debug.Log(e);
                }
            }// -- Idea #1: iterate through each node, ended up causing an infinite loop though  */

            
            #region past ideas
            /*if (ConnectedNodes.Count != 0) {
                Value = prev != null ? prev.Value + 1 : 0;
            }
            var unset = ConnectedNodes.Where(o => o.Value < 0).ToArray();
            if (unset.Length > 1) {
                //BRANCH
                #region aside
                #endregion
                
                #region bside
                #endregion
            } else {
                unset[0].SetValue(unset[0]);
            } -- Idea #2: Ended up being unstable and causing too many issues memory wise due to the constant branching*/
#endregion
/*
            //If UNSET then SET
            if (ConnectedNodes.Count != 0 && Value == -1) {
                Value = prev != null ? prev.Value + 1 : 0;
            }

            var unset = ConnectedNodes.Where(o => o.Value < 0).ToArray();
            //BRANCH
            if (unset.Length > 1) {
                unset[0].SetValue();
            }
            //CONTINUE if set
            else if (unset.Length == 0 && !isEnd) {
                var next = ConnectedNodes.OrderByDescending(o => o.Value > 0).First();
                next.SetValue();
            } else {
                try {
                    unset[0].SetValue();
                } catch (IndexOutOfRangeException e) {
                    Debug.Log(e.ToString());
                }
            } //Idea is to iterate through like in #1, but repeat till there are no missing values
  */      }

        public List<Node> Pathing(Node StartNode, Node EndNode, List<Node> Visited) {
            if (Visited.Contains(StartNode)) {
                return null;
            } else {
                Visited.Add(StartNode);
                if (StartNode == EndNode) {
                    var tempList = new List<Node> {StartNode};
                    return tempList;
                } else {
                    var tempList = StartNode.ConnectedNodes.Where(o => !Visited.Contains(o)).OrderBy(o => o.Value)
                        .ToList();
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