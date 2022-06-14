﻿using ICSMauiDemo.Chat;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ICSMauiDemo;

public partial class MainPage : ContentPage
{
    MainPageViewModel _vm;
    MainPageViewModel VM
    {
        get
        {
            return _vm;
        }
        set
        {
            _vm = value;
            OnPropertyChanged();
        }
    }

    HubConnection connection;

    public MainPage()
	{
		InitializeComponent();
        BindingContext = new MainPageViewModel();
        VM = (MainPageViewModel)BindingContext;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!DesignMode.IsDesignModeEnabled)
        {
            // scrolls to last item in messages list. used for initial load
            VM.RefreshScrollDown = () =>
            {
                if (VM.Messages.Count > 0)
                {
                    MessagesListView.ScrollTo(VM.Messages.Count-1);
                }
            };
        }
    }

    private async void NameButtonClicked(object sender, EventArgs e)
    {
        // set the username
        var username = await DisplayPromptAsync("UserName", "What's your username?");

        // TODO: Remove this after testing
        //var username = "tina";

        if (string.IsNullOrEmpty(username))
            return;

        VM.UserName = username;

        Preferences.Default.Set("username", username);

        // then connect to chathub and start getting messages
        VM.ConnectCommand.Execute(null);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

}

