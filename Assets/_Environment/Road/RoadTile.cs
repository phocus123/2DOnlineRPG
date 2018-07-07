using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RPG.Environment
{
    public class RoadTile : Tile
    {
        [SerializeField] private Sprite[] roadSprites = null;

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            return base.StartUp(position, tilemap, go);
        }

        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);

                    if (HasRoad(tilemap, nPos))
                    {
                        tilemap.RefreshTile(nPos);
                    }
                }
            }
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            string composition = string.Empty;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x != 0 || y != 0)
                    {
                        if (HasRoad(tilemap, new Vector3Int(position.x + x, position.y + y, position.z)))
                        {
                            composition += 'R';
                        }
                        else
                        {
                            composition += 'E';
                        }
                    }
                }
            }

            tileData.sprite = roadSprites[5];

            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'R')
            {
                tileData.sprite = roadSprites[5];
            }
            if (composition[1] == 'R' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
            {
                tileData.sprite = roadSprites[6];
            }
            if (composition[1] == 'R' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'R')
            {
                tileData.sprite = roadSprites[1];
            }
            if (composition[1] == 'R' && composition[3] == 'E' && composition[4] == 'R' && composition[6] == 'E')
            {
                tileData.sprite = roadSprites[10];
            }
            if (composition[1] == 'E' && composition[3] == 'R' && composition[4] == 'E' && composition[6] == 'E')
            {
                tileData.sprite = roadSprites[2];
            }
            if (composition[1] == 'E' && composition[3] == 'R' && composition[4] == 'R' && composition[6] == 'E')
            {
                tileData.sprite = roadSprites[2];
            }
            if (composition[1] == 'R' && composition[3] == 'R' && composition[4] == 'E' && composition[6] == 'E')
            {
                tileData.sprite = roadSprites[8];
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'R' && composition[6] == 'E')
            {
                tileData.sprite = roadSprites[4];
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'R' && composition[6] == 'R')
            {
                tileData.sprite = roadSprites[9];
            }
            if (composition[1] == 'E' && composition[3] == 'R' && composition[4] == 'E' && composition[6] == 'R')
            {
                tileData.sprite = roadSprites[7];
            }
            if (composition[1] == 'R' && composition[3] == 'R' && composition[4] == 'R' && composition[6] == 'R')
            {
                tileData.sprite = roadSprites[0];
            }
            if (composition[1] == 'R' && composition[3] == 'E' && composition[4] == 'R' && composition[6] == 'R')
            {
                tileData.sprite = roadSprites[0];
            }
            if (composition[1] == 'R' && composition[3] == 'R' && composition[4] == 'E' && composition[6] == 'R')
            {
                tileData.sprite = roadSprites[0];
            }
            if (composition[1] == 'R' && composition[3] == 'R' && composition[4] == 'R' && composition[6] == 'E')
            {
                tileData.sprite = roadSprites[0];
            }
            if (composition[1] == 'E' && composition[3] == 'R' && composition[4] == 'R' && composition[6] == 'R')
            {
                tileData.sprite = roadSprites[0];
            }
            if (composition[1] == 'E' && composition[3] == 'R' && composition[4] == 'E' && composition[6] == 'E')
            {
                tileData.sprite = roadSprites[3];
            }
        }

        private bool HasRoad(ITilemap tilemap, Vector3Int position)
        {
            return tilemap.GetTile(position) == this;
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Tiles/RoadTile")]
        public static void CreateRoadTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save RoadTile", "Roadtile", "asset", "Save roadtile", "Assets");
            if (path == "")
            {
                return;
            }
            AssetDatabase.CreateAsset(CreateInstance<RoadTile>(), path);
        }
#endif
    }
}