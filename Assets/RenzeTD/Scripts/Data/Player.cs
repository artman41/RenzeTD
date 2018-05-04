using System.Collections.Generic;
using RenzeTD.Scripts.Enemies;

namespace RenzeTD.Scripts.Data {
    public class Player {
        public int Money; //Current amount of money held by the Player
        public int Health; //Current health of the player
        public Dictionary<Enemy, int> Killed; //Current amount of Enemies killed by the player
    }
}