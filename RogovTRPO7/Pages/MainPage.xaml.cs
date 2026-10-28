using RogovTRPO7.Models;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RogovTRPO7.Pages
{
    public partial class MainPage : Page
    {
        HttpClient httpClient = new HttpClient();
        public Restaurant? SelectedRestaurant { get; set; }
        public ObservableCollection<Restaurant> Restaurants { get; set; } = new();
        public string FilterCuisine { get; set; } = "";
        public string FilterMinRating { get; set; } = "";
        public string CompletedFilter = "?";

        public MainPage()
        {
            InitializeComponent();
            httpClient.BaseAddress = new Uri("http://localhost:5000/");
            Title = "Список ресторанов";
            DataContext = this;
            LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                //Крафтим ссылку с фильтрами
                if (!string.IsNullOrEmpty(FilterCuisine)) CompletedFilter += $"cuisine={FilterCuisine}";
                if (!string.IsNullOrEmpty(FilterMinRating))
                {
                    if (CompletedFilter != "?") CompletedFilter += "&";
                    CompletedFilter += $"minRating={FilterMinRating}";
                }
                string url = "api/v1/restaurant";
                if (CompletedFilter != "?") url += CompletedFilter;

                var list = await httpClient.GetFromJsonAsync<List<Restaurant>>(url);

                Restaurants.Clear();
                if (list != null) 
                    foreach (var item in list) 
                        Restaurants.Add(item);

                CompletedFilter = "?";
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Сервер недоступен: {ex.Message}");
            }
        }
        private async void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditPage());
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRestaurant != null)
                NavigationService.Navigate(new EditPage(SelectedRestaurant));
            else
                MessageBox.Show("Выберите ресторан");
        }
        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRestaurant == null)
            {
                MessageBox.Show("Выберите ресторан");
                return;
            }
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"api/v1/restaurant/{SelectedRestaurant.Id}");
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                        await LoadData();
                        break;
                    case HttpStatusCode.NotFound:
                        MessageBox.Show("Ресторан не найден на сервере");
                        break;
                    default:
                        MessageBox.Show($"Ошибка: {response.StatusCode}");
                        break;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}