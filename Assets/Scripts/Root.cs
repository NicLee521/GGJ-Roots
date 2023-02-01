using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Root : MonoBehaviour
{
    Collider thisCollider;
    public List<GameObject> connections = new List<GameObject>();
    public List<TileBase> connectedTiles = new List<TileBase>();
    public bool isStart = false;
    void Start()
    {
        thisCollider = gameObject.GetComponent<MeshCollider>();
        GetConnectedTiles();
        foreach(Tile tile in connectedTiles) {
            if(tile.GetType() == typeof(ResourceTile)) {
                ResourceTile rTile = tile as ResourceTile;
                Debug.Log(rTile.GetResourceColor());
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetConnectedTiles() {
        Bounds colBounds = thisCollider.bounds;
        Tilemap tileMap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        Vector3 rightWorld = new Vector3(colBounds.center.x + colBounds.extents.x, colBounds.center.y, 0);
        Vector3 leftWorld = new Vector3(colBounds.center.x - colBounds.extents.x, colBounds.center.y, 0);
        Vector3Int rightTile = tileMap.WorldToCell((Vector2) rightWorld);
        Vector3Int leftTile = tileMap.WorldToCell((Vector2) leftWorld);
        connectedTiles.Add(tileMap.GetTile(rightTile));
        connectedTiles.Add(tileMap.GetTile(leftTile));
    }

    void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Root") {
            if(!connections.Contains(col.gameObject)) {
                connections.Add(col.gameObject);
            }
        }
    }
}
