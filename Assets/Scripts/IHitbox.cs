using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitbox
{
    void Hit(float optionalDamage = 0f);
}
