using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPG.Characters;
using System;
using RPG.Core;
using System.Collections;

namespace RPG.CameraUI
{
    public class ActionButton : MonoBehaviour, IDropHandler
    {
        public delegate void OnActionButtonClicked(AbilityBehaviour abilityBehaviour);
        public event OnActionButtonClicked InvokeOnActionButtonClicked;

        [SerializeField] Image icon;
        [SerializeField] Image darkMask;

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
            gameManager = FindObjectOfType<GameManager>();
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            if (Ability != null)
            {
                var characterAttackSystem = Ability.Behaviour.Character.GetComponent<AttackSystem>();
                characterAttackSystem.InvokeOnAttackInitiated += TriggerCooldownMask; 
                InvokeOnActionButtonClicked(Ability.Behaviour);
            }
        }

        private void TriggerCooldownMask(float time)
        {
            StartCoroutine(StartCooldownTimer(time));
        }

        IEnumerator StartCooldownTimer(float time)
        {
            float timer = 0;
            while (time > timer)
            {
                timer += Time.deltaTime;
                darkMask.color = new Color(.5f, .5f, .5f, .5f);
                darkMask.fillAmount = timer / time;
                yield return new WaitForEndOfFrame();
            }
            ResetCooldown();
        }

        void ResetCooldown()
        {
            darkMask.color = new Color(.5f, .5f, .5f, 0);
            darkMask.fillAmount = 0;
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
            var uiManager = gameManager.uiManager;

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
