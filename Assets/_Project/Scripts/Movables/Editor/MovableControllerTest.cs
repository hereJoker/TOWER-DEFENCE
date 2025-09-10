using NUnit.Framework;

namespace _Project.Scripts.Movables.Editor
{
    public class MovableControllerTest
    {
        [Test]
        public void GIVEN_1_MOVABLE__MOVE_TILL_END__SHOULD_REMOVE_IT()
        {
            // Given
            var listener = new DummyListener();
            var controller = new MovableController(new DummyPath());
            controller.Register(listener);
            var movable = new DummyMovable();
            controller.RegisterMovable(movable);

            // Act
            controller.Update(1.1f);

            // Should
            Assert.AreEqual(movable, listener.Movable[0]);
        }

        [Test]
        public void GIVEN_1_MOVABLE__MOVE_TILL_END__SHOULD_POSITION_BE_LAST_PATH()
        {
            // Given
            var path = new DummyPath();
            var controller = new MovableController(path);
            var movable = new DummyMovable();
            controller.RegisterMovable(movable);

            // Act
            controller.Update(1.1f);

            // Should
            Assert.AreEqual(path.GetPositionFromTime(1.0f), movable.Position);
        }

        [Test]
        public void GIVEN_1_MOVABLE__MOVE_TILL_END__SHOULD_BE_FINISHED()
        {
            // Given
            var path = new DummyPath();
            var controller = new MovableController(path);
            var movable = new DummyMovable();
            controller.RegisterMovable(movable);

            // Act
            controller.Update(1.1f);

            // Should
            Assert.IsTrue(movable.IsFinish);
        }

        [Test]
        public void GIVEN_1_MOVABLE__MOVE_TILL_HALF__SHOULD_HALF_TILL_END()
        {
            // Given
            var path = new DummyPath();
            var controller = new MovableController(path);
            var movable = new DummyMovable();
            controller.RegisterMovable(movable);

            // Act
            controller.Update(0.5f);

            // Should
            Assert.AreEqual(path.GetPositionFromTime(0.5f), movable.Position);
        }

        [Test]
        public void GIVEN_1_MOVABLE__MOVE_TILL_HALF__SHOULD_NOT_BE_REMOVED()
        {
            // Given
            var listener = new DummyListener();
            var path = new DummyPath();
            var controller = new MovableController(path);
            controller.Register(listener);
            var movable = new DummyMovable();
            controller.RegisterMovable(movable);

            // Act
            controller.Update(0.5f);

            // Should
            Assert.AreEqual(0, listener.Movable.Count);
        }

        [Test]
        public void GIVEN_3_MOVABLE__MOVE_ONE_TILL_END__SHOULD_REMOVE_ONLY_ONE()
        {
            // Given
            var listener = new DummyListener();
            var path = new DummyPath();
            var controller = new MovableController(path);
            controller.Register(listener);
            controller.RegisterMovable(new DummyMovable
            {
                Speed = 0.01f
            });
            controller.RegisterMovable(new DummyMovable
            {
                Speed = 0.01f
            });
            controller.RegisterMovable(new DummyMovable());

            // Act
            controller.Update(0.4f);
            controller.Update(0.4f);
            controller.Update(0.4f);

            // Should
            Assert.AreEqual(1, listener.Movable.Count);
        }

        [Test]
        public void GIVEN_1_MOVABLE_WITH_SLOW_FACTOR__MOVE_TILL_END__SHOULD_NOT_BE_REMOVED()
        {
            // Given
            var listener = new DummyListener();
            var path = new DummyPath();
            var controller = new MovableController(path);
            controller.Register(listener);
            var movable = new DummyMovable();
            movable.SlowDownFactor = 0.5f;
            controller.RegisterMovable(movable);

            // Act
            controller.Update(0.1f);

            // Should
            Assert.AreEqual(0, listener.Movable.Count);
        }
    }

}