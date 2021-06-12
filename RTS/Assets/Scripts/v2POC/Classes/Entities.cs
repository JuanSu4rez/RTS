using UnityEngine;
using System.Collections;
using V2.Enums;
namespace V2.Classes
{
    public class Entities{
        public EntityType EntityType { get; set; }
        public KindsOfEntities kindsOfEntities { get; set; }
        public float HP {get;set;} // 30;        public float Attack {get;set;} // 4;        public float Armor {get;set;} // 0;        public float PierceArmor {get;set;} // 0;        public float Range {get;set;} // 4;        public float LineofSight {get;set;} // 6;        public float Speed {get;set;} // 0.96;        public float BuildTime {get;set;} // 35s;        public float FrameDelay {get;set;} // 15;        public float AttackDelay {get;set;} // 0.35s;        public float ReloadTime {get;set;} // 2s;        public float Accuracy {get;set;} // 80%
    }
}
