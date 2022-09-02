using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPlayer : MonoBehaviour
{

    enum State
	{
        None = -1,
        Idle, // �տ� ���Ⱑ ����(������ �� ����.)
        Throw, // �տ� ���Ⱑ ����(���⿡ �����ϰų� ������ ���� �� �ִ�.)
        Possess, // ���� ������ ��. (������ �� ������ ����)
        Wait, // ���Ⱑ ���ƿ��⸦ ��ٸ�. (����Ŭ �ݺ��� ���� �� ���� ������)
	}

    Vector2 initMousePos;
    Vector2 curMousePos;
    Vector2 deltaPos;
    Vector2 indicatedPos;
    bool holding = false;
    Vector2 cursorPos;
    int currentState = 0;
    int prevState = -1;
    Holder holder;
    Collider2D cursorBoxInfo;
    RaycastHit2D hit;
    int absColMask;

    const int ABSOLUTECOL = 10;
    
    public float movePower;
    public Image indicator;
    public float returnTime;
    public float deposRange;
    public WeaponMove weapon;
    public Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        absColMask = (1<<ABSOLUTECOL);
        anim = GetComponent<Animator>();
        holder = GetComponentInChildren<Holder>();
        StartCoroutine(CountSec());
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0) && currentState != ((int)State.Wait))
		{
            indicator.enabled = true;
            initMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            holding = true;
		}
        if (Input.GetMouseButtonUp(0) && currentState != ((int)State.Wait))
		{
            indicator.enabled = false;
            holding = false;
            if(currentState == ((int)State.Throw))
			{
                if((cursorBoxInfo = Physics2D.OverlapBox(indicatedPos, Vector2.one, 0)) && cursorBoxInfo.CompareTag("Weapon"))
				{
                    Debug.Log("����");
                    
                    StartCoroutine(WaitMove(0.5f, indicatedPos));
                    currentState = ((int)State.Possess);
                }
                
			}
            else if(currentState == ((int)State.Idle))
			{
                Debug.Log("����");
                weapon.MoveTo(indicatedPos);
                currentState = ((int)State.Throw);
			}
            else if(currentState == ((int)State.Possess))
			{
                Debug.Log("���ƿ�");
                if(hit = Physics2D.Raycast(transform.position, (indicatedPos - new Vector2(transform.position.x, transform.position.y)).normalized, (indicatedPos - new Vector2(transform.position.x, transform.position.y)).magnitude, absColMask))
                {
                    indicatedPos = hit.point;
                    indicatedPos.x = Mathf.Abs(indicatedPos.x + transform.localScale.x) < Mathf.Abs(indicatedPos.x - transform.localScale.x) ? indicatedPos.x + transform.localScale.x : indicatedPos.x - transform.localScale.x;
                    indicatedPos.x = Mathf.Abs(indicatedPos.y + transform.localScale.y) < Mathf.Abs(indicatedPos.y - transform.localScale.y) ? indicatedPos.y + transform.localScale.y : indicatedPos.y - transform.localScale.y;
				}
                transform.position = indicatedPos;
                weapon.MoveTo(holder.transform.position, 1f);
                currentState = ((int)State.Wait);
			}
            anim.SetInteger("PlayerState", currentState);
        }
        CursorMove();
    }

	private void LateUpdate()
	{
		if (holder.isHolding && currentState == ((int)State.Wait))
		{
            Debug.Log("����");
            currentState = ((int)State.Idle);
            anim.SetInteger("PlayerState", currentState);
        }
	}

	void CursorMove()
	{
		if (holding)
		{
            curMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            deltaPos = initMousePos - curMousePos;
            indicatedPos = new Vector2(deltaPos.x + transform.position.x, deltaPos.y + transform.position.y);
            cursorPos = Camera.main.WorldToScreenPoint(indicatedPos);
            
            indicator.transform.position = cursorPos;
        }
	}

    IEnumerator WaitMove(float sec, Vector2 pos)
	{
        yield return new WaitForSeconds(sec);
        transform.position = pos;
	}

    IEnumerator CountSec()
	{
		while (true)
		{
            yield return null;
            if(currentState != prevState)
			{
                prevState = currentState;
                yield return new WaitForSeconds(returnTime);
                if(prevState == currentState && currentState != ((int)State.Idle))
				{
                    if(currentState == ((int)State.Possess))
					{
                        Vector2 depos =  Random.insideUnitCircle * deposRange;
                        transform.position = new Vector3(transform.position.x + depos.x, transform.position.y + depos.y, transform.position.z);
                        currentState = ((int)State.Wait);
                        anim.SetInteger("PlayerState", currentState);
                        yield return new WaitForSeconds(1f);
					}
                    weapon.MoveTo(holder.transform.position);
                    currentState = ((int)State.Idle);
                    anim.SetInteger("PlayerState", currentState);
                    
                }
			}
		}
	}
}
