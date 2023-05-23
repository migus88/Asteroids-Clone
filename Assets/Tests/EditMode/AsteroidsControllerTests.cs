using System.Threading.Tasks;
using Migs.Asteroids.Game.Data;
using Migs.Asteroids.Game.Logic.Controllers;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    [TestFixture]
    public class AsteroidsControllerTests
    {
        private Mock<IAsteroidsService> _asteroidsServiceMock;
        private Mock<IAsteroidsSettings> _asteroidsSettingsMock;
        private Mock<ISpaceNavigationService> _spaceNavigationServiceMock;
        private Mock<IScoreService> _scoreServiceMock;
        private Mock<ISoundService> _soundServiceMock;
        private Mock<IAsteroid> _asteroidMock;
        private AsteroidsController _asteroidsController;

        [SetUp]
        public void SetUp()
        {
            _asteroidsServiceMock = new Mock<IAsteroidsService>();
            _asteroidsSettingsMock = new Mock<IAsteroidsSettings>();
            _spaceNavigationServiceMock = new Mock<ISpaceNavigationService>();
            _scoreServiceMock = new Mock<IScoreService>();
            _soundServiceMock = new Mock<ISoundService>();
            _asteroidMock = new Mock<IAsteroid>();

            _asteroidsController = new AsteroidsController(_asteroidsServiceMock.Object, _asteroidsSettingsMock.Object,
                _spaceNavigationServiceMock.Object, _scoreServiceMock.Object, _soundServiceMock.Object);
        }

        [Test]
        public void Reset_ReleasesAllAsteroids()
        {
            _asteroidsSettingsMock.Setup(a => a.AsteroidLevels).Returns(new AsteroidData[] { new() });
            _asteroidsServiceMock.Setup(a => a.GetObject()).Returns(_asteroidMock.Object);
            _asteroidsController.SpawnAsteroid(0, Vector3.zero, Quaternion.identity);

            _asteroidsController.Reset();

            _asteroidsServiceMock.Verify(a => a.ReturnObject(It.IsAny<IAsteroid>()), Times.Once);
            Assert.AreEqual(0, _asteroidsController.SpawnedAsteroids);
        }

        [Test]
        public void SpawnAsteroid_IncreasesCountOfAsteroids()
        {
            _asteroidsSettingsMock.Setup(a => a.AsteroidLevels).Returns(new AsteroidData[] { new() });
            _asteroidsServiceMock.Setup(a => a.GetObject()).Returns(_asteroidMock.Object);
            _asteroidsServiceMock.Setup(a => a.GetObject()).Returns(_asteroidMock.Object);

            _asteroidsController.SpawnAsteroid(0, Vector3.zero, Quaternion.identity);

            Assert.AreEqual(1, _asteroidsController.SpawnedAsteroids);
        }

        [Test]
        public void Tick_WrapsAllAsteroids()
        {
            _asteroidsSettingsMock.Setup(a => a.AsteroidLevels).Returns(new AsteroidData[] { new() });
            _asteroidsServiceMock.Setup(a => a.GetObject()).Returns(_asteroidMock.Object);
            _asteroidsController.SpawnAsteroid(0, Vector3.zero, Quaternion.identity);

            _asteroidsController.Tick();

            _spaceNavigationServiceMock.Verify(s => s.WrapAroundGameArea(It.IsAny<ISpaceEntity>()), Times.Once);
        }
        
        [Test]
        public void SpawnAsteroid_CreatesNewAsteroid()
        {
            _asteroidsSettingsMock.Setup(a => a.AsteroidLevels).Returns(new AsteroidData[] { new() });
            _asteroidsServiceMock.Setup(a => a.GetObject()).Returns(_asteroidMock.Object);
            var initialSpawnedAsteroids = _asteroidsController.SpawnedAsteroids;

            _asteroidsController.SpawnAsteroid(0, Vector3.zero, Quaternion.identity);

            Assert.AreEqual(initialSpawnedAsteroids + 1, _asteroidsController.SpawnedAsteroids);
        }
        
        [Test]
        public async Task OnAsteroidCollision_DestroysAsteroid()
        {
            // Arrange
            _asteroidsSettingsMock.Setup(a => a.AsteroidLevels).Returns(new AsteroidData[] { new() });
            _asteroidsServiceMock.Setup(a => a.GetObject()).Returns(_asteroidMock.Object);
            _asteroidsController.SpawnAsteroid(0, Vector3.zero, Quaternion.identity);
            var initialSpawnedAsteroids = _asteroidsController.SpawnedAsteroids;
    
            // Act
            _asteroidMock.Raise(a => a.Collided += null, _asteroidMock.Object);

            // Assert
            Assert.AreEqual(initialSpawnedAsteroids - 1, _asteroidsController.SpawnedAsteroids);
            _asteroidMock.Verify(a => a.Explode(), Times.Once);
            _scoreServiceMock.Verify(s => s.AddScore(It.IsAny<int>()), Times.Once);
        }
    }
}