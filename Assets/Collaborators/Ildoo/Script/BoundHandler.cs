using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundHandler : MonoBehaviour
{
    private void Start()
    {
        SingletonManager.CaptureManager.SetBound(gameObject);
    }
}