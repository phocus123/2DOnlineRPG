using UnityEngine;

namespace RPG.Characters
{
    public abstract class CharacterController : MonoBehaviour
    {
        public abstract DirectionParams GetDirectionParams();
    }

    public struct DirectionParams
    {
        public Vector2 direction;
        public int exitIndex;

        public DirectionParams(Vector2 direction, int exitIndex = 0)
        {
            this.direction = direction;
            this.exitIndex = exitIndex;
        }
    }
}