using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using RenzeTD.Scripts.Data;
using RenzeTD.Scripts.Level.Map;
using RenzeTD.Scripts.Level.Map.Pathing;
using RenzeTD.Scripts.Level.Wave;
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
		/// <summary>
		/// Amount of rows in the grid
		/// </summary>
		[DataMember]
		public int Rows = 9;
		/// <summary>
		/// Amount of columns in the grid
		/// </summary>
		[DataMember]
		public int Columns = 9;
		/// <summary>
		/// Amount of money to start with
		/// </summary>
		[DataMember]
		public int StartingMoney = 100;
		/// <summary>
		/// Offset of the grid from the center
		/// </summary>
		[DataMember]
		public Vector2 GridOffset = Vector2.zero;
		/// <summary>
		/// The type which holds the cells
		/// </summary>
		[DataMember]
		public CellHolder CellHolder;
		/// <summary>
		/// The side enemies spawn on
		/// </summary>
		[DataMember]
		public Side StartsFrom = Side.Top;
		/// <summary>
		/// The side enemies travel to
		/// </summary>
		[DataMember]
		public Side EndsOn = Side.Bottom;
		
		/// <summary>
		/// The total size of the grid
		/// </summary>
		public Vector2 GridSize => new Vector2(Rows, Columns);
		/// <summary>
		/// The theoretical size of the cells
		/// </summary>
		public Vector2 CellSize => new Vector2(GridSize.x/Columns, GridSize.y/Rows);

		/// <summary>
		/// The first node in the NodePath
		/// </summary>
		public Node StartNode;
		/// <summary>
		/// The last node in the NodePath
		/// </summary>
		public Node EndNode;
		
		/// <summary>
		/// Retrieves a NodePath using the Start and End nodes
		/// </summary>
		public NodePath Path => new NodePath(StartNode, EndNode);

		private PreservedData pd;
		private WorldManager wm;
		private EnemySpawner es;
		
		private bool _esCreated;
		/// <summary>
		/// Checks whether the EnemySpawner object exists
		/// </summary>
		private bool EsCreated
		{
			get { return _esCreated || FindObjectOfType<EnemySpawner>()!=null; }
			set { _esCreated = value; }
		}

		void Start () {
			pd = FindObjectOfType<PreservedData>();
			wm = FindObjectOfType<WorldManager>();
            es = FindObjectOfType<EnemySpawner>();
			InitWorld(); //sets up the initial world values
			InitCells(); //creates the cells needed for the map
			if(!pd.InEditMode) LoadMap(pd.SelectedMap.File); //if not in edit mode, a map is loaded, else the map is left blank
		}

		void InitWorld() {
			wm.Health = 100; //sets the total health to 100
			try {
				wm.Killed.Clear(); //attempts to clear the list of enemies killed
			} catch (NullReferenceException e) {
				Debug.Log(e.Message);
			}
			wm.Money = StartingMoney; //sets the starting money to the amount set in the map settings
		}

		/// <summary>
		/// Creates the grid of cells
		/// </summary>
		void InitCells() {
			GameObject CellObject = new GameObject(); //creates a prefab cell
			CellObject.AddComponent<Cell>(); //adds the cell script to the prefab
			CellObject.AddComponent<Node>(); //adds the node script to the prefab
			CellObject.AddComponent<RectTransform>(); //adds a rect transform as the cells are only used in a 2d plane
			CellObject.AddComponent<BoxCollider>(); //adds a box collider to allow for mouse input
			CellHolder = new CellHolder(Columns, Rows); //creates an instance of CellHolder with a width of Columns and a height of Rows
			
			var positions = new Vector2[Columns, Rows]; //creates an array of the total possible locations
			for (int i = 0; i < positions.GetLength(0); i++) { //while i is less than Columns
				for (int j = 0; j < positions.GetLength(1); j++) { //while j is less than Rows
					var x = new Vector2(1f*i, 1f*j); //creates an instance of vector2 with the positions as floats, [i, j]

					positions[i, j] = x; //sets the value at the index [i, j] to x
				}
			}
			
			for (int i = 0; i < Columns; i++) { //iterates through the columns
				for (int j = 0; j < Rows; j++) { //iterates through the rows
					//creates a vector 2 where the values are [(Columns index * cell width) + Xoffset, (Rows index * cell Height) + Yoffset]
					var pos = new Vector2(j * CellSize.x + GridOffset.x, i * CellSize.y + GridOffset.y);
					//Creates an instance of the CellPrefab at position Pos and adds it to the array of objects at Holder[Column index]
					CellHolder.Holder[i].Objects.Add(Instantiate(CellObject, pos, Quaternion.identity));
					//adds the Cell component of the insance to the array of Cells at Holder[Column index]
					CellHolder.Holder[i].Cells.Add(CellHolder.Holder[i].Objects[j].GetComponent<Cell>());
					
					//gets the previously instantiated object
					var go = CellHolder.Holder[i].Objects[j];
					//sets the name of the object to [Column index, Row Index] :: CellType
					go.name = $"[{i},{j}] :: {CellHolder.Holder[i].Cells[j].CellType}";
					//sets the parent of the object to the Map object
                    go.transform.SetParent(transform, false);
					//changes the local position to be the position at index [i, j] that were created before.
					//the vector [4, 4, 1] is subtracted due to the positions created being from 0 to 8,
					//because unity define 0,0 as the center, 4 needs to be subtracted so 0 is the center of the positions
					//therefore, the positions change from 0=>8 to -4=>4
                    go.transform.localPosition = (Vector3)positions[i, j] - new Vector3(4f, 4f, 1f);
                }
			}
			
			//The cell prefab is then destroyed
			Destroy(CellObject);
		}

		public void InitNodes() {
			//iterates through each node top to bottom until it reaches the start node
			for (int i = 0; i < Columns; i++) {
				for (int j = 0; j < Rows; j++) {
					if (StartNode != null) { //if the StartNode has already been found, stop searching for it
						break;
					}
					var node = CellHolder.Holder[i].Objects[j].GetComponent<Node>(); //gets the node at index i, j
					StartNode = node.Value == 0 ? node : null; //if the value of the node is 0, StartNode = node, else, StartNode = null
				}
				if (StartNode != null) {//if the StartNode has already been found, stop searching for it
					break;
				}
			} 
			
			//If the EnemySpawner has not been created or found, create it
			if (!EsCreated) {
				var g = new GameObject(); //Create a prefab, g
				g.AddComponent<EnemySpawner>(); //add the enemy spawner script
				g.AddComponent<RectTransform>(); //add a rect transform, as this is used in a 2d plane
				var go = Instantiate(g); //create an instance of the prefab
				es = go.GetComponent<EnemySpawner>(); //point the local variable es to the EnemySpawner component of the obejct
				go.name = "Enemy Spawner"; //set the object name to Enemy Spawner
				go.transform.position = StartNode.transform.position + new Vector3(0f, 1f); //Move the spawner to one unit above the Start Node #this should be out of the map and off screen#
				Destroy(g); //destroy the prefab
				EsCreated = true; //set the local variable as true so that another EnemySpawner is not created
			}

			StartNode?.SetValue(); //attempts to set the value of the StartNode and, therefore, every connected node
			Node n = StartNode; //sets the local node n to start node
			
			//iterates over every cell
			for (int i = 0; i < Columns; i++) {//iterates through columns
				for (int j = 0; j < Rows; j++) {//iterates through rows
					var node = CellHolder.Holder[i].Objects[j].GetComponent<Node>(); //attempts to get a node component from the object at i, j
					if (node.Value > n?.Value) n = node; //if node exists & node value is greater than n.Value, then n = node
				}
			}

			EndNode = n; //the node of greatest value will be the EndNode, therefore EndNode = n
		}

		/// <summary>
		/// Saves the Map in a directory using the name param
		/// </summary>
		/// <param name="name">The Name of the Map Directory</param>
		public void SaveMap(string name) {
			name = name.Replace(" ", "_"); //Santises the DirName input
			string json = JsonConvert.SerializeObject(this, Formatting.Indented); //converts the current MapData object to a json string
			var directory = $"{Settings.Instance.MapDirLocation}{name}"; //gets the Directory path of the new map
			Directory.CreateDirectory(directory); //creates the directory for the new map
			using (var f = new StreamWriter($"{directory}/map.json")) { //opens a StreamWriter to a map.json file inside the directory
				f.Write(json); //Writes the json string to the file
			} //closes the StreamWriter

			//Writes all the bytes returned from the GetPicture method to a file called map.png inside the directory
			File.WriteAllBytes($"{Settings.Instance.MapDirLocation}{name}/map.png", GetPicture()); 
		}

		/// <summary>
		/// Retrieves a byte array representing a png comprising of the stiched together textures of every cell in the map
		/// </summary>
		/// <returns>byte array representing a png file</returns>
		byte[] GetPicture() {
			//Gets the sprite of the first cell, doesn't really matter which cell since they should all be the same size,
			//	which is what the proceding code needs
			var cellSprite = CellHolder.Holder[0].Cells[0].GetComponent<SpriteRenderer>().sprite;
			int width = Columns * cellSprite.texture.width; //gets the total needed width (num of Columns * width)
			int height = Rows * cellSprite.texture.height; //gets the total needed height (num of Rows * heigth)
			Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false); //creates a generic texture using the width & height
			
			for (int Column = 0; Column < CellHolder.Holder.Length; Column++) { //iterates through columns
				for (int Row = 0; Row < CellHolder.Holder[Column].Cells.Count; Row++) { //iterates through rows
					var cell = CellHolder.Holder[Column].Cells[Row].GetComponent<SpriteRenderer>().sprite.texture; //gets the texture of the cell at [column, row]
					for (int pixelXpos = 0; pixelXpos < cell.width; pixelXpos++) { //iterates through the pixels of the cell on the x
						for (int pixelYpos = 0; pixelYpos < cell.height; pixelYpos++) { //iterates through the pixels of the cell on the y
							//sets the corresponding pixel in the generic texture to the pixel on the cell
							//To get the corresponding pixel, the offset is determined by Column*cellSprite.texture.width, to get the pixel shift
							//then the current xPos is added to determine the appropriate xCoOrd
							//this is then repeated for the Y axis
							tex.SetPixel(Column*cellSprite.texture.width + pixelXpos, Row*cellSprite.texture.width + pixelYpos, cell.GetPixel(pixelXpos, pixelYpos));
						}
					}
					tex.Apply(); //This updates the texture so that the changes in pixels are saved to the local object
				}
			}

			return tex.EncodeToPNG(); //this returns the texture as an image file to be saved
		}

		/// <summary>
		/// loads the map from a given map.json
		/// </summary>
		/// <param name="Map"></param>
		void LoadMap(FileInfo Map) {
			var x = JsonConvert.DeserializeObject<MapData>(File.ReadAllText(Map.FullName)); //reads the json and creates an instance of MapData from the data
			Rows = x.Rows; //sets the Rows variable of the current instance to be equal to the Rows variable of the loaded instance
			Columns = x.Columns; //sets the Columns variable of the current instance to be equal to the Columns variable of the loaded instance
			GridOffset = x.GridOffset; //sets the GridOffset variable of the current instance to be equal to the Offset variable of the loaded instance

			for (int i = 0; i < CellHolder.Holder.Length; i++) { //iterates through the columns of cells
				for (int j = 0; j < CellHolder.Holder[i].Cells.Count; j++) { //iterates through the rows of cells
					//sets the CellType at [i, j] of the current instance to be equal to the corresponding CellType of the loaded instance
					CellHolder.Holder[i].Cells[j].CellType = x.CellHolder.Holder[i].Cells[j].CellType;
				}
			}
		}

		/// <summary>
		/// Iterates through every cell in the map and sets the CellType to Empty
		/// </summary>
		public void ClearMap() {
			foreach (var cell in CellHolder.Holder.SelectMany(o => o.Cells)) { //gets every array Holder.Cells and return them as a single array
				cell.CellType = Cell.Type.Empty; //sets the CellType to Empty
			}
		}

		/// <summary>
		/// Draws a wire grid around the Map inside the inspector
		/// </summary>
		private void OnDrawGizmos() {
			//draws the cube centered at current positon, with a size of gridSize + [1, 1]
			//for some reason, the [1,1] is needed, else the grid will bisect the outer cells
			Gizmos.DrawWireCube(transform.position, GridSize + Vector2.one);
		}

	}
}
