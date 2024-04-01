using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using FishNet.Object;
using FishNet;

public class placeWall : NetworkBehaviour
{
    [Header("Utility Objects")]
    public GameObject[] util;
    public GameObject selectedUtilUI;
    public GameObject woodWallUI;
    public GameObject concreteWallUI;
    public GameObject ElectricFenceUI;
    public GameObject turretUI;
    public GameObject armoredTurretUI;
    public GameObject frenzyTurretUI;
    public GameObject shotgunTurretUI;
    public GameObject sniperTurretUI;
    public GameObject mudUI;
    public GameObject acidUI;
    public GameObject mineUI;
    public GameObject flashMineUI;
    public GameObject acidMineUI;
    public GameObject decoyMineUI;
    public int utilSlot = 0;
    public int utilCategory = 0;

    public float rotationSpeed;

    [Header("Utility Cooldowns")]
    public float woodWallCoolDown;
    public float concreteWallCoolDown;
    public float electricFenceCoolDown;
    public float basicTurretCoolDown;
    public float armoredTurretCoolDown;
    public float frenzyTurretCoolDown;
    public float shotgunTurretCoolDown;
    public float sniperTurretCoolDown;
    public float mudCooldown;
    public float acidCooldown;
    public float explosiveMineCoolDown;
    public float flashMineCoolDown;
    public float acidMineCoolDown;
    public float decoyMineCoolDown;
    float lastWoodWallUse;
    float lastConcreteWallUse;
    float lastElectricFenceUse;
    float lastBasicTurretUse;
    float lastArmoredTurretUse;
    float lastFrenzyTurretUse;
    float lastShotgunTurretUse;
    float lastSniperTurretUse;
    float lastMudUse;
    float lastAcidUse;
    float lastExplosiveMineUse;
    float lastFlashMineUse;
    float lastAcidMineUse;
    float lastDecoyMineUse;

    bool firstWoodWallUse=true;
    bool firstConcreteWallUse=true;
    bool firstElectricFenceUse=true;
    bool firstBasicTurretUse=true;
    bool firstArmoredTurretUse=true;
    bool firstFrenzyTurretUse=true;
    bool firstShotgunTurretUse=true;
    bool firstSniperTurretUse=true;
    bool firstMudUse=true;
    bool firstAcidUse=true;
    bool firstExplosiveMineUse=true;
    bool firstFlashMineUse=true;
    bool firstAcidMineUse=true;
    bool firstDecoyMineUse=true;

    public Text cooldownUI;
    string woodWallState = "READY";
    string concreteWallState = "READY";
    string electricFenceState = "READY";
    string basicTurretState = "READY";
    string armoredTurretState = "READY";
    string frenzyTurretState = "READY";
    string shotgunTurretState = "READY";
    string sniperTurretState = "READY";
    string mudState = "READY";
    string acidState = "READY";
    string explosiveMineState = "READY";
    string flashMineState = "READY";
    string acidMineState = "READY";
    string decoyMineState = "READY";


    float savedRotation = 0f;
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    public Vector3 correctedWorldPosition;
    public LayerMask layersToHit;
    // Start is called before the first frame update
    void Start()
    {
        //sets woodWall placement rotation at start
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        selectedUtilUI.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!base.IsOwner)
            return;

        //converts mouse position to a world position through camera
        screenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if(Physics.Raycast(ray, out RaycastHit hitData, 500, layersToHit))
        {
            worldPosition= hitData.point;
            
        }
        transform.position= worldPosition;
        selectedUtilUI.transform.position = worldPosition;
        selectedUtilUI.transform.rotation = Quaternion.Euler(0, savedRotation, 0);

        //selectedUtilUI = util[utilSlot];
        //object selection
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            utilCategory = 0;
            utilSlot = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            utilCategory = 1;
            utilSlot = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            utilCategory=2;
            utilSlot = 8;
        }
        if (Input.mouseScrollDelta.y < 0 && !Input.GetKey(KeyCode.LeftControl))
        {
            //utilSlot++;
            if (utilCategory == 0)
            {
                utilSlot++;
                utilSlot = utilSlot % 3;
            }
            if(utilCategory == 1)
            {
                utilSlot++;
                if (utilSlot > 7)
                {
                    utilSlot = 3;
                }
                utilSlot = utilSlot % 8;
            }
            if (utilCategory == 2)
            {
                utilSlot++;
                if (utilSlot > 13)
                {
                    utilSlot = 8;
                }
                utilSlot = (utilSlot % 14);
            }
            //utilSlot = utilSlot % 11;
        }
        if (Input.mouseScrollDelta.y > 0 && !Input.GetKey(KeyCode.LeftControl))
        {
            //utilSlot--;
            if (utilCategory == 0)
            {
                utilSlot--;
                if (utilSlot < 0)
                {
                    utilSlot += 3;
                }

                utilSlot = utilSlot % 3;
            }
            if(utilCategory == 1) {
                utilSlot--;
                if(utilSlot < 3)
                {
                    utilSlot = 7;
                }
                utilSlot = (utilSlot % 8);
            }
            if(utilCategory == 2)
            {
                utilSlot--;
                if (utilSlot < 8)
                {
                    utilSlot = 13;
                }
                utilSlot = utilSlot % 14;
            }

            /*
            if (utilSlot < 0)
            {
                utilSlot += 11;
            }
            utilSlot = utilSlot % 11;
        */
        }
        if(utilSlot==0)
        {
            selectedUtilUI = woodWallUI;
            woodWallUI.SetActive(true);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive(false);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(false);   
            decoyMineUI.SetActive(false);
        }
        if (utilSlot==1)
        {
            selectedUtilUI = concreteWallUI;
            //   selectedUtilUI = util[1];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(true);
            ElectricFenceUI.SetActive (false);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(false);
            decoyMineUI.SetActive(false);
        }

        if (utilSlot == 2)
        {
            selectedUtilUI = ElectricFenceUI;
            //   selectedUtilUI = util[2];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive(true);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(false);
            decoyMineUI.SetActive(false);
        }

        if (utilSlot==3)
        {
            selectedUtilUI = turretUI;
            //   selectedUtilUI = util[2];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive(false);
            turretUI.SetActive(true);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(false);
            decoyMineUI.SetActive(false);
        }

        if (utilSlot == 4)
        {
            selectedUtilUI = armoredTurretUI;
            //   selectedUtilUI = util[2];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive(false);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(true);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(false);
            decoyMineUI.SetActive(false);
        }

        if (utilSlot == 5)
        {
            selectedUtilUI = frenzyTurretUI;
            //   selectedUtilUI = util[2];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive(false);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(true);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(false);
            decoyMineUI.SetActive(false);
        }

        if (utilSlot == 6)
        {
            selectedUtilUI = shotgunTurretUI;
            //   selectedUtilUI = util[2];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive(false);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(true);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(false);
            decoyMineUI.SetActive(false);
        }

        if (utilSlot == 7)
        {
            selectedUtilUI = sniperTurretUI;
            //   selectedUtilUI = util[2];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive(false);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(true);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(false);
            decoyMineUI.SetActive(false);
        }
        if (utilSlot==8)
        {
            selectedUtilUI = mudUI;
            //  selectedUtilUI = util[3];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive (false);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(true);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(false);
            decoyMineUI.SetActive(false);
        }
        if (utilSlot==9)
        {
            selectedUtilUI = acidUI;
            //  selectedUtilUI = util[4];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive(false);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(true);
            mineUI.SetActive(false);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(false);
            decoyMineUI.SetActive(false);
        }
        if (utilSlot==10)
        {
            selectedUtilUI = mineUI;
            //   selectedUtilUI = util[5];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive (false);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(true);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(false);
            decoyMineUI.SetActive(false);
        }
        if (utilSlot == 11)
        {
            selectedUtilUI = flashMineUI;
            //   selectedUtilUI = util[5];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive(false);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
            flashMineUI.SetActive(true);
            acidMineUI.SetActive(false);
            decoyMineUI.SetActive(false);
        }
        if (utilSlot == 12)
        {
            selectedUtilUI = acidMineUI;
            //   selectedUtilUI = util[5];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive(false);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(true);
            decoyMineUI.SetActive(false);
        }
        if (utilSlot == 13)
        {
            selectedUtilUI = decoyMineUI;
            //   selectedUtilUI = util[5];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            ElectricFenceUI.SetActive(false);
            turretUI.SetActive(false);
            armoredTurretUI.SetActive(false);
            frenzyTurretUI.SetActive(false);
            shotgunTurretUI.SetActive(false);
            sniperTurretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
            flashMineUI.SetActive(false);
            acidMineUI.SetActive(false);
            decoyMineUI.SetActive(true);
        }
        /* if (Input.GetKeyDown(KeyCode.Alpha1))
     {
         utilSlot = 0;
         selectedUtilUI= woodWallUI;
         woodWallUI.SetActive(true);
         concreteWallUI.SetActive(false);
         turretUI.SetActive(false);
         mudUI.SetActive(false);
         acidUI.SetActive(false);
         mineUI.SetActive(false);
     }
     if (Input.GetKeyDown(KeyCode.Alpha2))
     {
         utilSlot = 1;
         selectedUtilUI = concreteWallUI;
         //   selectedUtilUI = util[1];
         woodWallUI.SetActive(false);
         concreteWallUI.SetActive(true);
         turretUI.SetActive(false);
         mudUI.SetActive(false);
         acidUI.SetActive(false);
         mineUI.SetActive(false);
     }
     if (Input.GetKeyDown(KeyCode.Alpha3))
     {
         utilSlot = 2;
         selectedUtilUI = turretUI;
         //   selectedUtilUI = util[2];
         woodWallUI.SetActive(false);
         concreteWallUI.SetActive(false);
         turretUI.SetActive(true);
         mudUI.SetActive(false);
         acidUI.SetActive(false);
         mineUI.SetActive(false);
     }
     if (Input.GetKeyDown(KeyCode.Alpha4))
     {
         utilSlot = 3;
         selectedUtilUI = mudUI;
         //  selectedUtilUI = util[3];
         woodWallUI.SetActive(false);
         concreteWallUI.SetActive(false);
         turretUI.SetActive(false);
         mudUI.SetActive(true);
         acidUI.SetActive(false);
         mineUI.SetActive(false);
     }
     if (Input.GetKeyDown(KeyCode.Alpha5))
     {
         utilSlot = 4;
         selectedUtilUI = acidUI;
         //  selectedUtilUI = util[4];
         woodWallUI.SetActive(false);
         concreteWallUI.SetActive(false);
         turretUI.SetActive(false);
         mudUI.SetActive(false);
         acidUI.SetActive(true);
         mineUI.SetActive(false);
     }
     if (Input.GetKeyDown(KeyCode.Alpha6))
     {
         utilSlot = 5;
         selectedUtilUI = mineUI;
         //   selectedUtilUI = util[5];
         woodWallUI.SetActive(false);
         concreteWallUI.SetActive(false);
         turretUI.SetActive(false);
         mudUI.SetActive(false);
         acidUI.SetActive(false);
         mineUI.SetActive(true);
     }
        */
        CheckCooldowns();
        //on left click place a woodWall
        CheckPlacement(utilSlot);
       
        //rotate woodWall clockwise
        if (Input.GetKey(KeyCode.E))
        {
            //transform.Rotate(0.0f, 15.0f, 0.0f, Space.World);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                selectedUtilUI.transform.Rotate(0.0f, rotationSpeed * Time.deltaTime * 100f, 0.0f, Space.World);
                savedRotation += rotationSpeed;
            }
            selectedUtilUI.transform.Rotate(0.0f, rotationSpeed * Time.deltaTime * 100f, 0.0f, Space.World);
            savedRotation += rotationSpeed;

        }
        //rotate woodWall counterclockwise
        if (Input.GetKey(KeyCode.Q))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                selectedUtilUI.transform.Rotate(0.0f, -rotationSpeed * Time.deltaTime * 100f, 0.0f, Space.World);
                savedRotation -= rotationSpeed;
            }
            //transform.Rotate(0.0f, -15.0f, 0.0f, Space.World);
            selectedUtilUI.transform.Rotate(0.0f, -rotationSpeed * Time.deltaTime * 100f, 0.0f, Space.World);
            savedRotation -= rotationSpeed;
            
        }
    }
    public void CheckPlacement(int utilSlot)
    {
        if (utilSlot == 0)
        {
            if (Input.GetMouseButtonDown(0) && (lastWoodWallUse + woodWallCoolDown < Time.time || firstWoodWallUse))
            {
                firstWoodWallUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastWoodWallUse = Time.time;

            }
        }
        if (utilSlot == 1)
        {
            if (Input.GetMouseButtonDown(0) && (lastConcreteWallUse + concreteWallCoolDown < Time.time || firstConcreteWallUse))
            {
                firstConcreteWallUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastConcreteWallUse = Time.time;

            }
        }
        if (utilSlot == 2)
        {
            if (Input.GetMouseButtonDown(0) && (lastElectricFenceUse + electricFenceCoolDown < Time.time || firstElectricFenceUse))
            {
                firstElectricFenceUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastElectricFenceUse = Time.time;

            }
        }
        if (utilSlot == 3)
        {
            if (Input.GetMouseButtonDown(0) && (lastBasicTurretUse + basicTurretCoolDown < Time.time || firstBasicTurretUse))
            {
                firstBasicTurretUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastBasicTurretUse = Time.time;

            }
        }
        if (utilSlot == 4)
        {
            if (Input.GetMouseButtonDown(0) && (lastArmoredTurretUse + armoredTurretCoolDown < Time.time || firstArmoredTurretUse))
            {
                firstArmoredTurretUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastArmoredTurretUse = Time.time;

            }
        }
        if (utilSlot == 5)
        {
            if (Input.GetMouseButtonDown(0) && (lastFrenzyTurretUse + frenzyTurretCoolDown < Time.time || firstFrenzyTurretUse))
            {
                firstFrenzyTurretUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastFrenzyTurretUse = Time.time;

            }
        }
        if (utilSlot == 6)
        {
            if (Input.GetMouseButtonDown(0) && (lastShotgunTurretUse + shotgunTurretCoolDown < Time.time || firstShotgunTurretUse))
            {
                firstShotgunTurretUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastShotgunTurretUse = Time.time;

            }
        }
        if (utilSlot == 7)
        {
            if (Input.GetMouseButtonDown(0) && (lastSniperTurretUse + sniperTurretCoolDown < Time.time || firstSniperTurretUse))
            {
                firstSniperTurretUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastSniperTurretUse = Time.time;

            }
        }
        if (utilSlot == 8)
        {
            if (Input.GetMouseButtonDown(0) && (lastMudUse + mudCooldown < Time.time || firstMudUse))
            {
                firstMudUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastMudUse = Time.time;

            }
        }
        if (utilSlot == 9)
        {
            if (Input.GetMouseButtonDown(0) && (lastAcidUse + acidCooldown < Time.time || firstAcidUse))
            {
                firstAcidUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastAcidUse = Time.time;

            }
        }
        if (utilSlot == 10)
        {
            if (Input.GetMouseButtonDown(0) && (lastExplosiveMineUse + explosiveMineCoolDown < Time.time || firstExplosiveMineUse))
            {
                firstExplosiveMineUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastExplosiveMineUse = Time.time;

            }
        }
        if (utilSlot == 11)
        {
            if (Input.GetMouseButtonDown(0) && (lastFlashMineUse + flashMineCoolDown < Time.time || firstFlashMineUse))
            {
                firstFlashMineUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastFlashMineUse = Time.time;

            }
        }
        if (utilSlot == 12)
        {
            if (Input.GetMouseButtonDown(0) && (lastAcidMineUse + acidMineCoolDown < Time.time || firstAcidMineUse))
            {
                firstAcidMineUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastAcidMineUse = Time.time;

            }
        }
        if (utilSlot == 13)
        {
            if (Input.GetMouseButtonDown(0) && (lastDecoyMineUse + decoyMineCoolDown < Time.time || firstDecoyMineUse))
            {
                firstDecoyMineUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                GameObject go = Instantiate(util[utilSlot]);
                InstanceFinder.ServerManager.Spawn(go, null);
                lastDecoyMineUse = Time.time;

            }
        }
    }
    public void CheckCooldowns()
    {
        if(lastWoodWallUse + woodWallCoolDown < Time.time||firstWoodWallUse)
        {
            woodWallState = "READY";
        }
        else
        {
            woodWallState = string.Format("{0}", Mathf.FloorToInt(lastWoodWallUse + woodWallCoolDown - Time.time)+1);
        }
        if (lastConcreteWallUse + concreteWallCoolDown < Time.time||firstConcreteWallUse)
        {
            concreteWallState = "READY";
        }
        else
        {
            concreteWallState = string.Format("{0}", Mathf.FloorToInt(lastConcreteWallUse + concreteWallCoolDown - Time.time)+1);
        }
        if (lastElectricFenceUse + electricFenceCoolDown < Time.time || firstElectricFenceUse)
        {
            electricFenceState = "READY";
        }
        else
        {
            electricFenceState = string.Format("{0}", Mathf.FloorToInt(lastElectricFenceUse + electricFenceCoolDown - Time.time) + 1);
        }
        if (lastBasicTurretUse + basicTurretCoolDown < Time.time||firstBasicTurretUse)
        {
            basicTurretState = "READY";
        }
        else
        {
            basicTurretState = string.Format("{0}", Mathf.FloorToInt(lastBasicTurretUse + basicTurretCoolDown - Time.time)+1);
        }
        if (lastArmoredTurretUse + armoredTurretCoolDown < Time.time || firstArmoredTurretUse)
        {
            armoredTurretState = "READY";
        }
        else
        {
            armoredTurretState = string.Format("{0}", Mathf.FloorToInt(lastArmoredTurretUse + armoredTurretCoolDown - Time.time) + 1);
        }
        if (lastFrenzyTurretUse + frenzyTurretCoolDown < Time.time || firstFrenzyTurretUse)
        {
            frenzyTurretState = "READY";
        }
        else
        {
            frenzyTurretState = string.Format("{0}", Mathf.FloorToInt(lastFrenzyTurretUse + frenzyTurretCoolDown - Time.time) + 1);
        }
        if (lastShotgunTurretUse + shotgunTurretCoolDown < Time.time || firstShotgunTurretUse)
        {
            shotgunTurretState = "READY";
        }
        else
        {
            shotgunTurretState = string.Format("{0}", Mathf.FloorToInt(lastShotgunTurretUse + shotgunTurretCoolDown - Time.time) + 1);
        }
        if (lastSniperTurretUse + sniperTurretCoolDown < Time.time || firstSniperTurretUse)
        {
            sniperTurretState = "READY";
        }
        else
        {
            sniperTurretState = string.Format("{0}", Mathf.FloorToInt(lastSniperTurretUse + sniperTurretCoolDown - Time.time) + 1);
        }
        if (lastMudUse + mudCooldown < Time.time||firstMudUse)
        {
            mudState = "READY";
        }
        else
        {
            mudState = string.Format("{0}", Mathf.FloorToInt(lastMudUse + mudCooldown - Time.time)+1);
        }
        if (lastAcidUse + acidCooldown < Time.time||firstAcidUse)
        {
            acidState = "READY";
        }
        else
        {
            acidState = string.Format("{0}", Mathf.FloorToInt(lastAcidUse + acidCooldown - Time.time) + 1);
        }
        if (lastExplosiveMineUse + explosiveMineCoolDown < Time.time||firstExplosiveMineUse)
        {
            explosiveMineState = "READY";
        }
        else
        {
            explosiveMineState = string.Format("{0}", Mathf.FloorToInt(lastExplosiveMineUse + explosiveMineCoolDown - Time.time) + 1);
        }
        if (lastFlashMineUse + flashMineCoolDown < Time.time || firstFlashMineUse)
        {
            flashMineState = "READY";
        }
        else
        {
            flashMineState = string.Format("{0}", Mathf.FloorToInt(lastFlashMineUse + flashMineCoolDown - Time.time) + 1);
        }
        if (lastAcidMineUse + acidMineCoolDown < Time.time || firstAcidMineUse)
        {
            acidMineState = "READY";
        }
        else
        {
            acidMineState = string.Format("{0}", Mathf.FloorToInt(lastAcidMineUse + acidMineCoolDown - Time.time) + 1);
        }
        if (lastDecoyMineUse + decoyMineCoolDown < Time.time || firstDecoyMineUse)
        {
            decoyMineState = "READY";
        }
        else
        {
            decoyMineState = string.Format("{0}", Mathf.FloorToInt(lastDecoyMineUse + decoyMineCoolDown - Time.time) + 1);
        }

        if (utilCategory == 0)
        {
            cooldownUI.text = string.Format("Wood Wall: {0}\nConcrete Wall: {1}\nElectric Fence: {2}", woodWallState, concreteWallState,electricFenceState);
        }
        if(utilCategory == 1)
        {
            cooldownUI.text= string.Format("Basic Turret: {0}\nArmored Turret: {1}\nFrenzy Turret: {2}\nShotgun Turret: {3}\nSniper Turret: {4}", basicTurretState, armoredTurretState, frenzyTurretState, shotgunTurretState, sniperTurretState);
        }
        if(utilCategory == 2)
        {
            cooldownUI.text= string.Format("Mud: {0}\nAcid: {1}\nExplosive Mine: {2}\nFlash Mine: {3}\nAcid Mine: {4}\nDecoy Mine: {5}", mudState, acidState, explosiveMineState,flashMineState,acidMineState,decoyMineState);
        }
    }
}
