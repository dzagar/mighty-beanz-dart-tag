using UnityEngine;

public class PlayerController : Player {

    public float speed;

    Vector3 curPos;
    Vector3 lastPos;

	// Use this for initialization
	void Start () {
        score = 0;
        origPos = transform.position;
        it = true;
        anim = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
        if (it && Time.timeScale == 1) {
            score += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && it)
        {
            Fire();
        }

        // If player has not moved in the last frame, stop animation.
        if (curPos == lastPos) {
            anim.Stop();
        }
        else {
            anim.Play();
        }
    }

    void FixedUpdate()
    {
        // Move player based on WASD input.
        curPos = transform.position;
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        transform.Rotate(0, x * 5.0f, 0); // Rotate with left/right.
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, z * 20.0f)); // Translate with up/down.
        lastPos = curPos;
    }
}
