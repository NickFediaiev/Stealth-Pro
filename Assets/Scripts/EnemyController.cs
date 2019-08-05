using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform playerTransform;
    Vector3 playerPosition;

    CharacterController enemyController;

    Vector3 enemyStartPosition = new Vector3(-1, 0, -1);
    Vector3 enemySecondPosition = new Vector3(-1, 0, 1);
    Vector3 enemyThirdPosition = new Vector3(1, 0, 1);
    Vector3 enemyFourthPosition = new Vector3(1, 0, -1);

    float timeToReachTarget = 3f;
    float speed = 3;

    bool moveToStartPosition = false;
    bool moveToSecondPosition = true;
    bool moveToThirdPosition = false;
    bool moveToFourthPosition = false;
    bool moving = false;

    Vector3 direction;
    Vector3 movement;

    public LayerMask viewMask;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = gameObject.GetComponent<CharacterController>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //Patrol();   
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToSecondPosition)
        {
            direction = enemySecondPosition - transform.localPosition;
            movement = direction.normalized * speed * Time.deltaTime;
            if (movement.magnitude > direction.magnitude)
            {
                moveToSecondPosition = false;
                moveToThirdPosition = true;
            }
            enemyController.Move(movement);
        }

        if (moveToThirdPosition)
        {
            direction = enemyThirdPosition - transform.localPosition;
            movement = direction.normalized * speed * Time.deltaTime;
            if (movement.magnitude > direction.magnitude)
            {
                moveToThirdPosition = false;
                moveToFourthPosition = true;
            }
            enemyController.Move(movement);
        }

        if (moveToFourthPosition)
        {
            direction = enemyFourthPosition - transform.localPosition;
            movement = direction.normalized * speed * Time.deltaTime;
            if (movement.magnitude > direction.magnitude)
            {
                moveToFourthPosition = false;
                moveToStartPosition = true;
            }
            enemyController.Move(movement);
        }
        if (moveToStartPosition)
        {
            direction = enemyStartPosition - transform.localPosition;
            movement = direction.normalized * speed * Time.deltaTime;
            if (movement.magnitude > direction.magnitude)
            {
                moveToStartPosition = false;
                moveToSecondPosition = true;
            }
            enemyController.Move(movement);
        }
    }

    public IEnumerator KillPlayer()
    {
        Vector3 direction;
        Vector3 movement;
        
        playerPosition = playerTransform.position;

        while (!Physics.Linecast(transform.position, playerPosition, viewMask))
        {
            //Like this Enemy should have used the last position of the Player to check if the Player would be in the line of sight
            //when the Enemy reached the last position of the player.
            #region
            /*
            if (!Physics.Linecast(transform.position, playerPosition, viewMask))
            {
                playerPosition = playerTransform.position;
            }
            */
            #endregion
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            direction = playerPosition - transform.position;
            movement = direction.normalized * speed * Time.deltaTime;
            enemyController.Move(movement);
            yield return null;
        }

        StartCoroutine(ReturnToPatrol());
        yield return null;
    }

    public void FollowPlayer()
    {
        moveToSecondPosition = false;
        moveToThirdPosition = false;
        moveToFourthPosition = false;
        moveToStartPosition = false;
        StartCoroutine(KillPlayer());
    }

    IEnumerator ReturnToPatrol()
    {
        StopCoroutine(KillPlayer());
        Debug.Log("Returning to patrol");
        while (true)
        {
            Vector3 direction = enemyStartPosition - transform.localPosition;
            Vector3 movement = direction.normalized * speed * Time.deltaTime;
            if (movement.magnitude < direction.magnitude) break;//movement = direction;
            enemyController.Move(movement);

            yield return null;
        }
        StopCoroutine(ReturnToPatrol());
        moveToStartPosition = true;
        yield return null;
    }

    #region //DRY coroutine to move enemy on patrol
    //IEnumerator Patrol(Vector3 nextPosition)
    //{
    //    while (true)
    //    {
    //        Vector3 direction = nextPosition - transform.localPosition;
    //        Vector3 movement = direction.normalized * speed * Time.deltaTime;
    //        if (movement.magnitude < direction.magnitude) break;
    //        gameObject.GetComponent<CharacterController>().Move(movement);
    //        yield return null;
    //    }

    //    moving = false;

    //    if (moveToSecondPosition)
    //    {
    //        moveToSecondPosition = false;
    //        moveToThirdPosition = true;
    //    }
    //    if (moveToThirdPosition)
    //    {
    //        moveToThirdPosition = false;
    //        moveToFourthPosition = true;
    //    }
    //    if (moveToFourthPosition)
    //    {
    //        moveToFourthPosition = false;
    //        moveToStartPosition = true;
    //    }
    //    if (moveToStartPosition)
    //    {
    //        moveToStartPosition = false;
    //        moveToSecondPosition = true;
    //    }

    //    yield return null;
    //}
    #endregion
}