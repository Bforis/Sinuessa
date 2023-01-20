using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace PVP {
	public class GameManagerHandler : MonoBehaviour {

        public GameManager gameManager;
        [SerializeField]
        public PlayerInputManager playerInputManager;

        private void Awake()
        {
            gameManager.Initialization();
            DontDestroyOnLoad(this);
        }
        
    }
}