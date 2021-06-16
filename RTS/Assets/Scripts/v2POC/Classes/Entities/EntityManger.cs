using UnityEngine;
using System.Collections;
using V2.Enums;
using System;

namespace V2.Classes.Entities
{
    public class EntityManger
    {
        public static readonly EntityManger entityManger = new EntityManger();
        public static void Init() {
        }
        private Entity[] poolOfEntities = null;
        public EntityManger() {
            InitArray();
            InitEntities();
        }
        private void InitArray() {
            var myEnumMemberCount = Enum.GetNames(typeof(EntityType)).Length;
            poolOfEntities = new Entity[myEnumMemberCount];
        }
        private void InitEntities() {
            EntityType entityType = EntityType.Citizen;
            KindsOfEntities kindOfEntity = KindsOfEntities.Citizen;
            var entityStats = new EntityStats() {
                HP = 25f,
                Attack = 3f,
                Armor = 0f,
                PierceArmor = 0f,
                Range = 0f,
                LineofSight = 4f,
                Speed = 0.8f,
                BuildTimeSeconds = 25f,
                FrameDelay = 0f,
                AttackDelaySeconds = 0f,
                ReloadTimeSeconds = 2f,
            };
            var index = entityType.Ordinal();
            poolOfEntities[index] = new Entity(entityType, kindOfEntity, entityStats);

            entityType = EntityType.House;
            kindOfEntity = KindsOfEntities.Building;
            entityStats = new EntityStats() {
                HP = 25f,
                Attack = 3f,
                Armor = 0f,
                PierceArmor = 0f,
                Range = 0f,
                LineofSight = 4f,
                Speed = 0.8f,
                BuildTimeSeconds = 25f,
                FrameDelay = 0f,
                AttackDelaySeconds = 0f,
                ReloadTimeSeconds = 2f,
            };
            index = entityType.Ordinal();
            poolOfEntities[index] = new Entity(entityType, kindOfEntity, entityStats);

            entityType = EntityType.GoldMine;
            kindOfEntity = KindsOfEntities.Resource;
            entityStats = new EntityStats() {
                HP = 1000f,
                Attack = 3f,
                Armor = 0f,
                PierceArmor = 0f,
                Range = 0f,
                LineofSight = 4f,
                Speed = 0.8f,
                BuildTimeSeconds = 25f,
                FrameDelay = 0f,
                AttackDelaySeconds = 0f,
                ReloadTimeSeconds = 2f,
            };
            index = entityType.Ordinal();
            poolOfEntities[index] = new Entity(entityType, kindOfEntity, entityStats);

            entityType = EntityType.Deer;
            kindOfEntity = KindsOfEntities.Resource;
            entityStats = new EntityStats() {
                HP = 10f,
                Attack = 0f,
                Armor = 0f,
                PierceArmor = 0f,
                Range = 0f,
                LineofSight = 4f,
                Speed = 0.8f,
                BuildTimeSeconds = 0f,
                FrameDelay = 0f,
                AttackDelaySeconds = 0f,
                ReloadTimeSeconds = 2f,
            };
            index = entityType.Ordinal();
            poolOfEntities[index] = new Entity(entityType, kindOfEntity, entityStats);
        }
        public Entity GetEntity(EntityType entityType) {
            var index = entityType.Ordinal();
            return poolOfEntities[index];
        }
    }
}
