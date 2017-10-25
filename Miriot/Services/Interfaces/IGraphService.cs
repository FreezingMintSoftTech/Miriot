﻿using Miriot.Common.Model;
using Miriot.Common.Model.Widgets;
using System.Threading.Tasks;

namespace Miriot.Core.Services.Interfaces
{
    public interface IGraphService
    {
        Task GetCodeAsync();

        void Initialize(OAuthWidgetInfo infos);

        bool IsInitialized { get; set; }

        Task<bool> LoginAsync();

        Task<GraphUser> GetUserAsync();
        Task LogoutAsync();
    }
}
