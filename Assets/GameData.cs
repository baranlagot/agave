using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "AgaveCase", order = 0)]
public class GameData : ScriptableObject
{
    public int boardWidth = 8;
    public int boardHeight = 8;
    public int moveCount = 14;
    public int targetScore = 50;
}