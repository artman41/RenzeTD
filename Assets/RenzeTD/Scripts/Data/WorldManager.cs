using System.Collections.Generic;
using RenzeTD.Scripts.Enemies;
using UnityEngine;

namespace RenzeTD.Scripts.Data {
    public class WorldManager : MonoBehaviour {
        public int Wave;

        public int Health;
        public int Money;
        public Dictionary<EnemyType.Colour, int> Killed;

        void Start()
        {
            Killed = new Dictionary<EnemyType.Colour, int>();
        }

    }
}