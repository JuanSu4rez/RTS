using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Enums;

namespace V2.Behaviours
{
    public class ResourceBehaviour : MonoBehaviour, V2.Interfaces.IResource, V2.Interfaces.IHealthPoint, V2.Interfaces.IDamagable
    {
        [SerializeField]
        private float _currentHealth;
        public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

        [SerializeField]
        private ResourceTypes resourceType;

        public ResourceTypes GetResource() {
            return resourceType;
        }

        // Use this for initialization
        void Start() {

            this.enabled = false;
        }

        // Update is called once per frame
        void Update() {

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