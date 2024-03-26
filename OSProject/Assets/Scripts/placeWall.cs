using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class placeWall : MonoBehaviour
{
    [Header("Utility Objects")]
    public GameObject[] util;
    public GameObject selectedUtilUI;
    public GameObject woodWallUI;
    public GameObject concreteWallUI;
    public GameObject turretUI;
    public GameObject mudUI;
    public GameObject acidUI;
    public GameObject mineUI;
    public int utilSlot = 0;

    public float rotationSpeed;

    [Header("Utility Cooldowns")]
    public float woodWallCoolDown;
    public float concreteWallCoolDown;
    public float basicTurretCoolDown;
    public float mudCooldown;
    public float acidCooldown;
    public float explosiveMineCoolDown;
    float lastWoodWallUse;
    float lastConcreteWallUse;
    float lastBasicTurretUse;
    float lastMudUse;
    float lastAcidUse;
    float lastExplosiveMineUse;
    bool firstWoodWallUse=true;
    bool firstConcreteWallUse=true;
    bool firstBasicTurretUse=true;
    bool firstMudUse=true;
    bool firstAcidUse=true;
    bool firstExplosiveMineUse=true;
    public Text cooldownUI;
    string woodWallState = "READY";
    string concreteWallState = "READY";
    string basicTurretState = "READY";
    string mudState = "READY";
    string acidState = "READY";
    string explosiveMineState = "READY";

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
        if (Input.mouseScrollDelta.y < 0 && !Input.GetKey(KeyCode.LeftControl))
        {
            utilSlot++;
            utilSlot = utilSlot % 6;
        }
        if (Input.mouseScrollDelta.y > 0 && !Input.GetKey(KeyCode.LeftControl))
        {
            utilSlot--;
            if (utilSlot < 0)
            {
                utilSlot += 6;
            }
            utilSlot = utilSlot % 6;
        }
        if(utilSlot==0)
        {
            selectedUtilUI = woodWallUI;
            woodWallUI.SetActive(true);
            concreteWallUI.SetActive(false);
            turretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
        }
        if (utilSlot==1)
        {
            selectedUtilUI = concreteWallUI;
            //   selectedUtilUI = util[1];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(true);
            turretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
        }
        if (utilSlot==2)
        {
            selectedUtilUI = turretUI;
            //   selectedUtilUI = util[2];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            turretUI.SetActive(true);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
        }
        if (utilSlot==3)
        {
            selectedUtilUI = mudUI;
            //  selectedUtilUI = util[3];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            turretUI.SetActive(false);
            mudUI.SetActive(true);
            acidUI.SetActive(false);
            mineUI.SetActive(false);
        }
        if (utilSlot==4)
        {
            selectedUtilUI = acidUI;
            //  selectedUtilUI = util[4];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            turretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(true);
            mineUI.SetActive(false);
        }
        if (utilSlot==5)
        {
            selectedUtilUI = mineUI;
            //   selectedUtilUI = util[5];
            woodWallUI.SetActive(false);
            concreteWallUI.SetActive(false);
            turretUI.SetActive(false);
            mudUI.SetActive(false);
            acidUI.SetActive(false);
            mineUI.SetActive(true);
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
                selectedUtilUI.transform.Rotate(0.0f, rotationSpeed, 0.0f, Space.World);
                savedRotation += rotationSpeed;
            }
            selectedUtilUI.transform.Rotate(0.0f, rotationSpeed, 0.0f, Space.World);
            savedRotation += rotationSpeed;

        }
        //rotate woodWall counterclockwise
        if (Input.GetKey(KeyCode.Q))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                selectedUtilUI.transform.Rotate(0.0f, -rotationSpeed, 0.0f, Space.World);
                savedRotation -= rotationSpeed;
            }
            //transform.Rotate(0.0f, -15.0f, 0.0f, Space.World);
            selectedUtilUI.transform.Rotate(0.0f, -rotationSpeed, 0.0f, Space.World);
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
                Instantiate(util[utilSlot]);
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
                Instantiate(util[utilSlot]);
                lastConcreteWallUse = Time.time;

            }
        }
        if (utilSlot == 2)
        {
            if (Input.GetMouseButtonDown(0) && (lastBasicTurretUse + basicTurretCoolDown < Time.time || firstBasicTurretUse))
            {
                firstBasicTurretUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                Instantiate(util[utilSlot]);
                lastBasicTurretUse = Time.time;

            }
        }
        if (utilSlot == 3)
        {
            if (Input.GetMouseButtonDown(0) && (lastMudUse + mudCooldown < Time.time || firstMudUse))
            {
                firstMudUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                Instantiate(util[utilSlot]);
                lastMudUse = Time.time;

            }
        }
        if (utilSlot == 4)
        {
            if (Input.GetMouseButtonDown(0) && (lastAcidUse + acidCooldown < Time.time || firstAcidUse))
            {
                firstAcidUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                Instantiate(util[utilSlot]);
                lastAcidUse = Time.time;

            }
        }
        if (utilSlot == 5)
        {
            if (Input.GetMouseButtonDown(0) && (lastExplosiveMineUse + lastExplosiveMineUse < Time.time || firstExplosiveMineUse))
            {
                firstExplosiveMineUse = false;
                util[utilSlot].transform.position = worldPosition;
                util[utilSlot].transform.rotation = selectedUtilUI.transform.rotation;
                Instantiate(util[utilSlot]);
                lastExplosiveMineUse = Time.time;

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
        if (lastBasicTurretUse + basicTurretCoolDown < Time.time||firstBasicTurretUse)
        {
            basicTurretState = "READY";
        }
        else
        {
            basicTurretState = string.Format("{0}", Mathf.FloorToInt(lastBasicTurretUse + basicTurretCoolDown - Time.time)+1);
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

        cooldownUI.text = string.Format("Wood Wall: {0}\nConcrete Wall: {1}\nBasic Turret: {2}\nMud: {3}\nAcid: {4}\nExplosive Mine: {5}",woodWallState,concreteWallState,basicTurretState,mudState,acidState,explosiveMineState);
    }
}
