using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour {

    public int numberOfZombies;
    public GameObject ZombieObject;

    private float _width;
    private float _length;
    private float _x;
    private float _z;
    private int _count = 0;

	// Use this for initialization
	void Start () {
        _x = transform.position.x;
        _z = transform.position.z;
        

        _width = transform.localScale.x*0.9f;
        _length = transform.localScale.z*0.9f;

        Debug.Log(_width);

        InvokeRepeating("spawnZombie", 5.0f, 3.0f);
    }
	
	// Update is called once per frame
	void Update () {
        

        //if (_count >= numberOfZombies) CancelInvoke();



    }

    void spawnZombie()
    {
        
        // Get random number from width
        var randWidth = Random.Range(-_width, _width);

        // Get random number from length
        var randLength = Random.Range(-_length, _length);

        // Instanciate a zombie
        var zombie = Instantiate(ZombieObject).GetComponent<zombie>();

        // Set position to
        //  x: x + randWith,
        //  y: 0.0f,
        //  z: z + randHeight
        zombie.transform.position = new Vector3(_x + randWidth/2.0f, 1.6f, _z + randLength/2.0f);

        _count++;
    }
}
