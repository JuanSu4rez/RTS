using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using V2.Enums;
using System;

namespace V2.Controllers
{
    public partial class UnitController
    {
        private int _positions = 0;
        public int AssingPosition() {
            bool indexWasFound = false;
            int index = 0;
            for(; index < cacheSurroundingPoints.Length && !indexWasFound;  index++  ) {
                if(!IsBitSet(index)) {
                    indexWasFound = true;
                    _positions = _positions | ( 1 << index );
                }
            }
            //due the last increment in the for
            index--;
            if(!indexWasFound) {
                index = -1;
            }
            Debug.Log("_positions " + Convert.ToString(_positions, 2));
            return index;
        }
        private bool IsBitSet(int pos) {
            return ( _positions & ( _positions << pos ) ) != 0;
        }
        internal void ReleasePosition(int indexPosition) {
            _positions &= ~( 1 << indexPosition );
        }
        private Vector3[] cacheSurroundingPoints = null;
        public Vector3[] GetSurroundingPoints() {
            if(!canCalculateStaticUnitPoints())
               calculateUnitPoints();
            return cacheSurroundingPoints;
        }
        private bool canCalculateStaticUnitPoints() {
            var gameObject = this.gameObject;
            V2.Interfaces.IUnitController iUnitController = this;
            if(( this.kindOfEntity == KindsOfEntities.Building ||
                this.kindOfEntity == KindsOfEntities.Resource ) &&
                cacheSurroundingPoints == null) {
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