using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
 }
