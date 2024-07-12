using System;
using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartManagedCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}