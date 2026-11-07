using System.Collections;
using UnityEngine;

namespace Game.Level
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _infectedMaterial;
        [SerializeField] private ParticleSystem _explosionPrefab;
        [SerializeField] private float _explodeDelay = 0.25f;

        private bool _isInfected;

        public float ExplodeDelay => _explodeDelay;

        public void Infect(float delay)
        {
            if (_isInfected)
                return;

            _isInfected = true;
            StartCoroutine(InfectRoutine(delay));
        }

        private IEnumerator InfectRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            _renderer.sharedMaterial = _infectedMaterial;

            yield return new WaitForSeconds(_explodeDelay);
            Instantiate(_explosionPrefab, transform.position + Vector3.up, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
