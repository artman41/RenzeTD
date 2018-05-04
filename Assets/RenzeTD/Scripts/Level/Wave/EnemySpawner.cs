using System;
using System.Collections.Generic;
using RenzeTD.Scripts.Enemies;
using RenzeTD.Scripts.Level.Map.Pathing;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Wave {
    public class EnemySpawner : MonoBehaviour {

        /// <summary>
        /// The delay in seconds to wait until spawing another enemy
        /// </summary>
        public int Delay = 1;
        /// <summary>
        /// creates a new instance of TimeSpawn, where the value is equal to Delay
        /// </summary>
        TimeSpan _Delay => new TimeSpan(0, 0, Delay);

        /// <summary>
        /// Stores the last time since enemies were spawned
        /// </summary>
        private TimeSpan TimeSinceSpawn;
        
        /// <summary>
        /// The enemies to be spawned
        /// </summary>
        public Queue<Enemy> Enemies;
        /// <summary>
        /// A pointer to the path of the map
        /// </summary>
        NodePath Path => md.Path;
        /// <summary>
        /// The current spawned Enemies
        /// </summary>
        public List<GameObject> ActiveEnemies;

        /// <summary>
        /// reference to the local MapData object
        /// </summary>
        private MapData md;
        
        void Start() {
            gameObject.SetActive(true); //activates the object on scene load
            md = FindObjectOfType<MapData>(); //sets md to the local MapData object
            ActiveEnemies = new List<GameObject>(); //initializes ActiveEnemies as a new list
        }

        /// <summary>
        /// Sets the queue of enemies to the given queue and sets the TimeSinceSpawn to the current time minus the SpawnDelay
        /// </summary>
        /// <param name="e">Queue of Enemies</param>
        public void BeginWave(Queue<Enemy> e) {
            TimeSinceSpawn = DateTime.Now.TimeOfDay.Subtract(_Delay); //sets the TimeSinceSpawn to currentTime - delay
            Enemies = e; //Sets local queue of enemies to given variable
        }

        void FixedUpdate() {
            if (Enemies != null) { //if there is a queue
                if (Enemies.Count == 0) { //if the queue is empty
                    Enemies = null; //deletes the queue
                    return; //ends function so the code underneath doesn't run
                }
                Debug.Log($"{Enemies.ToArray()}");
                var currentTime = DateTime.Now.TimeOfDay;
                if (currentTime > TimeSinceSpawn.Add(_Delay)) //if the current time is greater than TimeSinceLastSpawn + delay
                {
                    var enemy = Instantiate(Enemies.Dequeue(), transform); //creates an instance of Enemy as a child object of the current object
                    enemy.gameObject.SetActive(true); //activates the object
                    enemy.GetComponent<RectTransform>().position = Vector3.zero; //sets its position to 0,0,0
                    enemy.transform.position = md.StartNode.transform.position + new Vector3(0f, 1f, 0f); //sets its position to the startnode pos + [0,1]

                    enemy.FollowPath(Path); //forces the enemy to follow the gien NodePath
                    ActiveEnemies.Add(enemy.gameObject); //adds the enemy to the list of active Enemies
                    TimeSinceSpawn = currentTime; //sets the TimeSinceSpawn to currentTime
                }
            }
        }
        
    }
}