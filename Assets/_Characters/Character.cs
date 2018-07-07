using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] RuntimeAnimatorController runtimeAnimatorController;

    [Header("Obstacle Collider")]
    [SerializeField] Vector2 colliderSize;
    [SerializeField] Vector2 colliderOffset;

    [Header("Movement")]
    [SerializeField] float moveSpeed;

    Animator animator;
    Rigidbody2D rigidBody;
    BoxCollider2D obstacleCollider;

    PlayerControl playerControl;

    public bool IsMoving
    {
        get
        {
            return playerControl.Direction.x != 0 || playerControl.Direction.y != 0;
        }
    }

    bool isAlive = true;


    private void Awake()
    {
        AddRequiredComponents();
    }

    private void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void AddRequiredComponents()
    {
        animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = runtimeAnimatorController;

        rigidBody = gameObject.AddComponent<Rigidbody2D>();
        rigidBody.gravityScale = 0;

        obstacleCollider = gameObject.AddComponent<BoxCollider2D>();
        obstacleCollider.size = colliderSize;
        obstacleCollider.offset = colliderOffset;

        playerControl = GetComponent<PlayerControl>();
    }


    private void Move()
    {
        rigidBody.velocity = playerControl.Direction.normalized * moveSpeed;
    }

    public void HandleLayers()
    {
        if (isAlive)
        {
            if (IsMoving)
            {
                ActivateLayer("WalkLayer");

                animator.SetFloat("x", playerControl.Direction.x);
                animator.SetFloat("y", playerControl.Direction.y);
            }
            //else if (IsAttacking)
            //{
            //    ActivateLayer("AttackLayer");
            //}
            else
            {
                ActivateLayer("IdleLayer");
            }
        }
        //else
        //{
        //    ActivateLayer("DeathLayer");
        //}
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
    }
}
