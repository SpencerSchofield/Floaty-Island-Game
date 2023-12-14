using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
	Player player;
	PlayerMovement pm;
	Animator animator;
	// Start is called before the first frame update
	void Start()
	{
		player = FindObjectOfType<Player>();
		pm = FindObjectOfType<PlayerMovement>();
		animator = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update()
	{	
		bool onlyOnce = true;
		bool isGrounded = pm.grounded;
		bool isRunning = animator.GetBool("isRunning");
		bool isWalking = animator.GetBool("isWalking");
		
		bool forwardPressed = Input.GetKey("w");
		bool runPressed = Input.GetKey("left shift");
		bool jumped = Input.GetKey("space");
		
		if(!isWalking && forwardPressed)
		{
			animator.SetBool("isWalking",true);
		} else if (isWalking && !forwardPressed) animator.SetBool("isWalking", false);
		
		if(!isRunning && forwardPressed && runPressed &&player.UseStamina(10))
		{
			animator.SetBool("isRunning", true);	
		} 
		
		if(isRunning && !forwardPressed || !runPressed || !player.UseStamina(10)) 
		{
			animator.SetBool("isRunning",false);
		}
		
		if(!isGrounded && jumped )
		{
			animator.SetBool("isJumping", true);
			onlyOnce = false;
		} else if (isGrounded)
		animator.SetBool("isJumping", false);
		onlyOnce = true;
		//Falling animation
	}
}
