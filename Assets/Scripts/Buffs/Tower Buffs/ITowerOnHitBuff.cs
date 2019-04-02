using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerOnHitBuff {

    void OnHit(GameObject hit, Projectile projectile);
}
