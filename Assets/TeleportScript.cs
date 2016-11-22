using UnityEngine;
using System.Collections;

public class TeleportScript : MonoBehaviour {

    public GameObject dude;
    public GameObject disc;
    
    public float mouseSens = 1.0f;

    private Renderer discRenderer;
    private float discStartPosition;
    private Quaternion discStartRotation;
    private Vector3 discStartSize;
    private Vector3 target;
    private Transform targetTransform;
    private movement _movement;
    private bool quatIsSet = false;
    private Quaternion _defaultQuat;

    public Quaternion defaultQuat {
        get {

            if(!quatIsSet)
            {
                var leftEyeAnchor = this.gameObject.transform.GetChild(0).GetChild(0);
                _defaultQuat = Quaternion.Inverse(leftEyeAnchor.rotation);

                quatIsSet = true;
            }
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

        // Set default target
        target = this.transform.position;
        lookAtPlayer();
        
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (newControl)
        {
            updateTarget();
        }

    }

    void Update()
    {

        if (!newControl)
        {
            discRenderer.enabled = false;
            //lookAtPlayer();
            // Make the camera look at dude
            //transform.LookAt(dude.transform);

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
        this.transform.rotation = this.transform.rotation * defaultQuat;
     
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
            var meshRenderer = disc.GetComponent<MeshRenderer>();
            meshRenderer.material.color = new Color(1.0f, .0f, .0f);
            //meshRenderer.material.color = new Color(0.8897059f, 0.2355104f, 0.2355104f);


        }
        else
        {
            disc.transform.localScale = discStartSize;
            disc.transform.rotation = discStartRotation;
            disc.transform.position = new Vector3(disc.transform.position.x, discStartPosition, disc.transform.position.z);
            var meshRenderer = disc.GetComponent<MeshRenderer>();
            meshRenderer.material.color = new Color(0.2470588f, 0.3176471f, 0.7098039f);
        }

        disc.transform.position = new Vector3(target.x, target.y/*disc.transform.position.y*/, target.z);

    }
}
