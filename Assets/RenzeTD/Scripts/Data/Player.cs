using System.Collections.Generic;
using RenzeTD.Scripts.Enemies;

namespace RenzeTD.Scripts.Data {
    public class Player {
        public int Money;
        public int Health;
        public Dictionary<Enemy, int> Killed;
    }
}