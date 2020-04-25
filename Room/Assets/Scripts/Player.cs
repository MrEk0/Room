using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Player : MonoBehaviour
{
    public LayerMask roomMask;
    public CompositeCollider2D roomColider;
    [SerializeField] float speed=1f;
    [SerializeField] Transform phoneTransform;

    private Animator animator;
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Transform myTransform;
    private Vector3 target;
    private Vector2 movement;

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
            Debug.Log(index);
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
        target = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(target, Vector2.zero);
        if (hit && hit.collider.GetComponent<Player>())
        {
            target = myTransform.position;
            return;
        }

        float distance = Vector2.Distance( myTransform.position, target);
        RaycastHit2D distHit = Physics2D.Raycast(myTransform.position, target,distance, roomMask);
        //Debug.Log(distance);
        //Debug.Log(distHit.distance);

        if (distHit)
        {
            //get the closest point
            // find itinerary to the point
            float minDistance = Mathf.Infinity;
            float minDistance2 = Mathf.Infinity;
            Vector2 startPoint = new Vector2();
            Vector2 finalPathPoint=new Vector2();
            Vector2 finalPoint = roomColider.ClosestPoint(target);

            Vector2[] pathPoints= distHit.collider.GetComponent<PolygonCollider2D>().GetPath(1);
            foreach (Vector2 vector in pathPoints)
            {
                if(Vector2.Distance(vector, myTransform.position)<minDistance)
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
            else if(startPointIndex>finalPointIndex)
            {
                for (int i = startPointIndex; i >= finalPointIndex; i--)
                {
                    pathToGo.Add(pathPoints[i]);
                }
            }
            pathToGo.Add(finalPoint);
            target = startPoint;
        }
        else
        {
            //check if the target inside the room
            //if true go to target point
            // otherwise find itinerary
            Debug.Log("No");
        }

        //movement.x = Mathf.Clamp(target.x - myTransform.position.x, minMovePos, maxMovePos);
        //movement.y = Mathf.Clamp(target.y - myTransform.position.y, minMovePos, maxMovePos);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, target);
    }
}
