using ICSMauiDemo.Chat;
using ICSMauiDemo.Templates.CustomChatCells;

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

    //DataTemplate IncomingTemplate;
    //DataTemplate OutgoingTemplate;

    public MainPage()
	{
		InitializeComponent();
        BindingContext = new MainPageViewModel();
        VM = (MainPageViewModel)BindingContext;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //SetupDataTemplates();

        //Content = new StackLayout
        //{
        //    Margin = new Thickness(20),
        //    Children = {
        //            new Label {
        //                Text = "ListView with a DataTemplateSelector",
        //                FontAttributes = FontAttributes.Bold,
        //                HorizontalOptions = LayoutOptions.Center,
        //                TextColor = Color.FromArgb("#000000")
        //            },
        //            new ListView {
        //                Margin = new Thickness(0,20,0,0),
        //                ItemsSource = VM.Messages,
        //                ItemTemplate = new MyDataTemplateSelector {
        //                    IncomingTemplate = IncomingTemplate,
        //                    OutgoingTemplate = OutgoingTemplate
        //                }
        //            }
        //        }
        //};

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
       // var username = await DisplayPromptAsync("Enter something", "Please enter something", keyboard: Keyboard.Numeric);

        // TODO: Remove this after testing
        var username = "tina";

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

    //void SetupDataTemplates()
    //{
    //    IncomingTemplate = new DataTemplate(() =>
    //    {
    //        var grid = new Grid();
    //        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });

    //        var nameLabel = new Label { FontAttributes = FontAttributes.Bold };
    //        nameLabel.SetBinding(Label.TextProperty, "UserName");
    //        nameLabel.TextColor = Color.FromArgb("#000000");

    //        grid.Children.Add(nameLabel);

    //        return new ViewCell
    //        {
    //            View = grid
    //        };
    //    });

    //    OutgoingTemplate = new DataTemplate(() =>
    //    {
    //        var grid = new Grid();
    //        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });

    //        var nameLabel = new Label { FontAttributes = FontAttributes.Bold };
    //        nameLabel.SetBinding(Label.TextProperty, "UserName");
    //        nameLabel.TextColor = Color.FromArgb("#000000");

    //        grid.Children.Add(nameLabel);

    //        return new ViewCell
    //        {
    //            View = grid
    //        };
    //    });
    //}
}

