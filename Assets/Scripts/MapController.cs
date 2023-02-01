using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct TileData {
    public int resourceNum;
    public string resourceColor;
    public TileBase tile;
    public bool takenFrom;
    public List<GameObject> attachedRoots;
    public Dictionary<string, int> numOfColorRoots;

    public TileData(TileBase tile) {
        this.takenFrom = false;
        this.attachedRoots = new List<GameObject>();
        this.numOfColorRoots = new Dictionary<string, int>(){
            {"blue", 0},
            {"red", 0},
            {"green", 0},
            {"yellow", 0},
        };
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

    public void SetAttachedRoots(GameObject root) {
        Root rootScript = root.GetComponent<Root>();
        if(!this.attachedRoots.Contains(root)) {
            this.attachedRoots.Add(root);
            this.numOfColorRoots[rootScript.GetRootColorString()] += 1;
        }
        if(rootScript.isStart) {
            return;
        }
        foreach (GameObject connectedRoot in rootScript.connections){
            if(this.attachedRoots.Contains(connectedRoot)) {
                continue;
            }
            SetAttachedRoots(connectedRoot);
        }
    }
}

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
