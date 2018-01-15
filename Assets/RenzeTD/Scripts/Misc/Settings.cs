using System;
using System.CodeDom;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace RenzeTD.Scripts.Misc {
    public class Settings {
        static readonly string SettingsLocation = "Game/Settings/";
        
        private static Settings _Instance;
        public static Settings Instance {
            get {
                if (_Instance != null) return _Instance;
                try {
                    _Instance = JsonConvert.DeserializeObject<Settings>($"{SettingsLocation}Data.json");
                } catch (Exception) {
                    _Instance = new Settings();
                    using (var f = new StreamWriter($"{SettingsLocation}Data.json")) {
                        f.Write(JsonConvert.SerializeObject(_Instance, Formatting.Indented));
                    }
                }
                return _Instance;
            }
            set { _Instance = value; }
        }

        public Settings() {
            Directory.CreateDirectory(MapDirLocation);
            Directory.CreateDirectory(EnemyLocation);
        }
        
        public string TileLocation { get; } = "Game/Tiles/";
        
        public string MapDirLocation { get; } = "Game/Maps/";
        public string EnemyLocation { get; } = "Game/Settings/";
        
    }
}