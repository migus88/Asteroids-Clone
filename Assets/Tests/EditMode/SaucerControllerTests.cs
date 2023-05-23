using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Logic.Controllers;
using Migs.Asteroids.Game.Logic.Interfaces.Controllers;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using Tests.EditMode.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using Assert = NUnit.Framework.Assert;

namespace Tests.EditMode
{
    [TestFixture]
    public class SaucerControllerTests
    {
        private Mock<ISaucerService> _saucerServiceMock;
        private Mock<ISaucerSettings> _saucerSettingsMock;
        private Mock<IPlayer> _playerMock;
        private Mock<IScoreService> _scoreServiceMock;
        private Mock<ISpaceNavigationService> _spaceNavigationServiceMock;
        private Mock<IProjectilesService> _projectilesServiceMock;
        private Mock<IProjectilesSettings> _projectilesSettingsMock;
        private Mock<ISoundService> _soundServiceMock;
        private SaucerController _saucerController;

        [SetUp]
        public void SetUp()
        {
            _saucerServiceMock = new Mock<ISaucerService>();
            _saucerSettingsMock = new Mock<ISaucerSettings>();
            _playerMock = new Mock<IPlayer>();
            _scoreServiceMock = new Mock<IScoreService>();
            _spaceNavigationServiceMock = new Mock<ISpaceNavigationService>();
            _projectilesServiceMock = new Mock<IProjectilesService>();
            _projectilesSettingsMock = new Mock<IProjectilesSettings>();
            _soundServiceMock = new Mock<ISoundService>();

            _saucerController = new SaucerController(
                _saucerServiceMock.Object,
                _saucerSettingsMock.Object,
                _playerMock.Object,
                _scoreServiceMock.Object,
                _spaceNavigationServiceMock.Object,
                _projectilesServiceMock.Object,
                _projectilesSettingsMock.Object,
                _soundServiceMock.Object
            );
        }

        [Test]
        public async Task Init_PreloadsCorrectNumberOfSaucers()
        {
            _saucerSettingsMock.Setup(s => s.MaxSaucersOnScreen).Returns(5);

            await _saucerController.Init();

            _saucerServiceMock.Verify(s => s.Preload(5), Times.Once);
        }

        [Test]
        public void Enable_SetsIsEnabledToTrue()
        {
            _saucerController.Enable();

            var value = ReflectionUtils.GetPrivateFieldValue<SaucerController, bool>("_isEnabled", _saucerController);

            Assert.IsTrue(value);
        }

        [Test]
        public void Disable_SetsIsEnabledToFalse()
        {
            _saucerController.Disable();

            var value = ReflectionUtils.GetPrivateFieldValue<SaucerController, bool>("_isEnabled", _saucerController);

            Assert.IsFalse(value);
        }

        [Test]
        public void Reset_DisablesAndReleasesSaucer()
        {
            _saucerController.Enable();

            var saucerMock = new Mock<ISaucer>();
            _saucerServiceMock.Setup(s => s.GetObject()).Returns(saucerMock.Object);
            ReflectionUtils.SetPrivateFieldValue("_saucer", saucerMock.Object, _saucerController);

            _saucerController.Reset();

            _saucerServiceMock.Verify(s => s.ReturnObject(It.IsAny<ISaucer>()), Times.Once);

            var value = ReflectionUtils.GetPrivateFieldValue<SaucerController, bool>("_isEnabled", _saucerController);

            Assert.IsFalse(value);
        }

        [Test]
        public void Tick_WhenDisabled_PerformsNoOperations()
        {
            _saucerController.Disable();

            _saucerController.Tick();

            _saucerServiceMock.Verify(s => s.Preload(It.IsAny<int>()), Times.Never);
            _spaceNavigationServiceMock.Verify(s => s.IsObjectOutOfGameArea(It.IsAny<ISpaceEntity>(), It.IsAny<float>()), Times.Never);
        }

        [Test]
        public void Tick_WhenSaucerIsNull_IncrementsTimeSinceLastSaucer()
        {
            _saucerController.Enable();
            var deltaTime = Time.deltaTime;

            var initialTime = ReflectionUtils.GetPrivateFieldValue<SaucerController, float>("_timeSinceLastSaucer", _saucerController);
            
            _saucerController.Tick();

            var afterTime = ReflectionUtils.GetPrivateFieldValue<SaucerController, float>("_timeSinceLastSaucer", _saucerController);
            Assert.AreEqual(initialTime + deltaTime, afterTime);
        }
    }
}