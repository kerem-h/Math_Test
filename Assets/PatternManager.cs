using System;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    #region Singleton

    

    public static PatternManager Instance;

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
    #endregion

    private void Start()
    {
        DeleteAllPatterns();
    }

    public GameObject[] Patterns;
    public Transform PatternParent;

    public List<GameObject> CurrentPatterns =  new();
    
    public void DeleteAllPatterns()
    {
        PatternParent.gameObject.SetActive(false);

        foreach (Transform child in PatternParent)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void SpawnPattern(int index, string[] values)
    {
        PatternParent.gameObject.SetActive(true);
        var pattern = Instantiate(Patterns[index-1], PatternParent);
        pattern.GetComponent<Pattern>().SetPattern(values);
        CurrentPatterns.Add(pattern);
    }
    
}
