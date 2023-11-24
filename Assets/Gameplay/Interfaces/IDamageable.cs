using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public interface IDamageable
    {
        void OnDamaged(float damage);
    }
}
