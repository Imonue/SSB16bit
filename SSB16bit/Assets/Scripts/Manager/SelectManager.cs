using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    public static SelectManager instance;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCharacter(string characterID)
    {
        NetworkMananger.instance.SendSelectCharacterMessage(characterID);
    }
}
