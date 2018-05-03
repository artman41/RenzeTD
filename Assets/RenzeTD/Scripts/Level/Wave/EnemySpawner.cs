using System;
using System.Collections.Generic;
using RenzeTD.Scripts.Enemies;
using RenzeTD.Scripts.Level.Map.Pathing;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Wave {
    public class EnemySpawner : MonoBehaviour {

        public int Delay = 10;
        TimeSpan _Delay => new TimeSpan(0, 0, Delay);

        private TimeSpan TimeSinceSpawn;
        
        public Queue<Enemy> Enemies;
        NodePath Path => md.Path;
        public List<GameObject> ActiveEnemies;

        private MapData md;
        
        void Start() {
            gameObject.SetActive(true);
            md = FindObjectOfType<MapData>();
            ActiveEnemies = new List<GameObject>();
        }

        public void BeginWave(Queue<Enemy> e) {
            TimeSinceSpawn = DateTime.Now.TimeOfDay.Subtract(_Delay);
            Enemies = e;
        }

        void FixedUpdate() {
            if (Enemies != null) {
                if (Enemies.Count == 0) {
                    Enemies = null;
                    return;
                }
                Debug.Log($"{Enemies.ToArray()}");
                var enemy = Instantiate(Enemies.Dequeue(), transform);
                enemy.gameObject.SetActive(true);
                enemy.GetComponent<RectTransform>().position = Vector3.zero;
                enemy.transform.position = md.StartNode.transform.position +  new Vector3(0f, 1f, 0f);
                
                enemy.FollowPath(Path);
                ActiveEnemies.Add(enemy.gameObject);
            }
        }
        
    }
}