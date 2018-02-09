using System.Collections.Generic;

namespace RenzeTD.Scripts.Data {
    public class Player {
        public int Money;
        public int Health;
        public Dictionary<EnemyData.ColourType, int> Killed;
    }
}