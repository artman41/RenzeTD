using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RenzeTD.Scripts.Misc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RenzeTD.Scripts.Level {
    public class Node : MonoBehaviour{
        
        public Node ParentNode { get; set; }
        private List<Node> _ChildNodes = new List<Node>();
        public Node[] ChildNodes {
            get { return _ChildNodes.ToArray(); }
            set { _ChildNodes = value.ToList();
                UpdateParent(value);
            }
        }
        public Flag Flag { get; set; }

        public void UpdateParent(Node[] n) {
            foreach (var v in n) {
                v.ParentNode = this;
            }
        }
        
        public Node NextNode() {
            return ChildNodes.ElementAt(Random.Range(0, ChildNodes.Length - 1));
        }
        
    }
}