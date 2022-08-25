using CyberCar.ModCanvas;
using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace CyberCar.Zenject
{
    public class GameSceneInstaller: MonoInstaller
    {
        public Transform StartPosition;
        [Header("Init prefabs")]
        [SerializeField] private CarCntrl car_prefab;
        [SerializeField] private CarGameManager manager_prefab;
        [SerializeField] private CarCanvasCntrl carCanvas_prefab;
        [Header("Objects inb game")]
        [SerializeField] private CarCntrl car;
        [SerializeField] private CarGameManager manager;
        [SerializeField] private CarCanvasCntrl carCanvas;
        
        public override void InstallBindings()
        {
        
            /*Подключение сигналов*/
            SignalBusInstaller.Install(Container);
            /*сигналы*/
            InitialSignals();
            /*Контроллеры*/
            manager = Container.InstantiatePrefabForComponent<CarGameManager>(manager_prefab, new Vector3(0,0,0), Quaternion.identity,null);
            car = Container.InstantiatePrefabForComponent<CarCntrl>(car_prefab, StartPosition.position, Quaternion.identity,null);
            carCanvas = Container.InstantiatePrefabForComponent<CarCanvasCntrl>(carCanvas_prefab, new Vector3(0,0,0), Quaternion.identity,null);
            carCanvas.GameManager = manager;
            car.GameManager = manager;
            manager.Car = car;
            manager._CarCanvas = carCanvas;
            //Bindings
            Container.Bind<CarCntrl>().FromInstance(car).AsSingle();
            Container.Bind<CarGameManager>().FromInstance(manager).AsSingle();
       

            void InitialSignals()
            {
                Container.DeclareSignal<Signal_is_die>();
                Container.DeclareSignal<Signal_start_game>();
                Container.DeclareSignal<Signal_turn_car>();
                Container.DeclareSignal<Signal_nitro>();
                Container.DeclareSignal<Signal_stop_nitro>();
                Container.DeclareSignal<Signal_Show_alert_icon>();
                Container.DeclareSignal<Signal_Hide_alert_icon>();
                Container.DeclareSignal<SignalSetEfect>();
                Container.DeclareSignal<SignalStopEfect>();
               
            }

        }
    }
}