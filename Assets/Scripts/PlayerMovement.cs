using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	
	[Header("Movement")]
	private float moveSpeed;
	public float walkSpeed;
	public float sprintSpeed;
	public float groundDrag;

	public float jumpForce;
	public float jumpCooldown;
	public float airMultiplier;
	public bool readyToJump = true;
	public bool exitingSlope;
	
	[Header("KeyBinds")]
	public KeyCode jumpKey = KeyCode.Space;
  	public KeyCode sprintKey = KeyCode.LeftShift;
	
	[Header("Ground Check")]
	public float playerHeight;
	public LayerMask whatIsGround;
	bool grounded;
	
	[Header("Slope Handling")]
	public float maxSlopeAngle;
	private RaycastHit slopeHit;
	
	public Transform orientation;
	float horizontalInput;
	float verticalInput;
	Vector3 moveDirection;
	Rigidbody rb;
	public MovementState state;
	public enum MovementState
	{
		walking,
		sprinting,
		air
	}
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;		
	}
	
	/*
	MyInput() function is used to get the player's input direction that the player wants to travel in and 
	checks if they can jump, by checking if they are on the ground and enough time has passed since they last pressed jump. 
	*/
	private void MyInput()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Vertical");
		
		if(Input.GetKey(jumpKey) && readyToJump && grounded)
		{
			readyToJump = false;
			Jump();
			
			Invoke(nameof(ResetJump), jumpCooldown);
		}
				
	}
	
	private void StateHandler()
	{
		//Mode - Sprinting
		if(grounded && Input.GetKey(sprintKey))
		{
			state = MovementState.sprinting;
			moveSpeed = sprintSpeed;
		}
		//Mode - Walking
		else if (grounded)
		{
			state = MovementState.walking;
			moveSpeed = walkSpeed;
		}
		//Mode - Air
		else
		{
			state = MovementState.air;
		}
	}
	
	/*
	MovePlayer() function handles the direction the player is moving, it also allows me to change the speed they walk
	while on the floor and the speed they have while jumping / falling in air
	*/
	private void MovePlayer()
	{
		//calculate movement direction
		moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
		
		//on slope
		if(OnSlope() && exitingSlope)
		{
			rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
			
			if(rb.velocity.y > 0)
			{
				rb.AddForce(Vector3.down * 80f, ForceMode.Force);
			}
		}
		
		//on ground
		else if(grounded){
		rb.AddForce(moveDirection.normalized * moveSpeed*10f,ForceMode.Force);
		}
		//in air
		else if(!grounded)
		{
			rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

		}

		//turn off gravity when on slope
		rb.useGravity = !OnSlope();
		
		
	}
	
	/*
	SpeedControl() only function is to cap the player's movement speed 
	*/
	private void SpeedControl()
	{
		//limiting speed on slope
		if(OnSlope() && !exitingSlope)
		{
			if(rb.velocity.magnitude > moveSpeed){
			rb.velocity = rb.velocity.normalized * moveSpeed;
		} 
		}
		//limiting speed on ground or air
		else 
		{
			Vector3 flatVal = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
		
		if(flatVal.magnitude > moveSpeed){
			Vector3 limitedVal = flatVal.normalized * moveSpeed;
			rb.velocity = new Vector3(limitedVal.x, rb.velocity.y, limitedVal.z);
		} 
		}
		
		
	}
	
	/*
	Jump() is used to calculate the force of the jump and apply that force in an upwards trajectory
	*/
	private void Jump()
	{
		exitingSlope = true;
		//rest y velocity
		rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
		rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
		Debug.Log("Jump!");
		
	}
	
	/*
	ResetJump() is used to change readyToJump bool to true after a certain amount of time
	*/
	private void ResetJump()
	{
		readyToJump = true;
		exitingSlope= false;
	}

	private bool OnSlope()
	{
		if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
		{
			float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
			Debug.Log("On Slope");
			Debug.Log(angle);
			return angle < maxSlopeAngle && angle != 0;
			
		}
		return false;
	}
	
	private Vector3 GetSlopeMoveDirection()
	{
		return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
	}

	/*
	Update() is called once per frame
	*/ 
	void Update()
	{
		//ground check
		grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
		Debug.Log(grounded);
		MyInput();
		SpeedControl();
		StateHandler();
		
		//handle drag
		if(grounded)
		{
			rb.drag = groundDrag;
		}
		else
		{
			rb.drag = 0;
		}
		
		Debug.Log("Speed: " + rb.velocity);
		Debug.Log("Speedo: " + rb.position.magnitude);
		
	}
	
	/*
	FixedUpdate is called everyframe but after Update() - its best to keep any rigibody / physics updates with FixedUpdate
	*/
	void FixedUpdate()
	{
		MovePlayer();
	}
}

