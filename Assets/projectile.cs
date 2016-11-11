using UnityEngine;
using System.Collections;
using System;

public class projectile : MonoBehaviour {

    public float speed;
    public int damage;
    private Rigidbody body;
    private bool _disabled = false;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionStay(Collision collision)
    {
        // Get the collideing game object
        GameObject gameObject = collision.collider.gameObject;

        // Get projectile componenet
        zombie z = gameObject.GetComponent<zombie>();

        if (z && !_disabled)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            body.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            _disabled = true;
        }

    }

    public bool isDisabled()
    {
        return _disabled;
    }
}
