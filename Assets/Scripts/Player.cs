using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] GameInput gameInput;
    [SerializeField] float movementSpeed = 7f;
    [SerializeField] LayerMask counterMask;
    private bool isWalking;
    private Vector3 prevMoveDir = Vector3.zero;
    private Counter selectedCounter;
    public event EventHandler<OnCounterSelectEventArgs> OnCounterSelect;
    public static Player instance { get; private set; }

    public class OnCounterSelectEventArgs : EventArgs
    {
        public Counter selectedCounter;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("There is more than one player instance!");
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        gameInput.OnInteraction += GameInput_OnInteraction;

    }

    private void GameInput_OnInteraction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    void Update()
    {

        HandleMovement();
        HandleInteraction();

    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movementDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float interactDistance = 1.0f;

        if(movementDir != Vector3.zero)
        {
            prevMoveDir = movementDir;
        }

        if(Physics.Raycast(transform.position, prevMoveDir, out RaycastHit hitInfo, interactDistance, counterMask))
        {
            if (hitInfo.transform.TryGetComponent<Counter>(out Counter counter))
            {
                if (counter != selectedCounter)
                {
                    SetSelectedCounter(counter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(Counter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnCounterSelect?.Invoke(this, new OnCounterSelectEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    private void HandleMovement()
    {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movementDir = new Vector3(inputVector.x, 0f, inputVector.y);
        isWalking = movementDir != Vector3.zero;

        float playerHeight = 1f;
        float playerRadius = 0.5f;
        float moveDistance = Time.deltaTime * movementSpeed;

        //smooth movement diagonally by the collider
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movementDir, moveDistance);
        if (canMove)
        {
            transform.position += movementDir  * moveDistance;
        }
        else
        {
            Vector3 movementDirX = new Vector3(inputVector.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movementDirX, moveDistance);
            if (canMove)
            {
                transform.position += movementDirX * moveDistance;
            }
            else
            {
                Vector3 movementDirZ = new Vector3(0f, 0f, inputVector.y).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movementDirZ, moveDistance);
                if (canMove)
                {
                    transform.position += movementDirZ * moveDistance;
                }
            }
        }

        //rotate player to move dir
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movementDir, rotateSpeed*Time.deltaTime);
    }
    public bool IsWalking()
    {
        return isWalking;
    }
}
