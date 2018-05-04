using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace RenzeTD.Scripts.Data {
    [Serializable]
    public class Map {
        /// <summary>
        /// Map Name
        /// </summary>
        public string Name;
        
        /// <summary>
        /// The image representing the Map
        /// </summary>
        [SerializeField]
        private Texture2D _coverImage;

        /// <summary>
        /// Instance of File
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// Property Accessor for _coverImage
        /// </summary>
        public Texture2D CoverImage {
            get { return _coverImage; }
            set { _coverImage = value; }
        }
        
        /// <summary>
        /// Returns a sprite using CoverImage as a texture
        /// </summary>
        public Sprite CoverSprite {
            get {
                if (CoverImage != null) {
                    return Sprite.Create(CoverImage, new Rect(0, 0, CoverImage.width, CoverImage.height),
                        new Vector2(0, 0),
                        100.0f);
                } else {
                    return null;// Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height), Vector2.zero);
                }
            }
        }

        public Map(DirectoryInfo dir) {
            Name = dir.Name.Replace("_", " "); //Sets the Name var to the Name of the map
            var files = dir.GetFiles().Where(o => o.Name.Replace(o.Extension, "") == "map").ToArray(); //Gets all the files contained inside the Map Directory
            if (files.All(o => o.Extension.ToLower() != ".json")) { //Looks for a Map.Json file
                //If there is no Map.Json, the directory does not contain a map so throw an error
                throw new FileNotFoundException($"No files found at {dir.FullName}"); 
            }
            foreach (var f in files) { //iterates through each file in the directory
                switch(f.Extension.ToLower()){ //santises the extension so that any Case will satisfy the conditions
                    case ".json": //if is a Map Json
                        File = f; //Sets the local variable to the instance of the file
                        break;
                    case ".png": //if is the Cover Image png
                        CoverImage = new Func<Texture2D>(() => { //Lambda delegate function, inline method
                            var FileData = System.IO.File.ReadAllBytes(f.FullName); //Gets the Byte[] data
                            var Tex2D = new Texture2D(2, 2); //Creates a temporary texture to use as a base
                            if (Tex2D.LoadImage(FileData)) {// Load the imagedata into the texture (size is set automatically)
                                return Tex2D; // If data == readable -> return texture
                            }
                            return null; // Else return an invalid texture
                        }).Invoke();
                        break;
            }
        }
    }
    }
}