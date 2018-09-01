using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class MainMenuUI :MonoBehaviour
    {
        public CanvasGroup inGameMenu;
        public CanvasGroup keybindMenu;
        public CanvasGroup abilityMenu;
        public Button keybindsButton;
        public Button abilityBookButton;

        public static Action OnAbilityBookToggledOn = delegate { };
        public static Action OnAbilityBookToggledOff = delegate { };

        private void Awake()
        {
            PlayerControl.OnEscapeKeyDown += ToggleInGameMenu;
            keybindsButton.onClick.AddListener(ToggleKeybindsMenu);
            abilityBookButton.onClick.AddListener(ToggleAbilityBookMenu);
        }

        public void ToggleInGameMenu()
        {
            inGameMenu.alpha = inGameMenu.alpha > 0 ? 0 : 1;
            inGameMenu.blocksRaycasts = inGameMenu.blocksRaycasts == true ? false : true;

            if (abilityMenu.alpha == 1 || keybindMenu.alpha == 1)
            {
                CloseCanvases();
            }
        }

        public void CloseCanvases()
        {
            keybindMenu.alpha = 0;
            keybindMenu.blocksRaycasts = false;
            abilityMenu.alpha = 0;
            abilityMenu.blocksRaycasts = false;

            OnAbilityBookToggledOff();
        }

        void ToggleKeybindsMenu()
        {
            ToggleInGameMenu();
            UIHelper.ToggleCanvasGroup(keybindMenu);
        }

        void ToggleAbilityBookMenu()
        {
            if (abilityMenu.alpha == 0)
            {
                OnAbilityBookToggledOn();
            }
            ToggleInGameMenu();
            UIHelper.ToggleCanvasGroup(abilityMenu);
        }
    }
}