public interface IEvent
{
    public void Invoke(IEvent @event);
}