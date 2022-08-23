using System;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace CyberCar.MenuCntrls
{
    public class ShopView : MonoBehaviour
    {
        public Transform ModelPoint;
        public ShopCntrl _ShopCntrl = new ShopCntrl();
        public Camera mainCamera;
        SignalBus _signalBus;
        [Header("Work Items")]
        public ShopItemCntrl ShopItem;
        public Transform ShopContainer;
        private List<CarParams> ShopCars;
        public CarModelCntrl CurShopModel;
        public Transform ColorPanel;
        public ColorBtn ColorBtn_prefab;
        public List<Vector3> CameraPositions;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<ShowMenuPanle>(ShowShop);
        }

        private void Start()
        {
            mainCamera = Camera.main;
            ModelPoint = Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity).transform;
            CarSave car = SaveLoadCntrl.LoadGame();
            if (car != null)
            {
                ShopCars = _ShopCntrl.GetCarsList();
                foreach (var VARIABLE in ShopCars)
                {
                    if (VARIABLE.id == car.carParamsId)
                    {
                        setShopCar(VARIABLE);
                        loadColor(VARIABLE, car.colorId); 
                        CurShopModel.transform.rotation = Quaternion.identity;
                    }
                }
                
            }
        }

        public void SaveCar()
        {
            _ShopCntrl.SaveGame();
        }

        void ShowShop(ShowMenuPanle panle)
        {
            if (panle.idPanel == 2)
            {
                mainCamera = Camera.main;
                mainCamera.cullingMask = 1; 
                mainCamera.transform.position = CameraPositions[0];
                if(CurShopModel) CurShopModel.transform.Rotate(0.0f, -145, 0f );
               
                SetShop();
            }
            else
            {
                mainCamera = Camera.main;
                mainCamera.transform.position = CameraPositions[1];
                if(CurShopModel)  CurShopModel.transform.rotation = Quaternion.identity;
                
                mainCamera.cullingMask = 3;
            }
        }
        public void SetShop()
        {
            foreach(Transform child in ShopContainer) {
                Destroy(child.gameObject);
            }
            ShopCars = _ShopCntrl.GetCarsList();
            foreach (var car in ShopCars)
            {
                ShopItemCntrl item = Instantiate(ShopItem, ShopContainer);
                item.SetData(car,this);
            }
        }
        public void SetColors(CarParams car)
        {
            if (car.TexturesColor.Count == 0)
            {
                ColorPanel.gameObject.SetActive(false); 
            }
            else
            {
                ColorPanel.gameObject.SetActive(true);  
            }

            foreach(Transform child in ColorPanel) {
                Destroy(child.gameObject);
            }

            int i = 0;
            foreach (var color in car.TexturesColor)
            {
                ColorBtn item = Instantiate(ColorBtn_prefab, ColorPanel);
                item.SetData(color,this,i);
                i++;
            }
        }

        public void setShopCar(CarParams car)
        {
            _ShopCntrl.CurCar = car;
           if(CurShopModel) Destroy(CurShopModel.gameObject);
            CurShopModel = Instantiate(car.CarModel, ModelPoint);
            CurShopModel.transform.rotation = Quaternion.identity;
            CurShopModel.transform.Rotate(0.0f, -145, 0f );
            BoxCollider box =  CurShopModel.AddComponent<BoxCollider>();
            box.size = new Vector3(5, 5, 5);
            CurShopModel.AddComponent<DragRotation>();
            SetColors(car);
        }

        public void setColorCar(Color myColor, int colorId)
        {
            _ShopCntrl.CurColor = colorId;
            CurShopModel._renderer.material.color = myColor;
        }
        public void loadColor (CarParams car, int colorId)
        {
            int i = 0;
            foreach (Color color in car.TexturesColor)
            {
                if (i == colorId)
                {
                    _ShopCntrl.CurColor = colorId;
                    CurShopModel._renderer.material.color = color;
                    break;;
                }

                i++;
            }
            
        }
    }
}