using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public float actGap;

    public List<GameObject> Attacks;

    public Animator anim;

    enum Act
	{
        None = -1,
        Teleport,
        Charge,
        Summon,

        Max
	}
    void Awake()
    {
        StartCoroutine(Acting());
    }

    IEnumerator Acting()
	{
		while (true)
		{
            yield return null;
            int no = Random.Range(((int)Act.Teleport), ((int)Act.Max));
            Vector2 place = new Vector2(Random.Range(StageMan.Instance.min.x, StageMan.Instance.max.x), Random.Range(StageMan.Instance.min.y, StageMan.Instance.max.y));
            if (no == ((int)Act.Teleport))
			{
                anim.SetTrigger("Tel");
                transform.position = place;
                Instantiate(Attacks[(int)Act.Teleport], place, Quaternion.identity);
            }
			else if(no == ((int)Act.Summon))
			{
                //Vector2 place = new Vector2(Random.Range(StageMan.Instance.min.x, StageMan.Instance.max.x), Random.Range(StageMan.Instance.min.y, StageMan.Instance.max.y));
                Instantiate(Attacks[(int)Act.Summon], place, Quaternion.identity);
            }
            else if(no == ((int)Act.Charge))
			{
                Instantiate(Attacks[(int)Act.Charge], transform.position, Quaternion.identity, transform);
                //Vector2 place = new Vector2(Random.Range(StageMan.Instance.min.x, StageMan.Instance.max.x), Random.Range(StageMan.Instance.min.y, StageMan.Instance.max.y));
                while (WeaponMove.Approximate(place, transform.position, 0.05f))
				{
                    transform.position = Vector2.Lerp(transform.position, place, 0.05f);
                    yield return null;
				}

			}
            yield return new WaitForSeconds(actGap);
		}
	}
}
