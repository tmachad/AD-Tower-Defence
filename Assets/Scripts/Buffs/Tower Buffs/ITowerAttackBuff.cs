using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerAttackBuff {

    void Attack(Enemy[] targetsInRange, Tower tower);
}
