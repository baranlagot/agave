using UnityEngine;

/// <summary>
/// Represents a factory for creating game objects.
/// </summary>
/// <typeparam name="T">The type of the game object.</typeparam>
public interface IGameObjectFactory<T> where T : MonoBehaviour
{
    /// <summary>
    /// Creates a game object.
    /// </summary>
    /// <param name="position">The position of the game object.</param>
    /// <param name="rotation">The rotation of the game object.</param>
    /// <returns>The created game object.</returns>
    T CreateObject(Vector3 position, Quaternion rotation);

    /// <summary>
    /// Releases a game object.
    /// </summary>
    /// <param name="obj">The game object to release.</param>
    void ReleaseObject(T obj);
}