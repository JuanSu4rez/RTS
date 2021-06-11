using UnityEngine;
using V2.Enums;

namespace V2.Controllers
{
    public partial class UnitController : V2.Interfaces.IDamagable, V2.Interfaces.ISelectable, V2.Interfaces.IEntityType , V2.Interfaces.ISurroundingPoints
    {
        [SerializeField]
        private EntityType entityType;
        public EntityType UnitType => entityType;
        [SerializeField]
        private KindsOfEntities kindOfEntity;
        public KindsOfEntities KindOfUnit => kindOfEntity;
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
                if(cacheSurroundingPoints.Length != 0)
                    cacheSurroundingPoints = new Vector3[0];
                return cacheSurroundingPoints;
            }
            var gameObject = this.gameObject;
            V2.Interfaces.IUnitController iUnitController = this;
            switch(this.kindOfEntity) {
                case KindsOfEntities.Building:
                    if(cacheSurroundingPoints == null) {
                        cacheSurroundingPoints = V2.Utils.SurroundingPoints.GetPoints(ref gameObject, ref iUnitController);
                    }
                    return cacheSurroundingPoints;
                    break;
                case KindsOfEntities.Resource:
                    if(cacheSurroundingPoints == null) {
                        cacheSurroundingPoints = V2.Utils.SurroundingPoints.GetPoints(ref gameObject, ref iUnitController);
                    }
                    return cacheSurroundingPoints;
                    break;
            }
  
            var currentPosition = V2.Classes.Grid.grid.getCenteredGridPositionFromWorldPosition(this.transform.position);
            if(lastPosition !=  currentPosition) {
                lastPosition = currentPosition;
                cacheSurroundingPoints = V2.Utils.SurroundingPoints.GetPoints(ref gameObject, ref iUnitController);
            }
            return cacheSurroundingPoints;
        }
    }
}