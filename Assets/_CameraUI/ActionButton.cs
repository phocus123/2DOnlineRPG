using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPG.Characters;
using System;
using RPG.Core;
using System.Collections;
using TMPro;

namespace RPG.CameraUI
{
    public class ActionButton : MonoBehaviour, IDropHandler
    {
        public delegate void OnActionButtonClicked(AbilityBehaviour abilityBehaviour);
        public event OnActionButtonClicked InvokeOnActionButtonClicked;

        [SerializeField] Image icon;
        [SerializeField] Image darkMask;
        [SerializeField] TextMeshProUGUI cooldownText;

        Button button;
        GameManager gameManager;

        public Ability Ability { get; set; }
        public Button Button { get { return button; } }
        public Image Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            if (Ability != null)
            {
                Ability.Behaviour.OnAttackInitiated += TriggerCooldownMask; 
                InvokeOnActionButtonClicked(Ability.Behaviour);
            }
        }

        private void TriggerCooldownMask(float time)
        {
            StartCoroutine(StartCooldownTimer(time));
            Ability.Behaviour.OnAttackInitiated -= TriggerCooldownMask;
        }

        IEnumerator StartCooldownTimer(float time)
        {
            float timer = 0;
            icon.color = Color.grey;
            darkMask.sprite = icon.sprite;
            darkMask.color = Color.white;

            while (time > timer)
            {
                timer += Time.deltaTime;
                darkMask.fillAmount = timer / time;
                cooldownText.text = Mathf.Round(time - timer).ToString();
                yield return new WaitForEndOfFrame();
            }
            ResetCooldown();
        }

        void ResetCooldown()
        {
            darkMask.color = Color.clear;
            darkMask.sprite = null;
            icon.color = Color.white;
            darkMask.fillAmount = 0;
            cooldownText.text = string.Empty;
        }

        public void SetAbility(Ability ability, DragItem item)
        {
            Ability = ability;
            icon.sprite = item.Moveable.Icon;
            icon.color = Color.white;
        }

        public void RemoveAbility()
        {
            Ability = null;
            icon.sprite = null;
            icon.color = Color.clear;
        }

        public void OnDrop(PointerEventData eventData)
        {
            DragItem item = eventData.pointerDrag.GetComponent<DragItem>();
            var uiManager = gameManager.uIManager;

            if (item.Moveable != null && item.Moveable is Ability)
            {
                SetAbility(item.Moveable as Ability, item);

                Ability a = Array.Find(gameManager.MasterAbilityList, x => x.Icon.name == item.Moveable.Icon.name);
                int index = Array.FindIndex(uiManager.ActionButtons, x => x.Button.name == Button.name);
                gameManager.savegameManager.AbilityDict.Add(index + 1, a.name);
            }
        }
    }
}
