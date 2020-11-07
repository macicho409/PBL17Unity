using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Enums;
using UnityEngine.UI;

public class SelectObject : MonoBehaviour
{
    #region [vars]

    public NeedDisplay FoodSlider { get; set; }
    public NeedDisplay WaterSlider { get; set; }
    public NeedDisplay DreamSlider { get; set; }
    public NeedDisplay SexSlider { get; set; }
    public NeedDisplay ToiletSlider { get; set; }

    RaycastHit hitInfo;

    bool hit;
    #endregion

    void Start()
    {
        hitInfo = new RaycastHit();
        FoodSlider = new NeedDisplay(GameObject.Find(SliderEnum.SliderFood.ToString()).GetComponent<Slider>());
        WaterSlider = new NeedDisplay(GameObject.Find(SliderEnum.SliderWater.ToString()).GetComponent<Slider>());
        DreamSlider = new NeedDisplay(GameObject.Find(SliderEnum.SliderDream.ToString()).GetComponent<Slider>());
        SexSlider = new NeedDisplay(GameObject.Find(SliderEnum.SliderSex.ToString()).GetComponent<Slider>());
        ToiletSlider = new NeedDisplay(GameObject.Find(SliderEnum.SliderToilet.ToString()).GetComponent<Slider>());
    }

    // Update is called once per frame
    void Update()
    {
        FoodSlider.Slider = GameObject.Find(SliderEnum.SliderFood.ToString()).GetComponent<Slider>();
        WaterSlider.Slider = GameObject.Find(SliderEnum.SliderWater.ToString()).GetComponent<Slider>();
        DreamSlider.Slider = GameObject.Find(SliderEnum.SliderDream.ToString()).GetComponent<Slider>();
        SexSlider.Slider = GameObject.Find(SliderEnum.SliderSex.ToString()).GetComponent<Slider>();
        ToiletSlider.Slider = GameObject.Find(SliderEnum.SliderToilet.ToString()).GetComponent<Slider>();


        if (Input.GetMouseButtonDown(0))
        {   
            hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);        
        }

        if (hit)
        {   
            //Nie wiem czy zadziała powinnien zwrocic null kiiedy nic nie znajdzie 
            if (hitInfo.transform.gameObject.GetComponent<PhysiologicalModel>())
            {
                UpdateNeed(FoodSlider, hitInfo.transform.gameObject.GetComponent<PhysiologicalModel>().FoodNeed.Value);
                UpdateNeed(WaterSlider, hitInfo.transform.gameObject.GetComponent<PhysiologicalModel>().WaterNeed.Value);
                UpdateNeed(DreamSlider, hitInfo.transform.gameObject.GetComponent<PhysiologicalModel>().DreamNeed.Value);
                UpdateNeed(SexSlider, hitInfo.transform.gameObject.GetComponent<PhysiologicalModel>().SexNeed.Value);
                UpdateNeed(ToiletSlider, hitInfo.transform.gameObject.GetComponent<PhysiologicalModel>().ToiletNeed.Value);
            }
        }
    }


    private void UpdateNeed(NeedDisplay slider, float value)
    {
        slider.Value = value;
    }

}
