using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool it;
    public GameObject youreItIndicator;
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public string youreItMsg;
    public Vector3 origPos;
    public float score;

    protected Animation anim;

    int count = 5;

    void OnCollisionEnter(Collision collision)
    {
        // If the player is hit by a projectile and they are not currently it
        if (collision.gameObject.name.Contains("Projectile") && !it)
        {
            // Destroy all projectiles currently existing
            var projectiles = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (var p in projectiles)
            {
                Destroy(p);
            }
            // You've been tagged. You're going to be it!
            youreItIndicator.SetActive(true);
            StartCoroutine(Countdown());
        }
    }

    /// <summary>
    /// Fire a projectile.
    /// </summary>
    public virtual void Fire() {
        var projectile = (GameObject)Instantiate(
            projectilePrefab,
            projectileSpawn.position,
            projectileSpawn.rotation);

        // Add velocity to the bullet
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * 6;
        projectile.GetComponent<Projectile>().shooter = transform;

        // Destroy the bullet after 2 seconds
        Destroy(projectile, 2.0f);
    }

    /// <summary>
    /// Player has hit an enemy player with a projectile.
    /// </summary>
    public void HitEnemy() {
        it = false;
        youreItIndicator.SetActive(false);
    }

    /// <summary>
    /// Countdown until player is it.
    /// </summary>
    /// <returns>The countdown.</returns>
    IEnumerator Countdown() {
        while (count >= 0) {
            while (Time.timeScale == 0)
            {
                yield return null;
            }
            score += 1;
            youreItIndicator.GetComponent<TextMesh>().text = count.ToString();
            yield return new WaitForSecondsRealtime(1);
            count -= 1;
        }
        youreItIndicator.GetComponent<TextMesh>().text = youreItMsg;
        it = true;
        count = 5;
    }
}
