using System;
using UnityEngine;

namespace TestGame
{
    public class ParticleStopEventMediator : MonoBehaviour
    {
        public event Action OnExplosionEffectStop;

        private void OnParticleSystemStopped()
        {
            OnExplosionEffectStop?.Invoke();
        }

        public void Reset()
        {
            OnExplosionEffectStop = null;
        }
    }
}