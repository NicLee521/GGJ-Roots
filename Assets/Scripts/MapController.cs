using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct TileData {
    public int resourceNum;
    public string resourceColor;
    public TileBase tile;

    public TileData(TileBase tile) {
        this.tile = tile;
        switch(tile.name) {
            case "BlueResource":
                this.resourceColor = "blue";
                break;
            case "YellowResource":
                this.resourceColor = "yellow";
                break;
            case "GreenResource":
                this.resourceColor = "green";
                break;
            case "RedResource":
                this.resourceColor = "red";
                break;
            default:
                this.resourceColor = "none";
                break;
        }
        this.resourceNum = Random.Range(20,60);
    }
}

public class MapController : MonoBehaviour
{
    public Tilemap tileMap;
    public Dictionary<Vector3Int, TileData> mapDict = new Dictionary<Vector3Int, TileData>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Vector3Int position in tileMap.cellBounds.allPositionsWithin){
            TileBase tile = tileMap.GetTile(position);
            if(tile != null) {
                mapDict.Add(position, new TileData(tile));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
