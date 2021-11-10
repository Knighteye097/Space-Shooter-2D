using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    //ID for PowerUps
    [SerializeField]
    private int _PowerupID; // 0 - TripleShot, 1 - Speed, 2 - Shields 

    [SerializeField]
    private AudioClip _powerupAudioClip; // creating the reference to the audio clip we want to play at the instant our powerobject gets destroyed.
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //move down at a speed of 3
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        //Unlike enemy the powerup should be destroyed as it moves out of the screen
        if(transform.position.y <= -5.74f)
        {
            Destroy(this.gameObject);
        }    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //below is what will happen if a powerup collides with player firstly the the powerup will get destroy and then the powerup will start working whose code is written in player script
        if(other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_powerupAudioClip, transform.position);// here as we can see that our objects is getting destroyed in an instant so we need to play the audio clip at this moment only for that we have this predefined method which takes tha audioclip reference and the position where we want to play the audio and we don't need AudioSource component for this we just need a reference to that audio clip that we want to play.
            Destroy(this.gameObject);

            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                switch (_PowerupID) //dekho har ek powerup ki apni ek id hai aur powerup script teeno powerup prefabs sae attached hai aur har ek powerup ki apni ek int type ki id hai ussi id ko humlog yha pae switch maar rhe hai. 
                {
                    case 0 : player.TripleShotActive();
                             break;
                    case 1 : player.SpeedPowerupActive();
                             break;
                    case 2 : player.ShieldPowerupActive();
                             break;
                    default: Debug.LogError("Undefined Powerup!");
                             break;
                }
            }

        }
    }
}
