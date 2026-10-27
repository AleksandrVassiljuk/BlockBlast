
using BlockBlast.Services;

namespace BlockBlast.Views
{
    public partial class RecordsPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public RecordsPage()
        {
            InitializeComponent();

            _databaseService = new DatabaseService();
        }

        // Laeb rekordid andmebaasist
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var records =
                await _databaseService.GetRecordsAsync();

            RecordsList.ItemsSource = records;

            int bestScore =
                await _databaseService.GetBestScoreAsync();

            BestScoreLabel.Text = bestScore.ToString();
        }

        // Tagasi mängu juurde
        private async void BackButton_Clicked(
            object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
