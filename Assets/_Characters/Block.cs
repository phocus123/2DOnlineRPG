using System;
using UnityEngine;

namespace RPG.Characters
{
    [Serializable]
    public class Block
    {
        [SerializeField] private GameObject block1 = null, block2 = null;

        public void Deactivate()
        {
            block1.SetActive(false);
            block2.SetActive(false);
        }

        public void Activate()
        {
            block1.SetActive(true);
            block2.SetActive(true);
        }

    }
}