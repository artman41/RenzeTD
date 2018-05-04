using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Map.Pathing {
    [RequireComponent(typeof(Cell))]
    [Serializable]
    public class Node : MonoBehaviour {
        /// <summary>
        /// The Nodes connected to the current node
        /// </summary>
        public List<Node> ConnectedNodes { get; set; } = new List<Node>();
        /// <summary>
        /// A float value used by the Node Editor Script
        /// </summary>
        public float Folded;
        /// <summary>
        /// true if this is the Node Enemies start travelling at
        /// </summary>
        public bool isStart;
        /// <summary>
        /// true if this is the Node Enemies travel to
        /// </summary>
        public bool isEnd;
        /// <summary>
        /// The current value of the Node, where bigger = next node
        /// </summary>
        public int Value = -1;
        /// <summary>
        /// The current Type, e.g Up-Down Pipe, Left-Right Pipe
        /// </summary>
        public Cell.Type CellType;

        /// <summary>
        /// Gets the position of the Node based upon the name of the object,
        /// where the name is in the format "CellType :: [X,Y]"
        /// </summary>
        private int[] CellLoc {
            get {
                var n = name.Split(" :: ".ToCharArray())[0].Replace("[", "").Replace("]", "").Split(',');
                return new[] {int.Parse(n[0]), int.Parse(n[1])};
            }
        }

        /// <summary>
        /// Gets the Possible connected Node locations of the current Node
        /// </summary>
        /// <returns>An array of [X,Y] values</returns>
        int[][] PossibleLocations() {
            return PossibleLocations(transform.GetComponent<Cell>().CellType);
        }
        
        /// <summary>
        /// Gets the Possible Connect Node locations of a given CellType
        /// </summary>
        /// <param name="t">CellType</param>
        /// <returns>An array of [X,Y] values</returns>
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
            if (CellType == Cell.Type.Empty || CellType == Cell.Type.Turret) { //Only creates a node for the cell if the Type is not Empty or a Turret
                return;
            }

            List<GameObject> surrounded = new List<GameObject>();
            foreach (Transform t in transform.parent) { //iterates through each object in the parent object
                var loc = t.GetComponent<Node>().CellLoc; //gets the location of the current iteration object
                if (
                        t.gameObject != gameObject && //Checks that the Current Iteration Object is not the current object
                        t.GetComponent<Cell>().CellType != Cell.Type.Empty && t.GetComponent<Cell>().CellType != Cell.Type.Turret &&//If the Iteration Object is not of Type Empty or a Turret
                        (CellLoc[0] - 1 <= loc[0] && loc[0] <= CellLoc[0] + 1) && //Checks whether the current X pos of the Current Iteration Object is either directly to the Left or Right of the current position
                        (CellLoc[1] - 1 <= loc[1] && loc[1] <= CellLoc[1] + 1) //Checks whether the current Y pos of the Current Iteration Object is either directly to the Up or Down of the current position
                    ) {
                    surrounded.Add(t.gameObject); //Adds the Iteration object to a the current list of surrounded objects
                }
            }
            
            //Gets the possible prev/next node locations
            var pos = PossibleLocations();
            //Gets the connected Cells
            var conCells = surrounded.Where(o => { //iterates through surrounded objects and returns an array where the below condition is true
                var n = o.GetComponent<Node>(); //gets the node component of object
                bool con = false; //initialize local variable connected as False
                for (int i = 0; i < pos.Length; i++) { //for each possible location
                    if (con) { //if already know it's a connected node, just skip the for loop
                        break;
                    }

                    var dLoc = pos[i]; //get the possible location at index i
                    con = n.CellLoc[0] == (CellLoc[0] + dLoc[0]) && n.CellLoc[1] == (CellLoc[1] + dLoc[1]); //if the location of the lobject is equal to a possible location, then connected = true
                }

                return con; //returns whether the node is connected
            });

            foreach (var c in conCells) { //foreach item in connected cells
                ConnectedNodes.Add(c.GetComponent<Node>()); //add the node component of the cell to the local array of ConnectedNodes
            }

            var cell = GetComponent<Cell>(); //get the local cell object
            var md = transform.parent.GetComponent<MapData>(); //Get the MapData component
            //Sets the current node to StartNode based upon where the Enemies start from and the current CellType
            switch (md.StartsFrom) { //Gets the side enemies start on
                case MapData.Side.Top: //If side is top
                    if (new[] {Cell.Type.UpDown, Cell.Type.UpLeft, Cell.Type.UpRight, Cell.Type.UpTJunc}
                        .Contains(cell.CellType) && //if the current CellType is UpDown, UpLeft etc.
                        CellLoc[1] == md.Rows - 1 //if the Y == total rows - 1, where bottom left cell is [0,0] and top right cell is [Columns, Rows]
                    ) {
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
            //Sets the current node to EndNode based upon where the Enemies End on and the current CellType, exactly the same as above
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

            if (isStart) { //sets the current value to 0 is the current node is the start node
                Value = 0;
            }
        }

        void Update() {
            if (isEnd && Value < 0) { //if the current node is the end and it's value has not been set
                transform.parent.GetComponent<MapData>().InitNodes(); //initializes all nodes in the map
            }
        }

        /// <summary>
        /// Sets the value of the node to prevNode value + 1
        /// </summary>
        public void SetValue() {
            var nodes = ConnectedNodes.OrderByDescending(o => o.Value); //get connected nodes
            var prev = nodes.FirstOrDefault(o => o.isStart || o.Value > -1); //gets the previous node, where the node is either the start or its value is greater than -1
            
            if (ConnectedNodes.Count != 0) { //if the node has some connected nodes
                Value = prev != null ? prev.Value + 1 : 0; //if the previous node is not null, then current value = prev value + 1, else current value = 0
            }
            var unset = ConnectedNodes
                .Where(o=> o != prev) //gets any nodes that haven't been set already
                .Where(o => o.Value < 0 || //gets any nodes that have a value less than 0
                            //OR
                            o.CellType == Cell.Type.DownTJunc && //gets any nodes where type is a T Junc AND the location isn't to the right
                            CellLoc != new []
                             {
                                 o.CellLoc[0] + o.PossibleLocations()[2][0], o.CellLoc[1] + o.PossibleLocations()[2][1]
                             }
                      )
                .ToArray(); //Done so on any loops past a T-Junc it will reset the value to a greater one
            
            if (unset.Length > 1) { //if there are more than 1 nodes connected that are unset
                if (GetComponent<Cell>().CellType != Cell.Type.DownTJunc) { //if the celltype is not a DownTJunc (therefore an Up Junction)
                    //BRANCH
                   // Parallel.ForEach(unset, (n) => { n.SetValue(); });
                    //TODO: FIX PARALLEL
                } else { //if celltype is a DownTJunc
                    //Debug.Log($"[{CellLoc[0]+ PossibleLocations()[2][0]},{CellLoc[1]+ PossibleLocations()[2][1]}]");
                    var next = ConnectedNodes.First(o => o.name.Contains($"[{CellLoc[0]+ PossibleLocations()[2][0]},{CellLoc[1]+ PossibleLocations()[2][1]}]")); //gets the node underneath the current node
                    try {
                        next?.SetValue(); //attempts to set the value of the node
                    } catch (IndexOutOfRangeException e) {
                        Debug.Log(e);
                    }
                }
            } else { //if the node is not a junction
                var next = unset.Length != 0 ? unset[0] : null; //if there are unset nodes, gets the node, otherwise sets next to null
                
                try {
                    next?.SetValue(); //attempts to set the node value
                } catch (IndexOutOfRangeException e) {
                    Debug.Log(e);
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

        /// <summary>
        /// Gets the Next Nodes,
        /// where the next node has a value greater than the current value
        /// </summary>
        /// <returns>Array of Connected nodes where node.Value > currentValue</returns>
        public Node[] GetNextNodes() {
            return ConnectedNodes.Where(o => o.Value > Value).ToArray();
        }
    }
}