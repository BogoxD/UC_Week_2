using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager instance;
    void Start()
    {
        if (instance)
            Destroy(instance);
        else
            instance = this;

        DontDestroyOnLoad(instance);
    }
}
