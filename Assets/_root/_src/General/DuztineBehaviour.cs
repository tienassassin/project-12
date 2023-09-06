using UnityEngine;

public class DuztineBehaviour : MonoBehaviour
{
    private Transform _cachedTransform;

    public Transform Transform
    {
        get
        {
            if (!_cachedTransform)
            {
                _cachedTransform = this.transform;
            }

            return _cachedTransform;
        }   
    }
}
