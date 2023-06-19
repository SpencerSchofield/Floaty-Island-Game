using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed;
	public float groundDrag;
	public KeyCode sprintKey =KeyCode.LeftShift;
	
	[Header("Ground Check")]
	public float playerHeight;
	public LayerMask whatIsGround;
	bool grounded;
	
	public Transform orientation;
	float horizontalInput;
	float verticalInput;
	Vector3 moveDirection;
	Rigidbody rb;
	
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;		
	}
	
	private void MyInput()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Vertical");		
	}
	
	private void MovePlayer()
	{
		//calculate movement direction
		moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
		rb.AddForce(moveDirection.normalized * moveSpeed*10f,ForceMode.Force);
		
		
		if(Input.GetKey(sprintKey) && grounded)
		{
			rb.AddForce(moveDirection.normalized * moveSpeed*20f,ForceMode.Force);
		}
	}
	
	private void SpeedControl()
	{
		Vector3 flatVal = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
		
		if(flatVal.magnitude > moveSpeed){
			Vector3 limitedVal = flatVal.normalized * moveSpeed;
			rb.velocity = new Vector3(limitedVal.x, rb.velocity.y, limitedVal.z);
		}
		
	}


	// Update is called once per frame
	void Update()
	{
		//ground check
		grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
		
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
	void FixedUpdate()
	{
		MovePlayer();
	}
}
