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
        
        /// <summary>
        /// Whether the UI should be shown
        /// </summary>
        bool showMenu;
        
        /// <summary>
        /// The type of UI to be shown
        /// </summary>
        private GUIType GuiType;

        /// <summary>
        /// The size and pos of the containing UI Elements.
        /// i.e. the UI container
        /// </summary>
        public Rect MenuBox;

        /// <summary>
        /// Amount of UI objects currently displayed
        /// </summary>
        private int guiObjects;

        /// <summary>
        /// Reference to the local PreservedData object
        /// </summary>
        private PreservedData pd;
        
        private void Start() {
            MenuBox = new Rect(Screen.width / 5 * 1.5f, Screen.height / 5, Screen.width / 5 * 2, Screen.height / 3 * 2); //sets the UI Container to a dynamic section of the screen
            pd = FindObjectOfType<PreservedData>(); //points the local variable to the instance of PreservedData
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) { //If the escape key was pressed
                GuiType = GUIType.Pause; //set the type of gui to be shown to Pause
                //bitwise NOT, equal to showMenu = !showMenu 
                // works by comparing against true;
                // :: showmenu = false, Bit value = 0x0
                // :: 0x0 NOT 0x1 = 0x1, showMenu = true
                // :: 0x1 NOT 0x1 = 0x0, showMenu = false
                showMenu ^= true; 
            }
            //Checks whether settings should be saved
            Settings.Instance.DirtyCheck();
        }

        private void OnGUI() {
            if (showMenu) { //if menu should be shown
                guiObjects = 0; //set the num of displayed objects to 0
                GUI.Box(MenuBox, new GUIContent()); //creates a blank box at MenuBox.position, with a size of MenuBox.size
                GUI.Label(
                    new Rect(MenuBox.center.x - 30f, MenuBox.y * ++guiObjects, 60f, 30f), //creates a rectangle at the upper center of the UI Container
                        GuiType.ToString(), //Gets the Gui Type as text, e.g GUIType.Pause = "Pause"
                        new GUIStyle {
                            alignment = TextAnchor.MiddleCenter, //centers any text
                            fontStyle = FontStyle.Bold, //bolds any text
                            normal = new GUIStyleState {
                                textColor = Color.white //sets the font colour to white
                            }
                        }
                    );
                
                switch (GuiType) { //shows different UI objects depending on GUIType
                    case GUIType.Pause: //If the pause menu
                        //creates a button at the y off the UI Container, with an offset of 20 * numOfObjects before increasing guiObjects by 1
                        if (GUI.Button(new Rect(MenuBox.center.x - 120f,MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Continue")) {
                            showMenu ^= true; //hides the menu
                        }

                        GUISeparator(); //adds a blank space between objects so that UI objects don't overlap
                        
                        //creates a button at the y off the UI Container, with an offset of 20 * numOfObjects before increasing guiObjects by 1
                        if (GUI.Button(new Rect(MenuBox.center.x - 120f,MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Main Menu")) {
                            SceneChanger.ChangeScene(SceneChanger.NavigationType.Menu); //changes the scene to the main menu
                        }

                        GUISeparator(); //adds a blank space between objects so that UI objects don't overlap
                
                        //creates a button at the y off the UI Container, with an offset of 20 * numOfObjects before increasing guiObjects by 1
                        if (GUI.Button(new Rect(MenuBox.center.x - 120f,  MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Options")) {
                            GuiType = GUIType.Options; //changes the current GUIType to Options
                        }

                        GUISeparator(); //adds a blank space between objects so that UI objects don't overlap

                        if (!pd.InEditMode) { //check if not in edit mode as certain functions should not be available when editing

                            //creates a button at the y off the UI Container, with an offset of 20 * numOfObjects before increasing guiObjects by 1
                            if (GUI.Button(new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Restart")) {
                                SceneChanger.ChangeScene(SceneChanger.NavigationType.Level); //changes the scene to the current scene so that it is reloaded
                            }

                            GUISeparator(); //adds a blank space between objects so that UI objects don't overlap
                        }

                        //creates a button at the y off the UI Container, with an offset of 20 * numOfObjects before increasing guiObjects by 1
                        if (GUI.Button(new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Exit")) {
                            Application.Quit(); //Causes the game to exit
                        }

                        if (!pd.InEditMode) { //check if not in edit mode as certain functions should not be available when editing

                            GUISeparator(); //adds a blank space between objects so that UI objects don't overlap

                            GUISeparator(); //adds a blank space between objects so that UI objects don't overlap

                            GUISeparator(); //adds a blank space between objects so that UI objects don't overlap

                            //creates a button at the y off the UI Container, with an offset of 20 * numOfObjects before increasing guiObjects by 1
                            if (GUI.Button(new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Create Map")) {
                                showMenu ^= true; //Hide map
                                pd.SelectedMap = null; //Clear the selected Map
                                pd.InEditMode = true; //Go to edit mode
                                SceneChanger.ChangeScene(SceneChanger.NavigationType.Level); //reload the scene to get a blank map with the current EditMode
                            }
                        }

                        break;
                    case GUIType.Options: //If in the options menu
                        
                        //Creates a label with the same offset as previous components, but -5. This allows the label to 'sit atop' UI components
                        GUI.Label(new Rect(MenuBox.center.x - 106f,MenuBox.y - 5 + 20f * ++guiObjects, 60f, 30f), "Music Volume", new GUIStyle {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold,
                            normal = new GUIStyleState {
                                textColor = Color.white
                            }
                        });
                        
                        //Creates a slider which returns a value when modified, where its current value is Settings.MusicVolume
                        Settings.Instance.MusicVolume = GUI.HorizontalSlider(
                            new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f),
                            Settings.Instance.MusicVolume, 0f, 1f);
                        
                        //Adds a gap between the UI Components
                        GUISeparator();
                        
                        //Creates a label with the same offset as previous components, but -5. This allows the label to 'sit atop' UI components
                        GUI.Label(new Rect(MenuBox.center.x - 80f,MenuBox.y - 5 + 20f * ++guiObjects, 60f, 30f), "Sound Effects Volume", new GUIStyle {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold,
                            normal = new GUIStyleState {
                                textColor = Color.white
                            }
                        });
                        
                        //Creates a slider which returns a value when modified, where its current value is Settings.SoundFXVolume
                        Settings.Instance.SoundFXVolume = GUI.HorizontalSlider(
                            new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f),
                            Settings.Instance.SoundFXVolume, 0f, 1f);
                        
                        //Adds a gap between the UI Components
                        GUISeparator();
                        
                        //Creates a button with an offset like all previous buttons
                        if (GUI.Button(new Rect(MenuBox.center.x - 120f, MenuBox.y + 20f * ++guiObjects, 240f, 30f), "Back")) {
                            GuiType = GUIType.Pause; //Changes the menu back to the Pause menu
                        }
                        
                        break;
                }
            }
        }

        /// <summary>
        /// Creates a blank label at a set position, while incrementing the total UI objects
        /// This causes a visual trick and allows the components to appear evenly spaced
        /// </summary>
        void GUISeparator() {
            GUI.Label(new Rect(MenuBox.center.x - 120f,MenuBox.y + 30f * ++guiObjects, 240f, 10f), "");
        }
    }
}