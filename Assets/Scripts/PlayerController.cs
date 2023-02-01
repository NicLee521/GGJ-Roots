using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

public struct Hex {
    Vector3 topPoint;
    Vector3 topRightPoint;
    Vector3 botRightPoint;
    Vector3 botPoint;
    Vector3 botLeftPoint;
    Vector3 topLeftPoint;
    public Vector3 center;
    public Vector3[] pointPositions;
    public Hex(Vector3 tpos, Tilemap tileMap) {
        this.center = new Vector3(tpos.x, tpos.y ,0);;
        this.topPoint = new Vector3(tpos.x, tpos.y + tileMap.layoutGrid.cellSize.y/2,0);
        this.topRightPoint = new Vector3(tpos.x + tileMap.layoutGrid.cellSize.x/2, tpos.y + tileMap.layoutGrid.cellSize.y/4, 0);
        this.botRightPoint = new Vector3(tpos.x + tileMap.layoutGrid.cellSize.x/2, tpos.y - tileMap.layoutGrid.cellSize.y/4, 0);
        this.botPoint = new Vector3(tpos.x, tpos.y - tileMap.layoutGrid.cellSize.y/2, 0);
        this.botLeftPoint = new Vector3(tpos.x - tileMap.layoutGrid.cellSize.x/2, tpos.y - tileMap.layoutGrid.cellSize.y/4, 0);
        this.topLeftPoint = new Vector3(tpos.x - tileMap.layoutGrid.cellSize.x/2, tpos.y + tileMap.layoutGrid.cellSize.x/4, 0);
        this.pointPositions = new Vector3[]{this.topPoint, this.topRightPoint, this.botRightPoint, this.botPoint, this.botLeftPoint, this.topLeftPoint};
    }

    public Vector2 GetCenterToPointByIndex(int index) {
        return this.pointPositions[index] - this.center;
    }
}
public class PlayerController : MonoBehaviour
{
    

    [SerializeField] Tilemap tileMap;
    [SerializeField] LineRenderer lineRenderer1;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject root;
    [SerializeField] TMP_Text blueNum;
    [SerializeField] TMP_Text redNum;
    [SerializeField] TMP_Text yellowNum;
    [SerializeField] TMP_Text greenNum;


    private Color color = Color.blue;
    public string colorString = "blue";
    private int blueResource =  100;
    private int redResource =  100;
    private int greenResource =  100;
    private int yellowResource =  100;

    public UnityEvent rootPlacement;

    void Start() {
        Vector3Int centerCell = new Vector3Int(0,0,0);
        Vector3 tpos = tileMap.GetCellCenterWorld(centerCell);
        Hex hex = new Hex(tpos, tileMap);
        LineRenderer start = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        start.startWidth = .25f;
        start.useWorldSpace = true;
        CreateStartRoots(hex, start);
        if(rootPlacement == null) {
            rootPlacement = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update() {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePos = tileMap.WorldToCell((Vector2) worldPoint);
        Vector3 tpos = tileMap.GetCellCenterWorld(tilePos);
        Hex hex = new Hex(tpos, tileMap);
        lineRenderer1.positionCount  = hex.pointPositions.Length;
        lineRenderer1.loop = true;
        lineRenderer1.SetPositions(hex.pointPositions);
        lineRenderer.positionCount  = 2;
        Vector3[] linePos = CheckMouse(worldPoint, hex);
        lineRenderer.SetPositions(linePos);
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !RootAlreadyHere(linePos) && RootConnected(linePos)) {
            CreateRootAtRenderer();
        }
    }

    bool RootAlreadyHere(Vector3[] linePos) {
        Collider[] hitColliders = Physics.OverlapSphere((linePos[0]+linePos[1])/2, .1f);
        return hitColliders.Length > 0;
    }

    bool RootConnected(Vector3[] linePos) {
        Collider[] hitColliders1 = Physics.OverlapSphere(linePos[0], .1f);
        Collider[] hitColliders2 = Physics.OverlapSphere(linePos[1], .1f);

        return (hitColliders1.Length > 0 || hitColliders2.Length > 0);
    }

    void CreateRootAtRenderer() {
        if(!ChangeResourceValues(-10, colorString)) {
            return;
        }
        CreateRoot(lineRenderer, color);
        rootPlacement.Invoke();
        if(!ChangeResourceValues(-5, null)) {
            Debug.Log("Lose");
        }  
    }

    void CreateStartRoots(Hex hex, LineRenderer start) {
        start.positionCount = 2;
        start.SetPositions(new Vector3[]{hex.pointPositions[5], hex.pointPositions[0]});
        //top left edge
        CreateRoot(start, Color.blue);
        start.SetPositions(new Vector3[]{hex.pointPositions[0], hex.pointPositions[1]});
        //top right edge
        CreateRoot(start, Color.red);
        start.SetPositions(new Vector3[]{hex.pointPositions[2], hex.pointPositions[3]});
        //bottom right edge
        CreateRoot(start, Color.yellow);
        start.SetPositions(new Vector3[]{hex.pointPositions[3], hex.pointPositions[4]});
        //bottom left edge
        CreateRoot(start, Color.green);
    }

    void CreateRoot(LineRenderer lr, Color rootColor) {
        Mesh lineMesh = new Mesh();
        lr.BakeMesh(lineMesh);
        root.GetComponentInChildren<MeshFilter>().mesh = lineMesh;
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = rootColor;
        GameObject newRoot = Instantiate(root, Vector3.zero, Quaternion.identity);     
        newRoot.GetComponentInChildren<MeshRenderer>().material= mat;  
        newRoot.GetComponentInChildren<MeshCollider>().sharedMesh= lineMesh;
    }

    bool CanPayColorPrice(int price, string colorToCheck) {
        if(price >= 0) {
            return true;
        }
        price = Mathf.Abs(price);
        switch(colorToCheck){
            case "blue":
                return (blueResource >= price);
            case "red":
                return (redResource >= price);
            case "green":
                return (greenResource >= price);
            case "yellow":
                return (yellowResource >= price);
            default:
                return (yellowResource >= price && greenResource >= price && redResource >= price && blueResource >= price);
        }
    }

    public bool ChangeResourceValues (int numToChange, string colorToReduce) {
        if(!CanPayColorPrice(numToChange, colorToReduce)) {
            return false;
        }
        switch(colorToReduce){
            case "blue":
                blueResource += numToChange;
                break;
            case "red":
                redResource += numToChange;
                break;
            case "green":
                greenResource += numToChange;
                break;
            case "yellow":
                yellowResource += numToChange;
                break;
            default:
                blueResource += numToChange;
                yellowResource += numToChange;
                greenResource += numToChange;
                redResource += numToChange;
                break;
        }
        blueNum.text = blueResource.ToString();
        redNum.text = redResource.ToString();
        greenNum.text = greenResource.ToString();
        yellowNum.text = yellowResource.ToString();
        return true;
    }

    Vector3[] CheckMouse(Vector3 worldPoint, Hex hex) {
        for(int i = 0; i < hex.pointPositions.Length; i++) {
            int k = i + 1;
            if(k == hex.pointPositions.Length) {
                k = 0;
            }
            Vector2 positionIToCenter = hex.GetCenterToPointByIndex(i);
            Vector2 positionKToCenter = hex.GetCenterToPointByIndex(k);
            float angleArea = Vector2.Angle(positionIToCenter, positionKToCenter);
            if(CheckIfInTriangle(worldPoint, hex.center,  positionIToCenter, positionKToCenter, angleArea)) {
                return new Vector3[]{hex.pointPositions[i], hex.pointPositions[k]};
            }
        }

        return new Vector3[0];
    }

    bool CheckIfInTriangle(Vector2 worldPoint, Vector2 center, Vector2 first, Vector2 second, float angle) {
        float angleOfFirst = Vector2.Angle(worldPoint - center, first );
        float angleOfSecond = Vector2.Angle(worldPoint - center, second);
        return (angleOfFirst < angle && angleOfSecond < angle);
    }

    public void SetColor(TMP_Dropdown dropdown) {
        int value = dropdown.value;
        switch(value) {
            case 0:
                color = Color.blue;
                colorString = "blue";
                break;
            case 1:
                color = Color.red;
                colorString = "red";
                break;
            case 2:
                color = Color.yellow;
                colorString = "yellow";
                break;
            case 3:
                color = Color.green;
                colorString = "green";
                break;
            default:
                color = Color.blue;
                colorString = "blue";
                break;
        }
    }
}
