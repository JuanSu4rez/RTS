using System;
using UnityEngine;
using V2.Enums;
using V2.Classes.Entities;

namespace V2.Controllers
{
    public partial class UnitController : V2.Interfaces.IDamagable, V2.Interfaces.ISelectable, V2.Interfaces.IEntityType, V2.Interfaces.ISurroundingPoints
    {
        private Entity entityObject;
        private Vector3 lastPosition;
        [SerializeField]
        private EntityType entityType;
        public EntityType EntityType {
            get => entityType;
            set {
                entityType = value;
                SetEntityObject();
                //after evolving recalc values of the object
            }
        }
        [SerializeField]
        private KindsOfEntities kindOfEntity;
        public KindsOfEntities KindOfEntity => kindOfEntity;
        public bool IsSelected { get; set; }
        private void SetEntityObject() {
            entityObject = EntityManger.entityManger.GetEntity(entityType);
        }
        private void InitLastCenteredPosition() {
            lastPosition = V2.Classes.Grid.grid.getCenteredGridPositionFromWorldPosition(this.transform.position);
        }
        private void TagValidation() {
            if(String.IsNullOrEmpty(this.gameObject.tag) ||
              this.kindOfEntity.ToString() != this.gameObject.tag) {
                this.gameObject.tag = this.kindOfEntity.ToString();
            }
        }
        private void InitEntityData() {
            if(KindOfEntity != Enums.KindsOfEntities.Resource) {
                this._maxHealth = entityObject.Stats.HP;
                this._currentHealth = this.MaxHealth;
            }
        }
        public float AddDamage(float damage) {
            var result = this.CurrentHealth - damage;
            if(result < 0) {
                result += damage;
            }
            else {
                result = damage;
            }
            this.CurrentHealth -= result;
            return result;
        }
        
    }
}