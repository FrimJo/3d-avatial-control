using UnityEngine;
using System.Collections;

public class TeleportScript : MonoBehaviour {

    public GameObject dude;
    public GameObject disc;

    private Renderer discRenderer;
    private Vector3 target;
	// Use this for initialization
	void Start () {
        discRenderer = disc.GetComponent<Renderer>();
        discRenderer.enabled = false;
        target = transform.position;
        lookAtPlayer();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) {
            updateTarget();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lookAtPlayer();
            updateTarget();
        }
    }

    private void lookAtPlayer()
    {
        this.transform.position = new Vector3(target.x, this.transform.position.y, target.z);
        this.transform.LookAt(dude.transform.position);
    }

    void updateTarget()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            target = hit.point;
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
        disc.transform.position = new Vector3(target.x, disc.transform.position.y, target.z);
    }
}
