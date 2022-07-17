namespace Game.General.Views
{
    using System.Collections.Generic;
    using System.Linq;
    using BodyParts;
    using Cysharp.Threading.Tasks;
    using Services;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using Zenject;

    public class SelectorView : MonoBehaviour
    {
        [Inject]
        private IDiceSelector _diceSelector;

        [SerializeField]
        GraphicRaycaster m_Raycaster;

        PointerEventData m_PointerEventData;

        [SerializeField]
        EventSystem m_EventSystem;

        [SerializeField]
        private LayoutGroup[] groups;


        void Update()
        {
            //Check if the left Mouse button is clicked
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //Set up the new Pointer Event
                m_PointerEventData = new PointerEventData(m_EventSystem);
                //Set the Pointer Event Position to that of the mouse position
                m_PointerEventData.position = Input.mousePosition;

                //Create a list of Raycast Results
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                m_Raycaster.Raycast(m_PointerEventData, results);

                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray

                if (_diceSelector.Selected != null)
                {
                    var firstOrDefault = results.Select(x => x.gameObject)
                        .FirstOrDefault(result => result.GetComponent<BodyPartView>() != null);
                    if (firstOrDefault != null)
                    {
                        var bodyPart = firstOrDefault.GetComponent<BodyPartView>();
                        if (bodyPart != null)
                        {
                            var selectorSelected = _diceSelector.Selected;
                            if (selectorSelected != null)
                            {
                                bodyPart.Add(_diceSelector.Selected);
                                _diceSelector.Deselect();
                            }
                        }
                        foreach (var layoutGroup in groups)
                        {
                            Recalcul(layoutGroup);
                        }
                    }
                    else
                    {
                        SelectNewDice(results);
                    }
                }

                else
                {
                    SelectNewDice(results);
                }
            }

            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                //Set up the new Pointer Event
                m_PointerEventData = new PointerEventData(m_EventSystem);
                //Set the Pointer Event Position to that of the mouse position
                m_PointerEventData.position = Input.mousePosition;

                //Create a list of Raycast Results
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                m_Raycaster.Raycast(m_PointerEventData, results);

                var diceElement = results.Select(x => x.gameObject)
                    .FirstOrDefault(result => result.GetComponent<DiceElement>() != null);
                if (diceElement == null)
                {
                    foreach (var layoutGroup in groups)
                    {
                        Recalcul(layoutGroup);
                    }
                    return;
                }

                var element = diceElement.GetComponent<DiceElement>();

                _diceSelector.Deselect();
                element.ResetParent();
                _diceSelector.Remove(element);
                foreach (var layoutGroup in groups)
                {
                    Recalcul(layoutGroup);
                }
            }
        }

        private void SelectNewDice(List<RaycastResult> results)
        {
            var diceElement = results.Select(x => x.gameObject)
                .FirstOrDefault(result => result.GetComponent<DiceElement>() != null);
            if (diceElement == null)
            {
                foreach (var layoutGroup in groups)
                {
                    Recalcul(layoutGroup);
                }
                return;
            }

            var element = diceElement.GetComponent<DiceElement>();
            if (_diceSelector.Selected == element)
            {
                _diceSelector.Deselect();
                foreach (var layoutGroup in groups)
                {
                    Recalcul(layoutGroup);
                }
                
                return;
            }

            _diceSelector.Deselect();
            _diceSelector.Select(element);
            foreach (var layoutGroup in groups)
            {
                Recalcul(layoutGroup);
            }
        }

        public async void Recalcul(LayoutGroup layoutGroup)
        {
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();
            await UniTask.WaitForEndOfFrame();
            layoutGroup.gameObject.SetActive(false);
            layoutGroup.gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
        }
        
    }
}