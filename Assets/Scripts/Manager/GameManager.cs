using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Action MoveMade;
    public static Action MatchMade;
    [SerializeField]
    private LevelData _levelData;
    [SerializeField]
    private TMP_Text _movesText;
    [SerializeField]
    private TMP_Text _goalText;
    [SerializeField]
    private GameObject _nextLevelScreen;
    [SerializeField]
    private GameObject _gameOverScreen;
    private int _moveCount;
    private int _goalCount;
    private void OnEnable()
    {
        _moveCount = _levelData.maxMoves;
        _goalCount = _levelData.goal;
        MoveMade += UpdateMoveCount;
        MatchMade += UpdateGoalCount;
        _movesText.text = _moveCount.ToString();
        _goalText.text = _goalCount.ToString();
    }

    private void OnDisable()
    {
        MoveMade -= UpdateMoveCount;
        MatchMade -= UpdateGoalCount;
    }

    private void UpdateMoveCount()
    {
        _moveCount--;
        _movesText.text = _moveCount.ToString();
        if (_moveCount <= 0 && _nextLevelScreen.activeInHierarchy == false)
                _gameOverScreen.SetActive( true );
    }

    private void UpdateGoalCount()
    {
        _goalCount -= 2;
        _goalText.text = _goalCount.ToString();
        if (_goalCount <= 0 && _gameOverScreen.activeInHierarchy == false)
            _nextLevelScreen.SetActive( true );
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene( nextSceneIndex );
        }
        else
        {
            SceneManager.LoadScene( 0 );
        }
    }
    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene( currentSceneIndex );
    }
}