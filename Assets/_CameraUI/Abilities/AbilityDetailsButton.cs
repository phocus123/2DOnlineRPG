using RPG.Characters;
using RPG.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.CameraUI
{
    public class AbilityDetailsButton : MonoBehaviour
    {
        public delegate void OnButtonDestroyed(AbilityDetailsButton button);
        public event OnButtonDestroyed InvokeOnButtonDestroyed;

        private void OnDestroy()
        {
            if (InvokeOnButtonDestroyed != null)
            {
                InvokeOnButtonDestroyed(this);
            }
        }
    }
}