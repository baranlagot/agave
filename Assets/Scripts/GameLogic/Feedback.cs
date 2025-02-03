using UnityEngine;

public abstract class Feedback : MonoBehaviour
{
    [SerializeField] public Color feedbackColor = Color.white;

    protected abstract void CustomInitialize();
    protected abstract void CustomPlayFeedback(Vector3 position);

    public virtual void Initialize()
    {
        CustomInitialize();
    }

    public virtual void Play(Vector3 position)
    {
        CustomPlayFeedback(position);
    }

}
