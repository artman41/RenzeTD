using UnityEngine;

namespace RenzeTD.Scripts.Level.Map {
    public class MapData {
        public RowData[] Rows { get; set; } = new RowData[9];
        private readonly GameObject parent;
        public bool Filled;

        public MapData(GameObject go) {
            parent = go;
            for (int i = 0; i < go.transform.childCount; i++) {
                var x = go.transform.GetChild(i);
                if (x.name == $"Row {i}") {
                    Rows[i] = x.GetComponent<RowData>();
                }
            }
            if (Rows[8] != null){Filled = true;}
        }
        
        public void FillMap() {
            var go = parent;
            for (int i = 0; i < Rows.Length; i++) {
                var g = new GameObject {
                    name = $"Row {i}"
                };
                g.transform.parent = go.transform;
                g.transform.position = new Vector3(-go.transform.localScale.x, g.transform.localScale.y*i, g.transform.position.z);
                Rows[i] = g.AddComponent<RowData>();
                g.AddComponent<RectTransform>();
                g.transform.position = new Vector3(0f, 0f, 0f);
                Rows[i].FillRow();
            }
        }
    }
}