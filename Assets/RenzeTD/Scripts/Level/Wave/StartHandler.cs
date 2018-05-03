using UnityEngine;

namespace RenzeTD.Scripts.Level.Wave {
    public class StartHandler : MonoBehaviour{

        public void BeginWave() {
            FindObjectOfType<WaveManager>().Begin();
        }
        
    }
}