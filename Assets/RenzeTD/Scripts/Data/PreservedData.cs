using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RenzeTD.Scripts.Misc;
using RenzeTD.Scripts.Selection;
using UnityEngine;

namespace RenzeTD.Scripts.Data {
    [ExecuteInEditMode]
    public class PreservedData : MonoBehaviour {
        public List<Map> AvailableMaps = new List<Map>();

        public Map SelectedMap;
        public bool InEditMode => SelectedMap.Name == string.Empty;

        void Awake() {
            try {
                DontDestroyOnLoad(transform.gameObject);
            } catch (InvalidOperationException e) {
                
            }
        }

        void Start() {
            if (AvailableMaps.Any()) {
                AvailableMaps.Clear();
            }
            Directory.CreateDirectory(Settings.Instance.MapDirLocation);
            InitMaps();
        }

        void InitMaps() {
            if (AvailableMaps.Count == 0) { //Iterate through maps and make a collection
                foreach (var mDirectory in new DirectoryInfo(Settings.Instance.MapDirLocation).GetDirectories()) {
                    Map m = null;
                    try {
                        m = new Map(mDirectory);
                    } catch (FileNotFoundException e) {
                        Debug.Log(e.Message);
                    }

                    if (m != null) {
                        AvailableMaps.Add(m);
                    }
                }
            }
        }
    }
}