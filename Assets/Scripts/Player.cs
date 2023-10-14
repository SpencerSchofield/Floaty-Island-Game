using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public HealthBar hb;
	public StaminaBar sb;
	public int health;
	public float currentStamina;
	public float maxStamina;
	
	void Start()
	{
		health = 100;
		currentStamina = 100;
		maxStamina = 100;
	}
	
	/*
	HealPlayer() heals player by certain amount
	*/
	public void HealPlayer(int amount)
	{
		health += amount;
		hb.SetHealth(health);
	}
	
	/*
	DamagePlayer() remove health from player
	*/
	public void DamagePlayer(int amount)
	{
		health -= amount;
		hb.SetHealth(health);
	}
	
	/*
	UseStamina(float amount) is used whenever the player preforms an action that requires stamina use (sprinting, climbing, etc).
	It will start a coroutine to auto start regenerating stamina. Takes in float "amount" of stamina to deplete from current stamina.
	returns true when stamina is being depleted. 
	*/
	private Coroutine regen;
	public bool UseStamina(float amount)
	{
		if(currentStamina - amount >= 0)
		{
			currentStamina -= amount*Time.deltaTime;
			sb.SetStamina(currentStamina);
			if(regen != null){
				StopCoroutine(regen);
			}
			regen = StartCoroutine(RegenStamina());
			return true;
		} 
	return false;
	}
	/*
	RegenStamina() coroutine that auto fills the players stamina when they run out.
	*/
	private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
	private IEnumerator RegenStamina()
	{
		yield return new WaitForSeconds(2);
		
		while(currentStamina < maxStamina)
		{
			currentStamina += 20;
			sb.SetStamina(currentStamina);
			currentStamina=Mathf.Clamp(currentStamina,0,maxStamina);
			yield return regenTick;
		}
		regen = null;
	}

	void Update()
	{
		//test to see UI health bar working correctly.
		if(Input.GetMouseButtonDown(0))
		{
			Debug.Log("OUCH");
			health -= 20;
			hb.SetHealth(health);
		}
	}
	
	
	
}
