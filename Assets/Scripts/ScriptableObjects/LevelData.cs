using UnityEngine;

[CreateAssetMenu( fileName = "NewLevelData", menuName = "Game/LevelData", order = 1 )]
public class LevelData : ScriptableObject
{
    public int maxMoves;
    public int goal;
}