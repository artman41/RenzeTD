using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RenzeTD.Scripts.Misc {
    public class InputManager : MonoBehaviour {

        enum GUIType {
            Menu,
            Options
        }
        
        bool showMenu;
        private GUIType GuiType;

        public Rect MenuBox;

        private int guiObjects;
        
        private void Start() {
            MenuBox = new Rect(Screen.width / 5 * 1.5f, Screen.height / 5, Screen.width / 5 * 2, Screen.height / 3 * 2);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                GuiType = GUIType.Menu;
                showMenu ^= true;
            }
            Settings.Instance.DirtyCheck();
        }

        private void OnGUI() {
            if (showMenu) {
                guiObjects = 0;
                GUI.Box(MenuBox, new GUIContent());
                GUI.Label(new Rect(MenuBox.center.x - 30f,MenuBox.y * ++guiObjects, 60f, 30f), GuiType.ToString(), new GUIStyle {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold,
                    normal = new GUIStyleState {
                        textColor = Color.white
                    }
                });
                
                switch (GuiType) {
                    case GUIType.Menu:
                        if (GUI.Button(new Rect(MenuBox.center.x - 120f,MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Main Menu")) {
                            SceneChanger.ChangeScene(SceneChanger.NavigationType.Menu);
                        }

                        GUISeparator();
                
                        if (GUI.Button(new Rect(MenuBox.center.x - 120f,  MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Options")) {
                            GuiType = GUIType.Options;
                        }

                        GUISeparator();
                
                        if (GUI.Button(new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Exit")) {
                            Application.Quit();
                        }

                        break;
                    case GUIType.Options:
                        GUI.Label(new Rect(MenuBox.center.x - 106f,MenuBox.y - 5 + 20f * ++guiObjects, 60f, 30f), "Music Volume", new GUIStyle {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold,
                            normal = new GUIStyleState {
                                textColor = Color.white
                            }
                        });
                        
                        Settings.Instance.MusicVolume = GUI.HorizontalSlider(
                            new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f),
                            Settings.Instance.MusicVolume, 0f, 1f);
                        
                        GUISeparator();
                        
                        GUI.Label(new Rect(MenuBox.center.x - 80f,MenuBox.y - 5 + 20f * ++guiObjects, 60f, 30f), "Sound Effects Volume", new GUIStyle {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold,
                            normal = new GUIStyleState {
                                textColor = Color.white
                            }
                        });
                        
                        Settings.Instance.SoundFXVolume = GUI.HorizontalSlider(
                            new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f),
                            Settings.Instance.SoundFXVolume, 0f, 1f);
                        
                        GUISeparator();
                        
                        if (GUI.Button(new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Back")) {
                            GuiType = GUIType.Menu;
                        }
                        
                        break;
                }
            }
        }

        void GUISeparator() {
            GUI.Label(new Rect(MenuBox.center.x - 120f,MenuBox.y + 30f * ++guiObjects, 240f, 10f), "");
        }
    }
}