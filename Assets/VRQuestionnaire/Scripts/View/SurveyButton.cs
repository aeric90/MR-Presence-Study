using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class SurveyButton : TransitionEffect
    {
        public delegate void OnTabSelectedHandler(SurveyButton tab);
        public event OnTabSelectedHandler OnTabSelected;
        public GameObject relatedPanel;
        [HideInInspector]
        public bool isClickedSurveyButton = false;

        public override void Awake()
        {
            base.Awake();
            if (null != relatedPanel)
            {
                relatedPanel.SetActive(false);
            }
        }

        public void OnHovered()
        {
            ActivateMaterial();
        }

        public void OnClicked()
        {
            if (null != OnTabSelected)
            {
                OnTabSelected(this);
            }
        }

        public void OnReleased()
        {
            if (!isClickedSurveyButton)
            {
                Deactivate();
            }
        }

        public void OnLeave()
        {
            if (!isClickedSurveyButton)
            {
                DeactivateMaterial();
            }
        }

        public void Activate()
        {
            ActivateAllEffects();
            if (null != relatedPanel)
            {
                relatedPanel.SetActive(true);
            }
        }

        public void Deactivate()
        {
            DeactivateAllEffects();
            if (null != relatedPanel)
            {
                relatedPanel.SetActive(false);
            }
        }
    }