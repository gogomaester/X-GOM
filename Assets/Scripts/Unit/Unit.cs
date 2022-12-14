using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float stoppingDistance = .05f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Animator unitAnimator;

    private Vector3 targetPosition;
    private GridPosition gridPosition;

    private void Awake()
    {
        targetPosition = transform.position;   
    }

    private void Start()
    {
        // Get grid position from the units world position
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        Debug.Assert(gridPosition != null);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        // Running - Prevent the unit from moving after it's almost at the position
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance) 
        {
            // We calculate normalized direction & then move the unit in that direction
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
            // Animation
            unitAnimator.SetBool("isRunning", true);
            // Rotation
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        } 
        // Stopped
        else
        {
            unitAnimator.SetBool("isRunning", false);
        }

        // Check whether Unit already left the tile & entered a new one -> due to lable changes
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            // Unit has changed grid position
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public override string ToString()
    {
        return this.transform.name; 
    }


}
