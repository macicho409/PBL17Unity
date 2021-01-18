
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

    public Image covidImage;

    private PhysiologicalModel characterPhysiologicalModel;
    private Arrow arrow;
    private Arrow previousarrow;

    private NeedSlider foodSlider;
    private NeedSlider waterSlider;
    private NeedSlider dreamSlider;
    private NeedSlider sexSlider;
    private NeedSlider toiletSlider;
    private NeedSlider healthSlider;
    private NeedSlider HigherOrderNeedsSlider;


    #endregion

    void Start()
    {
        foodSlider = new NeedSlider(GameObject.Find(SliderEnum.SliderFood.ToString()).GetComponent<Slider>());
        waterSlider = new NeedSlider(GameObject.Find(SliderEnum.SliderWater.ToString()).GetComponent<Slider>());
        dreamSlider = new NeedSlider(GameObject.Find(SliderEnum.SliderDream.ToString()).GetComponent<Slider>());
        sexSlider = new NeedSlider(GameObject.Find(SliderEnum.SliderSex.ToString()).GetComponent<Slider>());
        toiletSlider = new NeedSlider(GameObject.Find(SliderEnum.SliderToilet.ToString()).GetComponent<Slider>());
        healthSlider = new NeedSlider(GameObject.Find(SliderEnum.SliderHealth.ToString()).GetComponent<Slider>());
        HigherOrderNeedsSlider = new NeedSlider(GameObject.Find(SliderEnum.HigherOrderNeedsSlider.ToString()).GetComponent<Slider>());

        charackterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //Shot ray from center of Main Camera on Mouse click. If the ray encounters an Object with PhysiologicalModel attached to it then get PhysiologicalModel from Object 
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.GetComponent<PhysiologicalModel>())
        {
            GetNeeds(hitInfo);
        }


        if (characterPhysiologicalModel != null)
        {
            UpdateNeedsDisplay();
            UpdateArrow();
        }




        UpdateImageCovid();
    }

    private void GetNeeds(RaycastHit hitInfo)
    {
        characterPhysiologicalModel = hitInfo.transform.GetComponent<PhysiologicalModel>();
        charackterName.text = hitInfo.transform.name;
        arrow = hitInfo.transform.GetComponent<Arrow>();
    }

    private void UpdateNeedsDisplay()
    {
        foodSlider.Value = characterPhysiologicalModel.FoodNeed.Value;
        waterSlider.Value = characterPhysiologicalModel.WaterNeed.Value;
        dreamSlider.Value = characterPhysiologicalModel.DreamNeed.Value;
        sexSlider.Value = characterPhysiologicalModel.SexNeed.Value;
        toiletSlider.Value = characterPhysiologicalModel.ToiletNeed.Value;
        HigherOrderNeedsSlider.Value = characterPhysiologicalModel.HigherOrderNeeds.Value;
        healthSlider.Value = characterPhysiologicalModel.Health;
    }


    private void UpdateImageCovid()
    {
        try
        {
            if (hitInfo.transform.GetComponent<Covid>().CovidInfection.Infected)
                covidImage.color = new Color(0.9f, 0.9f, 0.9f);
            else
                covidImage.color = new Color(0.6f, 0.6f, 0.6f);
        }
        catch { }
    }

    private void UpdateArrow()
    {
        if (arrow != previousarrow)
        {
            try
            {
                previousarrow.Switch = false;
            }
            catch { }

        }
        else
        {
            arrow.Switch = true;
        }
        previousarrow = arrow;
    }


}