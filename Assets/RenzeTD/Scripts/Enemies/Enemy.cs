using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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

        public EnemyType.Colour Type {
            get { return _type; }
            set {
                _type = value; 
                UpdateInfo();
            }
        }
        
        public int Value;

        private NodePath path;
        public List<Node> PATH;
        private Node TargetNode;
        bool Killed;
        private bool useV2;
        private Vector2 TargetPos;
        private Vector2 DestroyPos;

        public void Start(){
            UpdateInfo();
            transform.position = (Vector3)((Vector2) transform.position) - Vector3.back;
        }

        void UpdateInfo([CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0) {
            //Debug.Log($"{memberName} @ {sourceLineNumber} in {sourceFilePath}");
            
            if(pd == null) pd = FindObjectOfType<PreservedData>();
            if(sr == null) sr = GetComponent<SpriteRenderer>();
            if(wm == null) wm = FindObjectOfType<WorldManager>();
            
            sr.sprite = GetSprite();
            Value = GetValue();
        }
        
        Sprite GetSprite() {
            var x = pd.EnemyCollection;
            var y = x.First(o => o.EnemyColour == Type);
            return y.EnemySprite;
        }

        int GetValue() {
            return pd.EnemyCollection.First(o => o.EnemyColour == Type).EnemyValue;
        }

        public bool Kill() {
            Killed = true;
            Destroy(this);
            return Killed;
        }
        
        private void OnDestroy() {
            if (Killed) {
                wm.Killed.Add(Type, 1);
                wm.Money += Value;
            } else {
                wm.Health -= Value;
            }
        }

        public void FollowPath(NodePath path = null) {
            if (this.path == null && path != null) {
                this.path = path;
            }
            try {
                GoTo(this.path?.Dequeue());
            } catch (InvalidOperationException e) {
                useV2 = true;
                TargetPos = transform.position + Vector3.down;
                DestroyPos = TargetPos;
            }
            PATH = this.path?.ToArray().ToList();
        }

        void GoTo(Node n) {
            currentLerpTime = 0f;
            TargetNode = n;
        }

        public float currentLerpTime;
        private float lerpTime = 1f;
        
        void FixedUpdate() {
            if (TargetNode == null && !useV2) return;
            //increment timer once per frame
            Debug.Log($"{name} :: curLerpTime ;; {currentLerpTime}");
            if (currentLerpTime > lerpTime) {
                currentLerpTime = lerpTime;
            } else {
                currentLerpTime += Time.fixedDeltaTime;
            }
            if (!useV2) {
                if ((Vector2) TargetNode.transform.position == (Vector2) transform.position) {
                    TargetNode = null;
                    this.Value++;
                    FollowPath();
                } else {
                    Debug.Log(
                        $"tNodPos {(Vector2) TargetNode.transform.position} != {(Vector2) transform.position}");
                    var p = TargetNode.transform.position;
                    p.Set(p.x, p.y, -1f);
                    transform.position = Vector3.Lerp(transform.position, p,
                        currentLerpTime / lerpTime);
                }
            } else {
                transform.position = Vector3.Lerp(transform.position, TargetPos,
                    currentLerpTime / lerpTime);
                if ((Vector2) transform.position == DestroyPos) {
                    Destroy(this);
                }
            }
        }
        
        /// <summary>
        /// https://answers.unity.com/questions/1013011/convert-recttransform-rect-to-screen-space.html
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Rect RectTransformToScreenSpace(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            return new Rect((Vector2)transform.position - (size * 0.5f), size);
        }
        
    }
}