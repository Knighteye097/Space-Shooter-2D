using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    //below variables are added to play the enemy explosion sound.
    [SerializeField]
    private AudioClip _enemyExplosionAudio; // this variable contain the enemy explosion audio clip
    private AudioSource _audioSource; // here we are adding the audio source so that the explosion clip could be accessed and played, the reason we aren't playng the clip directly is because what if in future we want to add enemy shooting laser audio.
    private bool _alreadyplayed = false;//this is created so that if our audio is played once then it won't get played again.
    private bool _alreadydead = false;//this is created to stop enemy from firing laser after the enemy is shot by player or by any laser. Actually the thing is that our enemy need to play explosion audio and for that it needs to exist for 2.4 seconds befor getting destroyed and in the mean time the enemy object exist and it can call firelaser methid in the update method which is  a bug.
    private Player _player;

    private Animator _enemyBlastAnim; //This variable is for initiating the animation of blasting the enemy.

    private float _fireRate = 5.0f;
    private float _nextFire = 0.0f;
    [SerializeField]
    private GameObject _enemyLaserPrefab;

    [SerializeField]
    private AudioClip _enemyLaserAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Player in enemy script is NULL");
        }

        _enemyBlastAnim = GetComponent<Animator>(); // Since enemy prefab has required animation and we are in enemy prefab's script so we don't need to use transform.gameObject

        if(_enemyBlastAnim == null)
        {
            Debug.LogError("Enemy Blast Animation is NULL!");
        }

        _audioSource = GetComponent<AudioSource>();

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source of enemy is NULL!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > _nextFire && _alreadydead == false)
        {
            fireLaser();
        }
        
        transform.Translate(Vector3.down * _speed * Time.deltaTime); // yha pae hum ye kr rhe hai ki iski madad sae hamara enemy obkect jo hai woh neechae ayega at a certain speed decided thorugh _speed variable
        
        if(transform.position.y <= -5.74f)
        {
            transform.position = new Vector3(Random.Range(-9.65f, 9.65f), 7.75f, 0);// Random.Range kae thorugh hum log iss object ko randomly -9.65 to 9.65 mein bula sakte hai 7.75 hamare game screen ke max height hai mtlb jaise hamara enemy object -5.74 ki height sae neechae jayega waise hi hamara enemy object 7.74 ki height pae x axis pae randomly kahi phirsae aajyega
        }
    }

    /* How to setup collision --> the first step jitnae bhi gameobject collide hongae un sabmein(one by one) inspector screen mein Box Collider section mein jaake isTrigger option ko check krna
                                  the second step is, dekho jitnae object bhi collision krenge sabmein physics(mtlb jab woh collide krenge tab kuch physics lagegi na) apply krni padegi aur woh humlog gameObject mein rigidbody component add krke enable krte hai, just inspector screen mein jao aur aakhri mein add component likha hota hai usmein rigidbody select krke add krlo
                                  dekho yha pae ek chiz dhyaan rakho ki har object pae rigidbody apply krna is not a good call instead jaise ki enemy aur player hai ye dono collide krenge toh kisi ek pae rigidbody laga do, effect rigidbody ka dono pae hoga(apne case mein enemy pae rigidbody laga hai) aur laser pae lagega hi kyonki woh enemy sae collide kregi hi....
                                  the third step is, now in unity there are certain function dedicated to collisions and one of them is "OnTriggerEnter" ismein humlog kuch nhi bs enemy ko destroy kr rhe hai 
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser") // jaise hamara enemy object bullet sae collide kare turant pehlae bullet ko aur phir enemy object ko destroy krdo
        {
            Destroy(other.gameObject);
            
            _enemyBlastAnim.SetTrigger("OnEnemyDeath");// Now in enemy death animation controller we have created a trigger named OnEnemyDeath after activating it our animation for enemy death will work.
            _speed = 0; // The reason we are doing this is because dekho jaise hi enemy ko goli lagae woh apni position pae ruk jaye aur pura animation krle aur phir destroy ho jaye agar isko zero nhi kiya toh woh neechae ata rehega
            playEnemyExplosionAudio();// this method will be called in everyframe 
            _alreadyplayed = true;// this variable will be set to true when the first collision will take place and after calling the audio playing method it will set itself to true and hence the audio won't be played afterthat and our enemy can easily get destroyed without playing the audio again and again per frame. 
            _alreadydead = true;
            Destroy(this.gameObject, 2.4f); // Yha 2.4f ka mtlb hai ki destroy krne sae pehlae 2.4f ka delay aa jaye the reason is 2.4 seconds ka animation hai toh humlog chahte hai ki goli lagte hai enemy apni position pae ruk kae animation play krke destroy ho jaye.

            _player.ScoreUpdate();
        }

        if(other.tag == "Player")//Ismein humlog enemy object ko destroy kr rhe hai jaise hi woh hamare player sae collide krega aur player ki health kam kr rhe hai
        {
            //Script Communication is something when gameobject tries to interact or tries to use methods of other gameobjects
            //Here we are doing scipt communication the reason is jaise hi enemy object player object sae collide kre toh player ki health kam honi chahiye aur woh player mein likha Damage function mein hai toh wha tak kaise pahucha jaye woh likha hai neechae. 
            //other.transform.GetComponent<Player>().Damage();//Dekho transfrom eklauta aisa component hai jo directly access hota hai aur humlog ko access krna hai player component(player script) kyonki usmein damage function hai toh humlog agar inpsector screen mein dekhae toh player script Player kae naam sae di hai toh iss syntax kae through humlog wha tak pahuch sakte hai transform.GetComponent<Player(component name)>().function_name();
            //The below way is a better way of script communication because we are also doing NULL checking!!
            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
            
            _enemyBlastAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            playEnemyExplosionAudio();
            _alreadyplayed = true;
            _alreadydead = true;
            Destroy(this.gameObject, 2.4f);
        }
    }


    //this method is created because we want to play our explosion audio clip once only and if you look at the collsion method then you can see that our enemy object will exist for 2.4 seconds before getting destoryed and that means in that 2.4 seconds everytime a laser or if player will collide with the enemy object the script will play the audio. That is something we don't want.
    public void playEnemyExplosionAudio()
    {
        if(_alreadyplayed == false)
        {
            _audioSource.clip = _enemyExplosionAudio;
            _audioSource.Play();
        }
    }

    private void fireLaser()
    {
        _nextFire = _fireRate + Time.time;

        Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);

        _audioSource.clip = _enemyLaserAudio;
        _audioSource.Play();
    }
}
