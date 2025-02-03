using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    private void Start()
    {
        EventManager.Subscribe<OnMoveChangedEvent>(OnMoveChanged);
        EventManager.Subscribe<OnNextLevelClickedEvent>(NextLevel);
        EventManager.Subscribe<OnRetryLevelClickedevent>(RetryLevel);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe<OnMoveChangedEvent>(OnMoveChanged);
        EventManager.Unsubscribe<OnNextLevelClickedEvent>(NextLevel);
        EventManager.Unsubscribe<OnRetryLevelClickedevent>(RetryLevel);
    }

    private void GameWin()
    {
        EventManager.Publish<OnGameWinEvent>(new OnGameWinEvent("Congrats you won!"));
    }

    private void GameLose()
    {
        EventManager.Publish<OnGameLoseEvent>(new OnGameLoseEvent("No more turns left"));
    }

    private void OnMoveChanged(OnMoveChangedEvent info)
    {
        bool noMovesLeftFlag = info.movesLeft <= 0;
        bool scoreAchievedFlag = info.scoreTotal >= gameData.targetScore;
        if (noMovesLeftFlag && !scoreAchievedFlag)
        {
            GameLose();
        }
        else if (scoreAchievedFlag)
        {
            GameWin();
        }
    }

    private void NextLevel(OnNextLevelClickedEvent info)
    {
        //basic reloading scene
        SceneManager.LoadScene(0);
    }

    private void RetryLevel(OnRetryLevelClickedevent info)
    {
        SceneManager.LoadScene(0);
    }

}
