using TMPro;
using UnityEngine;

public class GameLosePopUp : PopUp
{
    [SerializeField] private TMP_Text infoText;

    protected override void OnEnable()
    {
        buttonMessage = "Try Again";
        base.OnEnable();
        infoText.text = "You lose!";
    }

    protected override void OnButtonClickedCustom()
    {
        EventManager.Publish<OnRetryLevelClickedevent>(new OnRetryLevelClickedevent());
    }
}
