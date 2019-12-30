//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using UnityEngine.AI;

//public interface MonsterIdleBehavior
//{
//    void IdleBehavior(NavMeshAgent agent, Animation animation, Transform transform, string idleAnimation, string moveAnimation, Monster monster);
//}

//public class DefaultIdleBehavior : MonsterIdleBehavior {
//    public void IdleBehavior(NavMeshAgent agent, Animation animation, Transform transform, string idleAnimation, string moveAnimation, Monster monster) {
//        transform.GetComponent<MonsterCombatant>().DebugText("idle");
//        if (agent != null) {
//            agent.isStopped = true;
//        }
//        if (animation != null) {
//            animation.CrossFade(idleAnimation);
//            monster.GetComponent<MonsterAnimationController>().moving = false;
//            monster.GetComponent<MonsterAnimationController>().attacking = false;
//        }
//    }
//}

//public abstract class RandomMover : MonsterIdleBehavior
//{
//    public abstract void IdleBehavior(NavMeshAgent agent, Animation animation, Transform transform, string idleAnimation, string moveAnimation, Monster monster);

//    protected Vector3 GrabRandomNavMeshPosition(float distance, Transform transform)
//    {
//        transform.GetComponent<MonsterCombatant>().DebugText("moving at random");
//        Vector3 randomDirection = Random.insideUnitSphere * distance;
//        randomDirection += transform.position;
//        NavMeshHit hit;
//        NavMesh.SamplePosition(randomDirection, out hit, distance, -1);
//        return hit.position;
//    }
//}

//public class PatrollerBehavior : RandomMover {
//    private Vector3 waypoint1 = Vector3.positiveInfinity;
//    private Vector3 waypoint2 = Vector3.positiveInfinity;
//    private bool waypointSet1 = false;
//    private bool waypointSet2 = false;
//    private float pathTimer = 0;
//    private float currentTime = 0;
//    private bool confirmedPath = false;
//    private float distance = 60;

//    public override void IdleBehavior(NavMeshAgent agent, Animation animation, Transform transform, string idleAnimation, string moveAnimation, Monster monster) {
//        transform.GetComponent<MonsterCombatant>().DebugText("patrolling");
//        if (agent==null) return;
//        if (!waypointSet1) SetFirstWaypoint(transform);
//        if (!waypointSet2) {
//            SetSecondWaypoint(transform, agent);
//            return;
//        }
//        currentTime += Time.deltaTime;
//        if (currentTime >= pathTimer) CompletePath(agent);
//        else if (!confirmedPath) TestPath(agent, transform);
//    }

//    private void SetFirstWaypoint(Transform transform)
//    {
//        waypoint1 = transform.position;
//        waypointSet1 = true;
//        pathTimer = Random.Range(6, 25);
//    }

//    private void SetSecondWaypoint(Transform transform, NavMeshAgent agent)
//    {
//        PickWaypoint(transform);
//        waypointSet2 = true;
//        agent.destination = waypoint2;
//    }

//    private void CompletePath(NavMeshAgent agent)
//    {
//        currentTime = 0;
//        if (agent.destination == waypoint1) agent.destination = waypoint2;
//        else
//        {
//            confirmedPath = true;
//            agent.destination = waypoint1;
//        }
//    }

//    private void TestPath(NavMeshAgent agent, Transform transform)
//    {
//        NavMeshPath path = new NavMeshPath();
//        agent.CalculatePath(waypoint2, path);
//        if (path.status != NavMeshPathStatus.PathComplete) PickWaypoint(transform);
//    }

//    private void PickWaypoint(Transform transform, int attempts = 0) {
//        if (attempts>=600) return;
//        waypoint2 = GrabRandomNavMeshPosition(distance, transform);
//        if (Vector3.Distance(waypoint2, transform.position)<6) PickWaypoint(transform, attempts+1);
//        //var il = GameObject.FindGameObjectWithTag("ConfigObject").GetComponent<InitializeLevel>();
//        //int x = (int)((waypoint2.x + 120) / 2);
//        //int y = (int)((waypoint2.z + 120) / 2);
//        //if (waypoint2.x<-120 || waypoint2.x>120 || waypoint2.z<-120 || waypoint2.z>120) PickWaypoint(transform, attempts+1);
//        //else if (il.map.blocks[x,y]!=" ") PickWaypoint(transform, attempts+1);
//    }
//}

//public class WandererBehavior : RandomMover {
//    private Vector3 waypoint = Vector3.positiveInfinity;
//    private bool waypointSet = false;
//    private float pathTimer = 0;
//    private float currentTime = 0;
//    private bool confirmedPath = false;
//    private float distance = 60;

//    public override void IdleBehavior(NavMeshAgent agent, Animation animation, Transform transform, string idleAnimation, string moveAnimation, Monster monster) {
//        transform.GetComponent<MonsterCombatant>().DebugText("wandering");
//        if (agent == null) return;
//        if (!waypointSet) {
//            PickWaypoint(transform);
//            agent.destination = waypoint;
//            waypointSet = true;
//            pathTimer = Random.Range(3, 13);
//        }
//        currentTime += Time.deltaTime;
//        if (currentTime >= pathTimer || !confirmedPath) NewCycle(transform, agent);
//    }

//    private void NewCycle(Transform transform, NavMeshAgent agent)
//    {
//        currentTime = 0;
//        PickWaypoint(transform);
//        agent.destination = waypoint;
//    }

//    private void PickWaypoint(Transform transform) {
//        waypoint = GrabRandomNavMeshPosition(distance, transform);
//        if (Vector3.Distance(waypoint, transform.position) < 6) PickWaypoint(transform);
//        //var il = GameObject.FindGameObjectWithTag("ConfigObject").GetComponent<InitializeLevel>();
//        //int x = (int)((waypoint.x + 120) / 2);
//        //int y = (int)((waypoint.z + 120) / 2);
//        //if (waypoint.x < -120 || waypoint.x > 120 || waypoint.z < -120 || waypoint.z > 120) PickWaypoint(transform);
//        //else if (il.map.blocks[x, y] != " ") PickWaypoint(transform);
//        confirmedPath = !(Physics.Raycast(transform.position, waypoint - transform.position, Vector3.Distance(transform.position, waypoint), ~(1 << 8)));
//    }
//}

//public class ExplorerBehavior : RandomMover {
//    private Vector3 waypoint = Vector3.positiveInfinity;
//    private bool waypointSet = false;
//    private float pathTimer = 0;
//    private float currentTime = 0;
//    float distance = 240;

//    public override void IdleBehavior(NavMeshAgent agent, Animation animation, Transform transform, string idleAnimation, string moveAnimation, Monster monster) {
//        transform.GetComponent<MonsterCombatant>().DebugText("exploring");
//        if (agent == null) return;
//        if (!waypointSet) SetWaypoint(transform, agent);
//        currentTime += Time.deltaTime;
//        if (currentTime >= pathTimer) {
//            currentTime = 0;
//            PickWaypoint(transform);
//            agent.destination = waypoint;
//        }
//    }

//    private void SetWaypoint(Transform transform, NavMeshAgent agent)
//    {
//        PickWaypoint(transform);
//        agent.destination = waypoint;
//        waypointSet = true;
//        pathTimer = Random.Range(24, 97);
//    }

//    private void PickWaypoint(Transform transform) {
//        waypoint = GrabRandomNavMeshPosition(distance, transform);
//        if (Vector3.Distance(waypoint, transform.position) < 6) PickWaypoint(transform);
//        //var il = GameObject.FindGameObjectWithTag("ConfigObject").GetComponent<InitializeLevel>();
//        //int x = (int)((waypoint.x + 120) / 2);
//        //int y = (int)((waypoint.z + 120) / 2);
//        //if (waypoint.x < -120 || waypoint.x > 120 || waypoint.z < -120 || waypoint.z > 120) PickWaypoint(transform);
//        //else if (il.map.blocks[x, y] != " ") PickWaypoint(transform);
//    }
//}

//public class BossIdleBehavior : RandomMover {
//    public override void IdleBehavior(NavMeshAgent agent, Animation animation, Transform transform, string idleAnimation, string moveAnimation, Monster monster) {
//        transform.GetComponent<MonsterCombatant>().DebugText("returning home");
//        if (agent == null) return;
//        agent.destination = monster.GetComponent<Boss>().originalLocation;
//    }
//}