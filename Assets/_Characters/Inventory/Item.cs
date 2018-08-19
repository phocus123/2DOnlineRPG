﻿using UnityEngine;
using UnityEditor;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] string id;
        public string itemName;
        public Sprite icon;

        public string ID { get { return id; } }

        void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            id = AssetDatabase.AssetPathToGUID(path);
        }
    }
}