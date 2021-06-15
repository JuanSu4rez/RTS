using UnityEngine;
using System.Collections;
using V2.Tasks;
using V2.Interfaces;
using V2.Tasks.Unit;
using V2.Interfaces.Task;
using System;
using V2.Tasks.Unit.Citizen;
using V2.Enums;

namespace V2.Controllers
{
    public partial class UnitController : MonoBehaviour, IUnitController,IHealthPoint,ITeam{
        private ITask toDo;
        [SerializeField]
        private float _currentHealth;
        public float CurrentHealth {
            get => _currentHealth;
            set => _currentHealth = value;
        }
        [SerializeField]
        private float _maxHealth;
        public float MaxHealth {
            get => this._maxHealth;
            set {
                if(KindOfEntity == Enums.KindsOfEntities.Resource){
                    _maxHealth = value;
                }
            }
        }
        [SerializeField]
        private TeamsList _team;
        public TeamsList Team { get => _team; set => _team = value; }
        void Start() {
            AssingTask(DoingNothing.nothing);
            TagValidation();
            InitLastCenteredPosition();
            SetEntityObject();
            InitEntityData();
        }
        void Update() {
            //Debug.Log(Time.time + " " +this.GetType());
            if( IsAlive() &&
                toDo != null &&
                toDo as DoingNothing == null) {
                toDo.Update();
                if(toDo.IsComplete()) {
                    toDo = null;
                    AssingTask(DoingNothing.nothing);
                }
            }
        }
        public void AssingTask(ITask task) {
            toDo = task;
            if(!( task is DoingNothing ) &&
                 toDo.GameObject != this.gameObject) 
            { 
                toDo.GameObject = this.gameObject;
            }
        }
        public ITask GetTask() {
            return toDo;
        }
        public bool IsAlive() {
            return CurrentHealth > 0;
        }
    }
}