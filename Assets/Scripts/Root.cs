using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Root : MonoBehaviour
{
    Collider thisCollider;
    PlayerController player;
    MapController map;
    public List<GameObject> connections = new List<GameObject>();
    public List<Vector3Int> connectedTiles = new List<Vector3Int>();
    public bool isStart = false;
    public string color;
    void Start()
    {
        thisCollider = gameObject.GetComponentInParent<MeshCollider>();
        player = GameObject.FindObjectOfType<PlayerController>();
        map = GameObject.FindObjectOfType<MapController>();
        gameObject.GetComponent<SpriteRenderer>().sprite = player.GetCorrectSprite(color);
        gameObject.GetComponent<Animator>().runtimeAnimatorController = player.GetCorrectController(color);
        player.rootPlacement.AddListener(RootPlaced);
        GetConnectedTiles();
        
    }

    public void RootPlaced() {
        foreach(Vector3Int tilePos in connectedTiles) {
            TileData tileData = map.mapDict[tilePos];
            HandleResourceTile(tileData, tilePos);
            HandleFontTile(tileData, tilePos);
        }
    }

    void HandleFontTile(TileData tileData, Vector3Int tilePos) {
        if(!tileData.tile.name.Contains("Font")) {
            return;
        }
        float percentTarget = 40.0f;
        switch(tileData.tile.name) {
            case "FontRed":
                if(GetPercentOfRootsOfColor("red", tileData) >= percentTarget) {
                    player.fontRed = true;
                }
                break;
            case "FontBlue":
                if(GetPercentOfRootsOfColor("blue", tileData) >= percentTarget) {
                    player.fontBlue = true;
                }
                break;
            case "FontYellow":
                if(GetPercentOfRootsOfColor("yellow", tileData) >= percentTarget) {
                    player.fontYellow = true;
                }
                break;
            case "FontGreen":
                if(GetPercentOfRootsOfColor("green", tileData) >= percentTarget) {
                    player.fontGreen = true;
                }
                break;
            default:
                break;
        }
    }

    float GetPercentOfRootsOfColor(string color, TileData tileData) {
        int totalRoots = 0;
        foreach (var item in tileData.numOfColorRoots){
            totalRoots += item.Value;
        }
        return ((float)tileData.numOfColorRoots[color]/ (float)totalRoots)*100.0f;
    }
    void HandleResourceTile(TileData tileData, Vector3Int tilePos) {
        if(!tileData.tile.name.Contains("Resource")) {
            return;
        }
        int resourcePerTurn = 10;
        if(!tileData.takenFrom && tileData.resourceNum > 0 && color == tileData.resourceColor) {
            if(resourcePerTurn > tileData.resourceNum) {
                resourcePerTurn = tileData.resourceNum;
            }
            tileData.resourceNum -= resourcePerTurn;
            player.ChangeResourceValues(resourcePerTurn, tileData.resourceColor);
            tileData.takenFrom = true;
            map.mapDict[tilePos] = tileData;
        }
    }

    void SetRootsForTiles() {
        foreach(Vector3Int tile in connectedTiles) {
            map.mapDict[tile].SetAttachedRoots(gameObject);
        }
    }

    void GetConnectedTiles() {
        Bounds colBounds = thisCollider.bounds;
        Tilemap tileMap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        Vector3 rightWorld = new Vector3(colBounds.center.x + colBounds.extents.x, colBounds.center.y, 0);
        Vector3 leftWorld = new Vector3(colBounds.center.x - colBounds.extents.x, colBounds.center.y, 0);
        Vector3Int rightTile = tileMap.WorldToCell((Vector2) rightWorld);
        Vector3Int leftTile = tileMap.WorldToCell((Vector2) leftWorld);
        connectedTiles.Add(rightTile);
        connectedTiles.Add(leftTile);
    }

    void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Root") {
            if(!connections.Contains(col.gameObject)) {
                connections.Add(col.gameObject);
            }
            SetRootsForTiles();
        }
    }
}
