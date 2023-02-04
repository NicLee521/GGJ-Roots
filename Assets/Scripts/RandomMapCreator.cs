using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class RandomMapCreator : MonoBehaviour
{
    public List<Sprite> tileSprites = new List<Sprite>();
    public Sprite winSprite;
    public Dictionary<Tile, int> numberOfTileTypes = new Dictionary<Tile, int>();
    public int maxResourceTilesOfOneColor = 3;
    public Tilemap tileMapToRender;
    public int height;
    public int width;
    private Vector3Int[] winPositions;
    // Start is called before the first frame update
    void Start()
    {
        SetTileRepository();
        height = (int) (Camera.main.orthographicSize * 2.0f + 1) ;
	    width = (int) ((height - 2) * Camera.main.aspect);
        winPositions = new Vector3Int[]{new Vector3Int((width/2 - 6), height/2 - 3, 0), new Vector3Int((width/2 + 6) - width, height/2 - 3, 0),new Vector3Int((width/2 - 5), (height/2  + 3) - height, 0),new Vector3Int((width/2 + 6) - width, (height/2 + 3) - height, 0) };
        int[,] map = GenerateArray(width, height, false);
        RenderMap(map, tileMapToRender);
        MakeWinTiles();
    }

    void SetTileRepository() {
        foreach (Sprite tileSprite in tileSprites) {
            Tile tile = Tile.CreateInstance<Tile>();
            tile.sprite = tileSprite;
            tile.name = tileSprite.name;
            numberOfTileTypes.Add(tile, 0);
        }
    }

    void MakeWinTiles() {
        string[] tileNames = new string[]{"FontRed", "FontGreen", "FontYellow", "FontBlue"};
        for(int i = 0; i < tileNames.Length; i++) {
            Tile tile = Tile.CreateInstance<Tile>();
            tile.sprite = winSprite;
            tile.name = tileNames[i];
            tileMapToRender.SetTile(winPositions[i], tile);
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
                    Vector3Int pos = new Vector3Int(x-(width/2), y-(height/2), 0);
                    if(winPositions.Contains(pos)) {
                        continue;
                    }
                    Tile tile = GetChoosenTile();
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
