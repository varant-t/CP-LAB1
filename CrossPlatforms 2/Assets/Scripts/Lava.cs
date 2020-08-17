using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    CharacterMovement character;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("In lava");
        if(gameObject.tag == "Player")
        {
            character.TakeDamage(2);
        }

    }

    void OnTriggerStay(Collider other)
    {


    }
}
