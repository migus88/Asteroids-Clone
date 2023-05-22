using Migs.Asteroids.Game.Logic.Interfaces.Services;

namespace Migs.Asteroids.Game.Logic.Services
{
    public class ScoreService : IScoreService
    {
        public int CurrentScore
        {
            get => _currentScore;
            private set
            {
                if (_currentScore != value)
                {
                    ScoreChanged?.Invoke(_currentScore, value);
                }

                _currentScore = value;
            }
        }
        
        public event ScoreChange ScoreChanged;
        
        private int _currentScore;
        
        public void Reset()
        {
            _currentScore = 0;
        }

        public void AddScore(int addedScore)
        {
            if (addedScore < 0)
            {
                return;
            }
            
            CurrentScore += addedScore;
        }
    }
}