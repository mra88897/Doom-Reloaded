using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMoveStatus { NotMoving, Walking, Running, NotGrounded, Landing}


[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 1.0f;
    [SerializeField] private float _runSpeed = 4.5f;
    [SerializeField] private float _jumpSpeed = 7.5f;
    [SerializeField] private float _stickToGroundForce = 5.0f;
    [SerializeField] private float _gravityMultiplier = 2.5f;  //tweak the standard gravity of the physics system
    //[SerializeField] private CurveControlledBob _headBob = new CurveControlledBob();


    //standard asset script, will enable us to rotate person and camera independently 
    [SerializeField] private UnityStandardAssets.Characters.FirstPerson.MouseLook _mouseLook;


    private Camera _camera = null;
    private bool _jumpButtonPressed = false;
    private Vector2 _inputVector = Vector2.zero;
    private Vector3 _moveDirection = Vector3.zero;
    private bool _previouslyGrounded = false;
    private bool _isWalking = true;
    private bool _isJumping = false;
    private Vector3 _localSpaceCameraPos = Vector3.zero;

    //timers
    private float _fallingTimer = 0.0f;

    private CharacterController _characterController = null;
    private PlayerMoveStatus _movementStatus = PlayerMoveStatus.NotMoving;


    //getters 
    public PlayerMoveStatus movementStatus { get { return _movementStatus; } }
    public float walkSpeed { get { return _walkSpeed; } }
    public float runSpeed { get { return _runSpeed; } }

    protected void Start()
    {
        _characterController = GetComponent<CharacterController>();

        _camera = Camera.main; //look for tag Main Camera 

        _movementStatus = PlayerMoveStatus.NotMoving;

        _fallingTimer = 0.0f;

        _mouseLook.Init(transform, _camera.transform);

    }

    protected void Update()
    {
        if (_characterController.isGrounded)
        {
            _fallingTimer = 0.0f;
        }
        else
        {
            _fallingTimer += Time.deltaTime;
        }

        //helps mouselook have time to process mouse and rotate camera 
        if (Time.timeScale > Mathf.Epsilon)
            _mouseLook.LookRotation(transform, _camera.transform);

        if (!_jumpButtonPressed)
            _jumpButtonPressed = Input.GetButtonDown("Jump");

        //calculating character status 
        if (!_previouslyGrounded && _characterController.isGrounded)
        {
            if (_fallingTimer > 0.5f)
            {
                //sound
            }

            _moveDirection.y = 0f;
            _isJumping = false;
            _movementStatus = PlayerMoveStatus.Landing;
        }
        else
        if (!_characterController.isGrounded)
            _movementStatus = PlayerMoveStatus.NotGrounded;
        else
        if (_characterController.velocity.sqrMagnitude < 0.01f)
            _movementStatus = PlayerMoveStatus.NotMoving;
        else
        if (_isWalking)
            _movementStatus = PlayerMoveStatus.Walking;
        else
            _movementStatus = PlayerMoveStatus.Running;

        _previouslyGrounded = _characterController.isGrounded;
    }

    protected void FixedUpdate()
    {
        // Query InputManager. Axis mapped by default to WASD and cursor keys
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool waswalking = _isWalking;
        _isWalking = !Input.GetKey(KeyCode.LeftShift);

        //adjust speed 
        float speed;
        if (_isWalking)
        {
            speed = _walkSpeed;
        }
        else
        {
            speed = _runSpeed;
        }

        _inputVector = new Vector2(horizontal, vertical);

        //only want unit vectors (magnitude 1)
        if (_inputVector.sqrMagnitude > 1) _inputVector.Normalize();

        //move where camera is pointed
        Vector3 desiredMove = (transform.forward * _inputVector.y) + (transform.right * _inputVector.x);

        //fixes bumping into sloped ground. raycast gets the normal of the object collided into and moves 90deg to normal
        //cast the rays halfway of character height 
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, _characterController.radius, Vector3.down ,out hitInfo, _characterController.height/2f, 1))
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        _moveDirection.x = desiredMove.x * speed;
        _moveDirection.z = desiredMove.z * speed;
        
        if (_characterController.isGrounded)
        {
            _moveDirection.y = -_stickToGroundForce;

            if (_jumpButtonPressed)
            {
                _moveDirection.y = _jumpSpeed;
                _jumpButtonPressed = false;
                _isJumping = true;
                //add jumping sound?
            }
        }
        else
        {
            _moveDirection += Physics.gravity * _gravityMultiplier * Time.fixedDeltaTime; 
               
        }

        _characterController.Move(_moveDirection * Time.fixedDeltaTime);

    }



}
