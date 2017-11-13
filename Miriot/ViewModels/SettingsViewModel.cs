﻿using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Miriot.Common.Model;
using Miriot.Services;
using Miriot.Core.ViewModels.Widgets;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Miriot.Services.Interfaces;

namespace Miriot.Core.ViewModels
{
    public class SettingsViewModel : CustomViewModel
    {
        #region Commands
        public RelayCommand ActionSave { get; set; }
        public RelayCommand ActionDelete { get; set; }
        #endregion

        #region Variables
        private readonly IDialogService _dialogService;
        private readonly IFaceService _faceService;
        private readonly IDispatcherService _dispatcher;
        private readonly IRomeService _romeService;
        private User _user;
        private ObservableCollection<WidgetModel> _widgets;
        #endregion

        #region Properties
        public ObservableCollection<WidgetModel> Widgets
        {
            get { return _widgets; }
            set { Set(() => Widgets, ref _widgets, value); }
        }

        public User User
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }
        #endregion

        public SettingsViewModel(
            IDialogService dialogService,
            IDispatcherService dispatcher,
            IRomeService romeService)
        {
            _dialogService = dialogService;
            _dispatcher = dispatcher;
            _romeService = romeService;

            ActionSave = new RelayCommand(OnSave);
            ActionDelete = new RelayCommand(async () => await OnDelete());

            Widgets = new ObservableCollection<WidgetModel>();
        }

        private async Task OnDelete()
        {
            var isSuccess = false; // await _faceService.DeletePerson(User.Id);

            if (isSuccess)
                _dispatcher.Invoke(async () => await _dialogService.ShowMessage("Utilisateur supprimé", "Information"));
            else
                _dispatcher.Invoke(async () => await _dialogService.ShowMessage("Erreur lors de la suppression. Réessayez.", "Alerte"));
        }

        private void OnSave()
        {
            foreach (var w in Widgets)
            {
                AddRemoveWidget(w);
            }

            Task.Run(async () =>
            {
                var isSuccess = await UpdateUserAsync();

                if (isSuccess)
                    _dispatcher.Invoke(async () => await _dialogService.ShowMessage("Modifications sauvegardées", "Information"));
                else
                    _dispatcher.Invoke(async () => await _dialogService.ShowMessage("Erreur lors de la sauvegarde. Réessayez.", "Alerte"));
            });
        }

        private async Task<bool> UpdateUserAsync()
        {
            return false; //await _faceService.UpdatePerson(User, User.Picture);
        }

        private void AddRemoveWidget(WidgetModel w)
        {
            // If desactivated
            if (!w.IsActive)
            {
                var ww = User.UserData.Widgets.FirstOrDefault(e => e.Type == w.Type);

                // Should be existing
                if (ww != null)
                {
                    // Remove
                    User.UserData.Widgets.Remove(ww);
                }
            }
            else // if activated
            {
                var ww = User.UserData.Widgets.FirstOrDefault(e => e.Type == w.Type);

                // Should not be existing
                if (ww == null)
                    User.UserData.Widgets.Add(w.ToWidget());
                else
                {
                    var newW = w.ToWidget();
                    ww.Infos = newW.Infos;
                    ww.X = newW.X;
                    ww.Y = newW.Y;
                }
            }
        }

        protected override async Task InitializeAsync()
        {
            if (User == null)
                return;

            Widgets.Clear();

            foreach (var type in Enum.GetValues(typeof(WidgetType)))
            {
                var wt = (WidgetType)type;

                var widgetEntity = User.UserData.Widgets.FirstOrDefault(e => e.Type == wt);

                var widgetModel = wt.ToModel(widgetEntity);

                if (widgetEntity != null)
                {
                    await widgetModel.LoadInfos();
                    widgetModel.SetActive();
                }
                else
                {
                    widgetModel.IsActive = false;
                }

                Widgets.Add(widgetModel);
            }
        }
    }
}
