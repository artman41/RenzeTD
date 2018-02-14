using System;
using System.Linq;
using RenzeTD.Scripts.Data;
using RenzeTD.Scripts.Level.Map;
using UnityEngine;
using UnityEngine.Events;

namespace RenzeTD.Scripts.Level {
    public class ClickHandler : MonoBehaviour {
        public enum ClickType {
            SHOPTURRET,
            TURRETMENU
        }

        private PreservedData pd;

        void Start() {
            pd = FindObjectOfType<PreservedData>();
        }

        public void FunctionHandler(ClickType ct, GameObject go) {
            Debug.Log($"Handling action from ({go.name}) of type ({ct})");
            switch (ct) {
                case ClickType.SHOPTURRET:
                    ShopTurret(go);
                    break;
                case ClickType.TURRETMENU:
                    TurretMenu(go);
                    break;
            }
        }

        public void ShopTurret(GameObject go) {
            Debug.Log("ShopTurret clicked");
        }

        public void TurretMenu(GameObject go) {
            if (pd.InEditMode) {
                var c = go.GetComponent<Cell>();
                var x = c.CellType.Next();
                if (x == Cell.Type.Turret || x.ToString().ToLower().Contains("junc")) x = x.Next();
                c.CellType = x;
            } else {
                var turret = go.GetComponent<Turret>();
                if (turret == null) return;
            }
            Debug.Log("Turretmenu clicked");
        }
    }
}