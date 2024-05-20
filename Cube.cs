using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private HashSet<Platform> _collidedPlatforms;
    private static readonly float _minLifetime = 2f;
    private static readonly float _maxLifetime = 5f;

    public event Action<Cube> OnCubeDestroyed;

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
        if (collision.gameObject.TryGetComponent(out Platform platform))
        {
            if (!_collidedPlatforms.Contains(platform))
            {
                _collidedPlatforms.Add(platform);
                ChangeColor();
                float lifetime = UnityEngine.Random.Range(_minLifetime, _maxLifetime);
                StartCoroutine(DestroyAfterTime(lifetime));
            }
        }
    }

    private void ChangeColor()
    {
        _renderer.material.color = UnityEngine.Random.ColorHSV();
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        OnCubeDestroyed?.Invoke(this);
    }
}