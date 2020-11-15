using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Enums;
using UnityEngine.UI;
using Mapbox.Unity.Utilities;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    #region [vars]
    RaycastHit hitInfo = new RaycastHit();

    private TextMeshProUGUI charackterName;

    private PhysiologicalModel characterPhysiologicalModel;
    private Helath characterHealth;

    private NeedSlider foodSlider;
    private NeedSlider waterSlider;
    private NeedSlider dreamSlider;
    private NeedSlider sexSlider;
    private NeedSlider toiletSlider;
    private NeedSlider healthSlider;

    #endregion

    void Start()
    {
        foodSlider = new NeedSlider(GameObject.Find(SliderEnum.SliderFood.ToString()).GetComponent<Slider>());
        waterSlider = new NeedSlider(GameObject.Find(SliderEnum.SliderWater.ToString()).GetComponent<Slider>());
        dreamSlider = new NeedSlider(GameObject.Find(SliderEnum.SliderDream.ToString()).GetComponent<Slider>());
        sexSlider = new NeedSlider(GameObject.Find(SliderEnum.SliderSex.ToString()).GetComponent<Slider>());
        toiletSlider = new NeedSlider(GameObject.Find(SliderEnum.SliderToilet.ToString()).GetComponent<Slider>());
        healthSlider = new NeedSlider(GameObject.Find(SliderEnum.SliderHealth.ToString()).GetComponent<Slider>());

        charackterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //Shot ray from center of Main Camera on Mouse click. If the ray encounters an Object with PhysiologicalModel attached to it then get PhysiologicalModel from Object 
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.GetComponent<PhysiologicalModel>())
            GetNeeds(hitInfo);

        if (characterPhysiologicalModel != null) 
            UpdateNeedsDisplay();

        if (characterHealth != null)
            UpdateHealthDisplay();

    }

    private void GetNeeds(RaycastHit hitInfo)
    {
        characterPhysiologicalModel = hitInfo.transform.GetComponent<PhysiologicalModel>();
        characterHealth = hitInfo.transform.GetComponent<Helath>();
        charackterName.text = hitInfo.transform.name;
    }

    private void UpdateNeedsDisplay()
    {
        foodSlider.Value = characterPhysiologicalModel.FoodNeed.Value;
        waterSlider.Value = characterPhysiologicalModel.WaterNeed.Value;
        dreamSlider.Value = characterPhysiologicalModel.DreamNeed.Value;
        sexSlider.Value = characterPhysiologicalModel.SexNeed.Value;
        toiletSlider.Value = characterPhysiologicalModel.ToiletNeed.Value;
    }

    private void UpdateHealthDisplay()
    {
        healthSlider.Value = characterHealth.Value;
    }
}