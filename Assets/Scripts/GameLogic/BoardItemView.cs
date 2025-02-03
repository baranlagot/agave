using BoardLogic;
using UnityEngine;
using DG.Tweening;

public class BoardItemView : MonoBehaviour, IBoardItemView, IPoolable
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public IBoardItem BoardItem { get; private set; }
    public IGameObjectFactory<BoardItemView> Factory { get; private set; }

    private bool isSelected;

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

    public void MoveTo(Vector3 to, float duration)
    {
        transform.DOMove(to, duration).SetEase(Ease.Linear);
    }

    public void FallTo(Vector3 to, float delay)
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

    public void SetSelected()
    {
        if (isSelected)
        {
            return;
        }
        else
        {
            isSelected = true;
            transform.DOScale(Vector3.one * 1.05f, 0.1f);
        }
    }

    public void SetDeselected()
    {
        if (!isSelected)
        {
            return;
        }
        else
        {
            isSelected = false;
            transform.DOScale(Vector3.one, 0.1f);
        }
    }

    private void LoadSprite()
    {
        Debug.Log($"Loading sprite for {BoardItem.Name}");
        var sprite = Resources.Load<Sprite>($"Sprites/{BoardItem.Name}");
        spriteRenderer.sprite = sprite;
    }

    public void OnSpawned()
    {
        Debug.Log("Spawned");
        transform.DOKill();
        isSelected = false;
        transform.localScale = Vector3.one;
    }

    public void OnDespawned()
    {
        transform.DOKill();
        Debug.Log("Despawned");
        isSelected = false;
        transform.localScale = Vector3.one;
    }
}
