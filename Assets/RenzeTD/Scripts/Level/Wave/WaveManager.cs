using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using RenzeTD.Scripts.Data;
using RenzeTD.Scripts.Enemies;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Wave {
    public class WaveManager : MonoBehaviour {

        /// <summary>
        /// List of enemy prefabs
        /// </summary>
        [SerializeField]
        private List<Enemy> _PossibleEnemies;

        /// <summary>
        /// The current enemies to be spawned
        /// </summary>
        public Queue<Enemy> Enemies;
        
        /// <summary>
        /// a reference to the local WorldManager object
        /// </summary>
        public WorldManager wm;
        /// <summary>
        /// a reference to the local PreservedData object
        /// </summary>
        public PreservedData pd;
        /// <summary>
        /// a reference to the local EnemySpawner object
        /// </summary>
        public EnemySpawner es;
        
        /// <summary>
        /// Gets all the possible enemies to be spawned
        /// </summary>
        public List<Enemy> PossibleEnemies {
            get {
                if(_PossibleEnemies.All(o => o != null)) //if there are no null objects in PossibleEnemies
                    _PossibleEnemies.Sort((a, b) => a.Value.CompareTo(b.Value)); //sorts the possible enemies based on on their value
                return _PossibleEnemies;
            }
            set {
                _PossibleEnemies = value;
            }
        }
        
        private void Start() {
            wm = FindObjectOfType<WorldManager>(); //sets wm to a reference to the local WorldManager object
            pd = FindObjectOfType<PreservedData>(); //sets pd to a reference to the local PreservedData object
            es = FindObjectOfType<EnemySpawner>(); //sets es to a reference to the local EnemySpawner object
            if (Enemies == null) { //if the queue doesn't exist
                Enemies = new Queue<Enemy>(); //create a new queue
            }
            GameObject x; //create a temp gameobject variable
            if (transform.Cast<Transform>().All(o => o.name != "Prefabs")) { //if the current object doesn't contain any objects called Prefabs
                x = new GameObject("Prefabs"); //Set x equal to a new Gameobject called Prefabs
                x.transform.SetParent(transform); //set the parent of x to the current object
            } else { //if an object called Prefabs exists
                x = transform.Cast<Transform>().First(o => o.name == "Prefabs").gameObject; //set x to the Prefabs object
            }
            wm.Wave = 0; //set the current wave to 0
            if (PossibleEnemies.Count == 0) { //if there are no current enemies
                foreach (EnemyType.Colour o in Enum.GetValues(typeof(EnemyType.Colour))) { //for each possible Enemy colour
                    var g = new GameObject(); //create a new gameobject
                    var e = g.AddComponent<Enemy>(); //add an Enemy script
                    e.Type = o; //set the type of enemy to the current colour object
                    e.name = $"Enemy Prefab {o}"; //set the name to "Enemy prefab [colour]"
                    e.gameObject.SetActive(false); //disable the object
                    e.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); //set the scale of the object to 0.2f (20%)
                    e.transform.SetParent(x.transform); //set the parent of the prefab to the Prefabs object
                    PossibleEnemies.Add(e); //add the prefab to the list of possible enemies
                }
            }
        }

        void Update() {
            if(es == null) es = FindObjectOfType<EnemySpawner>(); //sets es to a reference to the local EnemySpawner object if it is null
            
        }

        /// <summary>
        /// Begins the wave
        /// </summary>
        public void Begin() {
            var x = ++wm.Wave; //increases the wave and sets x to that total
            var triedTested = new List<Enemy>(); //creates a temp list of tested Enemies
            while (x > 0) { //if x is greater than 0
                var e = PossibleEnemies.Last(o => !triedTested.Contains(o)); //gets the last possible enemy that hasn't been tested, where the list is sorted (small -> big)
                triedTested.Add(e); //adds the possible enemy to the list of tested Enemies
                int i = x / e.Value; //divides x by the value of the enemy, and sets it to an int. This removes any decimal values, meaning that 0.xx becomes 0 and 1.xxx becomes 1 etc.
                for (int j = 0; j < i; j++) {
                    Enemies.Enqueue(e); //adds i amount of enemies to the queue of enemies
                }
                x -= i; //subtracts i from x
                
            } //This whole process results in the biggest possible enemies being added to the list, with them being dynamically created from the wave number.
            
            //e.g:
            // Enemy    | Value
            //  Red     |     1
            //  Blue    |     2
            //  Green   |     3
            //  Yellow  |     4
            // x = wave = 15
            // -> 15/4 = 3.75 = 3
            // -> 3 x Yellow
            // x = 15 - 3 = 12
            // -> 12/3 = 4
            // -> 4 x green
            // x = 12 - 4 = 8
            // -> 8/2 = 4
            // -> 4 x blue
            // x = 8 - 4 = 4
            // -> 4/1 = 4
            // -> 4 x red
            // x = 4 - 4 = 0
            // ---
            // Enemies in queue:
            // 3 x Yellow,
            // 4 x Green,
            // 4 x Blue,
            // 4 x Red
            
            es.BeginWave(Enemies); //Begins the wave of enemies with the created queue of enemies
        }
        
    }
}