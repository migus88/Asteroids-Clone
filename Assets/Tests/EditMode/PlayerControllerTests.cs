using System.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Services.Interfaces;
using Migs.Asteroids.Game.Logic.Controllers;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;
using Migs.Asteroids.Game.Logic.Interfaces.Services;
using Migs.Asteroids.Game.Logic.Interfaces.Settings;
using Moq;
using NUnit.Framework;
using Tests.EditMode.Utils;

namespace Tests.EditMode
{
    public class PlayerControllerTests
    {
        private Mock<IPlayer> _playerMock;
        private Mock<IPlayerInputService> _inputServiceMock;
        private Mock<IPlayerSettings> _playerSettingsMock;
        private Mock<IProjectilesSettings> _projectilesSettingsMock;
        private Mock<ISpaceNavigationService> _spaceNavigationServiceMock;
        private Mock<IProjectilesService> _projectilesServiceMock;
        private Mock<ISoundService> _soundServiceMock;
        private Mock<IGameUiService> _gameUiServiceMock;
        private Mock<ICrossDomainServiceResolver> _crossDomainServiceResolverMock;

        private PlayerController _playerController;

        [SetUp]
        public void SetUp()
        {
            _playerMock = new Mock<IPlayer>();
            _inputServiceMock = new Mock<IPlayerInputService>();
            _playerSettingsMock = new Mock<IPlayerSettings>();
            _projectilesSettingsMock = new Mock<IProjectilesSettings>();
            _spaceNavigationServiceMock = new Mock<ISpaceNavigationService>();
            _projectilesServiceMock = new Mock<IProjectilesService>();
            _soundServiceMock = new Mock<ISoundService>();
            _gameUiServiceMock = new Mock<IGameUiService>();
            _crossDomainServiceResolverMock = new Mock<ICrossDomainServiceResolver>();
            _crossDomainServiceResolverMock.Setup(r => r.ResolveService<IGameUiService>())
                .Returns(_gameUiServiceMock.Object);

            _playerController = new PlayerController(
                _playerMock.Object,
                _inputServiceMock.Object,
                _playerSettingsMock.Object,
                _projectilesSettingsMock.Object,
                _spaceNavigationServiceMock.Object,
                _projectilesServiceMock.Object,
                _crossDomainServiceResolverMock.Object,
                _soundServiceMock.Object);
        }

        [Test]
        public void Shoot_ShootsProjectile_WhenShootingButtonPressedAndFireRateTimePassed()
        {
            _inputServiceMock.Setup(i => i.IsShootingButtonPressStarted).Returns(true);
            _playerSettingsMock.Setup(s => s.FireRate).Returns(0);
            _projectilesServiceMock.Setup(s => s.GetAvailablePlayerProjectile(It.IsAny<int>()))
                .Returns(Mock.Of<IProjectile>());

            _playerController.Enable();
            _playerController.Tick();

            _projectilesServiceMock.Verify(s => s.GetAvailablePlayerProjectile(It.IsAny<int>()), Times.Once);
            _soundServiceMock.Verify(s => s.PlaySpaceshipLaser(), Times.Once);
        }

        [Test]
        public void Tick_RotatesPlayer_WhenRotationButtonIsPressed()
        {
            _inputServiceMock.Setup(i => i.IsRotationButtonPressed).Returns(true);
            _playerSettingsMock.Setup(s => s.RotationSpeed).Returns(1);

            _playerController.Enable();
            _playerController.Tick();

            _playerMock.Verify(p => p.Rotate(It.IsAny<float>(), It.IsAny<float>()), Times.Once);
        }

        [Test]
        public void Tick_TriggersHyperspace_WhenHyperspaceButtonPressed()
        {
            _inputServiceMock.Setup(i => i.IsHyperspaceButtonPressStarted).Returns(true);

            _playerController.Enable();
            _playerController.Tick();

            _playerMock.Verify(p => p.Hide(), Times.Once);
            _playerMock.Verify(p => p.Show(), Times.Once);
        }
        
        [Test]
        public void FixedTick_Thrusts_WhenAccelerationButtonIsPressed()
        {
            ReflectionUtils.SetPrivateFieldValue("_shouldAddForce", true, _playerController);
    
            _playerController.Enable();
            _playerController.FixedTick();

            _playerMock.Verify(p => p.AddForce(It.IsAny<float>()), Times.Once);
        }

        [Test]
        public void MakePlayerImmuneToDamage_SetsDamageImmunity_WhenCalled()
        {
            var durationInSeconds = 1;

            _playerController.MakePlayerImmuneToDamage(durationInSeconds);

            _playerMock.Verify(p => p.SetDamageImmunity(true), Times.Once);
        }

        [Test]
        public void OnCollision_DecrementsLives_WhenPlayerCanBeDestroyed()
        {
            _playerController.Lives = 3;
            var spaceEntityMock = new Mock<ISpaceEntity>();

            ReflectionUtils.SetPrivateFieldValue<PlayerController, bool>("_canBeDestroyed", true, _playerController);
            
            _playerMock.Raise(p => p.Collided += null, spaceEntityMock.Object);

            Assert.AreEqual(2, _playerController.Lives);
            _gameUiServiceMock.Verify(s => s.SetLives(2), Times.Once);
        }

        [Test]
        public void Disable_SetsIsEnabledToFalse_WhenCalled()
        {
            _playerController.Enable();
            var isPlayerControllerEnabled = ReflectionUtils.GetPrivateFieldValue<PlayerController, bool>("_isEnabled", _playerController);
            Assert.IsTrue(isPlayerControllerEnabled);

            _playerController.Disable();
            isPlayerControllerEnabled = ReflectionUtils.GetPrivateFieldValue<PlayerController, bool>("_isEnabled", _playerController);
            Assert.IsFalse(isPlayerControllerEnabled);
        }
    }
}