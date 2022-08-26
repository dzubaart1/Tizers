using CyberCar.MenuCntrls;
using DefaultNamespace;
using UnityEngine;
using Zenject;
namespace CyberCar.Zenject
{
    public class MenuSceneInstaller: MonoInstaller
    {
        [Header("Init prefabs")]
        [SerializeField] private CyberCarMenuView CanvasView;
        
        [Header("Objects inb game")]
        private CyberCarMenuView _carMenu;

        public override void InstallBindings()
        {
            /*Подключение сигналов*/
            SignalBusInstaller.Install(Container);
            /*сигналы*/
            InitialSignals();
            _carMenu = Container.InstantiatePrefabForComponent<CyberCarMenuView>(CanvasView, new Vector3(0,0,0), Quaternion.identity,null);
            //Bindings
            Container.Bind<CyberCarMenuView>().FromInstance(_carMenu).AsSingle();
            
            
        }
        void InitialSignals()
        {
            Container.DeclareSignal<GetShopItemSignal>();
            Container.DeclareSignal<ShowMenuPanle>();
            Container.DeclareSignal<ShowShop>();
            Container.DeclareSignal<Signal_start_game>();
          
               
        }
    }
    
}