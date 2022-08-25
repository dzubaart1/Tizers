using System.Collections.Generic;
using CyberCar.ModShopItems;
using DefaultNamespace;
using TMPro;
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
        [Header("Work Items")] public ShopItemCntrl ShopItem;
        public Transform ShopContainer;
        private List<CarParams> ShopCars;
        public CarModelCntrl CurShopModel;
        public ColorBtn ColorBtn_prefab;
        public List<Vector3> CameraPositions;
        public TMP_Text CarName;

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
                if (CurShopModel) CurShopModel.transform.Rotate(0.0f, -145, 0f);

                SetShop();
            }
            else
            {
                mainCamera = Camera.main;
                mainCamera.transform.position = CameraPositions[1];
                if (CurShopModel) CurShopModel.transform.rotation = Quaternion.identity;

                mainCamera.cullingMask = 3;
            }
        }

        public void SetShop()
        {
            foreach (Transform child in ShopContainer)
            {
                Destroy(child.gameObject);
            }

            ShopCars = _ShopCntrl.GetCarsList();
            foreach (var car in ShopCars)
            {
                ShopItemCntrl item = Instantiate(ShopItem, ShopContainer);
                item.SetData(car, this);
            }
        }

        public void SetShopColors()
        {
            foreach (Transform child in ShopContainer)
            {
                Destroy(child.gameObject);
            }

            List<ColorItem> Colors = _ShopCntrl.GetColorsList();
            foreach (var color in Colors)
            {
                ShopItemCntrl item = Instantiate(ShopItem, ShopContainer);
                item.SetData(color, this);
            }
        }

        public void SetShopBacklights()
        {
            foreach (Transform child in ShopContainer)
            {
                Destroy(child.gameObject);
            }

            List<BackLightItem> backLight = _ShopCntrl.GetBackLists();
            foreach (var back in backLight)
            {
                ShopItemCntrl item = Instantiate(ShopItem, ShopContainer);
                item.SetData(back, this);
            }
        }

        public void setShopCar(CarParams car)
        {
            _ShopCntrl.CurCar = car;
            if (CurShopModel) Destroy(CurShopModel.gameObject);
            CurShopModel = Instantiate(car.CarModel, ModelPoint);
            CurShopModel.transform.rotation = Quaternion.identity;
            CurShopModel.transform.Rotate(0.0f, -145, 0f);
            BoxCollider box = CurShopModel.AddComponent<BoxCollider>();
            box.size = new Vector3(5, 5, 5);
            CurShopModel.AddComponent<DragRotation>();
            CarName.text = car.Name;
        }

        public void setColorCar(Color myColor, int colorId)
        {
            _ShopCntrl.CurColor = colorId;
            CurShopModel._renderer.material.color = myColor;
        }

        public void setBacklightCar(BackLightItem backLight, int backId)
        {
            _ShopCntrl.CurBacklight = backId;
            CurShopModel.setData(backLight.backlight);
        }

        public void loadColor(CarParams car, int colorId)
        {
            List<ColorItem> Colors = _ShopCntrl.GetColorsList();
            foreach (var VARIABLE in Colors)
            {
                if (VARIABLE.id == colorId)
                {
                    setColorCar(VARIABLE.color, VARIABLE.id);
                }
            }
        }
    }
}