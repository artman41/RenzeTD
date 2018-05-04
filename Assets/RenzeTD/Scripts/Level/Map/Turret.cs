using System.Collections.Generic;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Map {
    public class Turret : MonoBehaviour {
        public enum TurretUpgrade {
            Start,
            Upgraded,
            Final
        }

        /// <summary>
        /// The possible upgrades to the bottom of the turret
        /// </summary>
        public Dictionary<TurretUpgrade, Sprite> BaseUpgradeTree;
        /// <summary>
        /// The possible upgrades to the top of the turret
        /// </summary>
        public Dictionary<TurretUpgrade, Sprite> TopperUpgradeTree;

        void Start() {
            if (BaseUpgradeTree == null) {
                BaseUpgradeTree = new Dictionary<TurretUpgrade, Sprite>(); //if the dictionary is not initalized, create a new instance
            }
            if (TopperUpgradeTree == null) {
                TopperUpgradeTree = new Dictionary<TurretUpgrade, Sprite>(); //if the dictionary is not initalized, create a new instance
            }
        }
    }
}