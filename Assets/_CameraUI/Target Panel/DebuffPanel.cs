using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class DebuffPanel : MonoBehaviour
    {
        public GameObject DebuffSlotObject;

        public void Init(Ability ability, float currentDuration, float maxDuration)
        {
            GameObject debuffSlot =  Instantiate(DebuffSlotObject, transform);
            debuffSlot.GetComponent<DebuffSlot>().Init(ability, currentDuration, maxDuration);
        }
    }
}