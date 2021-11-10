using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;

    [SerializeField]
    private GameObject _explosionPrefab;
    

    private SpawnManager _spawnManager;
    //We nwwe to instantiate explosion at the position of asteroid and also animate it.
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager in Asteroid is NULL!");
        }   
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime); // For rotation on z-axis
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.25f);
            _spawnManager.startSpawning(); // ye line execute hotae hi saare enemies and powerups anae lagenegae mtlb humlog yae chahtae thae ki asteroid destroy honae kae baad hamara game chalu ho.
        }
    }
}
