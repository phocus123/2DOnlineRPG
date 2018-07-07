using System;
using UnityEngine;

namespace RPG.Environment
{
    public class Obstacle : MonoBehaviour, IComparable<Obstacle>
    {

        public SpriteRenderer ObstacleRenderer { get; set; }

        private Color defaultColor;
        private Color fadedColor;

        public int CompareTo(Obstacle other)
        {
            if (ObstacleRenderer.sortingOrder > other.ObstacleRenderer.sortingOrder)
            {
                return 1;
            }
            else if (ObstacleRenderer.sortingOrder < other.ObstacleRenderer.sortingOrder)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        // Use this for initialization
        void Start()
        {
            ObstacleRenderer = GetComponent<SpriteRenderer>();
            defaultColor = ObstacleRenderer.color;
            fadedColor.a = 0.5f;
        }

        public void FadeOut()
        {
            ObstacleRenderer.color = fadedColor;
        }

        public void FadeIn()
        {
            ObstacleRenderer.color = defaultColor;
        }
    }
}