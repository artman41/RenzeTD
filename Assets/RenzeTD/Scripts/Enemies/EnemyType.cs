using System;
using UnityEngine;

namespace RenzeTD.Scripts.Enemies {
    [Serializable]
    public class EnemyType{
        public enum Colour {
            Red,
            Blue,
            Green,
            Yellow
        }

        [SerializeField]
        public Colour EnemyColour;
        [SerializeField]
        public Sprite EnemySprite;
        [SerializeField]
        public int EnemyValue;

    }
}