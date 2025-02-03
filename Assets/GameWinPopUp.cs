using TMPro;
using UnityEngine;

public class GameWinPopUp : PopUp
{
    [SerializeField] private TMP_Text infoText;

    protected override void OnEnable()
    {
        buttonMessage = "Next";
        base.OnEnable();
        infoText.text = "You win!";
    }

    protected override void OnButtonClickedCustom()
    {
        EventManager.Publish<OnNextLevelClickedEvent>(new OnNextLevelClickedEvent());
    }
}
