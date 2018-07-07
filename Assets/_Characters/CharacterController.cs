using UnityEngine;

namespace RPG.Characters
{
    public abstract class CharacterController : MonoBehaviour
    {
        public abstract Vector2 GetDirection();
    }
}