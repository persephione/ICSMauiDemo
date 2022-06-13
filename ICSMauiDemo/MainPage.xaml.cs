using ICSMauiDemo.Chat;

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
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //MessagesListView.ScrollTo(VM.Messages[VM.Messages.Count - 1], ScrollToPosition.End, false);
                    });
                }
            };

            VM.RefreshScrollDownAnimated = () =>
            {
                if (VM.Messages.Count > 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //MessagesListView.ScrollTo(VM.Messages[VM.Messages.Count - 1], ScrollToPosition.End, true);
                    });
                }
            };
        }
    }

    private async void NameButtonClicked(object sender, EventArgs e)
    {
        // set the username
        var username = await DisplayPromptAsync("Enter something", "Please enter something", keyboard: Keyboard.Numeric);

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

