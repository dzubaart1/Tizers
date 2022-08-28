using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace CyberCar.ModCanvas
{
    public class ModalPanelView : MonoBehaviour
    {
        SignalBus _signalBus;
        public int PanelId;
        public CanvasGroup _CanvasGroup;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<ShowMenuModalPlane>(CheckOnShow);
           
        }
        void CheckOnShow(ShowMenuModalPlane panel)
        {
            Debug.Log(panel.idPanel);
            CanvasGroupCntrl.ChangeStateCanvas(_CanvasGroup, panel.idPanel == PanelId);
        }
        private void Start()
        {
            _CanvasGroup = GetComponent<CanvasGroup>();
            CanvasGroupCntrl.ChangeStateCanvas(_CanvasGroup,false);
        }
    }
}