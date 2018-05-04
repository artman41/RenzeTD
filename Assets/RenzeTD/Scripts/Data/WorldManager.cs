using System.Collections.Generic;
using RenzeTD.Scripts.Enemies;
using UnityEngine;

namespace RenzeTD.Scripts.Data {
    public class WorldManager : MonoBehaviour {
        public int Wave; //The current wave of enemies

        public int Health; //current health of the player
        public int Money; //current money of the player
        public Dictionary<EnemyType.Colour, int> Killed; //amount of enemies killed by the player

        void Start()
        {
            Killed = new Dictionary<EnemyType.Colour, int>(); //initializes Dictionary variable
        }

    }
}