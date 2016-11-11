using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {

    public float speed;
    public Transform cam;
    public GameObject projectile;
    public int rateOfFire;
    
    public bool grounded;
    public float maxVelocityChange;
    public float jumpHeight;

    private Vector3 lookTarget;
    private Vector3 shootTarget;
    private Rigidbody rigidbody;
    private float timer;


    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        timer = Time.realtimeSinceStartup;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (grounded)
        {

            var targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
           
            targetVelocity = cam.transform.TransformDirection(targetVelocity);
            targetVelocity *= speed;
            var velocityChange = (targetVelocity - rigidbody.velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            lookTarget = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            shootTarget = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        
        rigidbody.transform.LookAt(lookTarget);
        grounded = false;

    }

    void shoot()
    {
        var position = transform.position;
        var direction = transform.forward;
        var test = shootTarget - position;
        test.Normalize();

        projectile p = Instantiate(projectile).GetComponent<projectile>();

        // Set start position
        p.transform.position = position;// + direction;

        // Set travel direction
        p.transform.forward = direction;

        Rigidbody pBody = p.GetComponent<Rigidbody>();
        pBody.velocity = new Vector3(.0f, .0f, .0f);

        pBody.velocity = test * p.speed;
    }

    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Debug.Log("Jump!");
            //rigidbody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.VelocityChange);
            rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
           
            //transform.Translate(Vector3.up * jumpHeight * Time.deltaTime, Space.World);
        }

        
        if (Input.GetMouseButton(1) || Input.GetMouseButtonDown(1))
        {
            
            Debug.Log(Time.realtimeSinceStartup - timer);
            Debug.Log(Time.realtimeSinceStartup);
            
            // If enough time pasted sice last shoot
            if (Time.realtimeSinceStartup - timer >= 1.0f/rateOfFire)
            {
                Debug.Log("shoot!");
                timer = Time.realtimeSinceStartup;
                shoot();
            }
            
        }
    }

    bool isTouchingGround()
    {
        return grounded; // transform.position.y <= initPosY; //Physics.Raycast(transform.position + Vector3.left * 0.3, -Vector3.up, distToGround)
    }

    void OnCollisionStay(Collision collision){
        
        if (collision.transform.tag != "Not Ground") {
            grounded = true;
        }

        // Get the collideing game object
        GameObject gameObject = collision.collider.gameObject;

        // Get projectile componenet
        zombie z = gameObject.GetComponent<zombie>();

        // If colliding game object is an projectile
        if (z)
        {
            //rigidbody.AddForce(new Vector3(0.0f, 1.0f, 0.0f));
            //z.transform.forward
            // Destroy it
            //Destroy(this.gameObject);

        }
    }


}
