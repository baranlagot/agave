using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopUp : MonoBehaviour
{
    protected abstract void OnButtonClickedCustom();

    [SerializeField] protected Button closePopUpButton;
    protected string buttonMessage;

    protected virtual void OnEnable()
    {
        closePopUpButton.onClick.AddListener(OnButtonClicked);
        closePopUpButton.GetComponentInChildren<TMP_Text>().text = buttonMessage;
    }

    protected virtual void OnDisable()
    {
        closePopUpButton.onClick.RemoveListener(OnButtonClicked);
    }


    protected void OnButtonClicked()
    {
        OnButtonClickedCustom();
        gameObject.SetActive(false);
    }

}