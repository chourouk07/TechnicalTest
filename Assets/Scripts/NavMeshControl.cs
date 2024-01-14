using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshControl : MonoBehaviour
{
    public Transform goal;
    AnimationsController _animationsController;
    NavMeshAgent agent;
    [SerializeField] private float _detectionDistance = 5f;
    [SerializeField] private float _attackDistance = 2.5f;
    [SerializeField] private float _attackCooldown = 1.5f;  
    private float _attackTimer = 0f;
    private Vector3 _initialPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _animationsController = GetComponent<AnimationsController>();
        _initialPosition = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, goal.position) < _detectionDistance)
        {
            Quaternion lookRotation = Quaternion.LookRotation(goal.position - transform.position);
            lookRotation.x = 0f;
            lookRotation.z = 0f;

            // Apply the rotation to the enemy
            transform.rotation = lookRotation;

            agent.destination = goal.position;
            if (Vector3.Distance(transform.position, goal.position) < _attackDistance)
            {
                // check if enough time has passed since the last attack
                if (_attackTimer <= 0f)
                {
                    if (_animationsController != null)
                        _animationsController.Attack();
                    _attackTimer = _attackCooldown;
                }
                else
                {
                    _animationsController.Reload();
                    _attackTimer -= Time.deltaTime;  
                }
            }
        }
        else
        {
            agent.destination = _initialPosition;
        }
    }

}
