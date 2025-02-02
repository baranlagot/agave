using UnityEngine;

public class PoolableParticleSystem : MonoBehaviour, IPoolable
{
    public PoolFactory<PoolableParticleSystem> psFactory;
    public ParticleSystem ps;

    public void Initialize(ParticleSystem ps, PoolFactory<PoolableParticleSystem> psFactory)
    {
        this.ps = ps;
        this.psFactory = psFactory;
    }

    public void OnDespawned()
    {
        ps.Stop();
    }

    public void OnSpawned()
    {
        ps.time = 0;
    }

    public void Play(Vector3 position)
    {
        transform.position = position;
        ps.Play();
    }

    private void LateUpdate()
    {
        if (!ps.isPlaying)
        {
            psFactory.ReleaseObject(this);
        }
    }

}
