using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class RandomMapCreator : MonoBehaviour
{
    public List<Sprite> tileSprites = new List<Sprite>();
    public Dictionary<Tile, int> numberOfTileTypes = new Dictionary<Tile, int>();
    public int maxResourceTilesOfOneColor = 3;
    public Tilemap tileMapToRender;
    public int height;
    public int width; 
    // Start is called before the first frame update
    void Start()
    {
        SetTileRepository();
        height = (int) (Camera.main.orthographicSize * 2.0f + 1) ;
	    width = (int) ((height - 2) * Camera.main.aspect);
        int[,] map = GenerateArray(width, height, false);
        RenderMap(map, tileMapToRender);
    }

    void SetTileRepository() {
        foreach (Sprite tileSprite in tileSprites) {
            Tile tile = Tile.CreateInstance<Tile>();
            tile.sprite = tileSprite;
            tile.name = tileSprite.name;
            numberOfTileTypes.Add(tile, 0);
        }
    }
    
    public static int[,] GenerateArray(int width, int height, bool empty){
        int[,] map = new int[width, height];
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
            }
        }
        return map;
    }

    public void RenderMap(int[,] map, Tilemap tilemap){
        //Clear the map (ensures we dont overlap)
        tilemap.ClearAllTiles();
        //Loop through the width of the map
        
        for (int x = 0; x < map.GetUpperBound(0) ; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                // 1 = tile, 0 = no tile
                if (map[x, y] == 1)
                {
                    Tile tile = GetChoosenTile();
                    Vector3Int pos = new Vector3Int(x-(width/2), y-(height/2), 0);
                    tilemap.SetTile(pos, tile);
                }
            }
        }
    }

    public Tile GetChoosenTile() {

        int rand = Random.Range(0, numberOfTileTypes.Count*10000);
        KeyValuePair<Tile, int> choosenTile = numberOfTileTypes.ElementAt(rand/10000);
        if(choosenTile.Key.name.Contains("Resource") && choosenTile.Value >= maxResourceTilesOfOneColor) {
            return GetChoosenTile();
        }
        numberOfTileTypes[choosenTile.Key] += 1;
        return choosenTile.Key;
    }
}
