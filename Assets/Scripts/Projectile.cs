using UnityEngine;

public class Projectile : MonoBehaviour {

    public Transform shooter;
    public GameObject controller;

	// Use this for initialization
	void Start () {
        controller = GameObject.FindGameObjectWithTag("GameController");
	}

    void OnCollisionEnter(Collision collision)
    {
        // Send HitEnemy message to shooter if player is hit.
        if (collision.gameObject.tag == "Player" && collision.gameObject.transform != shooter && shooter.GetComponent<Player>().it) {
            shooter.SendMessage("HitEnemy");
        }
    }
}
