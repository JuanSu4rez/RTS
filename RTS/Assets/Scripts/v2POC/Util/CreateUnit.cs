using UnityEngine;
using System.Collections;
using V2.Interfaces;
using System.Collections.Generic;
using System;

namespace V2.Util
{
    public class CoolDown {
        private float time = 0;
        public CoolDown(float time ) {
            this.time = time;
        }
        public void Update() {
            if(!IsComplete())
            this.time -= Time.deltaTime;
        }
        public bool IsComplete() {
            return this.time > 0;
        }
    }
}