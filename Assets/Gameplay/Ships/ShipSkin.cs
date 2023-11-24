using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay.Ships
{
    [CreateAssetMenu(menuName = "Ship Skin")]
    public class ShipSkin : ScriptableObject
    {
        [SerializeField] Sprite _hull;
        [SerializeField] Sprite _mainSail;
        [SerializeField] Sprite _smallSail;
        [SerializeField] Sprite _mast;
        [SerializeField] Sprite _mastPole;
        
        public Sprite Hull => _hull;
        public Sprite MainSail => _mainSail;
        public Sprite SmallSail => _smallSail;
        public Sprite Mast => _mast;
        public Sprite MastPole => _mastPole;
    }
}
