using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RenzeTD.Scripts.Data;
using RenzeTD.Scripts.Level.Map;
using RenzeTD.Scripts.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RenzeTD.Scripts.Level {
    public class ClickHandler : MonoBehaviour {
        public enum ClickType {
            SHOPTURRET,
            TURRETMENU
        }

        /// <summary>
        /// the local PreservedData object
        /// </summary>
        private PreservedData pd;
        /// <summary>
        /// The sound to be played when a Cell is clicked in Edit Mode
        /// </summary>
        public AudioClip TileEdited;
        /// <summary>
        /// The AudioSource component on the current gameobject
        /// </summary>
        private AudioSource audio;

        void Start() {
            audio = GetComponent<AudioSource>(); //points the audio variable to the AudioSource component
            pd = FindObjectOfType<PreservedData>(); //points the pd variable to the PreservedData object
        }

        /// <summary>
        /// Performs an action on a gameobject, dependent upon its ClickType
        /// </summary>
        /// <param name="ct">ClickType</param>
        /// <param name="go">GameObject</param>
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

        /// <summary>
        /// Has yet to be implemented, but would be tiles are placed
        /// </summary>
        /// <param name="go"></param>
        public void ShopTurret(GameObject go) {
            Debug.Log("ShopTurret clicked");
        }

        /// <summary>
        /// Has two features.
        /// 1) In edit mode, change the tile & play an audio clip
        /// 2) In play mode, would be how turret upgrades are purchased
        /// </summary>
        /// <param name="go"></param>
        public void TurretMenu(GameObject go) {
            if (pd.InEditMode) { //checks if in edit mode
                Settings.Instance.SfxSources.Add(audio); //adds the audio source to the list of sources
                audio.clip = TileEdited; //sets the clip to be played to be the TileEdited clip
                audio.Play(); //plays the clip
                var c = go.GetComponent<Cell>(); //gets the cell component
                var x = c.CellType.Next(); //gets the next possible CellType
                if (x == Cell.Type.Turret || x.ToString().ToLower().Contains("junc")) x = x.Next(); //if the celltype is a turret or a junction, gets the next possible CellType
                c.CellType = x; //sets the cell type of the object to the previously retrieved CellType
            } else {
                var turret = go.GetComponent<Turret>(); //gets the Turret component of the game object
                if (turret == null) return; //doesn't do anything if the object isn't a turret
            }
            Debug.Log("Turretmenu clicked");
        }
    }
}