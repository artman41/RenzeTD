using System.Collections;
using System.Collections.Generic;

namespace RenzeTD.Scripts.Level.Map.Pathing {
    public class NodePath : IEnumerable {

        private readonly List<Node> Path;
        
        public IEnumerator GetEnumerator() {
            return Path.GetEnumerator();
        }

        public NodePath(Node Start, Node End) {
            Path = Start.Pathing(Start, End, Path);
        }
    }
}