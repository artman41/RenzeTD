using System.Collections.Generic;
using UnityEngine;

namespace RenzeTD.Scripts.Level.Map {
    public class Turret : MonoBehaviour {
        public enum TurretUpgrade {
            Start,
            Upgraded,
            Final
        }

        public Dictionary<TurretUpgrade, Sprite> BaseUpgradeTree;
        public Dictionary<TurretUpgrade, Sprite> TopperUpgradeTree;

        void Start() {
            if (BaseUpgradeTree == null) {
                BaseUpgradeTree = new Dictionary<TurretUpgrade, Sprite>();
            }
            if (TopperUpgradeTree == null) {
                TopperUpgradeTree = new Dictionary<TurretUpgrade, Sprite>();
            }
        }
    }
}