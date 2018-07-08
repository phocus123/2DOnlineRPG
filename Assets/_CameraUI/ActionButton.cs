using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.CameraUI
{
    public class ActionButton : MonoBehaviour, IDropHandler
    {
        //public IUseable Useable { get; set; }
        //public Ability Ability { get; set; }

        //private Player player;

        //[SerializeField] private Image icon;
        //public Image Icon
        //{
        //    get { return icon; }
        //    set { icon = value; }
        //}

        //private Button button;
        //public Button Button
        //{
        //    get
        //    {
        //        return button;
        //    }

        //    set
        //    {
        //        button = value;
        //    }
        //}

        //private void Start()
        //{
        //    player = Player.Instance.gameObject.GetComponent<Player>();
        //    button = GetComponent<Button>();
        //    button.onClick.AddListener(OnClick);
        //}

        //public void OnClick()
        //{
        //    if (Useable != null)
        //    {
        //        //Useable.Use();
        //    }
        //    player.UseAbility();
        //}

        //public void SetUseable(Ability ability, DragItem item)
        //{
        //    Ability = ability;
        //    Icon.sprite = item.Moveable.Icon;
        //    icon.color = Color.white;
        //}

        //public void RemoveUseable()
        //{
        //    Useable = null;
        //    icon.sprite = null;
        //    icon.color = Color.clear;
        //}

        public void OnDrop(PointerEventData eventData)
        {
            //DragItem item = eventData.pointerDrag.GetComponent<DragItem>();

            //if (item.Moveable != null && item.Moveable is Ability)
            //{
            //    SetUseable(item.Moveable as Ability, item);

            //    Ability a = Array.Find(AbilityManager.Instance.Abilities, x => x.Icon.name == item.Moveable.Icon.name);
            //    int index = Array.FindIndex(UIManager.Instance.ActionButtons, x => x.Button.name == Button.name);
            //    SaveGameManager.Instance.AbilityDict.Add(index + 1, a.name);
            //}
        }
    }
}
