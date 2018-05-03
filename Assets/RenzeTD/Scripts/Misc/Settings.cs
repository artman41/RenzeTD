using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace RenzeTD.Scripts.Misc {
    [DataContract]
    public class Settings {
        static readonly string GameDir = Application.isEditor ? "Game/" : "build_Data/Game/";
        
        static readonly string SettingsLocation = $"{GameDir}Settings/";
        
        private static Settings _Instance;
        [DataMember]
        private float _soundFxVolume;
        [DataMember]
        private float _musicVolume;
        private bool isDirty;

        public List<AudioSource> MusicSources = new List<AudioSource>();
        public List<AudioSource> SfxSources = new List<AudioSource>();
        
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
            //Directory.CreateDirectory(EnemyLocation);
            //TODO: Custom Enemies
            _musicVolume = 1f;
            _soundFxVolume = 1f;
        }
        
        [DataMember]
        public string MapDirLocation { get; } = $"{GameDir}Maps/";
        [DataMember]
        public string EnemyLocation { get; } = $"{GameDir}Settings/";

        public float MusicVolume {
            get { return _musicVolume; }
            set {
                _musicVolume = value;
                Debug.Log($"music vol set to {value}");
                SetDirty(true);
                UpdateAudio();
            }
        }

        public float SoundFXVolume {
            get { return _soundFxVolume; }
            set {
                _soundFxVolume = value;
                Debug.Log($"sfx vol set to {value}");
                SetDirty(true);
                UpdateAudio();
            }
        }

        void UpdateAudio() {
            MusicSources.ForEach(o => {
                if (o == null) {
                    MusicSources.Remove(o);
                } else {
                    o.volume = MusicVolume;
                }
            });
            SfxSources.ForEach(o => {
                if (o == null) {
                    SfxSources.Remove(o);
                } else {
                    o.volume = SoundFXVolume;
                }
            });
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
                SetDirty(false);
            }
        }

        bool SetDirty(bool b) {
            isDirty = b;
            return isDirty;
        }
    }
}