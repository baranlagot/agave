using UnityEngine;

public class ParticleFeedback : Feedback
{
    [SerializeField] private PoolableParticleSystem psPrefab;

    private PoolFactory<PoolableParticleSystem> psFactory;

    protected override void CustomPlayFeedback(Vector3 position)
    {
        var particle = psFactory.CreateObject(position, Quaternion.identity);
        particle.Initialize(particle.ps, psFactory);
        var main = particle.ps.main;
        main.startColor = feedbackColor;
        particle.Play(position);
    }

    protected override void CustomInitialize()
    {
        psFactory = new PoolFactory<PoolableParticleSystem>(psPrefab, 10, transform);
    }
}
