using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMapCreator : MonoBehaviour
{
    public List<Sprite> tileSprites = new List<Sprite>();
    public Dictionary<string, int> numberOfTileTypes = new Dictionary<string, int>();
    public int maxResourceTilesOfOneColor = 3;
    public Tilemap tileMapToRender;
    public int height;
    public int width; 
    // Start is called before the first frame update
    void Start()
    {
        height = (int) (Camera.main.orthographicSize * 2.0f + 3);
	    width = (int) (height * Camera.main.aspect-5);
        int[,] map = GenerateArray(width, height, false);
        RenderMap(map, tileMapToRender, new Tile());
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void RenderMap(int[,] map, Tilemap tilemap, Tile tile){
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
                    tile.sprite = tileSprites[0];
                    tilemap.SetTile(new Vector3Int(x-(width/2), y - (height/2), 0), tile);
                }
            }
        }
    }
}
