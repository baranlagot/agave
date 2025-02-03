using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text moveCountText;
    [SerializeField] private TMP_Text scoreCount;

    [SerializeField] private GameWinPopUp gameWinPopUp;
    [SerializeField] private GameLosePopUp gameLosePopUp;

    private void Start()
    {
        EventManager.Subscribe<OnMoveChangedEvent>(OnMoveCountChanged);
        EventManager.Subscribe<OnGameWinEvent>(OnGameWin);
        EventManager.Subscribe<OnGameLoseEvent>(OnGameLose);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe<OnMoveChangedEvent>(OnMoveCountChanged);
        EventManager.Unsubscribe<OnGameWinEvent>(OnGameWin);
        EventManager.Unsubscribe<OnGameLoseEvent>(OnGameLose);
    }

    private void OnMoveCountChanged(OnMoveChangedEvent onMoveChangedEvent)
    {
        moveCountText.text = "Moves " + onMoveChangedEvent.movesLeft;
        scoreCount.text = "Score " + onMoveChangedEvent.scoreTotal;
    }

    private void OnGameLose(OnGameLoseEvent info)
    {
        Instantiate(gameLosePopUp, transform);
    }

    private void OnGameWin(OnGameWinEvent info)
    {
        Instantiate(gameWinPopUp, transform);
    }

}