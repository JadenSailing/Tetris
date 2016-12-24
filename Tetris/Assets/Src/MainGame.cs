using UnityEngine;
using System.Collections;

public class MainGame : MonoBehaviour {

    public TetrisPanel tetrisPanel;
    public UISprite elementRes;
	// Use this for initialization
	void Start () {
        TouchManager.Instance.Init();
        
        TetrisManager.Instance.panel = tetrisPanel;
        TetrisManager.Instance.ElementSpriteRes = elementRes;
        TetrisManager.Instance.Init();
	}
	
	// Update is called once per frame
	void Update () {
        TouchManager.Instance.Update();
        TetrisManager.Instance.Update();
	}
}
