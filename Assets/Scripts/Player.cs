using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ye script jo hai woh humnae game scene mein main player pae laga rakhi hai
public class Player : MonoBehaviour
{
    [SerializeField] //This is wriiten so that our private variable gets visible on inspector screen and so that we can change the values through inspector field NOTE -> if a private value is changed through inspector field it will not get reflected back in this program
    private float _speed = 10.0f; // f is written to let know the compiler that this value is float otherwise the compiler will return some error
    
    [SerializeField]
    private GameObject _laserPreFab; // the value to this variable of type gameobject is provided through drag and drop method, jaise hi yha pae humnae isae declare kiya player ki inspector screen mein ye ajayega aur phir hum laser Prefab iss variable pae drag and drop kr dengae.

    [SerializeField]
    private GameObject _tripleShotPreFab;
    private bool _isTripleShotActive = false;
    
    private bool _isShieldActive = false;
    
    [SerializeField]
    private GameObject _ShieldVisualizer; //Used to add the animation of shield
    [SerializeField]
    private GameObject[] _EngineDamage; // Used to add the animation of engine damage on both the sides and also for the player explosion

    [SerializeField]
    private GameObject _explosionPrefab; //Used to add the explosionprefab which is a animation for explosion

    [SerializeField]
    private AudioClip _laserAudio; //Used to add the clip of laserFiring
    private AudioSource _audioSource; //Dekho agar laser audio bajani hai toh pehlae woh player kae pass honi chahiye na toh uskae liyae inspector screne mein jaake pehlae audio source component add kro phir usmein baad mein laser clip assign krdo. Used to store the clip of laser and play it whenever firelaser method is called.

    private float _fireRate = 0.2f;
    private float _nextFire = 0.0f;

    private int _lives = 3;
    private int _score = 0;

    private Animator _playerTurningAnim;// Used to animation of player turning and also to comeback to idle state after that.

    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    
    // Start is called before the first frame update
    void Start() // By default C# mein class methods private hotae hain jaise ki ye void start private type ka hai, yha pae private void start() bhi likh sakte hai
    {
        
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("The Game Manager is not linked!!");
        }

        _gameManager.GameOn();//Calling this method over here to start the escape key functionality.

        
        transform.position = new Vector3(0 ,0, 0); // This is for setting the posotion of player initially to zero.
        
        //yae neechae script communication kae liyae hai. Aur GameObject.Find is a expensive method so it is better to call is once that is why we are writing this in Start function and not in methods. 
        //yha pae sabse pehlae humlog Spawn_Manager ko hierarchy mein dhoondh rhe hai uskae liyae humlog nae GameObject.Find lagaya hai phir humlog Spawn_Manager kae script ko access kr rhe hai using GetComponent
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();// Here we are getting a reference to SpawnManger component for doing script communication. The reason for script communication is we don't want to instantiate enemy after our player is dead. So how do we do that the answer is script communication
        // ab player mein _spawnManager kae pass SpawnManager ka reference hai mtlb ab humlog SpawnManager kae methods ko access kr sakte hai!
        if(_spawnManager == null)// Checking for null
        {
            Debug.LogError("The Spawn Manager is NULL.");// This line is wriiten to show error to developers
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is not linked!!");
        }

        _playerTurningAnim = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        if(_audioSource == null)
        {
            Debug.LogError("The Player does not have any audio source!");
        }
        else
        {
            _audioSource.clip = _laserAudio; // yha pae humlog audiosource mein laseraudio assign kr rhe hai!. 
        }
    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire) // yha pae and condition hum fire rate set kr rhe hai time.time real time return krta hai aur ussae hum nextfire variable sae check kr rhe hai agar hamara real time nextfire sae jyada hoga tab hi hum fire kr payengae aur tabhi hamara laser object instantiate hoga
        {             
            fireLaser();            
        }
    }

    void calculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // float type because getaxis will always return float type value,this will return either 0, 1(if d or right key is pressed), -1(if a or left key is pressed)
        float verticalInput = Input.GetAxis("Vertical");  // this will return either 0, 1(if w or up key is pressed), -1(if s or down key is pressed)
        
        // 1 unit in unity is 1 meter in realgame 
        
        //the below code is used for animation of player turning.
        if(horizontalInput == -1)// dekho agar value -1 hai toh mtlb hamara player left side ja rha hai toh left side wala animation trigger ho jana chahiye
        {
            _playerTurningAnim.SetTrigger("OnPlayerTurnLeft");// ye kuch nhi animation controller mein parameters mein jaake trigger sae set kr sakte hai ye mtlb ek index value ki tarah kaam kr rha hai player left turn animation kae liyae!
        }

        if(horizontalInput == 1)// dekho agar value 1 hai toh mtlb hamara player right side ja rha hai toh right side wala animation trigger ho jana chahiye
        {
            _playerTurningAnim.SetTrigger("OnPlayerTurnRight");
        }

        if(horizontalInput == 0)// aur jaise hi hamara player turn hona rokae toh ab humae uska animation rokna padega toh uskae liyae meine PLayer Movement naam kae animation controller mein ek idle state bana rakhi hai taaki player uspae wapas aajaye
        {
            _playerTurningAnim.SetTrigger("OnPlayerIdle");
        }
        
        // below line is similar to new Vector3(1(x-axis), 0(y-axis), 0(z-axis)) * either 0,1,-1 * speed * realtime(Time.deltaTime)
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime); //this line means that our game object will move 1unit/sec time.deltatime is for moving our object 1unit/sec if we will not multiplly then our object will move at 1unit/frame or 60m/sec
        
        // below line is similar to new Vector3(0(x-axis), 1(y-axis), 0(z-axis)) * either 0,1,-1 * speed * realtime(Time.deltaTime)
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
       
        /* (Optimal Way for handling movement)
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
        */
        
        // below is the code for stopping the game object from moving out of the screen
        // x right is 10.26 ye value game screen dekh kae nikal lo ki kha pae hamara game object screen kae bahar ja rha hai
        // y down is -4
        // the reason we are writing all three coordinates is kyonki agar nhi likho gae toh tumhara game object apni place automatic change krega
        if(transform.position.x >= 10.26f) // yha pae hum game object ko left side(jo ki -10.26f hai) pae laa rhe hai kyonki hamara game object jaise hi 10.26f value game screen pae(x-axis pae) cross krega toh gayab ho jayega
        {
            transform.position = new Vector3(-10.26f, transform.position.y, 0);// iss line kae execute hotae hi hamara game object screen ki left side pae ajayega aur kyonki hum nhi chahte ki uski height change ho isliyae humnae y-axis ki value same rakhi hai 
        }
        else if(transform.position.x <= -10.26f)
        {
            transform.position = new Vector3(10.26f, transform.position.y, 0);
        }
        if(transform.position.y >= 0) // yha pae game object ko upar ki taraf rokne ke liyae kiya hai
        {
            transform.position = new Vector3(transform.position.x, 0, 0);// yae upar ki taraf rokne ke liyae kiya hai aur upar ki value zero hai jaise hi game object wha pae pahuchaega turant whi pae block ho jayega ussae upar nhi jaa payega...
        }
        else if(transform.position.y <= -4f) // yha pae game object ko neechae ki taraf rokne ke liyae condition lagayi hai
        {
            transform.position = new Vector3(transform.position.x, -4f, 0);
        }
        /* NOTE --> (optimal way for handling y-axis)kyonki humlog pae main ye chahte hai ki hamara game object 0 sae -4f kae beech mein hi rhe toh humlo chahe toh mathf.Clamp function use kr sakte hai
        like this --> transform.postion = new Vector3(transform.position.x, mathf.Clamp(transform.position.y(value which needs to be clamped) , -4f(min clamp value), 0(max clamp value)), 0(z-axis))
        */
    }

    void fireLaser()
    {
        if(_nextFire == 0.0f)//Dekho jab game start hoga toh phela shot maarte hi hamara instruction text disable ho jana chahiye usi kae liyae ye if likha hai
        {
            _uiManager.disableInstructionText();
        }
        
        _nextFire = Time.time + _fireRate; //kabhi agar bhool jana ye toh time.time unity search krna google pae aur udemy pae cool down system dekh lena              
        //Instantiate(_laserPreFab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity); // this function is for instantiate(creating the instance(object) of any variable) the laser from our spaceship new vector with 0.8f value in y axis is added because we want our laser to start from 0.8f ahead of our object if still confusion remove it and see the difference between the laser starting position
        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPreFab, transform.position, Quaternion.identity);
        }

        else
        {
            Instantiate(_laserPreFab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();// yha pae humlog laser fire krnae ki audio play kr rhe hai aur issae sabsae last mein isliyae likha hai kyonki light is faster than sound toh phele laser fire hogi phir uski audio play hogi
    }

    public void Damage()
    {
        if(_isShieldActive == true) // basically yha pae ye ho rha hai ki player ek hit sae bach jayega mtlb agar shield actve hai toh ek baar ke liyae agar woh kisi enemy sae collide krta hai toh uski lives kam nhi hongi.
        {
            _ShieldVisualizer.SetActive(false);
            _isShieldActive = false;
            return;
        }

        _lives--;

        EngineDamageVisualizer();//ye function damage show krne kae liyae hai mtlb left engine ya right engine animation ya player explosion

        _uiManager.livesUpdate(_lives); // Lives wali jo image lagi hai usko update krne kae liyae yae call kiya hai.
        
        if(_lives == 0)
        {
            _spawnManager.onPlayerDeath();// yha pae humlog spawnManager kae onPlayerDeath method ko bula rhe hai taaki sabhi enemies aur powerups ko aur spawn honae sae rok sake.
            Destroy(this.gameObject, 0.25f); //This delay is provided to show some realistic effects after explosion of our player ship takes place
            _uiManager.enableGameOver();//This line is written to enable game over text.
            _gameManager.GameOver(); // This is written to allow user to restart the game using game manager.
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine("TripleShotPowerDownRoutine");
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedPowerupActive()
    {
        _speed = _speed + 3.0f;
        StartCoroutine("SpeedPowerDownRoutine");
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = _speed - 3.0f;
    }

    public void ShieldPowerupActive()
    {
        _isShieldActive = true;
        _ShieldVisualizer.SetActive(true);
    }
 
    //To get the score
    //Communicate with UImanager to update the score
    public void ScoreUpdate()
    {
        _score = _score + 10;
        _uiManager.scoreUpdate(_score);
    }

    //Below is the code written to show the damage to the player aur aisae likha hai ki pehli baar mein koi bhi random side engine damage dikhaega
    public void EngineDamageVisualizer()
    {
        if(_lives == 2)//dekho jab pehli baar hit ho apna player toh koi bhi random side ka engine damage chalu krdo.
        {
            int randomEngine = Random.Range(0,2);
            _EngineDamage[randomEngine].SetActive(true);
        }

        if(_lives == 1)//par jab dusri baar hit ho tab mtlb pehli aar jab hit hua hoga oh koi engine damage chal rha hoga toh pehle check kro kuansa enginedamage active hai aur phir jo nhi active hai ussae bhi active krdo
        {
            if(_EngineDamage[0].activeSelf)//jaise maanlo agar right side wala engine(index value 0) damage active hai toh left side(index value 1) wala chalu krdo wrna right side wala chalu krdo.
            {
                _EngineDamage[1].SetActive(true);
            }
            else
            {
                _EngineDamage[0].SetActive(true);
            }
        }

        if(_lives == 0)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
