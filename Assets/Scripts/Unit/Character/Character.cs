using UnityEngine;
using System.Collections;
using System;

public class Character : Unit
{
    [SerializeField]
    private int lives = 5;

    public int Lives
    {
        get { return lives; }
        set
        {
            if (value < 5) lives = value;
            livesBar.Refresh();
        }
    }
    private LivesBar livesBar;

    [SerializeField]
    private float speed = 3.0F;
    [SerializeField]
    private float jumpForce = 15.0F;

    private bool isGrounded = false;

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }



    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    [SerializeField]
    private HitScript hit;

    private void Awake()
    {
        livesBar = FindObjectOfType<LivesBar>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded) State = CharState.Idle;

        if (Input.GetButton("Horizontal")) { Run(); }
        if (isGrounded && Input.GetButtonDown("Jump")) { Jump(); }
        if (isGrounded && Input.GetButtonDown("Fire1")) { Hit(); }
    }

    private void Hit()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        bool turnSide = true;
        if (isGrounded && direction.x > 0.0F) { turnSide = false; }
        else if(isGrounded && direction.x < 0.0F) { turnSide = true; }

        if (turnSide) { State = CharState.Hit; }
        else { State = CharState.BackHit; }
        
        

        hit.gameObject.layer = LayerMask.NameToLayer("Monster");
        Invoke(nameof(TurnOnCollisions), 0.15f);
    }

    private void TurnOnCollisions()
    {
        hit.gameObject.layer = LayerMask.NameToLayer("Hit");
    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;

        if (isGrounded) State = CharState.Run;
    }

    private void Jump()
    {
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    public Vector3 DirectionShooting()
    {
        Vector3 direction = transform.right * (sprite.flipX ? -1.0F : 1.0F);
        return direction;
    }

    public override void ReceiveDamage()
    {
        Lives--;

        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 8.0F, ForceMode2D.Impulse);

        Debug.Log(lives);
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F);

        isGrounded = colliders.Length > 1;

        if (!isGrounded) State = CharState.Jump;
    }

    public enum CharState
    {
        Idle,
        Run,
        Jump,
        Hit,
        BackHit
    }
}