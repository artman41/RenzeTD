using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using RenzeTD.Scripts.Data;
using RenzeTD.Scripts.Enemies;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Wave {
    public class WaveManager : MonoBehaviour {

        [SerializeField]
        private List<Enemy> _PossibleEnemies;

        public Queue<Enemy> Enemies;
        
        public WorldManager wm;
        public PreservedData pd;
        public EnemySpawner es;
        
        public List<Enemy> PossibleEnemies {
            get {
                if(_PossibleEnemies.All(o => o != null)) _PossibleEnemies.Sort((a, b) => a.Value.CompareTo(b.Value));
                return _PossibleEnemies;
            }
            set {
                _PossibleEnemies = value;
            }
        }
        
        private void Start() {
            wm = FindObjectOfType<WorldManager>();
            pd = FindObjectOfType<PreservedData>();
            es = FindObjectOfType<EnemySpawner>();
            if (Enemies == null) {
                Enemies = new Queue<Enemy>();
            }
            GameObject x;
            if (transform.Cast<Transform>().All(o => o.name != "Prefabs")) {
                x = new GameObject("Prefabs");
                x.transform.SetParent(transform);
            } else {
                x = transform.Cast<Transform>().First(o => o.name == "Prefabs").gameObject;
            }
            wm.Wave = 0;
            if (PossibleEnemies.Count == 0) {
                foreach (EnemyType.Colour o in Enum.GetValues(typeof(EnemyType.Colour))) {
                    var g = new GameObject();
                    var e = g.AddComponent<Enemy>();
                    e.Type = o;
                    e.name = $"Enemy Prefab {o}";
                    e.gameObject.SetActive(false);
                    e.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    e.transform.SetParent(x.transform);
                    PossibleEnemies.Add(e);
                }
            }
        }

        void Update() {
            if(es == null) es = FindObjectOfType<EnemySpawner>();
            
        }

        public void Begin() {
            var x = ++wm.Wave;
            var triedTested = new List<Enemy>();
            while (x != 0) {
                var e = PossibleEnemies.Last(o => !triedTested.Contains(o));
                triedTested.Add(e);
                int i = x / e.Value;
                for (int j = 0; j < i; j++) {
                    Enemies.Enqueue(e);
                }

                x -= i;
            }
            es.BeginWave(Enemies);
        }
        
    }
}