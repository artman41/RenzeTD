using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Map.Pathing {
    public class NodePath : IEnumerable<Node> {

        private readonly Queue<Node> Path;

        IEnumerator<Node> IEnumerable<Node>.GetEnumerator() {
            return Path.GetEnumerator();
        }

        public IEnumerator GetEnumerator() {
            return Path.GetEnumerator();
        }

        public NodePath(Node Start, Node End) {
            Path = new Queue<Node>();
            Node CurrentNode = Start;
            while (CurrentNode != End) {
                var x = CurrentNode.GetNextNodes();
                var n = x[Random.Range(0, x.Length)];
                Path.Enqueue(n);
                CurrentNode = n;
            }
        }

        public Node Dequeue() {
            return Path.Dequeue();
        }
    }
}