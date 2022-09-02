using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponMove : MonoBehaviour
{
	public float throwSpeed; // ���Ը� �뺯��
	public Animator anim;
	public float err;
	int ignoreLayer;

	const int ABSOLUTECOL = 10;

	Coroutine c;
	RaycastHit2D hit;

	private void Awake()
	{
		ignoreLayer  = (1 << ABSOLUTECOL);
		anim = GetComponent<Animator>();
	}

	public void MoveTo(Vector2 position, float delay = 0)
	{
		if(c != null)
			StopCoroutine(c);
		c = StartCoroutine(LerpMove(position, delay));
	}

	IEnumerator LerpMove(Vector3 pos, float del)
	{
		Debug.Log(pos +  "�� �̵�����");
		hit = Physics2D.Raycast(transform.position, (pos - transform.position).normalized, (pos - transform.position).magnitude, ignoreLayer);
		if (hit)
		{
			pos = hit.point;
		}
		yield return new WaitForSeconds(del);
		anim.SetTrigger("Spin");
		while (!Approximate(transform.position, pos, err))
		{
			transform.position = Vector2.Lerp(transform.position, pos, throwSpeed);
			yield return null;
			Debug.Log("�̵���");
		}
		//anim.SetBool("Spin", false);
		Debug.Log("�̵� �Ϸ�");
	}

	public static bool Approximate(float a, float b, float err)
	{
		return Mathf.Abs(a - b) < err;
	}

	public static bool Approximate(Vector2 a, Vector2 b, float err)
	{
		return Approximate(a.x, b.x, err) && Approximate(a.y, b.y, err);
	}
}
