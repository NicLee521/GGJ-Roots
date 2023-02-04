using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Root : MonoBehaviour
{
    Collider thisCollider;
    PlayerController player;
    MapController map;
    ObstacleController obstacle;
    public List<GameObject> connections = new List<GameObject>();
    public List<Vector3Int> connectedTiles = new List<Vector3Int>();
    public Tile emptyResource;
    public Tile capturedFont;
    public bool isStart = false;
    public string color;
    void Start()
    {
        thisCollider = gameObject.GetComponentInParent<MeshCollider>();
        player = GameObject.FindObjectOfType<PlayerController>();
        map = GameObject.FindObjectOfType<MapController>();
        obstacle = GameObject.FindObjectOfType<ObstacleController>();
        gameObject.GetComponent<SpriteRenderer>().sprite = player.GetCorrectSprite(color);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder  = 1;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = player.GetCorrectController(color);
        player.rootPlacement.AddListener(RootPlaced);
        Vector3Int[] currentTiles = GetConnectedTiles();
        foreach(Vector3Int tilePos in currentTiles) {
            TileData tile = map.mapDict[tilePos];
            HandleResourceTile(tile, tilePos);
            HandleFontTile(tile, tilePos);

        }
        gameObject.GetComponentInParent<RootParent>().root = this;
        
    }

    public void RootPlaced() {
        foreach(Vector3Int tilePos in connectedTiles) {
            TileData tileData = map.mapDict[tilePos];
            obstacle.IfTileIsTrigger(tilePos);
        }
    }

    void HandleFontTile(TileData tileData, Vector3Int tilePos) {
        if(!tileData.tile.name.Contains("Font")) {
            return;
        }
        float percentTarget = 40.0f;
        TutorialController tutorial = GameObject.FindObjectOfType<TutorialController>();

        switch(tileData.tile.name) {
            case "FontRed":
                if(GetPercentOfRootsOfColor("red", tileData) >= percentTarget) {
                    player.fontRed = true;
                    player.tileMap.SetTile(tilePos, capturedFont);
                }
                break;
            case "FontBlue":
                if(GetPercentOfRootsOfColor("blue", tileData) >= percentTarget) {
                    player.fontBlue = true;
                    player.tileMap.SetTile(tilePos, capturedFont);
                    if(tutorial != null) {
                        tutorial.PlayStep();
                    }
                }
                break;
            case "FontYellow":
                if(GetPercentOfRootsOfColor("yellow", tileData) >= percentTarget) {
                    player.fontYellow = true;
                    player.tileMap.SetTile(tilePos, capturedFont);
                }
                break;
            case "FontGreen":
                if(GetPercentOfRootsOfColor("green", tileData) >= percentTarget) {
                    player.fontGreen = true;
                    player.tileMap.SetTile(tilePos, capturedFont);
                }
                break;
            default:
                break;
        }
    }

    float GetPercentOfRootsOfColor(string color, TileData tileData) {
        return ((float)tileData.numOfColorRoots[color]/ (float)tileData.totalRoots)*100.0f;
    }

    void HandleResourceTile(TileData tileData, Vector3Int tilePos) {
        if(!tileData.tile.name.Contains("Resource")) {
            return;
        }
        if(!tileData.takenFrom && tileData.resourceNum > 0 && color == tileData.resourceColor) {
            player.ChangeResourceValues(tileData.resourceNum, tileData.resourceColor);
            tileData.resourceNum = 0;
            tileData.takenFrom = true;
            map.mapDict[tilePos] = tileData;
            player.tileMap.SetTile(tilePos, emptyResource);
        }
    }

    Vector3Int[] GetConnectedTiles() {
        Bounds colBounds = thisCollider.bounds;
        Tilemap tileMap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        Vector3 rightWorld = new Vector3(colBounds.center.x + colBounds.extents.x, colBounds.center.y, 0);
        Vector3 leftWorld = new Vector3(colBounds.center.x - colBounds.extents.x, colBounds.center.y, 0);
        Vector3Int rightTile = tileMap.WorldToCell((Vector2) rightWorld);
        Vector3Int leftTile = tileMap.WorldToCell((Vector2) leftWorld);
        connectedTiles.Add(rightTile);
        connectedTiles.Add(leftTile);
        return new Vector3Int[]{rightTile, leftTile};
    }

    
}
