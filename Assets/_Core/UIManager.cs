using RPG.CameraUI;
using RPG.Characters;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Core
{
    public class UIManager : MonoBehaviour
    {
        [Header("User Interface")]
        public AbilityBook abilityUI; //TODO Make all these ui classes monobehaviours
        public DialogueUI dialogueUI;
        public TargetFrameUI targetFrameUI;
        public MainMenuUI mainMenuUI;
        public CanvasGroup CharacterPanel;

        [Header("Ability Advancement")]
        public AbilityButtonPanel abilityButtonPanel;
        public AbilityDetailsPanel abilityDetailsPanel;
        public AbilityStatsPanel abilityStatsPanel;

        [Header("Experience")]
        [SerializeField] Text playerExperienceText;

        [Header("Action Bar")]
        [SerializeField] ActionButton[] actionButtons;
        [SerializeField] GameObject[] keybindButtons;

        [Header("Combat Text")]
        [SerializeField] GameObject combatTextPrefab;
        [SerializeField] Canvas combatTextCanvas;
        [SerializeField] float speed;

        GameManager gameManager;

        public ActionButton[] ActionButtons { get { return actionButtons; } }
        public DialogueUI DialogueUI { get { return dialogueUI; } }

        void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
            UpdateExperienceText();
        }

        public void ShowTargetFrame(GameObject enemy)
        {
            targetFrameUI.ShowTargetFrame(enemy);
        }

        public void HideTargetFrame()
        {
            targetFrameUI.HideTargetFrame();
        }

        public void ShowGuildAbilities(Guild guild)
        {
            abilityButtonPanel.Init(guild);
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

        public void UpdateExperienceText()
        {
            playerExperienceText.text = "Experience: " + gameManager.PlayerExperience.ToString();
        }

        public void TriggerCombatText(Vector2 position, float healthValue)
        {
            GameObject combatText = Instantiate(combatTextPrefab, position, Quaternion.identity, combatTextCanvas.transform);
            combatText.GetComponent<CombatText>().Initialise(speed, Vector2.up);
            combatText.GetComponent<TextMeshProUGUI>().text = healthValue.ToString();
        }
    }
}