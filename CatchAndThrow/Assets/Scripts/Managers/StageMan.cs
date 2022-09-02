using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMan : MonoBehaviour
{

    public static StageMan Instance;
    public Vector2 min;
    public Vector2 max;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        min = GetComponentsInChildren<Transform>()[1].position;
        max = GetComponentsInChildren<Transform>()[2].position;
    }

    
}
