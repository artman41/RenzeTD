using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using RenzeTD.Scripts.Level.Map;
using RenzeTD.Scripts.Level.Map.Pathing;
using RenzeTD.Scripts.Misc;
using UnityEngine;

namespace RenzeTD.Scripts.Level {
	[DataContract]
	public class MapData : MonoBehaviour {

		public enum Side {
			Top, Bottom,
			Left, Right
		}
		
		public Tuple<bool, FileInfo> isLoading { get; set; } = new Tuple<bool, FileInfo>(true, new FileInfo($"{Settings.Instance.MapDirLocation}map.json"));
		//Grid Specifications
		[DataMember]
		public int Rows;
		[DataMember]
		public int Columns;
		[DataMember]
		public Vector2 GridOffset;
		[DataMember]
		public CellHolder CellHolder;
		[DataMember]
		public Side StartsFrom = Side.Top;
		[DataMember]
		public Side EndsOn = Side.Bottom;
		
		public Vector2 GridSize => new Vector2(Rows, Columns);
		public Vector2 CellSize => new Vector2(GridSize.x/Columns, GridSize.y/Rows);
			
		// Use this for initialization
		void Start () {
			Directory.CreateDirectory(Settings.Instance.MapDirLocation);
			InitCells();
			if (isLoading.Item1) {
				LoadMap(isLoading.Item2);
			}
		}

		void InitCells() {
			GameObject CellObject = new GameObject();
			CellObject.AddComponent<Cell>();
			CellObject.AddComponent<Node>();

			CellHolder = new CellHolder(Rows, Columns);

			var scale = transform.lossyScale;
			
			var positions = new Vector2[Rows,Columns];
			for (int i = 0; i < positions.GetLength(0); i++) {
				for (int j = 0; j < positions.GetLength(1); j++) {
					var x = new Vector2(0, 0);
					if(j < (j/2)+0.5){
						x.y = -1f + j;
					} else if ((j / 2) + 0.5 < j) {
						x.y = -1f + j;
					} else {
						x.y = 0f;
					}
					
					if(i < (i/2)+0.5) {
						x.x = -1f + i;
					} else if ((i / 2) + 0.5 < i) {
						x.x = -1f + i;
					} else {
						x.x = 0f;
					}

					//x.x -= 0.5545906f;
					//x.y -= -0.002623327f;

					positions[i, j] = x;
				}
			}
			
			for (int i = 0; i < Rows; i++) {
				for (int j = 0; j < Columns; j++) {
					var pos = new Vector2(j * CellSize.x + GridOffset.x, i * CellSize.y + GridOffset.y);
					CellHolder.Holder[i].Objects.Add(Instantiate(CellObject, pos, Quaternion.identity, transform));
					CellHolder.Holder[i].Cells.Add(CellHolder.Holder[i].Objects[j].GetComponent<Cell>());
					
					CellHolder.Holder[i].Objects[j].name = $"[{i},{j}] :: {CellHolder.Holder[i].Cells[j].CellType}";
					CellHolder.Holder[i].Objects[j].transform.position = positions[i, j] - new Vector2(3f, 3f);
				}
			}
			
			Destroy(CellObject);
		}

		public void InitNodes() {
			Node n = null;
			for (int i = 0; i < Rows; i++) {
				for (int j = 0; j < Columns; j++) {
					if (n != null) {
						break;
					}
					var node = CellHolder.Holder[i].Objects[j].GetComponent<Node>();
					n = node.Value == 0 ? node : null;
				}
				if (n != null) {
					break;
				}
			} //iterates through each node top to bottom until it reaches the start node

			n?.SetValue();
		}

		public void SaveMap(string name) {
			string json = JsonConvert.SerializeObject(this, Formatting.Indented);
			using (var f = new StreamWriter($"{Settings.Instance.MapDirLocation}{name}/map.json")) {
				f.Write(json);
			}
			
		}

		void LoadMap(FileInfo Map) {
			var x = JsonConvert.DeserializeObject<MapData>(File.ReadAllText(Map.FullName));
			Rows = x.Rows;
			Columns = x.Columns;
			GridOffset = x.GridOffset;

			for (int i = 0; i < CellHolder.Holder.Length; i++) {
				for (int j = 0; j < CellHolder.Holder[i].Cells.Count; j++) {
					CellHolder.Holder[i].Cells[j].CellType = x.CellHolder.Holder[i].Cells[j].CellType;
				}
			}
		}

		private void OnDrawGizmos() {
			Gizmos.DrawWireCube(transform.position, GridSize);
		}

	}
}
