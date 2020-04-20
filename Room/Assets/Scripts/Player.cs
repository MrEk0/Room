using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed=1f;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }

        float distance = Vector2.SqrMagnitude(target - myTransform.position);

        animator.SetFloat(SPEEDX, movement.x);
        animator.SetFloat(SPEEDY, movement.y);
        animator.SetFloat(DISTANCE, distance);
    }

    private void FixedUpdate()
    {
        Vector2 newPos = Vector2.MoveTowards(myTransform.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    private void MoveToCursor()
    {
        target = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        movement.x = Mathf.Clamp(target.x - myTransform.position.x, minMovePos, maxMovePos);
        movement.y = Mathf.Clamp(target.y - myTransform.position.y, minMovePos, maxMovePos);

    }
}
