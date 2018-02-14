using RenzeTD.Scripts.Level.Map;
using UnityEngine;
using UnityEngine.Events;

namespace RenzeTD.Scripts.Level {
    public class ClickHandler : MonoBehaviour {
        public enum ClickType {
            SHOPTURRET,
            TURRETMENU
        }

        //public UnityAction<ClickType, GameObject> ActionHandler;

        /*private void Start() {
            ActionHandler += FunctionHandler;
        }*/

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
            var turret = go.GetComponent<Turret>();
            if (turret == null) return;
            Debug.Log("Turretmenu clicked");
        }
    }
}