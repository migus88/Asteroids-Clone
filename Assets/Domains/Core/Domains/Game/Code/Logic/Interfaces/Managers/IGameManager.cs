namespace Migs.Asteroids.Game.Logic.Interfaces.Managers
{
    public interface IGameManager
    {
        int CurrentScore { get; }

        event ScoreChange ScoreChanged;
    }

    public delegate void ScoreChange(int oldScore, int newScore);
}