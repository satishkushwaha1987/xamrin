using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Database;
using CHSBackOffice.Models.Popup;
using CHSBackOffice.Support;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.ViewModels
{
    public class SystemParameterPageViewModel : BindableBase
	{
        #region Properties

        public string PageTitle => Support.CommonViewObjects.Instance.CurrentSystemParameter == null ? Resources.Resource.AddParameterTitle : Resources.Resource.EditParameterTitle;
        public string TopLabelText => Support.CommonViewObjects.Instance.CurrentSystemParameter == null ? Resources.Resource.ParameterAdd : Resources.Resource.ParameterUpdate;

        #region CurrentParameter

        private CHBackOffice.ApiServices.ChsProxy.Parameter1 _CurrentParameter;
        public CHBackOffice.ApiServices.ChsProxy.Parameter1 CurrentParameter
        {
            get => _CurrentParameter;
            set
            {
                SetProperty(ref _CurrentParameter, value);
                RaisePropertyChanged("");
            }
        }

        #endregion

        #region IsAdding

        private bool _IsAdding;
        public bool IsAdding
        {
            get => _IsAdding;
            set => SetProperty(ref _IsAdding, value);

        }

        #endregion

        #region InformationText

        private string _InformationText;
        public string InformationText
        {
            get => _InformationText;
            set => SetProperty(ref _InformationText, value);

        }

        #endregion

        #region InformationVisible

        private bool _InformationVisible;
        public bool InformationVisible
        {
            get => _InformationVisible;
            set => SetProperty(ref _InformationVisible, value);

        }

        #endregion

        #region QuestionText

        private string _QuestionText;
        public string QuestionText
        {
            get => _QuestionText;
            set => SetProperty(ref _QuestionText, value);

        }

        #endregion

        #region QuestionVisible

        private bool _QuestionVisible;
        public bool QuestionVisible
        {
            get => _QuestionVisible;
            set => SetProperty(ref _QuestionVisible, value);

        }

        #endregion


        private PopoverItem _selectedParameterId;
        public PopoverItem SelectedParameterId
        {
            get => _selectedParameterId;
            set
            {
                if (value == null) return;

                if (_selectedParameterId != null)
                    _selectedParameterId.Selected = false;

                SetProperty(ref _selectedParameterId, value);
                RaisePropertyChanged(nameof(SelectedParameterId));

                if (_selectedParameterId != null)
                    _selectedParameterId.Selected = true;


                if (CurrentParameter != null)
                    CurrentParameter.GroupId = _selectedParameterId.Key;
            }
        }

        #endregion

        #region Commands

        public ICommand ResetCommand => new Command(ResetCurrentParameter);
        public ICommand SaveCommand => new Command<string>(SaveCurrentParameter);
        public ICommand HideAdditionalCommand => new Command(() =>
        {
            InformationVisible = false;
            QuestionVisible = false;
            if (GoBackAfterOk) Services.Navigation.GoBack();
        }, () => { return true; });

        #endregion

        #region Fields

        ICHSServiceAgent _serviceAgent;
        bool GoBackAfterOk;

        #endregion

        #region .CTOR

        public SystemParameterPageViewModel(ICHSServiceAgent serviceAgent)
        {
            _serviceAgent = serviceAgent;
            ResetCurrentParameter();
        }

        #endregion

        #region Methods

        void ResetCurrentParameter()
        {
            IsAdding = CommonViewObjects.Instance.CurrentSystemParameter == null;

            if (IsAdding)
                CurrentParameter = new CHBackOffice.ApiServices.ChsProxy.Parameter1();
            else
            {
                CurrentParameter = new CHBackOffice.ApiServices.ChsProxy.Parameter1
                {
                    Description = CommonViewObjects.Instance.CurrentSystemParameter.Description,
                    GroupId = CommonViewObjects.Instance.CurrentSystemParameter.GroupId,
                    Id = CommonViewObjects.Instance.CurrentSystemParameter.Id,
                    Status = CommonViewObjects.Instance.CurrentSystemParameter.Status,
                    Type = CommonViewObjects.Instance.CurrentSystemParameter.Type,
                    Value = CommonViewObjects.Instance.CurrentSystemParameter.Value
                };
                SelectedParameterId = Support.CommonViewObjects.Instance.AllowedParameterGroups.FirstOrDefault(p => p.Key == CurrentParameter.GroupId);
            }
        }

        void ShowQuestion(string questionText)
        {
            InformationVisible = false;
            QuestionText = questionText;
            QuestionVisible = true;
        }

        void ShowInfo(string infoText)
        {
            QuestionVisible = false;
            InformationText = infoText;
            InformationVisible = true;
        }

        string CheckCurrentParameter()
        {
            if (String.IsNullOrEmpty(CurrentParameter.Id)) return "Id is empty";
            if (String.IsNullOrEmpty(CurrentParameter.GroupId)) return "GroupId is empty";
            if (String.IsNullOrEmpty(CurrentParameter.Value)) return "Value is empty";
            if (String.IsNullOrEmpty(CurrentParameter.Description)) return "Description is empty";

            return "";
        }

        async void SaveCurrentParameter(string saveAlreadyExists)
        {
            try
            {
                bool saveAlreadyExistsBool = saveAlreadyExists.ToLower().Contains("true");

                var checkResult = CheckCurrentParameter();
                if (!String.IsNullOrEmpty(checkResult))
                {
                    ShowInfo(checkResult);
                    return;
                }

                if (IsAdding && Support.CommonViewObjects.Instance.ExistingParamNames.Contains(CurrentParameter.Id.ToLower()) && !saveAlreadyExistsBool)
                {
                    ShowQuestion("This Id is already exists. Edit it?");
                    return;
                }

                bool result = false;
                try
                {
                    result =
                    IsAdding ?
                    await _serviceAgent.AddParameter(StateInfoService.SessionId, CurrentParameter.Id, CurrentParameter.GroupId, CurrentParameter.Value, CurrentParameter.Description) :
                    await _serviceAgent.UpdateParameter(StateInfoService.SessionId, CurrentParameter.Id, CurrentParameter.GroupId, CurrentParameter.Value, CurrentParameter.Description);
                }
                catch (Exception ex)
                {
                    Support.ExceptionProcessor.ProcessException(ex);
                    if (IsAdding && ex.Message.Contains("The SqlParameter is already contained by another SqlParameterCollection"))
                        result = true;
                }
                GoBackAfterOk = result;
                ShowInfo($"Parameter {(IsAdding ? "adding" : "saving")} was {(result ? "successful" : "failed")}");
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion
    }
}
