using System;

namespace Migs.Asteroids.Core.Logic.Services.Interfaces
{
    public interface IGameUiService
    {
        event Action RestartGamePressed;
        
        void ShowGameMenuPanel();
        void HideGameMenuPanel();
        void ShowGameOverPanel();
        void HideGameOverPanel();
        void SetScore(int score);
        void SetLives(int lives);
    }
}