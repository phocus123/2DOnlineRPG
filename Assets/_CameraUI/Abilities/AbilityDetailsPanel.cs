using RPG.Characters;
using UnityEngine;
using RPG.Core;
using UnityEngine.UI;

namespace RPG.CameraUI
{
    public class AbilityDetailsPanel : MonoBehaviour
    {
        public delegate void OnSelectedAbilityChanged(Ability ability);
        public event OnSelectedAbilityChanged InvokeOnSelectedAbilityChanged;

        public delegate void OnAbilityPointPurchased(Ability ability);
        public event OnAbilityPointPurchased InvokeOnAbilityPointPurchased;

        [Header("Ability Details")]
        public CanvasGroup upgradeCanvas;
        public CanvasGroup abilityDetailsCanvas;
        public Text abilityTitle;
        public Image icon;
        public Text level;
        public Text description;
        public Text experienceCost;
        public Text experiencePoints;
        public Text availablePoints;
        public Button allocateButton;
        public Button purchaseButton;
        public Button resetButton;

        UIManager uIManager;
        AbilityAdvancementManager abilityAdvancementManager;
        Ability currentSelectedAbility;

        public Ability CurrentSelectedAbility
        {
            get { return currentSelectedAbility; }
            set
            {
                InvokeOnSelectedAbilityChanged(currentSelectedAbility);
                currentSelectedAbility = value;
            }
        }

        void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
            abilityAdvancementManager = FindObjectOfType<AbilityAdvancementManager>();
        }

        void Start()
        {
            abilityAdvancementManager.InvokeOnAbilityPointChanged += UpdateAbilityCostDetails;
            //uIManager.abilityButtonPanel.InvokeOnAbilityCanvasClosed += RemoveButtonListeners;
        }

        public void Init(Ability currentSelectedAbility)
        {
            CurrentSelectedAbility = currentSelectedAbility;
            ShowBasicAbilityDetails();
            RemoveButtonListeners(currentSelectedAbility);
            AddButtonListeners(currentSelectedAbility);
        }

        public void UpdateAbilityCostDetails()
        {
            if (abilityAdvancementManager.CurrentAbilityPointTransaction == null)
            {
                availablePoints.text = "Available points: " + 0;
            }
            else
            {
                availablePoints.text = "Available points: " + abilityAdvancementManager.CurrentAbilityPointTransaction.CurrentPoints.ToString();
            }
            experiencePoints.text = "Current XP: " + FindObjectOfType<GameManager>().PlayerExperience.ToString();
            level.text = "Level: " + currentSelectedAbility.Level.ToString();
            experienceCost.text = "Experience Required: " + currentSelectedAbility.CurrentExperienceCost;
        }

        void ShowBasicAbilityDetails()
        {
            if (upgradeCanvas.alpha == 0)
            {
                UIHelper.ToggleCanvasGroup(upgradeCanvas);
                UIHelper.ToggleCanvasGroup(abilityDetailsCanvas);
            }

            icon.sprite = currentSelectedAbility.Icon;
            icon.color = Color.white;
            abilityTitle.text = currentSelectedAbility.name;
            description.text = currentSelectedAbility.Description;
            UpdateAbilityCostDetails();
        }

        void AddButtonListeners(Ability ability)
        {
            allocateButton.onClick.AddListener(() => uIManager.abilityStatsPanel.Init(this));
            purchaseButton.onClick.AddListener(() => InvokePurchaseEvent(ability));
            resetButton.onClick.AddListener(() => abilityAdvancementManager.RefundAbilityPoints(ability));
        }

        void RemoveButtonListeners(Ability ability)
        {
            allocateButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.RemoveAllListeners();
            resetButton.onClick.RemoveAllListeners();
        }

        void InvokePurchaseEvent(Ability ability)
        {
            if (InvokeOnAbilityPointPurchased != null)
            {
                InvokeOnAbilityPointPurchased(ability);
            }
        }
    }
}