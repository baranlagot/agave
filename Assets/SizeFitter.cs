using UnityEngine;

public class SizeFitter : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    public int referenceScreenWidth = 1080;
    public int referenceScreenHeight = 1920;
    public float referenceOrthographicSize = 22;

    private int referenceBoardWidth = 8;

    private int lastScreenWidth = 0;
    private int lastScreenHeight = 0;

    private void Start()
    {
        lastScreenHeight = Screen.height;
        lastScreenWidth = Screen.width;
        OnSizeChanged();
    }

    //for easy testing
    private void LateUpdate()
    {
        if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            OnSizeChanged();
        }
    }

    private void OnSizeChanged()
    {
        var referenceOrthoSizePerWidth = (float)referenceOrthographicSize / (float)referenceBoardWidth;
        var newOrthoSize = referenceOrthoSizePerWidth * (float)gameData.boardWidth;
        var widthBasedSize = newOrthoSize * (referenceScreenWidth / (float)Screen.width);
        var targetOrthographicSize = (widthBasedSize);

        Camera.main.orthographicSize = targetOrthographicSize;

    }

}
