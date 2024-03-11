using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    private Vector2 _moveVector;

    [SerializeField] float speed = 2.0f;

    private Animator _anim;
    private SpriteRenderer _sr;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Mirror();
        Jump();
        CheckingGround();
    }

    void Walk()
    {
        _moveVector.x = Input.GetAxisRaw("Horizontal");
        _anim.SetFloat("MoveX", Mathf.Abs(_moveVector.x));
        _rb.velocity = new Vector2(_moveVector.x * speed, _rb.velocity.y);
    }

    private bool _faceRight = true;
    void Mirror()
    {
        if ((_moveVector.x > 0 && !_faceRight) || (_moveVector.x < 0 && _faceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            _faceRight = !_faceRight;
        }
    }

    [SerializeField] float JumpForce = 10f;
    [SerializeField] int JumpValueIteration = 60;
    private bool _jumpControl;
    private int _jumpIteration = 0;
    
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_onGround)
            {
                _jumpControl = true;
            }
        }
        else
        {
            _jumpControl = false;
        }

        if (_jumpControl)
        {
            if (_jumpIteration++ < JumpValueIteration)
            {
                _rb.AddForce(Vector2.up * JumpForce / _jumpIteration);
            }
        }
        else
        {
            _jumpIteration = 0;
        }
        _anim.SetBool("IsFalling", _rb.velocity.y < 0);
    }

    private bool _onGround;
    [SerializeField] Transform GroundCheck;
    [SerializeField] float CheckRadius = 0.1f;
    [SerializeField] LayerMask Ground;

    void CheckingGround()
    {
        _onGround = Physics2D.OverlapCircle(GroundCheck.position, CheckRadius, Ground);
        _anim.SetBool("OnGround", _onGround);
    }


}
