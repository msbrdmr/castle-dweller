using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    public KeyType keyType;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();

        if (keyType == KeyType.Red)
        {
            _renderer.material.color = Color.red;
        }
        else if (keyType == KeyType.Blue)
        {
            _renderer.material.color = Color.blue;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, 90 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectKey();
            Destroy(gameObject);
        }
    }

    private void CollectKey()
    {
        var levelManager = FindObjectOfType<LevelManager>();
        levelManager.CollectKey(keyType);
        Destroy(gameObject, 0.1f);
    }
}
