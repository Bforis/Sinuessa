using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Spells : MonoBehaviour
{
    // INPUT DETECT
    [SerializeField]
    private PlayerInput playerInput; // Pour détection move/look
    private InputAction Look; // Direction for spells
    public Vector2 look;
    public Vector2 movement;
    //

    // COOLDOWN
    private float LightCooldown; // Cooldown léger
    private float MidCooldown; // Cooldown moyen;
    private float LongCooldown;
    private float NextCDBoules;
    private float NextCDTotem;
    private float NextCDThunder;
    private float NextCDTeleport;
    private float NextCDTornade;
    private float NextCDBolt;
    //

    public static int indexSpells; // Identifications Spells
    private int playerIndex = 0; // GET ID OF PLAYER
    private float speed;
    public bool casting;
    Animator anim;
    private Color playerColor;
   
    
    // CONTACT
    private bool ContactMasterSpell;
    private bool ContactReady;
    //

    // THUNDER
    private bool ActiveThunder = false;
    private bool moveTarget = false;
    public bool inRange = false;
    public float rangeTarget;
    public float LimitRangeTarget = 5;
    //
    private bool moveChargeZone = false;
    // GAME OBJECT
    public GameObject[] Spell; // List of spells
    public GameObject ZoneSpawn; // front of player
    [SerializeField]
    private GameObject TargetZone; // Rond vert
    private GameObject target;
    [SerializeField]
    private GameObject ChargeZone; // Rond vert
    private GameObject chargeTarget;
    private PlayerMovements movements;
    public Transform player;
    //

    void Start(){
        casting = false;
        playerColor = GetComponent<PlayerMovements>().colorPlayer;
    }

    private void Update(){
        ContactMasterSpell = GetComponent<PlayerActions>().ContactSpellMaster;
        ContactReady = GetComponent<PlayerActions>().ContactReady;
        
        DeCast();
    }

    private void FixedUpdate() {
        if (moveTarget == true){ // DETECT IF TARGETZONE EXISTS
        speed = 1000000f;
        if (casting == false){
            casting = true;
        }
        target.transform.Translate(movement / 3f); // speed of target
        rangeTarget = Vector2.Distance(player.transform.position, target.transform.position);
         if (rangeTarget <= LimitRangeTarget){ // DETECT RANGE FOR TARGETZONE
                inRange = true;
                target.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else {
                inRange = false;
                target.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    void Awake(){ 
        
        playerIndex = GameControls.playerIndex; // Get player index from GameControls for identify players
        anim = GetComponent<Animator>();
        LightCooldown = 0.8f;
        MidCooldown = 2f;
        LongCooldown = 5f;
        var direction = look;
        Look = playerInput.actions["Look"];
        Look.performed += (ctx) => { look = ctx.ReadValue<Vector2>(); }; // Convert Look => look = Vector2
    }

        // <========= FONCTION UNIVERSEL ========>
  
    private void Casting(){ // PERMET DE LANCER L'ANIMATION D'UN CAST ET DE BLOQUER LE MOUVEMENT DU JOUEUR EN ATTENDANT LA FONCTION IENUMERATOR WAIT
        if (casting == true){
        speed = 1000000f;
        PlayerMovements movements = GetComponent<PlayerMovements>();
        movements.speed = speed;
        anim.SetBool("Casting", true);
        anim.SetBool("IdleDroite", false);
        anim.SetBool("RunDroite", false);
        }
    }

    private void DeCast(){ // RETOUR NORMAL SPEED & ANIMATIONS
        if (casting == false){
            speed = 12f; // SPEED ACTUEL
            movements = GetComponent<PlayerMovements>();
            movements.speed = speed;
            anim.SetBool("Casting", false);
        }
        else{
            Casting();
        }
    }

    IEnumerator Wait(float sec){ // DONNE UN RETOUR A LA NORMAL ET SIGNE LA FIN DU CAST.
        yield return new WaitForSeconds(sec);
        casting = false;
    }
    
    IEnumerator WaitTarget(float sec){
        yield return new WaitForSeconds(sec);
        moveTarget = true;
    }

    IEnumerator WaitChargeZone(float sec){
        yield return new WaitForSeconds(sec);
        moveChargeZone = true;
    }

    public void OnMove(InputValue value){
        movement = value.Get<Vector2>();
    }

    // >========= FIN FONCTION UNI ============<

    // <========= EXEMPLE POUR LES SORTS A VENIR (PEUT ETRE QU'IL Y AURA DES DIFFERENCES COMME EXEMPLE ZERO TEMPS DE CAST) (EN LIEN AVEC PLAYERACTIONS.CS ET LE DICTIONNAIRE) =========>
    // <<<<<<<< BOULES >>>>>>>>>>

    public void Boules(InputAction input){ // LES PETITES BOULES
        if (input.triggered && Time.time > NextCDBoules && moveTarget == false && ContactReady == false && ContactMasterSpell == false){ // DETECTION DE L'INPUT ET TEMPS D'ATTENTE ENTRE LES TIRS/LES CAST
        casting = true; // DONNE L'AUTORISATION DE LANCER LE CAST
        StartCoroutine(CastingBoules()); // LANCEMENT DU CAST EN LIEN AVEC L'ENUMERATOR JUSTE EN DESSOUS, SYNCHRONISE LE LANCEMENT AVEC L'ANIMATION
        NextCDBoules = Time.time + LightCooldown; // COOLDOWN ENTRE LES TIRS/CAST
        StartCoroutine(Wait(LightCooldown)); // CAST = FALSE, PERMET DE REVENIR A LA NORMAL
        Casting();
        }
    } 
    IEnumerator CastingBoules(){ // COMPORTEMENT DU SORT
        yield return new WaitForSeconds(0.5f); // SYNCRHONISATION DU TIR AVEC L'ANIMATION
        GameObject newSpell = Instantiate(Spell[0], ZoneSpawn.transform.position, Quaternion.identity); // CREER LA BOULE
        newSpell.GetComponent<Rigidbody2D>().velocity = look.normalized * 15f; // LANCE LA BOULE DANS LA DIRECTION DU JOYSTICK AVEC UNE VITESSE CONSTANTE
        newSpell.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg); // PERMET DE TOURNER LE SPRITE DE LA BOULE DANS LE BON SENS (PAS TROP NECESSAIRE DE CE CAS LA)
        newSpell.GetComponent<boule>().playerIndex = playerIndex; // IDENTIFICATION OWNER AND ENEMY (VOIR boule.cs 21)
    }
    // >>>>>>> FIN BOULES <<<<<<<<<<

    // <<<<< TOTEMDEF >>>>>>

    public void TotemDef(InputAction input){ // DEMONS MID-RANGE/MELEE
        if (input.triggered && Time.time > NextCDTotem && moveTarget == false && ContactReady == false && ContactMasterSpell == false){
        casting = true;
        GameObject totem = Instantiate(Spell[1], ZoneSpawn.transform.position, Quaternion.identity); // CREER LA BOULE
        NextCDTotem = Time.time + 15f;
        totem.GetComponent<totemDef>().playerIndex = playerIndex;
        StartCoroutine(Wait(0.1f));
        Casting();
    }
    }
    // >>>>>> FIN TOTEMDEF <<<<<<

    // <<<<<<<<< THUNDER >>>>>>>>>>>>>

    public void Thunder(InputAction input){
        if (input.triggered && Time.time > NextCDThunder && moveTarget == false && ActiveThunder == false && ContactReady == false && ContactMasterSpell == false){
            casting = true;
            ActiveThunder = false;
            Destroy(target);
            StartCoroutine(WaitTarget(0.05f));
            GameObject targetZone = Instantiate(TargetZone, transform.position, Quaternion.identity);
            NextCDThunder = Time.time + MidCooldown;
            target = targetZone;
            Casting();
        }
        if (input.triggered && moveTarget == true && inRange == true){
            StartCoroutine(CastingThunder(target));
            StartCoroutine(Wait(0.05F));
            } 
    }

    IEnumerator CastingThunder(GameObject go){
        inRange = false;
        go.GetComponent<SpriteRenderer>().enabled = false;
        moveTarget = false;
        for (var i = 1; i < 10; i++){
        ActiveThunder = true;
        if (ActiveThunder == true){
        yield return new WaitForSeconds(0.2f);
        var position = Random.insideUnitSphere * 2;
        GameObject Thunder = Instantiate(Spell[2], position + go.transform.position, Quaternion.identity);
        Thunder.GetComponent<thunder>().playerIndex = playerIndex;
        }
        }
        ActiveThunder = false;
    }

    // >>>>>>> FIN THUNDER <<<<<<<<<<

    // <<<<<<<< TORNADE >>>>>>>>>>>

    public void Tornade(InputAction input){ 
        if (input.triggered && Time.time > NextCDTornade && moveTarget == false && ContactReady == false && ContactMasterSpell == false){ // DETECTION DE L'INPUT ET TEMPS D'ATTENTE ENTRE LES TIRS/LES CAST
        casting = true; // DONNE L'AUTORISATION DE LANCER LE CAST
        StartCoroutine(CastingTornades()); // LANCEMENT DU CAST EN LIEN AVEC L'ENUMERATOR JUSTE EN DESSOUS, SYNCHRONISE LE LANCEMENT AVEC L'ANIMATION
        NextCDTornade = Time.time + MidCooldown; // COOLDOWN ENTRE LES TIRS/CAST
        StartCoroutine(Wait(LightCooldown)); // CAST = FALSE, PERMET DE REVENIR A LA NORMAL
        Casting();
        }
    } 
    IEnumerator CastingTornades(){ // COMPORTEMENT DU SORT
        yield return new WaitForSeconds(0.5f); // SYNCRHONISATION DU TIR AVEC L'ANIMATION
        GameObject newSpell = Instantiate(Spell[3], ZoneSpawn.transform.position, Quaternion.identity); // CREER LA BOULE
        newSpell.GetComponent<Rigidbody2D>().velocity = look.normalized * 7f; // LANCE LA BOULE DANS LA DIRECTION DU JOYSTICK AVEC UNE VITESSE CONSTANTE
        //newSpell.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg); // PERMET DE TOURNER LE SPRITE DE LA BOULE DANS LE BON SENS (PAS TROP NECESSAIRE DE CE CAS LA)
        newSpell.GetComponent<tornade>().playerIndex = playerIndex; // IDENTIFICATION OWNER AND ENEMY (VOIR boule.cs 21)
    }
    // >>>>>>>> FIN TORNADE <<<<<<<<<<

    // >>>>>>>> BOLT <<<<<<<<<<<<

    public void Bolt(InputAction input){
        if (input.triggered && Time.time > NextCDBolt && moveTarget == false && ContactReady == false && ContactMasterSpell == false){
            casting = true;
            Destroy(chargeTarget);
            StartCoroutine(WaitChargeZone(0.05f));
            GameObject chargeZone = Instantiate(ChargeZone, transform.position, Quaternion.identity);
            chargeZone.GetComponent<Rigidbody2D>().velocity = look.normalized * 8f; // LANCE LA BOULE DANS LA DIRECTION DU JOYSTICK AVEC UNE VITESSE CONSTANTE
            NextCDBolt = Time.time + MidCooldown;
            chargeTarget = chargeZone;
            Casting();
            StartCoroutine(Wait(0.05F));
            StartCoroutine(DestroyChargeTarget(chargeZone));
        }
        if (input.triggered && moveChargeZone == true){
            CastingBolt(chargeTarget);
            }
    }

    void CastingBolt(GameObject go){
        go.GetComponent<SpriteRenderer>().enabled = false;
        moveChargeZone = false;
        GameObject bolt = Instantiate(Spell[4], go.transform.position, Quaternion.identity);
        bolt.GetComponent<bolt>().playerIndex = playerIndex;
    }

    IEnumerator DestroyChargeTarget(GameObject go){
        yield return new WaitForSeconds(1f);
        moveChargeZone = false;
        Destroy(go);
    }

    // >>>>>>>> FIN BOLT <<<<<<<<<<

    // <<<<<<<<<< TELEPORT >>>>>>>>>>

    public void Teleport(InputAction input){
        if (input.triggered && Time.time > NextCDTeleport && ContactReady == false && ContactMasterSpell == false){
            transform.position += (new Vector3(look.x, look.y, 0)) * 4f;
            NextCDTeleport = Time.time + MidCooldown;
        }
    }

    // >>>>>>>> FIN TELEPORT <<<<<<<<
}
