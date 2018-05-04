using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace RenzeTD.Scripts.Misc {
    [DataContract]
    public class Settings
    {
        /// <summary>
        /// A string containing the Path to the Game Folder
        /// </summary>
        private static readonly string GameDir = Application.isEditor ? "Game/" : $"{Application.dataPath}/Game/";
        
        /// <summary>
        /// A string containing the path to the Settings Folder
        /// </summary>
        static readonly string SettingsLocation = $"{GameDir}Settings/";
        
        /// <summary>
        /// The local instance of Settings
        /// </summary>
        private static Settings _Instance;
        
        /// <summary>
        /// A string containing the path to the Maps Folder
        /// </summary>
        [DataMember]
        public string MapDirLocation { get; } = $"{GameDir}Maps/";
        /// <summary>
        /// The local instance of SFXVol
        /// </summary>
        [DataMember]
        private float _soundFxVolume;
        /// <summary>
        /// The local instance of MusicVol
        /// </summary>
        [DataMember]
        private float _musicVolume;
        /// <summary>
        /// A check as to whether the data needs to be saved
        /// </summary>
        private bool isDirty;

        /// <summary>
        /// A list of sources that are to play music audio
        /// </summary>
        public List<AudioSource> MusicSources = new List<AudioSource>();
        /// <summary>
        /// A list of sources that are to play sfx audio
        /// </summary>
        public List<AudioSource> SfxSources = new List<AudioSource>();
        
        /// <summary>
        /// Retrieves the local instance of settings
        /// </summary>
        public static Settings Instance {
            get {
                if (_Instance != null) return _Instance; //if the instance has been instantiated, return it
                try { //if the instance has not been instantiated
                    Directory.CreateDirectory(SettingsLocation); //creates the Settings folder if it doesn't exist
                    _Instance = JsonConvert.DeserializeObject<Settings>(File.ReadAllText($"{SettingsLocation}Data.json")); //attempt to load Settings from file
                } catch (Exception e) { //if the file does not exist
                    Debug.Log(e);
                    _Instance = new Settings(); //Create a blank copy of Settings
                    SaveSettings(); //Save settings to file
                }
                return _Instance; //return the loaded/created version of Settings
            }
            set { _Instance = value; } //Updates the local instance to the given value
        }

        /// <summary>
        /// Creates an instance of Settings
        /// </summary>
        public Settings() {
            Directory.CreateDirectory(MapDirLocation); //Creates the Map directory if it does not exist
            //TODO: Custom Enemies
            _musicVolume = 1f; //sets the default value of music volume to 1
            _soundFxVolume = 1f; //sets the default value of sfx volume to 1
        }
        
        /// <summary>
        /// Retrieves the instance of music volume
        /// </summary>
        public float MusicVolume {
            get { return _musicVolume; } //retrieves the instance
            set {
                _musicVolume = value; //sets the instance to the value v
                Debug.Log($"music vol set to {value}");
                SetDirty(true); //updates isDirty so the settings are saved
                UpdateAudio(); //updates audio sources
            }
        }

        /// <summary>
        /// Retrieves the instance of sfx volume
        /// </summary>
        public float SoundFXVolume {
            get { return _soundFxVolume; } //retrieves the instance
            set {
                _soundFxVolume = value;  //sets the instance to the value v
                Debug.Log($"sfx vol set to {value}");
                SetDirty(true); //updates isDirty so the settings are saved
                UpdateAudio(); //updates audio sources
            }
        }

        /// <summary>
        /// Iterates through each list of sources and sets the volume of any
        ///     non-null objects to the corresponding volume
        /// </summary>
        void UpdateAudio() {
            MusicSources.ForEach(o => { //in line foreach, where the object is o
                if (o == null) { //if the object is null
                    MusicSources.Remove(o); //remove the object from the list
                } else { //if the object exists
                    o.volume = MusicVolume; //set the volume of the object to MusicVolume
                }
            });
            SfxSources.ForEach(o => {//in line foreach, where the object is o
                if (o == null) {//if the object is null
                    SfxSources.Remove(o); //remove the object from the list
                } else { //if the object exists
                    o.volume = SoundFXVolume; //set the volume of the object to sfxVolume
                }
            });
        }

        /// <summary>
        /// Saves the settings to a file
        /// </summary>
        static void SaveSettings() {
            File.Delete($"{SettingsLocation}Data.json"); //deletes the current settings Json
            using (var f = new StreamWriter($"{SettingsLocation}Data.json")) { //creates a stream writer to the file previously deleted
                f.Write(JsonConvert.SerializeObject(_Instance, Formatting.Indented)); //writes the json value of the current instance to the file
            } //closes the stream writer
        }

        /// <summary>
        /// Saves the Settings if isDirty & sets isDirty to false
        /// </summary>
        public void DirtyCheck() {
            if (isDirty) { //Checks if isDirty
                SaveSettings(); //Saves the settings
                SetDirty(false); //reset isDirty to false
            }
        }

        /// <summary>
        /// Sets the value of isDirty
        /// </summary>
        /// <param name="b">the value to set it to</param>
        /// <returns>the current value of isDirty</returns>
        bool SetDirty(bool b) {
            isDirty = b; //sets isDirty to the param
            return isDirty; //returns isDirty
        }
    }
}