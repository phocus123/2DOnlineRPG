using RPG.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.CameraUI
{
    [Serializable]
    public class GuildAbilityUI 
    {
        [Header("Canvas Groups")]
        public CanvasGroup mainCanvas;
        public CanvasGroup upgradeCanvas;
        public CanvasGroup abilityDetailsCanvas;
        public CanvasGroup statUpgradeCanvas;

        [Header("Ability Details")]
        public Text title;
        public Text abilityTitle;
        public Image icon;
        public Text level;
        public Text description;
        public Text exp;
        public Button upgradeButton;
        public GameObject buttonParent;
        public GameObject buttonPrefab;

        [Header("Ability Stats")]
        public Button acceptButton;
        public Button cancelButton;
        public Text statsAbilityTite;
        public Text points;

        [Header("Stat")]
        public GameObject statParent;
        public GameObject statPrefab;

        [SerializeField] List<CanvasGroup> canvasGroups = new List<CanvasGroup>();

        List<GameObject> abilityButtons = new List<GameObject>();
        List<GameObject> stats = new List<GameObject>();
        Guild currentGuild;
        int selectedAbilityindex;

        public Guild CurrentGuild { get { return currentGuild; } }

        public List<GameObject> AbilityButtons
        {
            get { return abilityButtons; }
            set { abilityButtons = value; }
        }

        public int SelectedAbilityIndex
        {
            get { return selectedAbilityindex; }
            set { selectedAbilityindex = value; }
        }

        public void OpenGuildAbilities(Guild guild)
        {
            currentGuild = guild;
            UIHelper.ToggleCanvasGroup(mainCanvas);
            title.text = guild.name;
            LoadAbilitiesDetails();
            cancelButton.onClick.AddListener(ReturnToAbilityDetails);
        }

        public void CloseGuildAbilities()
        {
            foreach (GameObject button in abilityButtons)
            {
                button.GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Destroy(button.gameObject);
            }

            UIHelper.CloseAllCanvasGroups(canvasGroups);
            abilityButtons.Clear();
        }

        void LoadAbilitiesDetails()
        {
            foreach (Ability ability in currentGuild.GuildAbilities)
            {
                var button = GameObject.Instantiate(buttonPrefab, buttonParent.transform);
                button.GetComponentInChildren<Text>().text = ability.name;
                button.GetComponent<Button>().onClick.AddListener(() => ShowAbilityDetails(ability));
                abilityButtons.Add(button);
                ability.AttachAbilityUITo(button);
            }
            //UIHelper.ToggleCanvasGroup(upgradeCanvas);
            //UIHelper.ToggleCanvasGroup(abilityDetailsCanvas);
            //var firstAbility = currentGuild.GuildAbilities[0];
            //ShowAbilityDetails(firstAbility);
        }

        void ShowAbilityDetails(Ability ability)
        {
            if (upgradeCanvas.alpha == 0)
            {
                UIHelper.ToggleCanvasGroup(upgradeCanvas);
                UIHelper.ToggleCanvasGroup(abilityDetailsCanvas);
            }

            icon.sprite = ability.Icon;
            icon.color = Color.white;
            abilityTitle.text = ability.name;
            description.text = ability.Description;
        }

        public void LoadAbilityStats(Ability ability)
        {
            UIHelper.ToggleCanvasGroup(abilityDetailsCanvas);
            UIHelper.ToggleCanvasGroup(statUpgradeCanvas);
            ShowAbilityStatDetails(ability);

            foreach (AbilityStat stat in ability.AbilityUI.StatParams.Stats)
            {
                var statObject = GameObject.Instantiate(statPrefab, statParent.transform);
                statObject.GetComponent<Text>().text = stat.abilityName;
                statObject.transform.GetChild(0).GetComponent<Text>().text = stat.Value.ToString();
                stats.Add(statObject);
            }
        }

        void ShowAbilityStatDetails(Ability ability)
        {
            statsAbilityTite.text = ability.name;
            points.text = "points: " + 1;
        }

        void ReturnToAbilityDetails()
        {
            foreach (GameObject statObject in stats)
            {
                GameObject.Destroy(statObject);
            }

            stats.Clear();
            UIHelper.ToggleCanvasGroup(abilityDetailsCanvas);
            UIHelper.ToggleCanvasGroup(statUpgradeCanvas);
        }
    }
}