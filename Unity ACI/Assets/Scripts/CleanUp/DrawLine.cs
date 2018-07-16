using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
[RequireComponent(typeof(LineRenderer))]
public class DrawLine : MonoBehaviour
{
    public static bool selfUpdate = false;
    public bool checkSameLineCollisionOnSelfUpdate = false;

    [SerializeField]
    Camera camera;
    private LineRenderer line;
    private bool isMousePressed = false;
    private List<Vector3> pointsList = new List<Vector3>();
    private List<Vector2> colliderList = new List<Vector2>();
    private Vector3 mousePos;
    private EdgeCollider2D _collider;
    Vector2[] _colliderVertexPositions;

    [SerializeField]
    GameObject dustParticle;

    float smooth = .5f;
    float timerForParticle;
    bool enteredBubble;

    // Structure for line points
    struct myLine
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;
    };

    public int VertexCount
    {
        get
        {
            return pointsList.Count;
        }
    }

    //Main script for drawing line to simulate cleaning
    void Awake()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.SetVertexCount(0);
        Material whiteDiffuseMat = new Material(Shader.Find("Particles/Additive (Soft)"));

        line.material = whiteDiffuseMat;

        if (GetComponent<EdgeCollider2D>() != null)
        {
            _collider = GetComponent<EdgeCollider2D>();
        }
        else
        {
            _collider = gameObject.AddComponent<EdgeCollider2D>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ToolInfo.toolInUse == "Broom" || ToolInfo.toolInUse == "SoapWater" || ToolInfo.toolInUse == "WaterHose" || ToolInfo.toolInUse == "Mop")
        {
            CleanPoints.ToClean--;
        }
    }
    void Update()
    {
        if (timerForParticle >= 0)
            timerForParticle -= Time.deltaTime;
       
        if (selfUpdate)
        {
            // If mouse button down, remove old line and set its color to green
            if (Input.GetMouseButtonDown(0))
            {
                isMousePressed = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StopLine();
                OnDrawLine();
 
            }
            // Drawing line when mouse is moving(presses)
            if (isMousePressed)
            {
                mousePos = camera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                AddVertex(mousePos, true);
                //Adding particle effect
                if (IsLineCollide() && checkSameLineCollisionOnSelfUpdate && timerForParticle <= 0)
                {
                    timerForParticle = 0.3f;
                    if (ToolInfo.toolInUse == "Broom")
                    {
                        GameObject go = Instantiate(Resources.Load<GameObject>("DustParticles"), new Vector3(0, 0, 0), Quaternion.identity);
                        Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        go.transform.position = (new Vector3(curPosition.x, curPosition.y, -55));
                    }
                    if (ToolInfo.toolInUse == "SoapWater")
                    {
                        GameObject go = Instantiate(Resources.Load<GameObject>("BubbleParticles"), new Vector3(0, 0, 0), Quaternion.identity);
                        Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        go.transform.position = (new Vector3(curPosition.x, curPosition.y, -55));
                    }

                    if (ToolInfo.toolInUse == "WaterHose")
                    {
                        GameObject go = Instantiate(Resources.Load<GameObject>("BubbleParticles"), new Vector3(0, 0, 0), Quaternion.identity);
                        Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        go.transform.position = (new Vector3(curPosition.x, curPosition.y, -55));
                    }
                    if (ToolInfo.toolInUse == "Mop")
                    {
                        GameObject go = Instantiate(Resources.Load<GameObject>("BubbleParticles"), new Vector3(0, 0, 0), Quaternion.identity);
                        Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        go.transform.position = (new Vector3(curPosition.x, curPosition.y, -55));
                    }
                }
            }
        }
    }

    public void StopLine()
    {
        isMousePressed = false;
    }
    public void AddVertex(Vector3 pos, bool isCollidable)
    {
        if (!pointsList.Contains(mousePos))
        {
            pointsList.Add(pos);
            line.SetVertexCount(pointsList.Count);
            Vector3 p;
            if (pointsList.Count >= 2)
            {
                p = new Vector3((pointsList[pointsList.Count - 2].x + pointsList[pointsList.Count - 1].x) * smooth,
                                (pointsList[pointsList.Count - 2].y + pointsList[pointsList.Count - 1].y) * smooth,
                                (pointsList[pointsList.Count - 2].z + pointsList[pointsList.Count - 1].z) * smooth);
            }
            else
                p = pointsList[pointsList.Count - 1];
            line.SetPosition(pointsList.Count - 1, p);
            //If collidable also set vertex positions
            if (isCollidable && _collider != null)
            {
                colliderList.Add(new Vector2(pos.x + .3f, pos.y + .9f));

                _colliderVertexPositions = new Vector2[colliderList.Count];
                for (int i = 0; i < colliderList.Count; i++)
                {
                    _colliderVertexPositions[i] = colliderList[i];
                }
                if (_colliderVertexPositions.Length >= 2)
                    _collider.points = _colliderVertexPositions;
            }

        }
    }
    //Called when drawing completed for opening edge collider
    public void OnDrawLine()
    {
        if (_collider != null)
            _collider.enabled = true;
    }
    //Resets line
    public void Reset()
    {
        line.SetVertexCount(0);
        pointsList = new List<Vector3>();
        if (_collider != null)
        {
            colliderList = new List<Vector2>();
            _collider.Reset();
            _collider.isTrigger = true;
            _collider.enabled = false;
        }
    }

    #region PRIVATES
    //    -----------------------------------    
    //  Following method checks is currentLine(line drawn by last two points) collided with line 
    //    -----------------------------------    
    private bool IsLineCollide()
    {
        if (pointsList.Count < 2)
            return false;
        int TotalLines = pointsList.Count - 1;
        myLine[] lines = new myLine[TotalLines];
        if (TotalLines > 1)
        {
            for (int i = 0; i < TotalLines; i++)
            {
                lines[i].StartPoint = (Vector3)pointsList[i];
                lines[i].EndPoint = (Vector3)pointsList[i + 1];
            }
        }
        for (int i = 0; i < TotalLines - 1; i++)
        {
            myLine currentLine;
            currentLine.StartPoint = (Vector3)pointsList[pointsList.Count - 2];
            currentLine.EndPoint = (Vector3)pointsList[pointsList.Count - 1];
            if (IsLinesIntersect(lines[i], currentLine))
                return true;
        }
        return false;
    }
    //    -----------------------------------    
    //    Following method checks whether given two points are same or not
    //    -----------------------------------    
    private bool CheckPoints(Vector3 pointA, Vector3 pointB)
    {
        return (pointA.x == pointB.x && pointA.y == pointB.y);
    }
    //    -----------------------------------    
    //    Following method checks whether given two line intersect or not
    //    -----------------------------------    
    private bool IsLinesIntersect(myLine L1, myLine L2)
    {
        if (CheckPoints(L1.StartPoint, L2.StartPoint) ||
            CheckPoints(L1.StartPoint, L2.EndPoint) ||
            CheckPoints(L1.EndPoint, L2.StartPoint) ||
            CheckPoints(L1.EndPoint, L2.EndPoint))
            return false;

        return ((Mathf.Max(L1.StartPoint.x, L1.EndPoint.x) >= Mathf.Min(L2.StartPoint.x, L2.EndPoint.x)) &&
               (Mathf.Max(L2.StartPoint.x, L2.EndPoint.x) >= Mathf.Min(L1.StartPoint.x, L1.EndPoint.x)) &&
               (Mathf.Max(L1.StartPoint.y, L1.EndPoint.y) >= Mathf.Min(L2.StartPoint.y, L2.EndPoint.y)) &&
               (Mathf.Max(L2.StartPoint.y, L2.EndPoint.y) >= Mathf.Min(L1.StartPoint.y, L1.EndPoint.y))
               );
    }
    #endregion
}