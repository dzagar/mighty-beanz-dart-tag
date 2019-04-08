using UnityEngine;

public class Wall : MonoBehaviour {
	
    void OnCollisionEnter (Collision collision) {
        // Destroy projectile if it hits the wall.
        if (collision.gameObject.tag == "Projectile") {
            Destroy(collision.gameObject);
        }
	}
}
