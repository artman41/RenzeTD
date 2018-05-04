using System;
using System.Collections.Generic;
using System.Linq;
using RenzeTD.Scripts.Data;
using RenzeTD.Scripts.Level.Map.Pathing;
using UnityEngine;

namespace RenzeTD.Scripts.Enemies {
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(RectTransform))]
    public class Enemy : MonoBehaviour {

        private PreservedData pd;
        private SpriteRenderer sr;
        private WorldManager wm;
        
        [SerializeField]
        private EnemyType.Colour _type;

        /// <summary>
        /// The colour type of the enemy
        /// </summary>
        public EnemyType.Colour Type {
            get { return _type; }
            set {
                _type = value; 
                UpdateInfo();
            }
        }
        
        /// <summary>
        /// The Money/Health value of the enemy
        /// </summary>
        public int Value;

        /// <summary>
        /// The current path to follow
        /// </summary>
        private NodePath path;
        /// <summary>
        /// The path to follow that can be viewed by external objects
        /// </summary>
        public List<Node> PATH;
        /// <summary>
        /// The node the path ends at
        /// </summary>
        private Node TargetNode;
        /// <summary>
        /// Whether the Enemy was destroyed by a tower or not
        /// </summary>
        bool Killed;
        /// <summary>
        /// Decides which pathing technique to use
        /// </summary>
        private bool useV2;
        /// <summary>
        /// The location to travel to
        /// </summary>
        private Vector2 TargetPos;
        /// <summary>
        /// The location the Enemy is destroyed at if not Killed
        /// </summary>
        private Vector2 DestroyPos;

        public void Start(){
            UpdateInfo();
            //positions the enemy infront of the background Sprite
            transform.position = (Vector3)((Vector2) transform.position) - Vector3.back;
        }

        /// <summary>
        /// Initializes all local variables
        /// </summary>
        void UpdateInfo() {
            if(pd == null) pd = FindObjectOfType<PreservedData>();
            if(sr == null) sr = GetComponent<SpriteRenderer>();
            if(wm == null) wm = FindObjectOfType<WorldManager>();
            
            sr.sprite = GetSprite();
            Value = GetValue();
        }
        
        /// <summary>
        /// Returns the sprite based on EnemyType
        /// </summary>
        /// <returns></returns>
        Sprite GetSprite() {
            var x = pd.EnemyCollection; //Gets Array of possible enemies
            var y = x.First(o => o.EnemyColour == Type); //Gets the first Possible Enemy that matches the current Type
            return y.EnemySprite; //Returns the Sprite of that Enemy
        }

        int GetValue() {
            return pd.EnemyCollection //Gets Array of possible enemies
                .First(o => o.EnemyColour == Type) //Gets the first Possible Enemy that matches the current Type
                .EnemyValue; //Returns the value of that Enemy
        }

        /// <summary>
        /// Destroys the Enemy and sets the local Killed variable to true
        /// </summary>
        /// <returns>The value of Killed</returns>
        public bool Kill() {
            Killed = true;
            Destroy(this);
            return Killed;
        }
        
        /// <summary>
        /// Handles the destruction of an Enemy
        /// </summary>
        private void OnDestroy() {
            if (Killed) { //If the enemy was killed by a tower
                wm.Killed.Add(Type, 1); //The enemy is added to the kill counter
                wm.Money += Value; //Money is increased by the value of the enemy
            } else { //If the enemy was destroyed some other way
                wm.Health -= Value; //The current player health is decreased
            }
        }

        public void FollowPath(NodePath path = null) {
            if (this.path == null && path != null) {
                this.path = path;
            } //if there is no current path, sets the path to the given path
            try {
                GoTo(this.path?.Dequeue()); //attempts to transpose to the position of the next node in the queue
            } catch (InvalidOperationException e) { //If there is no next node
                useV2 = true; //Sets the enemy to use the Secondary Pathing method
                //In theory, this should be the position of the last tile, just above the bottom of the map
                TargetPos = transform.position + Vector3.down; //Sets the targetPos to a position under the map
                DestroyPos = TargetPos; //Sets the destruction position to this targetPos
            }
            PATH = this.path?.ToArray().ToList(); //Sets the public reference path to the current Path
        }

        /// <summary>
        /// Rests the current lerp to 0 and updates the TargetNode
        /// </summary>
        /// <param name="n">the TargetNode</param>
        void GoTo(Node n) {
            currentLerpTime = 0f;
            TargetNode = n;
        }

        /// <summary>
        /// current value of a maximum *lerpTime*
        /// </summary>
        public float currentLerpTime;
        /// <summary>
        /// The total time to take moving from one node to another
        /// </summary>
        private float lerpTime = 1f;
        
        void FixedUpdate() {
            if (TargetNode == null && !useV2) return; //If there is no node to travel to & the Secondary Pathing is to be used, just end the method
            //increment timer once per frame
            Debug.Log($"{name} :: curLerpTime ;; {currentLerpTime}");
            if (currentLerpTime > lerpTime) { //santises the current lerp time, where lerpTime is the maximum possible value
                currentLerpTime = lerpTime;
            } else { //increases currentLerpTime by the UpdateInterval between frame updates
                currentLerpTime += Time.fixedDeltaTime;
            }
            if (!useV2) { //First Pathing Method
                if ((Vector2) TargetNode.transform.position == (Vector2) transform.position) { //If at the target position
                    TargetNode = null;
                    this.Value++;
                    FollowPath();
                } else { //if still travelling to target position
                    Debug.Log($"tNodPos {(Vector2) TargetNode.transform.position} != {(Vector2) transform.position}");
                    var p = TargetNode.transform.position; //Get position of target as temporary value
                    p.Set(p.x, p.y, -1f); //sets z of temporary position to -1f so the sprite doesn't move behind the background
                    //Changes the current position to a ratio (currentLerpTime/lerpTime) of the distance between the current Position and Position p
                    transform.position = Vector3.Lerp(transform.position, p, currentLerpTime / lerpTime);
                }
            } else { //Secondary Pathing Method
                //Changes the current position to a ratio (currentLerpTime/lerpTime) of the distance between the current Position and Position TargetPos
                transform.position = Vector3.Lerp(transform.position, TargetPos, currentLerpTime / lerpTime);
                if ((Vector2) transform.position == DestroyPos) { //Destroys the current object if the position matches DestroyPos
                    Destroy(this);
                }
            }
        }
        
    }
}