using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    public GameObject lookAt;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(lookAt.transform.position);
	}


    public void setWidth(float with)
    {
        var y = transform.localScale.y;
        var z = transform.localScale.z;
        transform.localScale = new Vector3(with/100, y, z);
    }

    public void setParent(Transform parent)
    {
        transform.parent = parent;
        //setParent(parent);
        transform.position = new Vector3(0.0f, 1.7f, 0.0f);
    }

}
