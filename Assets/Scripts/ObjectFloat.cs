using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFloat : MonoBehaviour
{
	public float floatSpeed = 1f;
	public float floatHeight = 30f;
	private Vector3 startPos;
	[SerializeField] private bool isStatic = false;
	
	// Start is called before the first frame update
	void Start()
	{
		startPos = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		//Make object float 
		if(!isStatic){
		float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
		transform.position = new Vector3(transform.position.x, startPos.y + newY, transform.position.z);
		} else 
		{
			transform.position = new Vector3(0,0,0) ; // this is wrong
		}
	}
}
