using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Map.Pathing {
    public class NodePath : IEnumerable<Node> {

        /// <summary>
        /// The nodes to follow, where the Queue is in a FIFO format
        /// </summary>
        private readonly Queue<Node> Path;

        /// <summary>
        /// Returns the Enumerator of Path to allow iteration of the object
        /// </summary>
        /// <returns>Enumerator of Path</returns>
        IEnumerator<Node> IEnumerable<Node>.GetEnumerator() {
            return Path.GetEnumerator();
        }

        /// <summary>
        /// Returns the Enumerator of Path to allow iteration of the object
        /// </summary>
        /// <returns>Enumerator of Path</returns>
        public IEnumerator GetEnumerator() {
            return Path.GetEnumerator();
        }

        /// <summary>
        /// Creates an instance of nodepath, from the Start to the End node
        /// </summary>
        /// <param name="Start">The start node</param>
        /// <param name="End">The end node</param>
        public NodePath(Node Start, Node End) {
            Path = new Queue<Node>(); //initializes Path as a blank queue
            Node CurrentNode = Start; //sets the current node to the start node
            while (CurrentNode != End) { //until the current node == End node
                Path.Enqueue(CurrentNode); //adds CurrentNode to the queue
                var x = CurrentNode.GetNextNodes(); //gets the next nodes connected to the current node
                var n = x[Random.Range(0, x.Length)]; //gets a Node n from a random index of x
                CurrentNode = n; //sets the Current Node to Node n
            }
        }

        /// <summary>
        /// Gets the next node in the Path
        /// </summary>
        /// <returns>Next Node</returns>
        public Node Dequeue() {
            return Path.Dequeue();
        }
    }
}