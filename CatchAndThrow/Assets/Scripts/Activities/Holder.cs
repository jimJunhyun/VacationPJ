using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{
    public bool isHolding;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Weapon"))
		{
			isHolding = true;
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Weapon"))
		{
			isHolding = false;
		}
	}
}
