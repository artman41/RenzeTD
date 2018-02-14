using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using RenzeTD.Scripts.Data;
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

		private PreservedData pd;
		
		// Use this for initialization
		void Start () {
			pd = FindObjectOfType<PreservedData>();
			InitCells();
			if(!pd.InEditMode) LoadMap(pd.SelectedMap.File);
		}

		void InitCells() {
			GameObject CellObject = new GameObject();
			CellObject.AddComponent<Cell>();
			CellObject.AddComponent<Node>();
			CellObject.AddComponent<RectTransform>();
			CellObject.AddComponent<BoxCollider>();
			CellHolder = new CellHolder(Rows, Columns);

			var scale = transform.lossyScale;
			
			var positions = new Vector2[Rows,Columns];
			for (int i = 0; i < positions.GetLength(0); i++) {
				for (int j = 0; j < positions.GetLength(1); j++) {
					var x = new Vector2(1f*i, 1f*j);

					positions[i, j] = x;
				}
			}
			
			for (int i = 0; i < Rows; i++) {
				for (int j = 0; j < Columns; j++) {
					var pos = new Vector2(j * CellSize.x + GridOffset.x, i * CellSize.y + GridOffset.y);
					CellHolder.Holder[i].Objects.Add(Instantiate(CellObject, pos, Quaternion.identity));
					CellHolder.Holder[i].Cells.Add(CellHolder.Holder[i].Objects[j].GetComponent<Cell>());
					
					var go = CellHolder.Holder[i].Objects[j];
					go.name = $"[{i},{j}] :: {CellHolder.Holder[i].Cells[j].CellType}";
					go.transform.position = new Vector3(positions[i, j].x, positions[i, j].y) - new Vector3(4f, 4f, 1f);
					go.transform.SetParent(transform, false);
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
			name = name.Replace(" ", "_");
			string json = JsonConvert.SerializeObject(this, Formatting.Indented);
			var directory = $"{Settings.Instance.MapDirLocation}{name}";
			Directory.CreateDirectory(directory);
			using (var f = new StreamWriter($"{directory}/map.json")) {
				f.Write(json);
			}

			File.WriteAllBytes($"{Settings.Instance.MapDirLocation}{name}/map.png", GetPicture());
		}

		byte[] GetPicture() {
			var cellSprite = CellHolder.Holder[0].Cells[0].GetComponent<SpriteRenderer>().sprite;
			int width = Rows * cellSprite.texture.width;
			int height = Columns * cellSprite.texture.height;
			Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
			
			for (int x = 0; x < CellHolder.Holder.Length; x++) {
				for (int y = 0; y < CellHolder.Holder[x].Cells.Count; y++) {
					var cell = CellHolder.Holder[x].Cells[y].GetComponent<SpriteRenderer>().sprite.texture;
					for (int i = 0; i < cell.width; i++) {
						for (int j = 0; j < cell.height; j++) {
							tex.SetPixel(x*cellSprite.texture.width + i, y*cellSprite.texture.width + j, cell.GetPixel(i, j));
						}
					}
					tex.Apply();
				}
			}

			return tex.EncodeToPNG();
		} //stitches map into a png

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

		public void ClearMap() {
			foreach (var cell in CellHolder.Holder.SelectMany(o => o.Cells)) { //combine nested foreach
				cell.CellType = Cell.Type.Empty;
			}
		}

		private void OnDrawGizmos() {
			Gizmos.DrawWireCube(transform.position, GridSize + Vector2.one);
		}

	}
}
