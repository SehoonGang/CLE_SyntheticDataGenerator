using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public event Action Initialize;

    public void Reset()
    {
        Initialize?.Invoke();
    }
}
