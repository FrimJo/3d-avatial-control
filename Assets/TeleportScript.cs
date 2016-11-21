using UnityEngine;
using System.Collections;

public class TeleportScript : MonoBehaviour {

    public GameObject dude;
    public GameObject disc;
    public GameObject cameraHouse;
    public float mouseSens = 1.0f;

    private Renderer discRenderer;
    private float discStartPosition;
    private Quaternion discStartRotation;
    private Vector3 discStartSize;
    private Vector3 target;
    private Transform targetTransform;
    private movement _movement;
    private Quaternion _defaultQuat;

    public Quaternion defaultQuat {
        get {
            return _defaultQuat;
        }
    }

    private bool newControl {
        get
        {
            return _movement.newControl;
        }
    }

	// Use this for initialization
	void Start()
    {
        _movement = dude.GetComponent<movement>();
        discRenderer = disc.GetComponent<Renderer>();
        discRenderer.enabled = false;
        discStartPosition = disc.transform.position.y;
        discStartSize = disc.transform.localScale;

        var leftEyeAnchor = this.gameObject.transform.GetChild(0).GetChild(0);
        _defaultQuat = Quaternion.Inverse(leftEyeAnchor.rotation);
        
        // Set default target
        target = this.transform.position;
        lookAtPlayer();
        
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (!newControl)
        {
            // If parent isn't set
            if (!transform.parent)
            {
                // add the camera to the dude
                transform.SetParent(cameraHouse.transform);

            }

            // Rotate the camera according to mouse
            var mouseY = Input.GetAxis("Mouse Y");

            var angle = transform.parent.rotation.eulerAngles.x + (mouseY * 1000.0f * Time.deltaTime);

            //if (angle < 0 || angle > -90)
            //{
            // Perform rotation
            var q = transform.parent.rotation;
            transform.parent.rotation = Quaternion.Euler(angle, q.eulerAngles.y, q.eulerAngles.z);

            //}
        }
        else
        {

            // If we have a parent set
            if (transform.parent) {
                transform.SetParent(null);
            }

            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                updateTarget();
            }
        }

    }

    void Update()
    {

        if (!newControl)
        {
            discRenderer.enabled = false;
            //lookAtPlayer();
            // Make the camera look at dude
            transform.LookAt(dude.transform);

        }
        else if (Input.GetMouseButtonDown(1))
        {
            discRenderer.enabled = true;
            lookAtPlayer();
            updateTarget();
        }
        

       
    }

    private void lookAtPlayer()
    {
        this.transform.position = new Vector3(target.x, this.transform.position.y, target.z);
        var lookAt = dude.transform.position;
        lookAt.y = this.transform.position.y;
        this.transform.LookAt(lookAt);
        this.transform.rotation = this.transform.rotation * _defaultQuat;
     
    }

    void updateTarget()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            target = hit.point;
            targetTransform = hit.transform;
            discRenderer.enabled = true;
            moveDisc();
        }
        else
        {
            discRenderer.enabled = false;
        }
    }

    void moveDisc()
    {
        if (target.y > discStartPosition)
        {
            disc.transform.localScale = new Vector3(.5f, 0.01f, .5f);
            disc.transform.rotation = targetTransform.rotation;
            disc.transform.up = targetTransform.forward;



        } else
        {
            disc.transform.localScale = discStartSize;
            disc.transform.rotation = discStartRotation;
            disc.transform.position = new Vector3(disc.transform.position.x, discStartPosition, disc.transform.position.z);
        }

        disc.transform.position = new Vector3(target.x, target.y/*disc.transform.position.y*/, target.z);

    }
}
