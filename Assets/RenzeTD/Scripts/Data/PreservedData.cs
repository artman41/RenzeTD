﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RenzeTD.Scripts.Enemies;
using RenzeTD.Scripts.Misc;
using RenzeTD.Scripts.Selection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RenzeTD.Scripts.Data {
    [ExecuteInEditMode]
    public class PreservedData : MonoBehaviour {
        public List<Map> AvailableMaps = new List<Map>();

        public Map SelectedMap;
        bool _InEditMode2 => SelectedMap.Name == string.Empty;

        public TimeSpan TutorialTime;

        
        [SerializeField]
        private EnemyType[] _enemyCollection;

        public EnemyType[] EnemyCollection {
            get { return _enemyCollection; }
            set {
                if(!value.GroupBy(o => o).Where(o => o.Count()> 1).ToDictionary(o => o.Key.EnemyColour, x => x.Count()).Any(o => o.Value > 1)) _enemyCollection = value;
                else throw new Exception();
            }
        }

        private bool _InEditMode;
        public bool InEditMode {
            get { return _InEditMode || _InEditMode2; }
            set { _InEditMode = value; }
        }

        void Awake() {
            try {
                DontDestroyOnLoad(transform.gameObject);
            } catch (InvalidOperationException e) {
                Debug.Log(e);
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
                        Debug.LogError(e.Message);
                    }

                    if (m != null) {
                        AvailableMaps.Add(m);
                    }
                }
            }
        }

        private void OnGUI() {
            switch (SceneChanger.GetSceneType(SceneManager.GetActiveScene().name)) {
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
            Settings.Instance.DirtyCheck();
        }
    }
}