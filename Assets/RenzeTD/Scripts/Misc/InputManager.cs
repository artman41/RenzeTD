using System;
using System.Threading.Tasks;
using RenzeTD.Scripts.Data;
using UnityEngine;

namespace RenzeTD.Scripts.Misc {
    public class InputManager : MonoBehaviour {

        enum GUIType {
            Pause,
            Options
        }
        
        bool showMenu;
        private GUIType GuiType;

        public Rect MenuBox;

        private int guiObjects;

        private PreservedData pd;
        
        private void Start() {
            MenuBox = new Rect(Screen.width / 5 * 1.5f, Screen.height / 5, Screen.width / 5 * 2, Screen.height / 3 * 2);
            pd = FindObjectOfType<PreservedData>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                GuiType = GUIType.Pause;
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
                    case GUIType.Pause:
                        if (GUI.Button(new Rect(MenuBox.center.x - 120f,MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Continue")) {
                            showMenu ^= true;
                        }

                        GUISeparator();
                        
                        if (GUI.Button(new Rect(MenuBox.center.x - 120f,MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Main Menu")) {
                            SceneChanger.ChangeScene(SceneChanger.NavigationType.Menu);
                        }

                        GUISeparator();
                
                        if (GUI.Button(new Rect(MenuBox.center.x - 120f,  MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Options")) {
                            GuiType = GUIType.Options;
                        }

                        GUISeparator();

                        if (!pd.InEditMode) {

                            if (GUI.Button(new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f),
                                "Restart")) {
                                SceneChanger.ChangeScene(SceneChanger.NavigationType.Level);
                            }

                            GUISeparator();
                        }

                        if (GUI.Button(new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Exit")) {
                            Application.Quit();
                        }

                        if (!pd.InEditMode) {

                            GUISeparator();

                            GUISeparator();

                            GUISeparator();

                            if (GUI.Button(new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f),
                                "Create Map")) {
                                showMenu ^= true;
                                pd.SelectedMap = null;
                                pd.InEditMode = true;
                                SceneChanger.ChangeScene(SceneChanger.NavigationType.Level);
                            }
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
                            GuiType = GUIType.Pause;
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