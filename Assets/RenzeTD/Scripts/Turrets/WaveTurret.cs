using UnityEngine;

namespace RenzeTD.Scripts.Turrets {
	public class WaveTurret : MonoBehaviour {
		private int i;

		private void Start() {
			i = 0;
		}
		
		void DoAttack() {
			Debug.Log($"Attack {i}");
			i++;
		}
	}
}
