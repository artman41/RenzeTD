using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace RenzeTD.Scripts.Selection {
    [Serializable]
    public class Map {
        public string Name;
        
        [SerializeField]
        private Texture2D _coverImage;
        
        public FileInfo File { get; set; }

        public Texture2D CoverImage {
            get { return _coverImage; }
            set { _coverImage = value; }
        }
        
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
            Name = dir.Name.Replace("_", " ");
            var files = dir.GetFiles().Where(o => o.Name.Replace(o.Extension, "") == "map").ToArray();
            if (files.All(o => o.Extension.ToLower() != ".json")) {
                throw new FileNotFoundException();
            }
            foreach (var f in files) {
                switch(f.Extension.ToLower()){
                    case ".json":
                        File = f;
                        break;
                    case ".png":
                        CoverImage = new Func<Texture2D>(() => { //Lambda delegate function, inline method
                            var FileData = System.IO.File.ReadAllBytes(f.FullName);
                            var Tex2D = new Texture2D(2, 2);
                            if (Tex2D.LoadImage(FileData)) {// Load the imagedata into the texture (size is set automatically)
                                return Tex2D; // If data = readable -> return texture
                            }
                            return null;
                        }).Invoke();
                        break;
            }
        }
    }
            
    }
}