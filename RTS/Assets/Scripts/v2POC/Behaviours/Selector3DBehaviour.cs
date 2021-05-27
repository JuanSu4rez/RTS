﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace V2.Behaviours
{
    public class Selector3DBehaviour : MonoBehaviour
    {
        private List<GameObject> selection = new List<GameObject>();
        int lastselection = 0;
        // Use this for initialization
        void Start() {
        }
        // Update is called once per frame
        void Update() {
            if(lastselection != selection.Count) 
            {
                lastselection = selection.Count;
                Debug.Log("selection " + lastselection);
            }
        }
        void OnCollisionEnter(Collision col) {
            bool add = false;
            switch(col.gameObject.tag) {
                case "Citizen":
                    add = true;
                    break;
                case "Military":
                    add = true;
                    break;
            }
            if(add) {
                selection.Add(col.gameObject);
                var iselectable = col.gameObject.GetComponent<ISelectable>();
                if(iselectable != null)
                    iselectable.IsSelected = true;
            }
        }

        void OnCollisionExit(Collision col) {
            bool add = false;
            switch(col.gameObject.tag) {
                case "Citizen":
                    add = true;
                    break;
                case "Military":
                    add = true;
                    break;
            }
            if(add) {
                selection.Remove(col.gameObject);
                var iselectable = col.gameObject.GetComponent<ISelectable>();
                if(iselectable != null)
                    iselectable.IsSelected = false;
            }

        }
    }
}