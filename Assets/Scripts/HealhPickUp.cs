using UnityEngine;

public class HealhPickUp : MonoBehaviour
{
	Player player;
	void Start()
	{
		player = FindObjectOfType<Player>();
	}
	void OnCollisionEnter(Collision collision)
	{
		Destroy(this.gameObject);
		player.HealPlayer(20);
	}


}
