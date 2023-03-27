using UnityEngine;


    public class TabPanelManager : MonoBehaviour
    {
        public Transform tabs;
        public SurveyButton initialActivatedTab;
        [HideInInspector]
        public SurveyButton currentSelectedTab;
        public bool autoEnableTriggerOnChildrenCollider = false;
        SurveyButton[] tabButtons;

        void Awake()
        {
            tabButtons = tabs.GetComponentsInChildren<SurveyButton>();
        } 

        void Start()
        {

            foreach (var item in tabButtons)
            {
                item.OnTabSelected += SelectTab;
            }

            if (null != initialActivatedTab)
            {
                initialActivatedTab.OnClicked();
                currentSelectedTab = initialActivatedTab;
            }

            if (autoEnableTriggerOnChildrenCollider)
            {
                Collider[] colliders = GetComponentsInChildren<Collider>(true);
                foreach (Collider collider in colliders)
                {
                    collider.isTrigger = true;
                }
            }
        }

        public void SelectTab(SurveyButton tab)
        {
            DeselectCurrentItem();
            tab.Activate();
            tab.isClickedSurveyButton = true;
            currentSelectedTab = tab;
        }

        public void DeselectCurrentItem()
        {
            if (null != currentSelectedTab)
            {
                currentSelectedTab.isClickedSurveyButton = false;
                currentSelectedTab.OnReleased();
            }
            currentSelectedTab = null;
        }
    }

