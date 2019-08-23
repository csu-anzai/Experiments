using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MonsterDestinationSetter : AIDestinationSetter
{
    MonsterController monsterController;

    private void Start()
    {
        monsterController = GetComponent<MonsterController>();
    }

    void Update()
    {
        if (monsterController.originPos != null && ai != null) ai.destination = monsterController.originPos;
    }
}
