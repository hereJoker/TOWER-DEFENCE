using NUnit.Framework;

namespace _Project.Scripts.Gameplay.Editor
{
    public class WinLoseCheckerTest
    {
        [Test]
        public void GIVEN_GAMEPLAYSTATE__DECREASE_LIVES__SHOULD_LOSE()
        {

            // Given
            var listener = new DummyWinLoseListener();
            var checker = new WinLoseChecker(1);
            checker.RegisterListener(listener);

            // Act
            checker.OnFinishMovable(null);

            // Should
            Assert.IsTrue(listener.IsLose);
            Assert.IsFalse(listener.IsWin);
        }

        [Test]
        public void GIVEN_GAMEPLAYSTATE__FINISH_WAVES__SHOULD_WIN()
        {

            // Given
            var listener = new DummyWinLoseListener();
            var checker = new WinLoseChecker(1);
            checker.RegisterListener(listener);

            // Act
            checker.OnFinishAllWaves();

            // Should
            Assert.IsTrue(listener.IsWin);
            Assert.IsFalse(listener.IsLose);
        }

        [Test]
        public void GIVEN_GAMEPLAYSTATE__DECREASE_ONE_LIVE__SHOULD_NOT_END_OF_LEVEL_AND_LIVES_CORRECT()
        {

            // Given
            var listener = new DummyWinLoseListener();
            var checker = new WinLoseChecker(20);
            checker.RegisterListener(listener);

            // Act
            checker.OnFinishMovable(null);

            // Should
            Assert.IsFalse(listener.IsLose);
            Assert.IsFalse(listener.IsWin);
            Assert.AreEqual(19, listener.Lives);
        }
    }
}