using RPG.Characters;
using RPG.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.CameraUI
{
    public class AbilityStatsPanel : MonoBehaviour
    {
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
        [Space]
        [SerializeField] AbilityDetailsPanel abilityDetailsPanel;
        [SerializeField] AbilityAdvancement abilityAdvancement;

        List<GameObject> stats = new List<GameObject>();
        List<GameObject> statOperators = new List<GameObject>();

        public delegate void OnStatOperatorCreated(GameObject button, AbilityStat stat);
        public event OnStatOperatorCreated InvokeOnStatOperatorCreated;
        public event Action<GameObject> OnStatOperatorDestroyed;
        public event Action OnAcceptButtonClicked;
        public event Action OnCancelButtonClicked;

        void Start()
        {
            abilityAdvancement.InvokeOnStatChanged += UpdatePage;
        }

        public void Init()
        {
            if (abilityAdvancement.AbilityPoints.CurrentPoints == 0)
                return;

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
            var statObject = stats.Find(x => x.GetComponent<StatDisplay>().NameText.text == stat.statName);
            float currentAmount = float.Parse(statObject.GetComponent<StatDisplay>().ValueText.text);
            statObject.GetComponent<StatDisplay>().ValueText.text = (currentAmount += amount).ToString(); 
        }

        void LoadAbilityStats(Ability ability)
        {
            foreach (AbilityStat stat in ability.AbilityUI.Stats)
            {
                var statObject = Instantiate(statPrefab, statParent.transform);
                var statOperatorParent = statObject.transform.GetChild(2).gameObject;
                var statDisplay = statObject.GetComponent<StatDisplay>();
                statDisplay.NameText.text = stat.statName;
                statDisplay.ValueText.text = stat.Value.ToString();
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
            OnAcceptButtonClicked();
            ReturnToAbilityDetails();
        }

        void CancelButtonClicked()
        {
            for (int i = 0; i < abilityAdvancement.StatPoints.AbilityChanges.Count; i++)
            {
                var statObject = stats.Find(x => x.GetComponent<StatDisplay>().NameText.text == abilityAdvancement.StatPoints.AbilityChanges[i].statName);
                var statDisplay = statObject.GetComponent<StatDisplay>();
                float currentAmount = float.Parse(statDisplay.ValueText.text);
                statDisplay.ValueText.text = (currentAmount += abilityAdvancement.StatPoints.AbilityChangeAmounts[i]).ToString();
            }

            OnCancelButtonClicked();
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
                OnStatOperatorDestroyed(statOperator);
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