using RPG.Characters;
using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Core
{
    public class UIManager : MonoBehaviour
    {
        [Header("User Interface")]
        [SerializeField] AbilityUI abilityUI;
        [SerializeField] DialogueUI dialogueUI;
        [SerializeField] TargetFrameUI targetFrameUI;
        [SerializeField] MainMenuUI mainMenuUI;

        [Header("Experience")]
        [SerializeField] Text playerExperienceText;

        [Header("Action Bar")]
        [SerializeField] ActionButton[] actionButtons;
        [SerializeField ]GameObject[] keybindButtons;


        GameManager gameManager;

        public ActionButton[] ActionButtons { get { return actionButtons; } }

        void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
            var playerAbilitySystem = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilitySystem>();

            UpdateExperienceText();
            abilityUI.Initialize(playerAbilitySystem);
        }

        public void ToggleInGameMenu()
        {
            mainMenuUI.ToggleInGameMenu();
        }

        public void CloseCanvases()
        {
            mainMenuUI.CloseCanvases(abilityUI);
        }

        public void ShowTargetFrame(GameObject enemy)
        {
            targetFrameUI.ShowTargetFrame(enemy);
        }

        public void HideTargetFrame()
        {
            targetFrameUI.HideTargetFrame();
        }

        public void ToggleCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
            canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
            ToggleInGameMenu();
        }

        public void UpdateKeyText(string key, KeyCode code)
        {
            Text temp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
            temp.text = code.ToString();
        }

        public void ClickActionButton(string buttonName)
        {
            Array.Find(ActionButtons, x => x.gameObject.name == buttonName).Button.onClick.Invoke();
        }

        public void OpenDialogue(DialogueParams dialogueParams)
        {
            StartCoroutine(dialogueUI.FadeBox());
            dialogueUI.dialogueBox.blocksRaycasts = true;
            dialogueUI.Initialize(dialogueParams);
        }

        public void UpdateExperienceText()
        {
            playerExperienceText.text = "Experience: " + gameManager.PlayerExperience.ToString();
        }
    }
}