using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuztineBehaviour : MonoBehaviour
{
    private Transform cachedTransform;

    public Transform Transform
    {
        get
        {
            if (!cachedTransform)
            {
                cachedTransform = this.transform;
            }

            return cachedTransform;
        }   
    }
}
