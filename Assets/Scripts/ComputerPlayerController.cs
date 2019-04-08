using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class ComputerPlayerController : Player {

    public int speed;
    public int maxDistance;
    public int minDistance;
    public float fireRate = 1.0f;

    Transform[] trees;
    Transform[] players;
    NavMeshAgent nav;
    Vector3 curPos;
    Vector3 lastPos;
    float nextFire = 3.0f;
    int nextDest = 0;
    bool hiding = false;

    // Use this for initialization
    void Start () {
        score = 0;
        origPos = transform.position;
        it = false;
        nav = GetComponent<NavMeshAgent>();
        nav.speed = speed;
        var playerGameObjects = GameObject.FindGameObjectsWithTag("Player").Where(go => go.name != name);
        players = playerGameObjects.Select(t => t.transform).ToArray();
        var treeGameObjects = GameObject.FindGameObjectsWithTag("Tree");
        trees = treeGameObjects.Select(t => t.transform).ToArray();
        anim = GetComponent<Animation>();
    }
	
	// Update is called once per frame
	void Update () {
        curPos = transform.position;
        Transform player;
        if (it) {
            if (Time.timeScale == 1) {
                // Only add to the score if the game is in progress.
                score += Time.deltaTime;
            }

            hiding = false;
            player = players[0];
            foreach (var p in players) {
                // Pick the closest player to chase.
                if (Vector3.Distance(transform.position, p.position) < Vector3.Distance(transform.position, player.position)){
                    player = p;
                }
            }
            nav.autoBraking = true;
            nav.destination = player.position;
            transform.LookAt(player);

            // Fire at the player if they are within the max distance.
            if (Vector3.Distance(transform.position, player.position) <= maxDistance) {
                Fire();
            }
        }
        else {
            // Run away!
            player = players.FirstOrDefault(p => p.GetComponent<Player>().it == true);
            if (player != null)
            {
                hiding = false;
                var runAwayPos = transform.position + (transform.position - player.position) * 3;
                nav.destination = runAwayPos;
            }
            else {
                // No one is it yet (countdown) so just MOVE!!!
                if (!hiding) {
                    nav.ResetPath();
                    hiding = true;
                }
                nav.autoBraking = false;
                ContinuousHiding();
            }
        }

        // If the player hasn't moved in the last frame, stop animating.
        if (curPos == lastPos) {
            anim.Stop();
        }
        else {
            anim.Play();
        }

        lastPos = curPos;
    }

    public override void Fire()
    {
        if (it && Time.time > nextFire){
            nextFire = Time.time + fireRate;
            base.Fire();
        }
    }

    void ContinuousHiding() {
        // Pick random trees to hide behind. #BadAIIsBad
        if (!nav.pathPending && nav.remainingDistance < 0.01f) {
            if (trees.Length == 0) return;
            nav.destination = trees[nextDest].position;
            nextDest = (nextDest + Random.Range(1,10)) % trees.Length;
        }
    }
}
