using System;
using UnityEngine;

namespace RPG.CameraUI
{
    [Serializable]
    public class MainMenuUI 
    {
        [SerializeField] CanvasGroup inGameMenu;
        [SerializeField] CanvasGroup keybindMenu;
        [SerializeField] CanvasGroup abilityMenu;

        public void ToggleInGameMenu()
        {
            inGameMenu.alpha = inGameMenu.alpha > 0 ? 0 : 1;
            inGameMenu.blocksRaycasts = inGameMenu.blocksRaycasts == true ? false : true;
        }

        public void CloseCanvases(AbilityBookUI abilityUI)
        {
            keybindMenu.alpha = 0;
            keybindMenu.blocksRaycasts = false;
            abilityMenu.alpha = 0;
            abilityMenu.blocksRaycasts = false;
            abilityUI.ResetPageIndex();
        }
    }
}