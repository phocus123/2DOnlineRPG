using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RPG.Environment
{
    public class TreeTile : Tile
    {
        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            go.GetComponent<SpriteRenderer>().sortingOrder = -position.y * 2;

            return base.StartUp(position, tilemap, go);
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Tiles/TreeTile")]
        public static void CreateTreeTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save TreeTile", "Treetile", "asset", "Save Treetile", "Assets");
            if (path == "")
            {
                return;
            }
            AssetDatabase.CreateAsset(CreateInstance<TreeTile>(), path);
        }
#endif
    }
}