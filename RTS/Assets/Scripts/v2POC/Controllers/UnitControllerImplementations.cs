using System;
using UnityEngine;
using V2.Enums;

namespace V2.Controllers
{
    public partial class UnitController : V2.Interfaces.IDamagable, V2.Interfaces.ISelectable, V2.Interfaces.IEntityType, V2.Interfaces.ISurroundingPoints
    {
        [SerializeField]
        private EntityType entityType;
        public EntityType EntityType => entityType;
        [SerializeField]
        private KindsOfEntities kindOfEntity;
        public KindsOfEntities KindOfEntity => kindOfEntity;
        public bool IsSelected { get; set; }
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
        private Vector3[] cacheSurroundingPoints = null;
        public Vector3[] GetSurroundingPoints() {
            if(!IsAlive()) {
                return cacheSurroundingPoints;
            }
            if(canCalculateStaticUnitPoints())
                return cacheSurroundingPoints;
            calculateUnitPoints();
            return cacheSurroundingPoints;
        }
        private bool canCalculateStaticUnitPoints() {
            var gameObject = this.gameObject;
            V2.Interfaces.IUnitController iUnitController = this;
            if(this.kindOfEntity == KindsOfEntities.Building ||
                this.kindOfEntity == KindsOfEntities.Resource) {
                cacheSurroundingPoints = V2.Utils.SurroundingPoints.GetPoints(ref gameObject, ref iUnitController);
                return true;
            }
            return false;
        }
        private void calculateUnitPoints() {
            var gameObject = this.gameObject;
            V2.Interfaces.IUnitController iunitController = this;
            var currentPosition = V2.Classes.Grid.grid.getCenteredGridPositionFromWorldPosition(this.transform.position);
            if(lastPosition != currentPosition) {
                lastPosition = currentPosition;
                cacheSurroundingPoints = V2.Utils.SurroundingPoints.GetPoints(ref gameObject, ref iunitController);
            }
        }
    }
}