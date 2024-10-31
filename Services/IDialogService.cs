﻿using System.Threading.Tasks;

namespace PlateDropletApp.Services
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string title, string message);
    }
}
