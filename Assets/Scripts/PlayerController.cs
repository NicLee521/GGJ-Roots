using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

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
    [SerializeField] Sprite greenSprite;
    [SerializeField] Sprite redSprite;
    [SerializeField] Sprite blueSprite;
    [SerializeField] Sprite yellowSprite;


    private Color color = Color.blue;
    public string colorString = "blue";
    private int blueResource =  100;
    private int redResource =  100;
    private int greenResource =  100;
    private int yellowResource =  100;

    public UnityEvent rootPlacement;
    public MapController map;

    public bool fontRed, fontBlue, fontGreen, fontYellow;

    void Start() {
        map = GameObject.FindObjectOfType<MapController>();
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
            CreateRootAtRenderer(hex);
            if(CheckForComplete()){
                Debug.Log("win");
            }
        }
    }

    bool CheckForComplete() {
        return (fontBlue && fontGreen && fontRed && fontYellow);
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

    void CreateRootAtRenderer(Hex hex) {
        if(!ChangeResourceValues(-10, colorString)) {
            return;
        }
        CreateRoot(lineRenderer, colorString, hex, false);
        rootPlacement.Invoke();
        if(!ChangeResourceValues(-5, null)) {
            Debug.Log("Lose");
        }
        map.ClearAllTaken();
    }

    void CreateStartRoots(Hex hex, LineRenderer start) {
        start.positionCount = 2;
        start.SetPositions(new Vector3[]{hex.pointPositions[5], hex.pointPositions[0]});
        //top left edge
        CreateRoot(start, "blue",  hex, true);
        start.SetPositions(new Vector3[]{hex.pointPositions[0], hex.pointPositions[1]});
        //top right edge
        CreateRoot(start, "red",  hex, true);
        start.SetPositions(new Vector3[]{hex.pointPositions[2], hex.pointPositions[3]});
        //bottom right edge
        CreateRoot(start, "yellow",  hex, true);
        start.SetPositions(new Vector3[]{hex.pointPositions[3], hex.pointPositions[4]});
        //bottom left edge
        CreateRoot(start, "green", hex, true);
    }

    void CreateRoot(LineRenderer lr, string rootColor, Hex hex, bool start = false) {
        Mesh lineMesh = new Mesh();
        lr.BakeMesh(lineMesh);
        Vector3[] linePositions = new Vector3[2];
        lr.GetPositions(linePositions);
        Vector3 dir =new  Vector3(linePositions[1].x,linePositions[1].y , 0);
        float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject parent = new GameObject("RootParent");
        parent.AddComponent<MeshCollider>();
        parent.GetComponentInChildren<MeshCollider>().sharedMesh= lineMesh;
        Quaternion i = Quaternion.identity;
        i.z = GetZRotation(linePositions, hex);
        GameObject newRoot = Instantiate(root,  lineMesh.bounds.center,  i);
        Debug.Log(newRoot.transform.rotation);
        //newRoot.transform.SetParent(parent.transform);          
        newRoot.GetComponent<SpriteRenderer>().sprite = greenSprite;
        newRoot.GetComponent<Root>().isStart = start;
        newRoot.GetComponent<Root>().color = rootColor;
    }

    float GetZRotation(Vector3[] linePos, Hex hex) {
        string edge = "";
        foreach(Vector3 linePoint in linePos) {
            if(linePoint == hex.topPoint) {
                edge += "Top-Point";
            } else if (linePoint == hex.topRightPoint) {
                edge += "Top-RightPoint";
            } else if (linePoint == hex.botRightPoint) {
                edge += "Bot-RightPoint";
            } else if (linePoint == hex.botPoint) {
                edge += "Bot-Point";
            } else if (linePoint == hex.botLeftPoint) {
                 edge += "Bot-LeftPoint";
            } else if (linePoint == hex.topLeftPoint) {
                edge += "Top-LeftPoint";    
            }
        }
        if(edge.Contains("Top-Point") && edge.Contains("Top-RightPoint")) {
            return -103.0f;
        }else if(edge.Contains("Top-RightPoint") && edge.Contains("Bot-RightPoint")) {
            return -173.0f;
        }else if(edge.Contains("Bot-RightPoint") && edge.Contains("Bot-Point")) {
            return -226.0f;
        }else if(edge.Contains("Bot-Point") && edge.Contains("Bot-LeftPoint")) {
            return 70.0f;
        }else if(edge.Contains("Bot-LeftPoint") && edge.Contains("Top-LeftPoint")) {
            return 15.0f;
        }else if(edge.Contains("Top-LeftPoint") && edge.Contains("Top-Point")) {
            return -37.0f;
        }
        return 0.0f;
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
