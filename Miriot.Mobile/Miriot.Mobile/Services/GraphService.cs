﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Miriot.Common.Model.Widgets;
using Miriot.Mobile.Views;
using Miriot.Services;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Miriot.Mobile.Services
{
    public class GraphService : IGraphService
    {
        private TaskCompletionSource<bool> _tcs;

        public GraphService()
        {
        }

        public bool IsInitialized { get; set; }

        public void Initialize()
        {
            // From Azure portal - Supinfo subscription
            var appClientId = "e57bfe1e-a88e-47f3-b47c-c414f8ca244b";
            IsInitialized = MicrosoftGraphService.Instance.Initialize(appClientId);
        }

        public async Task AuthenticateForDeviceAsync()
        {
            //if (!IsInitialized)
            //Initialize();
            _tcs = new TaskCompletionSource<bool>();

            var popup = new PopupLoginView(new Uri("http://aka.ms/devicelogin"));
            popup.Disappearing += Popup_Disappearing;

            await PopupNavigation.Instance.PushAsync(popup, true);

            await _tcs.Task;
            //await MicrosoftGraphService.Instance.LoginAsync();
        }

        void Popup_Disappearing(object sender, EventArgs e)
        {
            _tcs.SetResult(true);
        }

        public Task<string> GetCodeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GraphUser> GetUserAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> LoginAsync(bool hideError = false)
        {
            throw new NotImplementedException();
            // return await MicrosoftGraphService.Instance.LoginAsync();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
