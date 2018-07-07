﻿using UnityEngine;
using UnityEngine.Tilemaps;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap = null;

        private Transform target;
        private float xMax, xMin, yMin, yMax;

        private PlayerControl playerControl;

        // Use this for initialization
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            playerControl = target.GetComponent<PlayerControl>();

            Vector3 minTile = tilemap.CellToWorld(tilemap.cellBounds.min);
            Vector3 maxTile = tilemap.CellToWorld(tilemap.cellBounds.max);

            SetLimits(minTile, maxTile);

            playerControl.SetLimits(minTile, maxTile);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            //if (playerControl.IsAlive)
            //{
            //}
                transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), -10f);
        }

        private void SetLimits(Vector3 minTile, Vector3 maxTile)
        {
            Camera cam = Camera.main;

            float height = 2f * cam.orthographicSize;
            float width = height * cam.aspect;

            xMin = minTile.x + width / 2;
            xMax = maxTile.x - width / 2;

            yMin = minTile.y + height / 2;
            yMax = maxTile.y - height / 2;
        }
    }
}