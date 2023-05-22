using Migs.Asteroids.Game.Logic.Interfaces.Services;
using UnityEngine;

namespace Migs.Asteroids.Game.View.Services
{
    public class SoundService : MonoBehaviour, ISoundService
    {
        [Header("Spaceship")]
        [SerializeField] private AudioSource _spaceshipThrustersSource;
        [SerializeField] private AudioClip _spaceshipLaser;
        [SerializeField] private AudioClip _spaceshipExplosion;
        
        [Header("Saucer")]
        [SerializeField] private AudioSource _saucerThrustersSource;
        [SerializeField] private AudioClip _saucerLaser;
        [SerializeField] private AudioClip _saucerExplosion;

        [Header("Asteroids")]
        [SerializeField] private AudioClip _asteroidExplosion;

        public void PlaySpaceshipThrusters()
        {
            if (_spaceshipThrustersSource.isPlaying)
            {
                return;
            }
            
            _spaceshipThrustersSource.Stop();
            _spaceshipThrustersSource.Play();
        }

        public void StopSpaceshipThrusters()
        {
            if (!_spaceshipThrustersSource.isPlaying)
            {
                return;
            }
            
            _spaceshipThrustersSource.Stop();
        }

        public void PlaySpaceshipLaser()
        {
            AudioSource.PlayClipAtPoint(_spaceshipLaser, Vector3.zero);
        }

        public void PlaySpaceshipExplosion()
        {
            AudioSource.PlayClipAtPoint(_spaceshipExplosion, Vector3.zero);
        }

        public void PlaySaucerThrusters()
        {
            if (_saucerThrustersSource.isPlaying)
            {
                return;
            }
            
            _saucerThrustersSource.Stop();
            _saucerThrustersSource.Play();
        }

        public void StopSaucerThrusters()
        {
            if (!_saucerThrustersSource.isPlaying)
            {
                return;
            }
            
            _saucerThrustersSource.Stop();
        }

        public void PlaySaucerLaser()
        {
            AudioSource.PlayClipAtPoint(_saucerLaser, Vector3.zero);
        }

        public void PlaySaucerExplosion()
        {
            AudioSource.PlayClipAtPoint(_saucerExplosion, Vector3.zero);
        }

        public void PlayAsteroidExplosion()
        {
            AudioSource.PlayClipAtPoint(_asteroidExplosion, Vector3.zero);
        }
    }
}