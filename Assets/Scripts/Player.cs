using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player instance { get; private set; }

    public event EventHandler<OnCounterSelectEventArgs> OnCounterSelect;
    public event EventHandler<bool> OnHoldObject;

    [SerializeField] GameInput gameInput;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] LayerMask counterMask;
    [SerializeField] Transform KitchenObjectHoldPoint;
    [SerializeField] KitchenObject platePrefab;

    private bool isWalking;
    private Vector3 prevMoveDir = Vector3.zero;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    private bool blockedMovement = false;
    private KitchenObject holdingPlate;



    public class OnCounterSelectEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
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
        gameInput.OnInteractionAlternate +=GameInput_OnInteractionAlternate;

    }

    private void GameInput_OnInteractionAlternate(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteraction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    void Update()
    {
        if (!blockedMovement)
        {
            HandleMovement();
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movementDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float interactDistance = 0.5f;
        float interactSphereRadius = 1f;
        float halfPlayerHeight = transform.localScale.y;

        if (movementDir != Vector3.zero)
        {
            prevMoveDir = movementDir;
        }

        if (Physics.SphereCast(transform.position + new Vector3(0f, halfPlayerHeight/2, 0f) , interactSphereRadius, prevMoveDir, out RaycastHit hitInfo, interactDistance, counterMask))
        {
            if (hitInfo.transform.TryGetComponent<BaseCounter>(out BaseCounter counter))
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

    private void SetSelectedCounter(BaseCounter selectedCounter)
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
        float playerRadius = 1f;
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
            canMove = movementDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movementDirX, moveDistance);
            if (canMove)
            {
                transform.position += movementDirX * moveDistance;
            }
            else
            {
                Vector3 movementDirZ = new Vector3(0f, 0f, inputVector.y).normalized;
                canMove = movementDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movementDirZ, moveDistance);
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

    public void DisablePlayerMovement()
    {
        blockedMovement = true;
        isWalking = false;
    }

    public void EnablePlayerMovement()
    {
        blockedMovement = false;
    }

    private void AddPlate()
    {
        holdingPlate = Instantiate(platePrefab);
        holdingPlate.transform.parent  = GetTopPoint();
        holdingPlate.transform.localPosition = new Vector3(0f, -0.00025f, 0f);
    }

    private void RemovePlate()
    {
        holdingPlate.DestroySelf();
        holdingPlate = null;
    }

    //IKitchenObjectParent
    public void SetKitchenObjectParent(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        AddPlate();
        OnHoldObject?.Invoke(this, true);
    }

    public Transform GetTopPoint()
    {
        return KitchenObjectHoldPoint;
    }

    public void ClearKitchenObjectParent()
    {
        kitchenObject = null;
        RemovePlate();
        OnHoldObject?.Invoke(this, false);
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
