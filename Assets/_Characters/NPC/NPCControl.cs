using UnityEngine;

namespace RPG.Characters
{
    public class NPCControl : CharacterController
    {
        public override DirectionParams GetDirectionParams()
        {
            DirectionParams directionParams = new DirectionParams(Vector2.zero);
            return directionParams;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}