using RPG.Characters;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core;
using System.Collections.Generic;

namespace RPG.CameraUI
{
    public class AbilityStatsPanel : MonoBehaviour
    {
        public delegate void OnStatOperatorCreated(GameObject button, AbilityStat stat);
        public event OnStatOperatorCreated InvokeOnStatOperatorCreated;

        public delegate void OnStatOperatorDestroyed(GameObject button);
        public event OnStatOperatorDestroyed InvokeOnStatOperatorDestroyed;

        public delegate void OnAcceptButtonClicked();
        public event OnAcceptButtonClicked InvokeOnAcceptButtonClicked;

        public delegate void OnCancelButtonClicked();
        public event OnCancelButtonClicked InvokeOnCancelButtonClicked;

        [Header("Ability Stats")]
        public CanvasGroup abilityStatCanvas;
        public Button acceptButton;
        public Button cancelButton;
        public Text abilityName;
        public Text points;

        [Header("Stat")]
        public GameObject statParent;
        public GameObject statPrefab;
        public GameObject addPrefab;
        public GameObject subtractPrefab;

        AbilityDetailsPanel abilityDetailsPanel;
        AbilityAdvancement abilityAdvancement;
        List<GameObject> stats = new List<GameObject>();
        List<GameObject> statOperators = new List<GameObject>();

        private void Awake()
        {
            abilityAdvancement = FindObjectOfType<AbilityAdvancement>();
        }

        void Start()
        {
            abilityAdvancement.InvokeOnStatChanged += UpdatePage;
        }

        public void Init(AbilityDetailsPanel abilityDetailsPanel)
        {
            this.abilityDetailsPanel = abilityDetailsPanel;

            abilityName.text = abilityDetailsPanel.CurrentSelectedAbility.name;
            points.text = "points: " + abilityAdvancement.AbilityPoints.CurrentPoints.ToString();
            UIHelper.ToggleCanvasGroup(abilityDetailsPanel.abilityDetailsCanvas);
            UIHelper.ToggleCanvasGroup(abilityStatCanvas);
            LoadAbilityStats(abilityDetailsPanel.CurrentSelectedAbility);
            cancelButton.onClick.AddListener(CancelButtonClicked);
            acceptButton.onClick.AddListener(AcceptButtonClicked);
        }

        public void UpdatePage(AbilityStat stat, float amount)
        {
            points.text = "points: " + abilityAdvancement.AbilityPoints.CurrentPoints.ToString();
            var statObject = stats.Find(x => x.GetComponent<Text>().text == stat.abilityName);
            float currentAmount = float.Parse(statObject.transform.GetChild(0).GetComponent<Text>().text);
            statObject.transform.GetChild(0).GetComponent<Text>().text = (currentAmount += amount).ToString(); 
        }

        void LoadAbilityStats(Ability ability)
        {
            foreach (AbilityStat stat in ability.AbilityUI.Stats)
            {
                var statObject = Instantiate(statPrefab, statParent.transform);
                var statOperatorParent = statObject.transform.GetChild(1).gameObject;
                statObject.GetComponent<Text>().text = stat.abilityName;
                statObject.transform.GetChild(0).GetComponent<Text>().text = stat.Value.ToString();
                stats.Add(statObject);

                switch (stat.statOperator)
                {
                    case AbilityStat.StatOperator.Add:
                        var addOperator = Instantiate(addPrefab, statOperatorParent.transform);
                        statOperators.Add(addOperator);
                        InvokeOnStatOperatorCreated(addOperator, stat);
                        break;
                    case AbilityStat.StatOperator.Subtract:
                        var subtractOperator = Instantiate(subtractPrefab, statOperatorParent.transform);
                        statOperators.Add(subtractOperator);
                        InvokeOnStatOperatorCreated(subtractOperator, stat);
                        break;
                }
            }
        }

        void AcceptButtonClicked()
        {
            InvokeOnAcceptButtonClicked();
            ReturnToAbilityDetails();
        }

        void CancelButtonClicked()
        {
            for (int i = 0; i < abilityAdvancement.StatPoints.AbilityChanges.Count; i++)
            {
                var statObject = stats.Find(x => x.GetComponent<Text>().text == abilityAdvancement.StatPoints.AbilityChanges[i].abilityName);
                float currentAmount = float.Parse(statObject.transform.GetChild(0).GetComponent<Text>().text);
                statObject.transform.GetChild(0).GetComponent<Text>().text = (currentAmount += abilityAdvancement.StatPoints.AbilityChangeAmounts[i]).ToString();
            }

            InvokeOnCancelButtonClicked();
            ReturnToAbilityDetails();
        }

        void ReturnToAbilityDetails()
        {
            foreach (GameObject statObject in stats)
            {
                Destroy(statObject);
            }

            foreach (GameObject statOperator in statOperators)
            {
                Destroy(statOperator);
                InvokeOnStatOperatorDestroyed(statOperator);
            }

            stats.Clear();
            statOperators.Clear();
            cancelButton.onClick.RemoveAllListeners();
            acceptButton.onClick.RemoveAllListeners();
            UIHelper.ToggleCanvasGroup(abilityDetailsPanel.abilityDetailsCanvas);
            UIHelper.ToggleCanvasGroup(abilityStatCanvas);
        }
    }
}