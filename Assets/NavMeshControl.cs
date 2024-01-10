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
    private Vector3 _intiPosition;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _animationsController= GetComponent<AnimationsController>();
        _intiPosition = transform.position;
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, goal.position) < _detectionDistance)
        {
            agent.destination = goal.position;
            if (Vector3.Distance(transform.position, goal.position) < _attackDistance)
            {
                _animationsController.Attack();
            }
        }
        else
            agent.destination = _intiPosition;
    }


    
}
