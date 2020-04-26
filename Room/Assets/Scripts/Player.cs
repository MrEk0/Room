using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Player : MonoBehaviour
{
    public LayerMask roomMask;
    public CompositeCollider2D roomColider;
    public PolygonCollider2D polRoom;
    public BoxCollider2D randomCol;
    [SerializeField] float speed=1f;
    [SerializeField] Transform phoneTransform;

    private Animator animator;
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Transform myTransform;
    private Vector3 target;
    private Vector2 movement;

    Vector2 mousePos;
    RaycastHit2D distHit;

    private float minMovePos=-1f;
    private float maxMovePos = 1f;

    private const string SPEEDX = "SpeedX";
    private const string SPEEDY = "SpeedY";
    private const string DISTANCE = "Distance";

    //
    List<Vector2> pathToGo=new List<Vector2>();
    int index = 1;
        //

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = myTransform.position;
        //StartCutScene();
    }

    public void StartCutScene()
    {
        target = phoneTransform.position;
        movement.x = Mathf.Clamp(target.x - myTransform.position.x, minMovePos, maxMovePos);
        movement.y = Mathf.Clamp(target.y - myTransform.position.y, minMovePos, maxMovePos);
    }

    // Update is called once per frame
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

        if (distance == 0 && pathToGo.Count != 0 && index < pathToGo.Count)
        {
            target = pathToGo[index];
            index++;
        }
        else if (index>=pathToGo.Count)
        {
            pathToGo.Clear();
            index = 1;
            //Debug.Log(index);
        }

        //Debug.Log(index);
        Debug.DrawLine(myTransform.position, target);
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
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit && hit.collider.GetComponent<Player>())
        {
            target = myTransform.position;
            return;
        }

        float distance = Vector2.Distance( myTransform.position, mousePos);
        distHit = Physics2D.Raycast(myTransform.position, mousePos, distance, roomMask);

        if (distHit && !hit.collider)
        {
            //get the closest point
            // find itinerary to the point
            float minDistance = Mathf.Infinity;
            float minDistance2 = Mathf.Infinity;
            Vector2 startPoint = new Vector2();
            Vector2 finalPathPoint = new Vector2();
            Vector2 finalPoint;
            //if target insede
            if (!hit.collider)
            {
                finalPoint = roomColider.ClosestPoint(mousePos);
            }
            //otherwise finalpoint=target
            else
            {
                finalPoint = mousePos;
            }

            Vector2[] pathPoints = distHit.collider.GetComponent<PolygonCollider2D>().GetPath(1);
            foreach (Vector2 vector in pathPoints)
            {
                if (Vector2.Distance(vector, myTransform.position) < minDistance)
                {
                    startPoint = vector;
                    minDistance = Vector2.Distance(vector, myTransform.position);
                }
                if (Vector2.Distance(vector, finalPoint) < minDistance2)
                {
                    finalPathPoint = vector;
                    minDistance2 = Vector2.Distance(vector, finalPoint);
                }
            }

            int startPointIndex = Array.IndexOf(pathPoints, startPoint);
            int finalPointIndex = Array.IndexOf(pathPoints, finalPathPoint);

            pathToGo.Clear();
            if (startPointIndex < finalPointIndex)
            {
                for (int i = startPointIndex; i <= finalPointIndex; i++)
                {
                    pathToGo.Add(pathPoints[i]);
                }
            }
            else if (startPointIndex > finalPointIndex)
            {
                for (int i = startPointIndex; i >= finalPointIndex; i--)
                {
                    pathToGo.Add(pathPoints[i]);
                }
            }
            pathToGo.Add(finalPoint);
            target = startPoint;
        }
        else if (hit.collider)
        {
            //check player movement
            CheckPlayerPath();

            //roomColider.geometryType = CompositeCollider2D.GeometryType.Outlines;
            //roomColider.ClosestPoint(mousePos);
            //target = mousePos;
        }



        //movement.x = Mathf.Clamp(target.x - myTransform.position.x, minMovePos, maxMovePos);
        //movement.y = Mathf.Clamp(target.y - myTransform.position.y, minMovePos, maxMovePos);

    }

    private void CheckPlayerPath()
    {
        int pathCount = polRoom.pathCount;
        List<Vector2[]> paths = new List<Vector2[]>();

        for(int i=0; i<pathCount; i++)
        {
            paths.Add(polRoom.GetPath(i));
            //paths[i] = distHit.collider.GetComponent<PolygonCollider2D>().GetPath(i);
        }

        Vector2 playerPos = myTransform.position;
        PathDirections pathDirections=PathDirections.LeftAngled;

        if(playerPos.x>mousePos.x && playerPos.y>mousePos.y ||
            playerPos.x<mousePos.x && playerPos.y<mousePos.y)//if x mouse==player x
        {
            pathDirections = PathDirections.RighAngled;
        }
        else if(playerPos.x < mousePos.x && playerPos.y > mousePos.y ||
            playerPos.x > mousePos.x && playerPos.y < mousePos.y)//imporve?
        {
            pathDirections = PathDirections.LeftAngled;
        }


        List<Vector2> missedPoints = new List<Vector2>();
        for (int i = 0; i < paths.Count; i++)
        {
            foreach (Vector2 point in paths[i])
            {
                if (pathDirections == PathDirections.LeftAngled)
                {
                    if (point.x > playerPos.x && point.x < mousePos.x
                        && point.y < playerPos.y && point.y > mousePos.y ||
                        point.x < playerPos.x && point.x > mousePos.x
                        && point.y > playerPos.y && point.y < mousePos.y)
                    {
                        missedPoints.Add(point);
                    }
                }

                if (pathDirections == PathDirections.RighAngled)
                {
                    if (point.x > playerPos.x && point.x < mousePos.x
                       && point.y > playerPos.y && point.y < mousePos.y ||
                       point.x < playerPos.x && point.x > mousePos.x
                       && point.y < playerPos.y && point.y > mousePos.y)
                    {
                        missedPoints.Add(point);
                    }
                }
            }
        }

        if (missedPoints.Count == 0)
        {
            target = mousePos;
        }
        else
        {
            float maxDistance = Mathf.Infinity;
            pathToGo.Clear();
            Vector2 startPoint = new Vector2();

            while(missedPoints.Count!=0)
            {
                foreach (Vector2 point in missedPoints)
                {
                    if (Vector2.Distance(playerPos, point) < maxDistance)//change player for startpoint
                    {
                        startPoint = point;
                        maxDistance = Vector2.Distance(playerPos, point);
                    }
                }
                maxDistance = Mathf.Infinity;
                pathToGo.Add(startPoint);
                missedPoints.Remove(startPoint);
            }

            pathToGo.Add(mousePos);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, target);
    }
}

public enum PathDirections
{
    RighAngled,
    LeftAngled
}
