using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.CameraUI
{
    public class PageButton : MonoBehaviour, IPointerClickHandler
    {
        public delegate void OnPageButtonClicked(GameObject button);
        public event OnPageButtonClicked InvokeOnPageButtonClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            InvokeOnPageButtonClicked(gameObject);
        }
    }
}