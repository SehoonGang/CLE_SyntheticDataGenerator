using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundHandler : MonoBehaviour
{
    private void Awake()
    {
        SingletonManager.CaptureManager.Init(gameObject);
    }
}