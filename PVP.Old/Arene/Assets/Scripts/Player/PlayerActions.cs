using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
public class PlayerActions : MonoBehaviour
{
    // MENU
    [SerializeField]
    private GameObject Menu;
    public GameObject MenuP2;
    public GameObject MenuP3;
    public GameObject MenuP4;
    [SerializeField]
    private EventSystem eventSystem;
    public bool ContactSpellMaster;
    [SerializeField]
    public bool ContactReady = false;
    private bool inMenu = false;
    Dropdown dropdown;
    //

    [SerializeField]
    private int playerIndex = 0; // GET NUMBER OF PLAYER
    public static int ready = 0; // GET NUMBER OF PLAYER READY

    [SerializeField]
    private bool already = false;
    public static bool launch = false;
    private bool RESET;
    private bool BeginFight = false;

     // SPAWNPOINT
    public GameObject P1Spawn;
    public GameObject P2Spawn;
    public GameObject P3Spawn;
    public GameObject P4Spawn;
    //

    // INPUT
    [SerializeField]
    private PlayerInput playerInput; // GET PLAYER INPUT 

    private InputAction PressA;
    private bool aPress;
    private InputAction PressX;
    private bool xPress;
    private InputAction PressY;
    private bool yPress;
    private InputAction PressB;
    private bool bPress;
    private InputAction Look;
    public Vector2 look;
    private InputAction Move;
    //

    public static bool forceSingleInstance = true; // ???
    public Dictionary<InputAction, int> SpellInput;
    Animator anim;
    public GameObject indicator;

    void Awake(){
        DontDestroyOnLoad(this.gameObject); // don't destroy players
        playerIndex = GameControls.playerIndex; // Get player index from GameControls for identify players 
        // GET ACTIONS FROM PLAYER INPUT IN EDITOR
        PressA = playerInput.actions["PressA"];
        PressX = playerInput.actions["PressX"];
        PressY = playerInput.actions["PressY"];
        PressB = playerInput.actions["PressB"];
        Look = playerInput.actions["Look"];
        Move = playerInput.actions["Move"];

        anim = GetComponent<Animator>();

        // DO ACTIONS
        Look.performed += (ctx) => { look = ctx.ReadValue<Vector2>(); };

        // START POSITION && POSITION OF BEGINOFFIGHT();
        if (playerIndex == 1){
            this.gameObject.transform.position = P1Spawn.transform.position;
        }
        if (playerIndex == 2){
            this.gameObject.transform.position = P2Spawn.transform.position;
        }
        if (playerIndex == 3){
            this.gameObject.transform.position = P3Spawn.transform.position;
        }
        if (playerIndex == 4){
            this.gameObject.transform.position = P4Spawn.transform.position;
        }
        //
    }

    public void Start(){
        SpellInput = new Dictionary<InputAction, int>(); // Dictionary actions/spells - input actions
    }

    public void Update(){

        RESET = GameControls.RESET;
        // RESTART();

         foreach (KeyValuePair<InputAction, int> spell in SpellInput){ // parcourir dictionary actions/spell - input actions
            if (inMenu == false){
                //> ==== Animation spell cast sur cette ligne ==== <
                if (spell.Value == 1){ // Index 1 : Boules 
                GetComponent<Spells>().Boules(spell.Key);
            }
                if (spell.Value == 2){
                    GetComponent<Spells>().TotemDef(spell.Key);
                }
                if (spell.Value == 3){
                    GetComponent<Spells>().Thunder(spell.Key);
                }
                if (spell.Value == 4){
                    GetComponent<Spells>().Teleport(spell.Key);
                }
                if (spell.Value == 5){
                    GetComponent<Spells>().Tornade(spell.Key);
                }
                 if (spell.Value == 6){
                    GetComponent<Spells>().Bolt(spell.Key);
                }
            }
        }

        if (PressA.triggered){ // DETECT A 
            aPress = true;
        }
        else{
           aPress = false;
        }
        // OPEN MENU
        if (ContactSpellMaster == true && aPress == true && inMenu == false){ 
            inMenu = true;
            if (playerIndex == 2){
                Menu.transform.position = MenuP2.transform.position;
            }
            if (playerIndex == 3){
                Menu.transform.position = MenuP3.transform.position;
            }
            if (playerIndex == 4){
                Menu.transform.position = MenuP4.transform.position;
            }
            Menu.SetActive(true);
           //
        }
        if (ContactSpellMaster == false){
                ExitMenu();
        }

        if (inMenu == true){
            GetComponent<PlayerMovements>().enabled = false;
            anim.SetBool("RunDroite", false);
            anim.SetBool("IdleDroite", true);
            if (PressB.triggered){
                ExitMenu();
            }
        }

        BeginOfFight();

        if (ContactReady == true && aPress == true && already == false){
            already = true;
            ready += 1;
        }

        if (GameControls.scene.buildIndex == 1){
            ExitMenu();
            ContactReady = false; // RESET TOUT
            ContactSpellMaster = false;
            already = false;
            ready = 0;
        }
        if (GameControls.scene.buildIndex == 0){
            BeginFight = false; // PERMET DE RESET LES POSITIONS POUR LE MATCH SUIVANT (VOIR BEGINOFFIGHT());
        }

        // INDICATOR DIRECTION BELOW THE PLAYER
        var angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        angle -= 90;
        indicator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void ExitMenu(){ // EXIT MENU
        Menu.SetActive(false);
        GetComponent<PlayerMovements>().enabled = true;
        StartCoroutine(WaitMenu());
        inMenu = false;
    }
    //

    private void BeginOfFight(){
        if (BeginFight == false && GameControls.scene.buildIndex == 1){
            // START POSITION
        if (playerIndex == 1){
            this.gameObject.transform.position = P1Spawn.transform.position;
        }
        if (playerIndex == 2){
            this.gameObject.transform.position = P2Spawn.transform.position;
        }
        if (playerIndex == 3){
            this.gameObject.transform.position = P3Spawn.transform.position;
        }
        if (playerIndex == 4){
            this.gameObject.transform.position = P4Spawn.transform.position;
        }
        BeginFight = true;
        //
        }
    }

    IEnumerator WaitMenu(){ // Disable A pour empêcher de relancer directement le menu et de ne pas pouvoir le fermer
        PressA.Disable();
        yield return new WaitForSeconds(0.2f);
        PressA.Enable();
    }
    //

    // COLLIDER GROUP
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "NPC"){ // NPC SPELL
           ContactSpellMaster = true;
        }
        if (collider.gameObject.tag == "NPC2"){
            ContactReady = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "NPC"){ // NPC SPELL CLOSE
            ContactSpellMaster = false;
        }
        if (collider.gameObject.tag == "NPC2"){
            ContactReady = false;
        }
    }

    // mandatory
    void OnEnable() {
        PressA.Enable();
        PressX.Enable();
        PressY.Enable();
        PressB.Enable();
        Look.Enable();
        Move.Enable();
    }

    void OnDisable(){
        PressA.Disable();
        PressX.Disable();
        PressY.Disable();
        PressB.Disable();
        Look.Disable();
        Move.Disable();
    }
    //

    public void OnPlayerJoined(){ // Attribut l'input system du joueur (1, 2, 3...) à l'event system.
        eventSystem.GetComponent<InputSystemUIInputModule>().actionsAsset = playerInput.actions;
        playerInput.uiInputModule = eventSystem.GetComponent<InputSystemUIInputModule>();
    }

    //////// BOULES ////////

    public void DropdownY(Dropdown dd){ // FOR Y BUTTON
        int index = dd.value;
        switch (index){
            case 0:
               break;
            case 1:
            if (SpellInput.ContainsKey(PressY)){
                SpellInput[PressY] = 1;
            }
            else{
                SpellInput.Add(PressY, 1);
            }
            break;
            case 2:
            if (SpellInput.ContainsKey(PressY)){
                SpellInput[PressY] = 2;
            }
            else{
                SpellInput.Add(PressY, 2);
            }
            break;
            case 3:
            if (SpellInput.ContainsKey(PressY)){
                SpellInput[PressY] = 3;
            }
            else{
                SpellInput.Add(PressY, 3);
            }
            break;
            case 4:
            if (SpellInput.ContainsKey(PressY)){
                SpellInput[PressY] = 4;
            }
            else{
                SpellInput.Add(PressY, 4);
            }
            break;
            case 5:
            if (SpellInput.ContainsKey(PressY)){
                SpellInput[PressY] = 5;
            }
            else{
                SpellInput.Add(PressY, 5);
            }
            break;
            case 6:
            if (SpellInput.ContainsKey(PressY)){
                SpellInput[PressY] = 6;
            }
            else{
                SpellInput.Add(PressY, 6);
            }
            break;
        }
    }

    public void DropdownX(Dropdown dd){ // FOR X BUTTON
        int index = dd.value;
        switch (index){
            case 0:
               break;
            case 1:
            if (SpellInput.ContainsKey(PressX)){
                SpellInput[PressX] = 1;
            }
            else{
                SpellInput.Add(PressX, 1);
            }
            break;
            case 2:
            if (SpellInput.ContainsKey(PressX)){
                SpellInput[PressX] = 2;
            }
            else{
                SpellInput.Add(PressX, 2);
            }
            break;
            case 3:
            if (SpellInput.ContainsKey(PressX)){
                SpellInput[PressX] = 3;
            }
            else{
                SpellInput.Add(PressX, 3);
            }
            break;
            case 4:
            if (SpellInput.ContainsKey(PressX)){
                SpellInput[PressX] = 4;
            }
            else{
                SpellInput.Add(PressX, 4);
            }
            break;
            case 5:
            if (SpellInput.ContainsKey(PressX)){
                SpellInput[PressX] = 5;
            }
            else{
                SpellInput.Add(PressX, 5);
            }
            break;
            case 6:
            if (SpellInput.ContainsKey(PressX)){
                SpellInput[PressX] = 6;
            }
            else{
                SpellInput.Add(PressX, 6);
            }
            break;
        }
    }

    public void DropdownA(Dropdown dd){ // FOR A BUTTON
        int index = dd.value;
        switch (index){
            case 0:
               break;
            case 1:
            if (SpellInput.ContainsKey(PressA)){
                SpellInput[PressA] = 1;
            }
            else{
                SpellInput.Add(PressA, 1);
            }
            break;
            case 2:
            if (SpellInput.ContainsKey(PressA)){
                SpellInput[PressA] = 2;
            }
            else{
                SpellInput.Add(PressA, 2);
            }
            break;
            case 3:
            if (SpellInput.ContainsKey(PressA)){
                SpellInput[PressA] = 3;
            }
            else{
                SpellInput.Add(PressA, 3);
            }
            break;
            case 4:
            if (SpellInput.ContainsKey(PressA)){
                SpellInput[PressA] = 4;
            }
            else{
                SpellInput.Add(PressA, 4);
            }
            break;
            case 5:
            if (SpellInput.ContainsKey(PressA)){
                SpellInput[PressA] = 5;
            }
            else{
                SpellInput.Add(PressA, 5);
            }
            break;
            case 6:
            if (SpellInput.ContainsKey(PressA)){
                SpellInput[PressA] = 6;
            }
            else{
                SpellInput.Add(PressA, 6);
            }
            break;
        }
    }

    public void DropdownB(Dropdown dd){ // FOR B BUTTON
        int index = dd.value;
        switch (index){
            case 0:
               break;
            case 1:
            if (SpellInput.ContainsKey(PressB)){
                SpellInput[PressB] = 1;
            }
            else{
                SpellInput.Add(PressB, 1);
            }
            break;
            case 2:
            if (SpellInput.ContainsKey(PressB)){
                SpellInput[PressB] = 2;
            }
            else{
                SpellInput.Add(PressB, 2);
            }
            break;
            case 3:
            if (SpellInput.ContainsKey(PressB)){
                SpellInput[PressB] = 3;
            }
            else{
                SpellInput.Add(PressB, 3);
            }
            break;
            case 4:
            if (SpellInput.ContainsKey(PressB)){
                SpellInput[PressB] = 4;
            }
            else{
                SpellInput.Add(PressB, 4);
            }
            break;
            case 5:
            if (SpellInput.ContainsKey(PressB)){
                SpellInput[PressB] = 5;
            }
            else{
                SpellInput.Add(PressB, 5);
            }
            break;
            case 6:
            if (SpellInput.ContainsKey(PressB)){
                SpellInput[PressB] = 6;
            }
            else{
                SpellInput.Add(PressB, 6);
            }
            break;
        }
    }
}