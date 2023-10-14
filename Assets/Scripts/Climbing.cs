using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{	
	[Header("References")]
	public Player player;
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
	
	/*
	StateMachine() is used to determine if the player can start climbing
	*/
	
	private void StateMachine()
	{
		if(wallFront && Input.GetKey(KeyCode.W) && currentWallLookAngle < maxWallLookAngle)
		{
			if(!climbing && player.UseStamina(10)){
			StartClimbing();
			player.UseStamina(10);
			}
			else StopClimbing();
			
		}
		else 
		{
			if (climbing) StopClimbing();
		}
	}
	
	/*
	WallCheck() Sends out a sphere cast infront of the player to determine if they're standing in front of a wall.
	Also checks the current wall look angle which determines how much the player needs to be facing the wall before they can climb.
	*/
	private void WallCheck()
	{
		wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
		currentWallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
		
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
