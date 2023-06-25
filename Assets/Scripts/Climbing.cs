using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
	[Header("References")]
	public Transform orientation;
	public Rigidbody rb;
	public PlayerMovement pm;
	public LayerMask whatIsWall;
	
	[Header("Climbing")]
	public float climbSpeed;
	public float maxClimbTime;
	private float climbTimer;
	private bool climbing;
	
	[Header("Detection")]
	public float detectionLength;
	public float sphereCastRadius;
	public float maxWallLookAngle;
	public float currentWallLookAngle;
	
	private RaycastHit frontWallHit;
	private bool wallFront;
	
	private void StateMachine()
	{
		if(wallFront && Input.GetKey(KeyCode.W) && currentWallLookAngle < maxWallLookAngle)
		{
			if(!climbing && climbTimer > 0) StartClimbing();

			//timer
			if (climbTimer > 0) climbTimer -= Time.deltaTime;
			if (climbTimer < 0) StopClimbing();
		}
		else 
		{
			if (climbing) StopClimbing();
		}
	}
	
	private void WallCheck()
	{
		wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
		currentWallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
		
		if(pm.grounded)
		{
			climbTimer = maxClimbTime;
		}
		
	}
	
	private void StartClimbing()
	{
		climbing = true;
		pm.climbing = true;
	}
	
	private void ClimbingMovement()
	{
		rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
	}
	
	private void StopClimbing()
	{
		climbing = false;
		pm.climbing = false;
	}
	
	private void Update()
	{
		WallCheck();
		StateMachine();
		
		if (climbing) ClimbingMovement();
	}
	
}
