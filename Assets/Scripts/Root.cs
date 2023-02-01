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
    void Start()
    {
        thisCollider = gameObject.GetComponent<MeshCollider>();
        player = GameObject.FindObjectOfType<PlayerController>();
        map = GameObject.FindObjectOfType<MapController>();

        player.rootPlacement.AddListener(RootPlaced);
        GetConnectedTiles();
    }

    public void RootPlaced() {
        foreach(Vector3Int tilePos in connectedTiles) {
            TileData tileData = map.mapDict[tilePos];
            if(tileData.resourceColor != "none") {
                tileData.resourceNum -= 5;
                player.ChangeResourceValues(5, tileData.resourceColor);
                Debug.Log(tilePos + " : " + tileData.resourceNum);
                map.mapDict[tilePos] = tileData;
            }
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
        }
    }
}
