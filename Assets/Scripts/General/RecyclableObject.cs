using System.Collections;
using UnityEngine;

public class RecyclableObject : MonoBehaviour
{
    [HideInInspector]
    public string originalName;

    [SerializeField] private string category;
    [SerializeField] private bool isAutoRecycle;
    [SerializeField] private float lifeTime;
    private bool _isAvailable;

    public bool IsAvailable => _isAvailable;
    public string Category => category;

    public void OnSpawn()
    {
        _isAvailable = true;
        if (isAutoRecycle)
        {
            StartCoroutine(IEAutoRecycle());
        }
    }

    private IEnumerator IEAutoRecycle()
    {
        yield return new WaitForSeconds(lifeTime);
        Recycle();
    }

    public virtual void Recycle()
    {
        if (!_isAvailable) return;
        _isAvailable = false;

        if (ObjectPool.Instance)
        {
            ObjectPool.Instance.DestroyObject(this);
        }
    }
}