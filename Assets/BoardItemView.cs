using BoardLogic;
using UnityEngine;

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

    public void MoveTo(Vector3 to)
    {
        transform.position = to;
    }

    private void LoadSprite()
    {
        Debug.Log($"Loading sprite for {BoardItem.Name}");
        var sprite = Resources.Load<Sprite>($"Sprites/{BoardItem.Name}");
        spriteRenderer.sprite = sprite;
    }

}
