using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPreFab;

    [SerializeField]
    private GameObject _enemyContainer; // we are creating this because at present enemy object are getting Instantiated directly in the hierarchy and we don't want that it can be problematic while debugging. So what we can do is create a container and we can call all enemy objects under that container.
    // the way to do this is create a conatiner and then get a reference to the gameObject you want to put under the container and assign it to the container.
    
    //We don't need tripleShotPreFab container because as soon as TripleShotPowerUp goes out of the screen it'll get automatically deleted.  
    [SerializeField]
    private GameObject[] powerups; //  0-Triple Shot, 1-SpeedBooster, 2-Shield
    
    private bool stopSpawn = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    public void startSpawning()
    {
        StartCoroutine(spawnRoutine());//yha pae humlog coroutine ko start kr rhe hai and optimal way to do that is to pass the coroutine function name, another way is thorugh string like this --> StartCoroutine("spawnRoutine");
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    //A coroutine is like a function that has the ability to pause execution and return control to Unity but then to continue where it left off on the following frame.
    //It is essentially a function declared with a return type of IEnumerator and with the yield return statement included somewhere in the body. The yield return new WaitForSeconds(5.0f) will allow to wait for 5 seconds and then be resumed the following frame. To set a coroutine running, you need to use the StartCoroutine function
    //The difference between IEnumerator and void is the former one allows us to use yield 
    IEnumerator spawnRoutine()
    {
        yield return new WaitForSeconds(3.0f); // yha pae humlog asteroid destroy honae kae baad 3 seconds tak wait krengae phir spawning chalu kr dengae.
        while(stopSpawn == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.65f, 9.65f), 7.75f, 0);
            GameObject newEnemy = Instantiate(_enemyPreFab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform; // by this line all the spawned enemy now will spawn in enemy container here we are just getting the parent of new Enemy and asigning it to the enemy container.
            // the reason we wrote _enemyconatiner.transform is because we newEnemy reference is of transform type so we need to assign it to transform type only. Look only _enemyContainer is of GameObject type but _enemyConatiner.transform is of transform type.
            yield return new WaitForSeconds(5.0f);// means wait for 5 seconds after that the next line will be called. "yield retrun null" means to wait for a frame and then the next line after this line will be called
        } 
    }

    //The coroutine to start the powerup is written below.
    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f); // yha pae humlog asteroid destroy honae kae baad 3 seconds tak wait krengae phir spawning chalu kr dengae.
        while(stopSpawn == false)
        {
            yield return new WaitForSeconds(Random.Range(10, 16)); // Remember that 16 is exclusive and there if you want to include 16 then write both value as 10.0f and 16.0f that is float value 
            int randomPowerup = Random.Range(0, 3); // Here remember 3 is not included it is only 0,1 and 2.
            Vector3 posToSpawn = new Vector3(Random.Range(-9.65f, 9.65f), 7.75f, 0);
            Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
        }
    }
    
    public void onPlayerDeath()
    {
        stopSpawn = true;
    }
}
