using BoardLogic;
using UnityEngine;
using DG.Tweening;

public class BoardItemView : MonoBehaviour, IBoardItemView
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public IBoardItem BoardItem { get; private set; }
    public IGameObjectFactory<BoardItemView> Factory { get; private set; }

    public void Initialize(IBoardItem boardItem, IGameObjectFactory<BoardItemView> factory)
    {
        BoardItem = boardItem;
        Factory = factory;
        LoadSprite();
    }

    public void SetPosition(float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public void MoveTo(Vector3 to, float delay)
    {
        var initialDelay = delay;
        var fallDuration = 0.1f;
        var bounceOvershoot = 0.1f;
        var bounceDuration = 0.05f;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(initialDelay);
        sequence.Append(transform.DOMove(to, fallDuration).SetEase(Ease.InQuad));
        Vector3 upPos = to + Vector3.up * bounceOvershoot;
        sequence.Append(transform.DOMove(upPos, bounceDuration / 2).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOMove(to, bounceDuration / 2).SetEase(Ease.InQuad));
    }

    private void LoadSprite()
    {
        Debug.Log($"Loading sprite for {BoardItem.Name}");
        var sprite = Resources.Load<Sprite>($"Sprites/{BoardItem.Name}");
        spriteRenderer.sprite = sprite;
    }

}
