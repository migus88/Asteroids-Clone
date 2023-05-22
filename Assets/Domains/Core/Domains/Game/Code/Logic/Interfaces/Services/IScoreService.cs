namespace Migs.Asteroids.Game.Logic.Interfaces.Services
{
    public interface IScoreService
    {
        int CurrentScore { get; }
        event ScoreChange ScoreChanged;

        void Reset();
        void AddScore(int addedScore);
    }
    
    public delegate void ScoreChange(int oldScore, int newScore);
}