using BoardLogic;
using UnityEngine;
using DG.Tweening;

public class BoardItemView : MonoBehaviour, IBoardItemView, IPoolable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private const string SPRITE_RESOURCE_PATH = "Sprites/{0}";
    private bool isSelected;

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

    public void MoveTo(Vector3 to, float duration)
    {
        transform.DOMove(to, duration).SetEase(Ease.Linear);
    }

    public void FallTo(Vector3 targetPosition, float delay)
    {
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(delay)
                .Append(transform.DOMove(targetPosition, GameConstants.FALL_DURATION).SetEase(Ease.InQuad))
                .Append(GetBounceSequence(targetPosition));
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
            transform.DOScale(Vector3.one * GameConstants.SELECTION_SCALE, GameConstants.SELECTION_ANIMATION_DURATION);
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
            transform.DOScale(Vector3.one, GameConstants.SELECTION_ANIMATION_DURATION);
        }
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

    private void LoadSprite()
    {
        string spritePath = string.Format(SPRITE_RESOURCE_PATH, BoardItem.Name);
        var sprite = Resources.Load<Sprite>(spritePath);
        spriteRenderer.sprite = sprite;
    }

    private Sequence GetBounceSequence(Vector3 targetPosition)
    {
        Vector3 bouncePosition = targetPosition + Vector3.up * GameConstants.BOUNCE_OVERSHOOT;
        float halfBounceDuration = GameConstants.BOUNCE_DURATION / 2;

        return DOTween.Sequence()
            .Append(transform.DOMove(bouncePosition, halfBounceDuration).SetEase(Ease.OutQuad))
            .Append(transform.DOMove(targetPosition, halfBounceDuration).SetEase(Ease.InQuad));
    }


}
