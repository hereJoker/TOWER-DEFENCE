using NUnit.Framework;
using UnityEngine;

namespace _Project.Scripts.ObjectPlacer.Editor
{
    public class ObjectPlacerTests
    {
        [Test]
        public void GIVEN_PLACER__ADD_OBJECT__SHOULD_ADD()
        {
            // Given
            var placer = new ObjectBoundsPlacer(new Bounds(Vector3.zero, Vector3.one));
            var p1 = new DummyPlaceable();
            p1.Position = Vector3.zero;
            p1.Bounds = new Bounds(Vector3.zero, Vector3.one * 0.1f);
            
            // Act
            placer.Place(p1);
            
            // Should
            var isAble = placer.IsAbleToPlace(p1.Position);
            Assert.IsFalse(isAble);
        }
        
        [Test]
        public void GIVEN_PLACER__ADD_2_OBJECTS__SHOULD_ADD()
        {
            // Given
            var placer = new ObjectBoundsPlacer(new Bounds(Vector3.zero, Vector3.one * 20f));
            var p1 = new DummyPlaceable();
            p1.Position = Vector3.zero;
            p1.Bounds = new Bounds(Vector3.zero, Vector3.one * 0.1f);
            
            var p2 = new DummyPlaceable();
            p2.Position = Vector3.one * 2;
            p2.Bounds = new Bounds(Vector3.one * 2, Vector3.one * 0.1f);
            
            // Act
            Assert.IsTrue(placer.Place(p1));
            Assert.IsTrue(placer.Place(p2));
            
            // Should
            var isAble = placer.IsAbleToPlace(p1.Position);
            Assert.IsFalse(isAble);
            isAble = placer.IsAbleToPlace(p2.Position);
            Assert.IsFalse(isAble);
        }

        [Test]
        public void GIVEN_PLACER__ADD__OBJECT_OUTOFBOUNDS__SHOULD_NOT_ADD()
        {
            // Given
            var placer = new ObjectBoundsPlacer(new Bounds(Vector3.zero, Vector3.one));
            var p1 = new DummyPlaceable();
            p1.Position = Vector3.one * 30000f;
            p1.Bounds = new Bounds(Vector3.one * 30000f, Vector3.one * 0.1f);
            
            // Act
            var isPlace = placer.Place(p1);
            
            // Should
            Assert.IsFalse(isPlace);
        }
    }


    internal class DummyPlaceable : IPlaceable
    {
        public Vector3 Position { get; set; }
        public Bounds Bounds { get; set; }
    }
}