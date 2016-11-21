using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {

    public float speed;
    public Transform cam;
    public GameObject projectile;
    public int rateOfFire;
    public float mouseSens = 1000000.0f;


    public bool grounded;
    public float maxVelocityChange;
    public float jumpHeight;

    private Vector3 lookTarget;
    private Vector3 shootTarget;
    private Rigidbody rigidbody;
    private float timer;
    private bool _newControl = true;

    public bool newControl
    {
        get
        {
            return _newControl;
        }
        set
        {
            _newControl = value;
            //Cursor.visible = _newControl;

            // Set the position of the camera
            if (!_newControl)
            {
                // Set the cam behind teh avatar
                var p = transform.position;

                var dist = Vector3.Distance(cam.transform.position, transform.position);
                dist = dist > 20.0f ? 20.0f : dist;
                dist = dist < 0.0f ? 0.0f : dist;
                
                cam.transform.position = new Vector3(p.x, p.y + (20.0f - dist) + 5.0f, p.z - (20.0f - dist) + 5.0f);

                // Rotate the avatar away from the camera
                transform.forward = cam.forward;
                transform.rotation = new Quaternion(0.0f, transform.rotation.y, 0.0f, transform.rotation.w);

                
            }

            
        }
    }

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        rigidbody = GetComponent<Rigidbody>();
        timer = Time.realtimeSinceStartup;
    }

	
	// Update is called once per frame
	void FixedUpdate () {

        

        if (grounded)
        {

            var targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //if (newControl){
                targetVelocity = cam.transform.TransformDirection(targetVelocity);
            //}

            targetVelocity *= speed;
            var velocityChange = (targetVelocity - rigidbody.velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        }

        if (newControl)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                lookTarget = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                shootTarget = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
        
            rigidbody.transform.LookAt(lookTarget);
            
        }
        else
        {
            // Rotate the dude according to mouse
            var mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up * mouseX * 1000.0f * Time.deltaTime);
        }
        grounded = false;

    }

    void shoot() { 


        var position = transform.position;
        var forward = transform.forward;
        Vector3 direction;

        if (newControl)
        {

            direction = shootTarget - position;
            direction.Normalize();

            
        } else
        {
            direction = transform.forward;
        }

        projectile p = Instantiate(projectile).GetComponent<projectile>();

        // Set start position
        p.transform.position = position;// + direction;

        // Set travel direction
        p.transform.forward = forward;

        Rigidbody pBody = p.GetComponent<Rigidbody>();
        pBody.velocity = new Vector3(.0f, .0f, .0f);

        pBody.velocity = direction * p.speed;

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

        
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
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

        if (Input.GetKeyDown(KeyCode.T))
        {
            newControl = !newControl;
            Debug.Log("Controll toggled: " + newControl);
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
