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
    private Transform cameraHouse;
    private float timer;
    private bool _newControl = true;
    private Quaternion _defaultQuat;
    private float _defaultCameraHeight;
    public bool newControl
    {
        get
        {
            return _newControl;
        }

        set
        {
            //var camScript = cam.gameObject.GetComponent<TeleportScript>();
            //_defaultQuat = camScript.defaultQuat;

            _newControl = value;
            
            //Cursor.visible = _newControl;

            // Set the position of the camera
            if (!_newControl)
            {

                // Position the camera behind the avatar
                positionCameraBehindAvatar();

                // Add the camera to the Camera House
                cam.SetParent(cameraHouse);

                //cameraHouse.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);



            } else
            {
                // Remove the camera from the camera house
                cam.SetParent(null);

                // Reset the hight position of the camera
                cam.transform.position = new Vector3(cam.transform.position.x, _defaultCameraHeight, cam.transform.position.z);
            }

            
        }
    }

    public void positionCameraBehindAvatar()
    {
        // Position the camera behind the avatar
        cam.transform.position = transform.position - (transform.forward * 20.0f);// + (transform.up * 20.0f);
        cam.transform.forward = transform.forward;

        // Compensate for the sensor
        cam.transform.rotation = cam.transform.rotation * _defaultQuat;
    }

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        rigidbody = GetComponent<Rigidbody>();
        timer = Time.realtimeSinceStartup;

        var camScript = cam.gameObject.GetComponent<TeleportScript>();
        _defaultQuat = camScript.defaultQuat;
        _defaultCameraHeight = cam.transform.position.y;


        // Set the cameraHouse
        cameraHouse = transform.GetChild(0);
        
    }

	
	// Update is called once per frame
	void FixedUpdate () {

        

        if (grounded)
        {

            var targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (newControl){

                // Compoensate for sensor

                var q = Quaternion.AngleAxis(_defaultQuat.eulerAngles.z, Vector3.up);
                targetVelocity = q * targetVelocity;

                targetVelocity = cam.transform.TransformDirection(targetVelocity);
                
                // Compensate for the sensor
                //cam.transform.rotation = cam.transform.rotation * _defaultQuat;
            }
            else
            {
                targetVelocity = transform.TransformDirection(targetVelocity);

            }

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
            // Rotate the dude according to mouse X
            var mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up * mouseX * 100.0f * Time.deltaTime);

            /* Rotate camera house according to mouse Y */
            var mouseY = Input.GetAxis("Mouse Y");

            // If we have rotation
            if(mouseY != 0)
            {
                // Get current rotation for right
                var calculatedAngle = cameraHouse.rotation.eulerAngles;

                // Calc extra rotation based on sens and time
                var extraAngle = Vector3.right * -mouseY * 100.0f * Time.deltaTime;

                // Add the extra rotation for right
                calculatedAngle += extraAngle;

                // Check to see if it is to bigg or small

                if (calculatedAngle.x > 0.0f && calculatedAngle.x < 75.0f)
                {
                    //Add the rotation to the camera house
                    cameraHouse.rotation = Quaternion.Euler(calculatedAngle.x, calculatedAngle.y, calculatedAngle.z);
                }
            }

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
            
            rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
           
        }

        
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            
            
            // If enough time pasted sice last shoot
            if (Time.realtimeSinceStartup - timer >= 1.0f/rateOfFire)
            {

                timer = Time.realtimeSinceStartup;
                shoot();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            newControl = !newControl;

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
