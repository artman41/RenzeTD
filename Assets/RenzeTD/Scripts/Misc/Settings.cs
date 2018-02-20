using System;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace RenzeTD.Scripts.Misc {
    [DataContract]
    public class Settings {
        static readonly string SettingsLocation = "Game/Settings/";
        
        private static Settings _Instance;
        [DataMember]
        private float _soundFxVolume;
        [DataMember]
        private float _musicVolume;
        private bool isDirty;

        public static Settings Instance {
            get {
                if (_Instance != null) return _Instance;
                try {
                    _Instance = JsonConvert.DeserializeObject<Settings>(File.ReadAllText($"{SettingsLocation}Data.json"));
                } catch (Exception e) {
                    Debug.Log(e);
                    _Instance = new Settings();
                    SaveSettings();
                }
                return _Instance;
            }
            set { _Instance = value; }
        }

        public Settings() {
            Directory.CreateDirectory(MapDirLocation);
            Directory.CreateDirectory(EnemyLocation);
            _musicVolume = 1f;
            _soundFxVolume = 1f;
        }
        
        [DataMember]
        public string TileLocation { get; } = "Game/Tiles/";
        [DataMember]
        public string MapDirLocation { get; } = "Game/Maps/";
        [DataMember]
        public string EnemyLocation { get; } = "Game/Settings/";

        public float MusicVolume {
            get { return _musicVolume; }
            set {
                _musicVolume = value;
                Debug.Log($"music vol set to {value}");
                isDirty = true;
            }
        }

        public float SoundFXVolume {
            get { return _soundFxVolume; }
            set {
                _soundFxVolume = value;
                Debug.Log($"sfx vol set to {value}");
                isDirty = true;
            }
        }

        static void SaveSettings() {
            File.Delete($"{SettingsLocation}Data.json");
            using (var f = new StreamWriter($"{SettingsLocation}Data.json")) {
                f.Write(JsonConvert.SerializeObject(_Instance, Formatting.Indented));
            }
        }

        public void DirtyCheck() {
            if (isDirty) {
                SaveSettings();
                isDirty = false;
            }
        }
    }
}