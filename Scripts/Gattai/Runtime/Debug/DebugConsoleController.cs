using System.Collections.Generic;
using UnityEngine;

namespace Gattai.Runtime.Debug
{
    public class DebugConsoleController : MonoBehaviour
    {
        private bool _showConsole;
        private string _inputConsole;
        private Vector2 scroll;
        private DebugCommand HELP;
        public static bool _showHelp;
        public static List<object> commandsList;

        private void Awake()
        {
            HELP = new DebugCommand("help", "Show debug commands list.", "help", () =>
            {
                _showHelp = !_showHelp;
            });

            commandsList = new List<object>()
            {
                HELP,
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                ToggleConsole();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                HandleInput();
            }
        }

        public void ToggleConsole()
        {
            _showConsole = !_showConsole;

            if (_showConsole)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void OnGUI()
        {
            if (!_showConsole)
            {
                return;
            }

            float y = 0f;


            GUIStyle textStyle = new GUIStyle();
            textStyle.fontSize = 26;
            GUIStyleState styleState = new GUIStyleState();
            styleState.textColor = Color.green;
            textStyle.normal = styleState;

            if (_showHelp)
            {
                GUI.Box(new Rect(0, y, Screen.width, 200), "");

                Rect viewport = new Rect(0, y, Screen.width - 30, 20 * commandsList.Count);

                scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 190), scroll, viewport);

                for (int i = 0; i < commandsList.Count; i++)
                {
                    DebugCommandBase command = commandsList[i] as DebugCommandBase;

                    string label = $"{command.CommandFormat} = {command.CommandDesciption}";

                    Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                    GUI.Label(labelRect, label, textStyle);
                }

                GUI.EndScrollView();

                y += 200;
            }

            GUI.Box(new Rect(0, y, Screen.width, 60f), "");
            GUI.backgroundColor = new Color(0f, 0f, 0f, 0f);
            _inputConsole = GUI.TextField(new Rect(10f, y + 10f, Screen.width - 10f, 40f), _inputConsole, textStyle);
        }

        private void HandleInput()
        {
            string[] properties = _inputConsole.Split(' ');

            if (string.IsNullOrEmpty(_inputConsole))
            {
                return;
            }

            for (int i = 0; i < commandsList.Count; i++)
            {
                DebugCommandBase commandBase = commandsList[i] as DebugCommandBase;

                if (_inputConsole.Contains(commandBase.CommandId))
                {
                    if (commandsList[i] as DebugCommand != null)
                    {
                        (commandsList[i] as DebugCommand).Invoke();
                    }
                    else if (commandsList[i] as DebugCommand<int> != null)
                    {
                        (commandsList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                    }
                    else if (commandsList[i] as DebugCommand<int, int> != null)
                    {
                        (commandsList[i] as DebugCommand<int, int>).Invoke(int.Parse(properties[1]), int.Parse(properties[2]));
                    }
                }
            }

            _inputConsole = "";
        }
    }
}
