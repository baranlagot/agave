using UnityEngine;

public class PoolFactory<T> : IGameObjectFactory<T> where T : MonoBehaviour
{
    private ObjectPool<T> pool;

    public PoolFactory(T prefab, int initialSize, Transform parent = null)
    {
        pool = new ObjectPool<T>(prefab, initialSize, parent);
    }

    public T CreateObject(Vector3 position, Quaternion rotation)
    {
        T obj = pool.GetFromPool();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public void ReleaseObject(T obj)
    {
        pool.ReturnToPool(obj);
    }
}
