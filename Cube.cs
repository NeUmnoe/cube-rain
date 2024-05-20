using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private IObjectPool<Cube> _pool;
    private HashSet<Platform> _collidedPlatforms;
    private static readonly float _minLifetime = 2f;
    private static readonly float _maxLifetime = 5f;

    public void SetPool(IObjectPool<Cube> objectPool)
    {
        _pool = objectPool;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collidedPlatforms = new HashSet<Platform>();
    }

    private void OnEnable()
    {
        _collidedPlatforms.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Platform platform = collision.gameObject.GetComponent<Platform>();
        if (platform != null && !_collidedPlatforms.Contains(platform))
        {
            _collidedPlatforms.Add(platform);
            ChangeColor();
            float lifetime = Random.Range(_minLifetime, _maxLifetime);
            StartCoroutine(DestroyAfterTime(lifetime));
        }
    }

    private void ChangeColor()
    {
        _renderer.material.color = Random.ColorHSV();
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        _pool.Release(this);
    }
}