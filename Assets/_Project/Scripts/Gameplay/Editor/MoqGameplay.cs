namespace _Project.Scripts.Gameplay.Editor
{
    public class DummyWinLoseListener : IWinLoseListener
    {
        public bool IsWin;
        public bool IsLose;
        public int Lives;

        public void OnWin()
        {
            IsWin = true;
        }

        public void OnLose()
        {
            IsLose = true;
        }

        public void OnUpdateLives(in int lives)
        {
            Lives = lives;
        }
    }
}