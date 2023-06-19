using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed;
	public float groundDrag;
	public float jumpForce;
	public float jumpCooldown;
	public float airMultiplier;
	public bool readyToJump = true;
	
	public KeyCode jumpKey = KeyCode.Space;
	
	[Header("Ground Check")]
	public float playerHeight;
	public LayerMask whatIsGround;
	bool grounded;
	
	public Transform orientation;
	float horizontalInput;
	float verticalInput;
	Vector3 moveDirection;
	Rigidbody rb;
	
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
	/*
	MovePlayer() function handles the direction the player is moving, it also allows me to change the speed they walk
	while on the floor and the speed they have while jumping / falling in air
	*/
	private void MovePlayer()
	{
		//calculate movement direction
		moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
		
		//on ground
		if(grounded){
		rb.AddForce(moveDirection.normalized * moveSpeed*10f,ForceMode.Force);
		}
		
		//in air
		else if(!grounded)
		{
			rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
		}
	}
	/*
	SpeedControl() only function is to cap the player's movement speed 
	*/
	private void SpeedControl()
	{
		Vector3 flatVal = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
		
		if(flatVal.magnitude > moveSpeed){
			Vector3 limitedVal = flatVal.normalized * moveSpeed;
			rb.velocity = new Vector3(limitedVal.x, rb.velocity.y, limitedVal.z);
			//Debug.Log("Speed Capped");
		}
		
	}
	
	/*
	Jump() is used to calculate the force of the jump and apply that force in an upwards trajectory
	*/
	private void Jump()
	{
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
		if(grounded)
		{
			rb.drag = groundDrag;
		}
		else
		{
			rb.drag = 0;
		}
	}
	
	/*
	FixedUpdate is called everyframe but after Update() - its best to keep any rigibody / physics updates with FixedUpdate
	*/
	void FixedUpdate()
	{
		MovePlayer();
	}
}
