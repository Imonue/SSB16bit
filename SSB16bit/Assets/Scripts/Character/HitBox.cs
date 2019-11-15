using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Character character;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (character.GetUserType())
        {
            if (collision.tag.Equals("AnotherPlayer"))
            {
                this.character.TakeDamage(collision.GetComponentInParent<Character>().GetATK());
            }
        }
        else
        {
            if (collision.tag.Equals("Player"))
            {
                this.character.TakeDamage(collision.GetComponentInParent<Character>().GetATK());
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
