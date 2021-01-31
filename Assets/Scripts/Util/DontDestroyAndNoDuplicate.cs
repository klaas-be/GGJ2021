using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyAndNoDuplicate : MonoBehaviour
{
    private static DontDestroyAndNoDuplicate instance;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
