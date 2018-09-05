using UnityEngine;
using UnityEditor;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Items/Base Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] string id;
        public Sprite icon;

        public string ID { get { return id; } }

        void OnValidate()
        {
            //string path = AssetDatabase.GetAssetPath(this);
            //id = AssetDatabase.AssetPathToGUID(path);
        }

        public virtual void Destroy()
        {

        }
    }
}