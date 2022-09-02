using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttRange : MonoBehaviour
{
    public float DestSec;
	public float dam;

	public bool isDest;
	private void Awake()
	{
		if (isDest)
		{
			Destroy(gameObject, DestSec);
		}
		
	}
	// Update is called once per frame
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<Life>().CurHp -= dam;
		}
	}
}
