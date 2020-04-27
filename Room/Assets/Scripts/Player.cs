using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Player : MonoBehaviour
{
    //[SerializeField] LayerMask roomMask;
    //public CompositeCollider2D roomColider;
    [SerializeField] PolygonCollider2D polygonRoomCollider;
    [SerializeField] float speed=1f;
    [SerializeField] Transform phoneTransform;

    private Animator animator;
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Transform myTransform;
    private Vector3 target;
    private Vector2 movement;

    Vector2 mousePos;

    private float minMovePos=-1f;
    private float maxMovePos = 1f;

    private const string SPEEDX = "SpeedX";
    private const string SPEEDY = "SpeedY";
    private const string DISTANCE = "Distance";

    //
    List<Vector2> pathToGo=new List<Vector2>();
    List<Vector2[]> colliderPaths;
    CompositeCollider2D compositeRoomCollider;
    RaycastHit2D mousePosHit;
    int index = 0;
        //

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        compositeRoomCollider = polygonRoomCollider.gameObject.GetComponent<CompositeCollider2D>();

        ReceiveColliderPaths();
    }

    void Start()
    {
        target = myTransform.position;
    }

    private void ReceiveColliderPaths()
    {
        int pathCount = polygonRoomCollider.pathCount;
        colliderPaths = new List<Vector2[]>();

        for (int i = 0; i < pathCount; i++)
        {
            colliderPaths.Add(polygonRoomCollider.GetPath(i));
        }
    }

    public void StartCutScene()
    {
        target = phoneTransform.position;
        movement.x = Mathf.Clamp(target.x - myTransform.position.x, minMovePos, maxMovePos);
        movement.y = Mathf.Clamp(target.y - myTransform.position.y, minMovePos, maxMovePos);
    }

    void Update()
    {
        //if (GameManager.instance.isGamePaused)
        //    return;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            MoveToCursor();
        }

        float distance = Vector2.SqrMagnitude(target - myTransform.position);
        AnimationBehaviour(distance);
        PathMovement(distance);

        Debug.DrawLine(myTransform.position, target);//delete
    }

    private void PathMovement(float distance)
    {
        if (distance == 0 && pathToGo.Count != 0 && index < pathToGo.Count)
        {
            target = pathToGo[index];
            index++;
        }
        else if (index >= pathToGo.Count)
        {
            pathToGo.Clear();
            index = 0;
        }
    }

    private void AnimationBehaviour(float distance)
    {
        animator.SetFloat(SPEEDX, movement.x);
        animator.SetFloat(SPEEDY, movement.y);
        animator.SetFloat(DISTANCE, distance);
    }

    private void FixedUpdate()
    {
        //if (GameManager.instance.isGamePaused)
        //    return;

        Movement();
    }

    private void Movement()
    {
        Vector2 newPos = Vector2.MoveTowards(myTransform.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    private void MoveToCursor()
    {
        Vector2 playerPos = myTransform.position;

        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        mousePosHit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (mousePosHit && mousePosHit.collider.GetComponent<Player>())
        {
            target = myTransform.position;
            return;
        }

        CreatePlayerPath(playerPos);

        movement.x = Mathf.Clamp(target.x - myTransform.position.x, minMovePos, maxMovePos);
        movement.y = Mathf.Clamp(target.y - myTransform.position.y, minMovePos, maxMovePos);
    }

    private void CreatePlayerPath(Vector2 playerPos)
    {
        mousePos = GetRightMousePosition();
        PathDirections pathDirection = DefineDirections(playerPos);
        List<Vector2> crossedPoints = FormListOfCrossedPoints(playerPos, pathDirection);

        if (crossedPoints.Count == 0)
        {
            target = mousePos;
        }
        else
        {
            FormFinalPath(playerPos, crossedPoints);
        }
    }

    private Vector2 GetRightMousePosition()
    {
        if (mousePosHit &&
            mousePosHit.collider.GetComponent<PolygonCollider2D>() != null)
        {
            return mousePos;
        }
        else
        {
            return compositeRoomCollider.ClosestPoint(mousePos);
        }
    }

    private PathDirections DefineDirections(Vector2 playerPos)
    {
        PathDirections pathDirection=PathDirections.LeftAngled;

        if (playerPos.x >= mousePos.x && playerPos.y > mousePos.y ||
            playerPos.x <= mousePos.x && playerPos.y < mousePos.y)//if x mouse==player x
        {
            pathDirection = PathDirections.RightAngled;
        }
        else if (playerPos.x < mousePos.x && playerPos.y >= mousePos.y ||
            playerPos.x > mousePos.x && playerPos.y <= mousePos.y)//imporve?
        {
            pathDirection = PathDirections.LeftAngled;
        }

        return pathDirection;
    }

    private List<Vector2> FormListOfCrossedPoints(Vector2 playerPos, PathDirections pathDirections)
    {
        List<Vector2> crossedPoints = new List<Vector2>();
        List<int> crossedPointIndexes = new List<int>();
        for (int i = 0; i < colliderPaths.Count; i++)
        {
            Vector2[] path = colliderPaths[i];

            for (int j = 0; j < path.Length; j++)
            {
                int k = j == path.Length - 1 ? 0 : j + 1;
                bool isCrossed = isLinesIntersected(playerPos, mousePos, path[j], path[k]);
                //bool isMousePosBelongToLine = isMouseOnTheLine(path[j], path[k]);
                if (pathDirections == PathDirections.RightAngled && isCrossed /*&& !isMousePosBelongToLine*/)
                {
                    CheckRightAngledCrossing(playerPos, crossedPointIndexes, path, j, k);                 
                }
                if (pathDirections == PathDirections.LeftAngled && isCrossed /*&& !isMousePosBelongToLine*/)
                {
                    CheckLeftAngledCrossing(playerPos, crossedPointIndexes, path, j, k);
                }
            }
            crossedPoints = GetReformedPointsList(crossedPoints, crossedPointIndexes, path);
        }

        return crossedPoints;
    }

    private void CheckRightAngledCrossing(Vector2 playerPos, List<int> crossedPointIndexes, Vector2[] path, int j, int k)
    {
        if (path[j].x < playerPos.x && path[k].x < playerPos.x ||
            path[j].x < mousePos.x && path[k].x < mousePos.x)
        {
            float distancePoint1 = Vector2.Distance(path[j], mousePos);
            float distancePoint2 = Vector2.Distance(path[k], mousePos);

            int indexToAdd = distancePoint1 > distancePoint2 ? k : j;
            crossedPointIndexes.Add(indexToAdd);
        }
        else if (path[j].x < playerPos.x || path[j].x < mousePos.x)
        {
            crossedPointIndexes.Add(j);
        }
        else if (path[k].x < playerPos.x || path[k].x < mousePos.x)
        {
            crossedPointIndexes.Add(k);
        }
    }

    private void CheckLeftAngledCrossing(Vector2 playerPos, List<int> crossedPointIndexes, Vector2[] path, int j, int k)
    {
            if (path[j].x > playerPos.x && path[k].x > playerPos.x ||
                path[j].x > mousePos.x && path[k].x > mousePos.x)
            {
                float distancePoint1 = Vector2.Distance(path[j], mousePos);
                float distancePoint2 = Vector2.Distance(path[k], mousePos);

                int indexToAdd = distancePoint1 > distancePoint2 ? k : j;
                crossedPointIndexes.Add(indexToAdd);
            }
            else
            if (path[j].x > playerPos.x || path[j].x > mousePos.x)
            {
                crossedPointIndexes.Add(j);
            }
            else if (path[k].x > playerPos.x || path[k].x > mousePos.x)
            {
                crossedPointIndexes.Add(k);
            }
    }

    private List<Vector2> GetReformedPointsList(List<Vector2> crossedPoints, List<int> crossedPointIndexes, Vector2[] path)
    {
        if (crossedPoints.Count == 0)
        {
            crossedPoints = GetPathPoints(crossedPointIndexes, path);
        }
        else
        {
            List<Vector2> tempPointList = GetPathPoints(crossedPointIndexes, path);
            crossedPoints.AddRange(tempPointList);
        }

        crossedPointIndexes.Clear();
        return crossedPoints;
    }

    private List<Vector2> GetPathPoints(List<int> pointIndexes, Vector2[] currentPath)
    {
        List<Vector2> crossedPoints = new List<Vector2>();
        if (pointIndexes.Count == 0)
            return crossedPoints;

        int firstIndex = pointIndexes[0];
        int lastIndex = pointIndexes[pointIndexes.Count-1];

        if (firstIndex > lastIndex)
        {
            crossedPoints = GetIndexPathThroughZero(currentPath, firstIndex, lastIndex);
        }
        else
        {
            for (int i = firstIndex; i <= lastIndex; i++)
            {
                crossedPoints.Add(currentPath[i]);
            }
        }

        return crossedPoints;
    }

    private List<Vector2> GetIndexPathThroughZero(Vector2[] currentPath, int firstIndex, int lastIndex)
    {
        int lastPathIndex = currentPath.Length;
        List<int> indexPath = new List<int>();
        List<Vector2> crossedPoints = new List<Vector2>();

        for (int i = firstIndex; i < lastPathIndex; i++)
        {
            indexPath.Add(i);
        }

        for (int i = 0; i < lastIndex; i++)
        {
            indexPath.Add(i);
        }

        foreach (var index in indexPath)
        {
            crossedPoints.Add(currentPath[index]);
        }

        return crossedPoints;
    }

    private void FormFinalPath(Vector2 playerPos, List<Vector2> missedPoints)
    {
        float maxDistance = Mathf.Infinity;
        pathToGo.Clear();
        Vector2 pathPoint = new Vector2();
        Vector2 previousPathPoint = playerPos;

        while (missedPoints.Count != 0)
        {
            foreach (Vector2 point in missedPoints)
            {
                if (Vector2.Distance(playerPos, point) < maxDistance)
                {
                    pathPoint = point;
                    maxDistance = Vector2.Distance(playerPos, point);
                }
            }
            maxDistance = Mathf.Infinity;
            CheckDistanceToTarget(pathPoint, previousPathPoint);
            previousPathPoint = pathPoint;
            missedPoints.Remove(pathPoint);
        }

        pathToGo.Add(mousePos);
    }

    private void CheckDistanceToTarget(Vector2 pathPoint, Vector2 previousPathPoint)
    {
        float distanceToTarget = Vector2.Distance(pathPoint, mousePos);
        float distanceFromPrevPoint = Vector2.Distance(previousPathPoint, mousePos);

        if(distanceToTarget<distanceFromPrevPoint)
        {
            pathToGo.Add(pathPoint);
        }
    }

    private bool isLinesIntersected(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
    {
        Vector2 a = point2 -point1;
        Vector2 b = point3 - point4;
        Vector2 c = point1 - point3;

        float alphaNumerator = b.y * c.x - b.x * c.y;
        float alphaDenominator = a.y * b.x - a.x * b.y;
        float betaNumerator = a.x * c.y - a.y * c.x;
        float betaDenominator = a.y * b.x - a.x * b.y;

        bool doIntersect = true;

        if (alphaDenominator == 0 || betaDenominator == 0)
        {
            doIntersect = false;
        }
        else
        {
            if (alphaDenominator > 0)
            {
                if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
                {
                    doIntersect = false;

                }
            }
            else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
            {
                doIntersect = false;
            }

            if (doIntersect && betaDenominator > 0) {
                if (betaNumerator < 0 || betaNumerator > betaDenominator)
                {
                    doIntersect = false;
                }
            } else if (betaNumerator > 0 || betaNumerator < betaDenominator)
            {
                doIntersect = false;
            }
        }

        return doIntersect;
    }

    private bool isMouseOnTheLine(Vector2 point1, Vector2 point2)
    {
        bool isOnTheLine = false;

        //Entire line segment
        Vector2 point21 = point2 - point1;
        //The intersection and the first point
        Vector2 point1Mouse = mousePos - point1;

        //Need to check 2 things: 
        //1. If the vectors are pointing in the same direction = if the dot product is positive
        //2. If the length of the vector between the intersection and the first point is smaller than the entire line
        if (Vector2.Dot(point21, point1Mouse) > 0f && point21.sqrMagnitude >= point1Mouse.sqrMagnitude)
        {
            isOnTheLine = true;
        }

        return isOnTheLine;
    }

  

    private void OnDrawGizmos()//delete
    {
        Gizmos.DrawLine(transform.position, target);
    }
}

public enum PathDirections
{
    RightAngled,
    LeftAngled
}
