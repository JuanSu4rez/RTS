using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreResourceView : MonoBehaviour{
    private Text _resourceAmount;
    private Func<string> _handler = ()=> "0";
    // Start is called before the first frame update
    void Awake(){
        _resourceAmount = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update(){
        _resourceAmount.text = _handler();
    }

    public void Init(Func<string> handler) {
        _handler = handler;
        enabled = true;
    }
}
