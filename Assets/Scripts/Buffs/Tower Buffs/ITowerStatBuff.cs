using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerStatBuff {

    float ApplyRangeBuff(float range);

    float ApplyFireRateBuff(float fireRate);

    float ApplyDamageBuff(float damage);
}
