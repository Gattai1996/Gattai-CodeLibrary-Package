using System;
using Gattai.Runtime.Singletons;
using UnityEngine;

namespace Gattai.Runtime.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private GameState _currentGameState;
        public event Action<GameState> OnBeforeGameStateChanged;
        public event Action<GameState> OnAfterGameStateChanged;

        private void Start()
        {
            SetGameState(GameState.GameStarting);
        }

        public void SetGameState(GameState newGameState)
        {
            _currentGameState = newGameState;
            
            OnBeforeGameStateChanged?.Invoke(_currentGameState);

            switch (_currentGameState)
            {
                case GameState.GameStarting: GameStart(); break;
                case GameState.GameEnding: GameEnd(); break;
                case GameState.PlayerControlsFree: HandlePlayerControlsFree(); break;
                case GameState.PlayerControlsBlocked: HandlePlayerControlsBlocked(); break;
                default: UnityEngine.Debug.LogError($"{nameof(GameState)} not implemented on {nameof(GameManager)}."); break;
            }
            
            OnAfterGameStateChanged?.Invoke(_currentGameState);
        }

        private void GameStart()
        {
            
        }
        
        private void GameEnd()
        {
            
        }
        
        private void HandlePlayerControlsFree()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void HandlePlayerControlsBlocked()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
