using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class zombie : MonoBehaviour {

    public GameObject target;
    public float speed;
    public int hp;
    public Text uiCounter;

    private Rigidbody thisBody;
    private Rigidbody targetBody;
    private HealthBar healthBar;
    private static int count = 0;

    // Use this for initialization
    void Start () {
        thisBody = GetComponent<Rigidbody>();
        targetBody = target.GetComponent<Rigidbody>();
        healthBar = transform.FindChild("HealthBar").gameObject.GetComponent("HealthBar") as HealthBar;
        Debug.Log(healthBar);
        healthBar.setWidth(hp);
        uiCounter.text = "" + count.ToString();
    }

    // Update is called once per frame
    void Update () {

        // Get the target position
        var targetPosition = targetBody.transform.position;
        var selfPosition = thisBody.transform.position;
        
        // Move towards it
        var directionVector = targetPosition - selfPosition;
        var normDirectionVector = directionVector.normalized;
        normDirectionVector.y = 0.0f;

        // Set direction
        thisBody.transform.LookAt(targetPosition);
        thisBody.velocity = normDirectionVector * speed;
        
	}

    bool isDead()
    {
        return hp <= 0;
    }

    void OnCollisionStay(Collision collision)
    {
        // Get the collideing game objectsssssdw
        GameObject gameObject = collision.collider.gameObject;

        // Get projectile componenet
        projectile p = gameObject.GetComponent<projectile>();

        // If colliding game object is an projectile
        if (p && !p.isDisabled())
        {
            Debug.Log("hitt");

            // Get the damage of the projectile
            int damage = p.damage;

            // Reduce the zombies hp
            hp -= damage;

            // Update the health bar
            healthBar.setWidth(hp);

            // If zombie is dead
            if (isDead())
            {
                Debug.Log("Zombie died");

                // Destroy it
                Destroy(this.gameObject);

                // Increment the counter
            
                Debug.Log(count);
                count++;
                uiCounter.text = "" + count.ToString();
                
                Debug.Log(count);
                Debug.Log(uiCounter.text);

            }

   
        }

    }
}
