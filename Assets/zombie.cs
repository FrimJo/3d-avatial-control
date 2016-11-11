using UnityEngine;
using System.Collections;

public class zombie : MonoBehaviour {

    public GameObject target;
    public float speed;
    public int hp;

    private Rigidbody thisBody;
    private Rigidbody targetBody;
    private HealthBar healthBar;

    // Use this for initialization
    void Start () {
        thisBody = GetComponent<Rigidbody>();
        targetBody = target.GetComponent<Rigidbody>();
        healthBar = transform.FindChild("HealthBar").gameObject.GetComponent("HealthBar") as HealthBar;
        Debug.Log(healthBar);
        healthBar.setWidth(hp);
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
        //Debug.Log(normDirectionVector.ToString());


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
        // Get the collideing game object
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
                // Destroy it
                Destroy(this.gameObject);
            }

   
        }

    }
}
