using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    public float MaxHp;
    public float CurHp;

    public UnityEvent OnDead;
	// Update is called once per frame
	private void Awake()
	{
		CurHp = MaxHp;
	}
	void Update()
    {
        if(CurHp <= 0)
		{
            OnDead.Invoke();
		}
    }
}
