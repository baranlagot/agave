using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> poolQueue = new Queue<T>();
    private T prefab;
    private Transform parent; // optional parent for spawned objects

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        // Pre-instantiate initialSize objects
        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNewObject();
            poolQueue.Enqueue(obj);
            obj.gameObject.SetActive(false);
        }
    }

    private T CreateNewObject()
    {
        T newObj = Object.Instantiate(prefab, parent);
        // If T implements IPoolable, call OnDespawned immediately:
        if (newObj is IPoolable poolable)
            poolable.OnDespawned();
        return newObj;
    }

    public T GetFromPool()
    {
        T obj;
        if (poolQueue.Count > 0)
        {
            obj = poolQueue.Dequeue();
        }
        else
        {
            obj = CreateNewObject();
        }

        obj.gameObject.SetActive(true);
        // If IPoolable, call OnSpawned
        if (obj is IPoolable p)
            p.OnSpawned();

        return obj;
    }

    public void ReturnToPool(T obj)
    {
        // If IPoolable, call OnDespawned
        if (obj is IPoolable p)
            p.OnDespawned();

        obj.gameObject.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}
