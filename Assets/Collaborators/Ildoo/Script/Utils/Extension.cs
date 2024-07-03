using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static bool Contain(this LayerMask layerMask, int targetLayer)
    {
        return ((1 << targetLayer) & layerMask) != 0;
    }

    public static bool IsValid(this GameObject gameObject)
    {
        return gameObject != null && gameObject.activeInHierarchy;
    }

    public static bool IsValid(Component comp)
    {
        return comp != null && comp.gameObject.activeInHierarchy;
    }
}
