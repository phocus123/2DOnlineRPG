using UnityEngine;
using RPG.Characters;
using RPG.Core;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RPG.CameraUI
{
    public class AbilityButtonPanel : MonoBehaviour
    {
        public delegate void OnAbilityButtonsCreated(GameObject button);
        public event OnAbilityButtonsCreated InvokeOnAbilityButtonsCreated;

        public delegate void OnAbilityCanvasClosed(Ability ability);
        public event OnAbilityCanvasClosed InvokeOnAbilityCanvasClosed;

        public CanvasGroup guildAbilityCanvas;
        public Text title;
        public GameObject buttonPrefab;

        [SerializeField] List<CanvasGroup> guildCanvasGroups = new List<CanvasGroup>();

        Guild currentGuild;
        UIManager uIManager;
        AbilityDetailsPanel abilityDetailsPanel;
        List<GameObject> abilityButtons = new List<GameObject>();

        void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
            abilityDetailsPanel = uIManager.abilityDetailsPanel;
        }

        void Start()
        {
            uIManager.abilityStatsPanel.InvokeOnAcceptButtonClicked += RefreshButtonDetails;
        }

        public void Init(Guild currentGuild)
        {
            this.currentGuild = currentGuild;

            title.text = currentGuild.name;
            UIHelper.ToggleCanvasGroup(guildAbilityCanvas);
            LoadAbilitiesButtons();
        }

        void LoadAbilitiesButtons()
        {
            GameObject button = null;

            foreach (Ability ability in currentGuild.GuildAbilities)
            {
                button = Instantiate(buttonPrefab, gameObject.transform);
                button.transform.GetChild(0).GetComponent<Text>().text = ability.name;
                button.transform.GetChild(1).GetComponent<Text>().text = "Level: " + ability.Level.ToString();
                button.GetComponent<Button>().onClick.AddListener(() => ShowAbilityDetailsPanel(ability));
                abilityButtons.Add(button);
                ability.AttachAbilityUITo(button);
            }

            InvokeOnAbilityButtonsCreated(button);
        }

        public void CloseGuildAbilitiesCanvas()
        {
            foreach (GameObject button in abilityButtons)
            {
                button.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(button.gameObject);
            }

            if (abilityDetailsPanel.CurrentSelectedAbility != null)
            {
                InvokeOnAbilityCanvasClosed(abilityDetailsPanel.CurrentSelectedAbility);
            }

            UIHelper.CloseAllCanvasGroups(guildCanvasGroups);
            abilityButtons.Clear();
        }

        void ShowAbilityDetailsPanel(Ability ability)
        {
            abilityDetailsPanel.Init(ability);
        }

        void RefreshButtonDetails()
        {
            var abilityButton = abilityButtons.Find(x => x.GetComponent<AbilityUI>().Ability == abilityDetailsPanel.CurrentSelectedAbility);
            abilityButton.transform.GetChild(0).GetComponent<Text>().text = abilityDetailsPanel.CurrentSelectedAbility.name;
            abilityButton.transform.GetChild(1).GetComponent<Text>().text = "Level: " + abilityDetailsPanel.CurrentSelectedAbility.Level.ToString();
        }
    }
}