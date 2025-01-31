using UnityEngine;

public interface IGameObjectFactory<T> where T : MonoBehaviour
{
    T CreateObject(Vector3 position, Quaternion rotation);
    void ReleaseObject(T obj);
}