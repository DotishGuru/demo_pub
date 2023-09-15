using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform[] patrolingPoints;
    public float _standingTime;
    public float _moveSpeed;
    private Vector3 _target;
    private Vector3 _velocity;
    private float _deltaTime;
    private Rigidbody2D _rb;
    private Vector3 _previousPosition;

    void Start() 
    {
        _rb = GetComponent<Rigidbody2D>();
        _target = patrolingPoints[0].position;
    }

    void FixedUpdate() 
    {
        _deltaTime = Time.fixedDeltaTime;
        Patrol();
    }    

    void Patrol()
    {
        if(Vector2.Distance(transform.position, _target) > 0.001f)
        {                
            MoveEnemy();
        }
        else
        {
            SetPatrolingDestinationPoint();
        } 
    }

    private void MoveEnemy()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target, _moveSpeed * _deltaTime);
    }

    private void SetPatrolingDestinationPoint()
    {
        StopMovement();

        if(_target.x == patrolingPoints[0].position.x)
        {
            StartCoroutine("SetTarget", patrolingPoints[1].position);
        }
        else
        {
            StartCoroutine("SetTarget", patrolingPoints[0].position);
        }        
    }

    private void StopMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, transform.position, 0);
    }

    public IEnumerator SetTarget(Vector3 position)
    {
        StopMovement();

        yield return new WaitForSeconds(_standingTime);

        _target = position;
    }
}
