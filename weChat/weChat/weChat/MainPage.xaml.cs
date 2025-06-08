using System.Collections.ObjectModel;
using weChat.Model;
using weChat.services;
using System.Collections.ObjectModel;


namespace weChat
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Message> Messages { get; set; } = new();
        private CancellationTokenSource _cts = new CancellationTokenSource(); // ✅ Add this line

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            InitAndStart();
            LoadMessages(); // Fetch messages on page load
                            // ✅ Add this line to start auto-refresh loop

        }
        private async void InitAndStart()
        {
            await SupabaseService.Init();    // 🔥 MUST initialize first
            StartAutoRefresh();              // Then start auto refresh
        }

        private async Task LoadMessages()
        {
            if (SupabaseService.Client == null)
                return;

            var result = await SupabaseService.Client
                           .From<Message>()
                           .Select("*, users(id,username)")
                            // join to users table via sender_id
                           .Order(x => x.CreatedAt, Supabase.Postgrest.Constants.Ordering.Ascending)
                           .Get();



            MainThread.BeginInvokeOnMainThread(() =>
            {
                Messages.Clear();
                foreach (var msg in result.Models)
                {
                    Messages.Add(msg);
                }
            });
        }
        protected override void OnDisappearing()
        {
            _cts.Cancel();
            base.OnDisappearing();
        }

        private async void StartAutoRefresh()
        {
            while (!_cts.IsCancellationRequested)
            {
                await LoadMessages();
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }


        private async void OnSendClicked(object sender, EventArgs e)
        {
            var content = MessageEntry.Text?.Trim();
            if (string.IsNullOrEmpty(content))
                return;

            var message = new Message
            {
                Content = content,
                SenderId = Guid.Parse("db04181b-1959-4f7c-9df4-7f13cb743588"),
                CreatedAt = DateTime.UtcNow
            };

            var response = await SupabaseService.Client
                .From<Message>()
                .Insert(message);

            // Clear input
            MessageEntry.Text = "";

            // Refresh messages list
            LoadMessages();
        }


        private void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();

            if (string.IsNullOrEmpty(username))
            {
                DisplayAlert("Error", "Please enter a username.", "OK");
                return;
            }

            // TODO: Later, add Supabase user check/create here

            // If login successful, toggle UI
            LoginSection.IsVisible = false;
            ChatSection.IsVisible = true;

            // Optionally, store username globally for sending messages
            //App.Current.Properties["Username"] = username;
        }
    }

}



