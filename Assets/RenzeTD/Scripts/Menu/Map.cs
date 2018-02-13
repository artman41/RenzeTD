using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace RenzeTD.Scripts.Menu {
    [Serializable]
    public class Map {
        
        public string Name => File.DirectoryName?.Replace("_", " ");

        public FileInfo File { get; set; }
        public Sprite CoverImage { get; set; }

        public Map(DirectoryInfo dir) {
            var files = dir.GetFiles().Where(o => o.Name == "map");
            if (files.All(o => {
                var x = o.Extension.ToLower() != ".json";
                Debug.Log($"o.Extension.ToLower() != \".json\" :: {x}");
                return x;
            })) {
                throw new FileNotFoundException();
            }
            foreach (var f in files) {
                switch(f.Extension.ToLower()){
                    case ".json":
                        File = f;
                        break;
                    case ".png":
                        CoverImage = new Sprite();
                        var sTex = new Func<Texture2D>(() => { //Lambda delegate function, inline method
                            var FileData = System.IO.File.ReadAllBytes(f.FullName);
                            var Tex2D = new Texture2D(2, 2);
                            if (Tex2D.LoadImage(FileData)) {// Load the imagedata into the texture (size is set automatically)
                                return Tex2D; // If data = readable -> return texture
                            }
                            return null;
                        }).Invoke();
                        CoverImage = Sprite.Create(sTex, new Rect(0, 0, sTex.width, sTex.height), new Vector2(0, 0),
                            100.0f);
                        break;
            }
        }
    }
            
    }
}