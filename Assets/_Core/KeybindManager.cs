using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RPG.Core;

namespace RPG.Core
{
    public class KeybindManager : MonoBehaviour
    {
        public GameManager gameManager;

        public Dictionary<string, KeyCode> Keybinds { get; private set; }
        public Dictionary<string, KeyCode> Actionbinds { get; private set; }

        string bindName;

        private void Start()
        {
            Initialize();
        }

        public void BindKey(string key, KeyCode keybind)
        {
            Dictionary<string, KeyCode> currentDictionary = Keybinds;
            var uiManager = gameManager.uIManager;

            if (key.Contains("ACT"))
            {
                currentDictionary = Actionbinds;
            }
            if (!currentDictionary.ContainsKey(key))
            {
                currentDictionary.Add(key, keybind);
                uiManager.UpdateKeyText(key, keybind);
            }
            else if (currentDictionary.ContainsValue(keybind))
            {
                string myKey = currentDictionary.FirstOrDefault(x => x.Value == keybind).Key;
                uiManager.UpdateKeyText(key, KeyCode.None);
            }

            currentDictionary[key] = keybind;
            uiManager.UpdateKeyText(key, keybind);
            bindName = string.Empty;
        }

        public void KeyBindonClick(string bind)
        {
            bindName = bind;
        }

        private void Initialize()
        {
            Keybinds = new Dictionary<string, KeyCode>();
            Actionbinds = new Dictionary<string, KeyCode>();

            BindKey("UP", KeyCode.W);
            BindKey("DOWN", KeyCode.S);
            BindKey("LEFT", KeyCode.A);
            BindKey("RIGHT", KeyCode.D);

            BindKey("ACT1", KeyCode.Alpha1);
            BindKey("ACT2", KeyCode.Alpha2);
            BindKey("ACT3", KeyCode.Alpha3);
            BindKey("ACT4", KeyCode.Alpha4);
            BindKey("ACT5", KeyCode.Alpha5);
            BindKey("ACT6", KeyCode.Alpha6);
            BindKey("ACT7", KeyCode.Alpha7);
            BindKey("ACT8", KeyCode.Alpha8);
            BindKey("ACT9", KeyCode.Alpha9);
            BindKey("ACT10", KeyCode.LeftControl & KeyCode.Alpha1); // TODO Come up with a system to use combinations of keys pressed.

        }


        private void OnGUI()
        {
            if (bindName != string.Empty)
            {
                Event e = Event.current;

                if (e.isKey)
                {
                    BindKey(bindName, e.keyCode);
                }
            }
        }

    }
}