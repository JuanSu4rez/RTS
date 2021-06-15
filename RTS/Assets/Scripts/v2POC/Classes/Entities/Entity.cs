using UnityEngine;
using System.Collections;
using V2.Enums;
namespace V2.Classes.Entities
{
    public class Entity
    {
        public EntityType EntityType { get; private set; }
        public KindsOfEntities KindsOfEntitY { get; private set; }
        public EntityStats Stats { get; private set; }
        public Entity(EntityType entityType, KindsOfEntities kindsOfEntity, EntityStats stats){
            EntityType = entityType;
            KindsOfEntitY = kindsOfEntity;
            Stats = stats;
        }
    }
}
