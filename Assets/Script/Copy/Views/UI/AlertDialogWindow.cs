using System;

using UnityEngine;
using UnityEngine.UI;

using CFM.Framework.ViewModels.UI;

namespace CFM.Framework.Views.UI
{
    public class AlertDialogWindow : Window
    {
        public Text Title;

        public Text Message;

        public GameObject Content;

        public Button ConfirmButton;

        public Button NeutralButton;

        public Button CancelButton;

        public Button OutsideButton;

        public bool CanceledOnTouchOutside { get; set; }

        private IUIView contentView;

        private AlertDialogViewModel viewModel;

        public IUIView ContentView
        {
            get { return contentView; }
            set
            {
                if (contentView == value)
                    return;

                if (contentView != null)
                    GameObject.Destroy(contentView.Owner);

                contentView = value;
                if (contentView != null && contentView.Owner != null && Content != null)
                {
                    contentView.Visibility = true;
                    contentView.Transform.SetParent(Content.transform, false);
                    if (Message != null)
                        Message.gameObject.SetActive(false);
                }
            }
        }

        public AlertDialogViewModel ViewModel
        {
            get { return viewModel; }
            set
            {
                viewModel = value;
                OnChangeViewModel();
            }
        }

        protected virtual void Button_OnClick(int which)
        {
            try
            {
                viewModel.OnClick(which);
            }
            catch (Exception) { }
            finally
            {
                Dismiss();
            }
        }

        public virtual void Cancel()
        {
            Button_OnClick(AlertDialog.BUTTON_NEGATIVE);
        }

        protected override void OnCreate(IBundle bundle)
        {
            WindowType = WindowType.DIALOG;
        }

        protected void OnChangeViewModel()
        {
            if (Message != null)
            {
                if (!string.IsNullOrEmpty(this.viewModel.Message))
                {
                    Message.gameObject.SetActive(true);
                    Message.text = this.viewModel.Message;
                    if (contentView != null && this.contentView.Visibility)
                        contentView.Visibility = false;
                }
                else
                    Message.gameObject.SetActive(false);
            }

            if (Title != null)
            {
                if (!string.IsNullOrEmpty(viewModel.Title))
                {
                    Title.gameObject.SetActive(true);
                    Title.text = viewModel.Title;
                }
                else
                    Title.gameObject.SetActive(false);
            }

            if (ConfirmButton != null)
            {
                if (!string.IsNullOrEmpty(viewModel.ConfirmButtonText))
                {
                    ConfirmButton.gameObject.SetActive(true);
                    ConfirmButton.onClick.AddListener(() => { Button_OnClick(AlertDialog.BUTTON_POSITIVE); });
                    Text text = ConfirmButton.GetComponentInChildren<Text>();
                    if (text != null)
                        text.text = viewModel.ConfirmButtonText;
                }
                else
                {
                    ConfirmButton.gameObject.SetActive(false);
                }
            }

            if (CancelButton != null)
            {
                if (!string.IsNullOrEmpty(viewModel.CancelButtonText))
                {
                    CancelButton.gameObject.SetActive(true);
                    CancelButton.onClick.AddListener(() => { Button_OnClick(AlertDialog.BUTTON_NEGATIVE); });
                    Text text = CancelButton.GetComponentInChildren<Text>();
                    if (text != null)
                        text.text = viewModel.CancelButtonText;
                }
                else
                {
                    CancelButton.gameObject.SetActive(false);
                }
            }

            if (NeutralButton != null)
            {
                if (!string.IsNullOrEmpty(viewModel.NeutralButtonText))
                {
                    NeutralButton.gameObject.SetActive(true);
                    NeutralButton.onClick.AddListener(() => { Button_OnClick(AlertDialog.BUTTON_NEUTRAL); });
                    Text text = NeutralButton.GetComponentInChildren<Text>();
                    if (text != null)
                        text.text = viewModel.NeutralButtonText;
                }
                else
                {
                    NeutralButton.gameObject.SetActive(false);
                }
            }

            CanceledOnTouchOutside = viewModel.CanceledOnTouchOutside;
            if (OutsideButton != null && CanceledOnTouchOutside)
            {
                OutsideButton.gameObject.SetActive(true);
                OutsideButton.interactable = true;
                OutsideButton.onClick.AddListener(() => { Button_OnClick(AlertDialog.BUTTON_NEGATIVE); });
            }
        }
    }
}

