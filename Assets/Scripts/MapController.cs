using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    public Tilemap tileMap;
    public Dictionary<Vector3Int, TileData> mapDict = new Dictionary<Vector3Int, TileData>();

    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Vector3Int position in tileMap.cellBounds.allPositionsWithin){
            TileBase tile = tileMap.GetTile(position);
            if(tile != null) {
                mapDict.Add(position, new TileData(tile));
            }
        }
        player = GameObject.FindObjectOfType<PlayerController>();
    }

   public void ClearAllTaken() {
        List<Vector3Int> keysToChange = new List<Vector3Int>();
        foreach (KeyValuePair<Vector3Int, TileData> entry in mapDict) {
            if(entry.Value.takenFrom) {
                keysToChange.Add(entry.Key);
            }
        }
        foreach(Vector3Int tilePos in keysToChange) {
            TileData data = mapDict[tilePos];
            data.takenFrom =false;
            mapDict[tilePos] = data;
        }
   }
}
