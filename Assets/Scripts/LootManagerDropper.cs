using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManagerDropper : Dropper
{
    static LootManager _lootManager;

    private void Start() {
        if(_lootManager == null) _lootManager = GameObject.FindGameObjectWithTag("LootManager").GetComponent<LootManager>();
    }

    public override void Drop()
    {
        foreach(Drop drop in _drops) _lootManager.AddLoot(drop);
    }

    private void OnDestroy() {
        _lootManager = null;
    }
}
