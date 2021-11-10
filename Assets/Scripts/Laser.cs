using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        
        if(transform.position.y >= 7.15) // Bullet jaise hi gayab ho waise hi uska object destroy krdo
            {
                if(transform.parent != null)//Triple Shot mein bullet toh destroy ho jayega but TripleShot Parent nhi hoga toh usko destroy krne kae liyae ye likha hai. Dekho TripleShot ek parent prefab hai uskae child 3 laser object hai woh toh destroy ho jayengae jaise hi screen sae bahar niklengae but Triple Shot rehega toh usko delete aise krna hai.(Simple this is how you delete a parent GameObject)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject); // this.gameObject is used to get the prefab clone which we want to destroy
            }
    }
}
