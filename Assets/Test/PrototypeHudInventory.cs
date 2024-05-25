using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeHudInventory : MonoBehaviour
{
    [SerializeField] SOPlayerStats _stats;
    [SerializeField] UISkillsManager _skillsManager;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        int iterator = 1;
        foreach(var weapon in _stats.Weapons)
        {
            if(weapon == null) continue;
            //_skillsManager.SetInputSkill((KeyInputTypes)iterator, weapon.SpriteAndAnimationData.Sprite).UpdateInputText(iterator.ToString());
            iterator++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
