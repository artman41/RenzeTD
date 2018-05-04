using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RenzeTD.Scripts.Enemies;
using RenzeTD.Scripts.Misc;
using RenzeTD.Scripts.Selection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RenzeTD.Scripts.Data {
    /// <summary>
    /// The file which contains data to be preserved over scene changes
    /// </summary>
    [ExecuteInEditMode]
    public class PreservedData : MonoBehaviour {
        /// <summary>
        /// The total amount of maps that were correctly indexed at game Start
        /// </summary>
        public List<Map> AvailableMaps = new List<Map>();

        /// <summary>
        /// The current map to load
        /// </summary>
        public Map SelectedMap;
        
        /// <summary>
        /// A sanity check as to whether the Map has a name.
        /// No name = not a real map
        /// </summary>
        bool _InEditMode2 => SelectedMap.Name == string.Empty;

        /// <summary>
        /// The time until the Tutorial is finished
        /// </summary>
        public TimeSpan TutorialTime;

        
        /// <summary>
        /// The total possible enemies that can be spawned
        /// </summary>
        [SerializeField]
        private EnemyType[] _enemyCollection;

        public EnemyType[] EnemyCollection {
            get { return _enemyCollection; }
            set {
                if (!value.GroupBy(o => o) //Groups the array so that similar objects are together
                    .Where(o => o.Count() > 1) //Gets the amount where the amount of duplicates is greater than 1
                    .ToDictionary(o => o.Key.EnemyColour, x => x.Count()) //Creates a dictionary of <Colour, Num of Duplicates>
                    .Any(o => o.Value > 1)) { //Returns true if any have a count of duplicates > 1
                    _enemyCollection = value; //Set collection to value if the condition above was false
                }
                else throw new Exception(); //Throws an exception otherwise
            }
        }

        /// <summary>
        /// A check as to whether the map should be able to be edited
        /// </summary>
        private bool _InEditMode;
        public bool InEditMode {
            get { return _InEditMode || _InEditMode2; }
            set { _InEditMode = value; }
        }

        void Awake() {
            try {
                DontDestroyOnLoad(transform.gameObject); //prevents the object from being destroyed on scene change
            } catch (InvalidOperationException e) {
                Debug.Log(e);
            }
        }

        void Start() {
            if (AvailableMaps.Any()) {
                AvailableMaps.Clear(); //clears the list of availableMaps if it contains any objects
            }
            Directory.CreateDirectory(Settings.Instance.MapDirLocation); //Creates the Map directory if it doesn't already exist
            InitMaps(); //Fill the AvailableMaps array
        }

        void InitMaps() {
            if (AvailableMaps.Count == 0) { //Iterate through maps and make a collection
                foreach (var mDirectory in new DirectoryInfo(Settings.Instance.MapDirLocation).GetDirectories()) {
                    Map m = null; //create a new Map object
                    try {
                        m = new Map(mDirectory); //Set the Map object to an instance of map
                    } catch (FileNotFoundException e) {
                        Debug.LogError(e.Message);
                    }

                    if (m != null) {
                        AvailableMaps.Add(m); //Adds map to array if temp var was set to an instance of Map
                    }
                }
            }
        }

        private void OnGUI() {
            switch (SceneChanger.GetSceneType(SceneManager.GetActiveScene().name)) { //Shows a different GUI dependent on currently loaded scene
                case SceneChanger.NavigationType.Menu:
                    break;
                case SceneChanger.NavigationType.Options:
                    break;
                case SceneChanger.NavigationType.Level:
                    //TODO: Level tutorial text
                    break;
                case SceneChanger.NavigationType.Leaderboard:
                    break;
                case SceneChanger.NavigationType.Selection:
                    break;
            }
        }

        private void OnApplicationQuit() {
            Settings.Instance.DirtyCheck(); //Saves the settings on game exit
        }
    }
}