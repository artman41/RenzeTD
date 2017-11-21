using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RenzeTD.Scripts.Menu {
	public class MenuHandler : MonoBehaviour {

		public State GameState = State.MainMenu;

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {

		}

		void OnGUI() {
			switch(GameState) {
				case State.MainMenu:
					bool startGame = GUI.Button(new Rect(new Vector2(Screen.width / 2 - 100f / 2, Screen.height / 2 - 30f / 2), new Vector2(100f, 30f)), "Start Game");
					if (startGame) {
						GameState = State.LevelSelect;
					}
					break;
				case State.LevelSelect:
					Rect[,] Levels = new Rect[3,3];
					var boxSize = new Vector2(300, 300);
					for (int i = 0; i < 3; i++) {
						for (int j = 0; j < 3; j++) {
							Levels[i,j] = new Rect(new Vector2(i * (Screen.width - boxSize.x)/4, j * (Screen.width - boxSize.y)/4), boxSize);
						}
					} //Populate Array with values required.
					
					break;
				case State.LoadLevel:
					break;
				default:
					GUI.Label(new Rect(new Vector2(Screen.width / 2 - 100f / 2, Screen.height / 2 - 30f / 2), new Vector2(100f, 30f)), $"EDIT {GetType().Name} TO ACCOUNT FOR GAMESTATE: {GameState}");
					break;
			}
		}
	}
}