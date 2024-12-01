using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool rotating = true;
    public float rotationSpeed = 1;
    public Vector3 rotation;
    [SerializeField] private float changeTime = 3.0f;
    private float timer;
    public int direction = 1;

    public bool patrolling = false;
    public float patrolSpeed = 1;
    public float patrolChangeTime = 3.0f;
    private Vector2 patrolPosition;
    private float patrolTimer;
    public int patrolDirection = 1;
    public bool vertical;

    private void Awake()
    {
        timer = changeTime;
        patrolTimer = patrolChangeTime;
        patrolPosition = new Vector2(transform.position.x, transform.position.y);
        rotation = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private void FixedUpdate()
    {       

        if (rotating)
        {
            patrolling = false;

            transform.Rotate(Vector3.forward, rotationSpeed * direction * Time.deltaTime);
        }

        if (patrolling)
        {
            rotating = false;

            if (vertical)
            {
                patrolPosition.y = patrolPosition.y + patrolSpeed * patrolDirection * Time.deltaTime;
            }
            else
            {
                patrolPosition.x = patrolPosition.x + patrolSpeed * patrolDirection * Time.deltaTime;
            }

            transform.position = patrolPosition;

        }
      
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        patrolTimer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        if (patrolTimer < 0)
        {
            if (patrolling)
            {
                transform.Rotate(Vector3.forward, 180);
            }
           
            patrolDirection = -patrolDirection;
            patrolTimer = patrolChangeTime;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject + " collided with Enemy");
    }

}
