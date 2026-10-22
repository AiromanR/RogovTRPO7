using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using RogovTRPO7.Models;

namespace RogovTRPO7.Pages
{
    public partial class MainPage : Page
    {
        HttpClient httpClient = new HttpClient();

        public ObservableCollection<Restaurant> Restaurants { get; set; } = new();

        public MainPage()
        {
            InitializeComponent();
            httpClient.BaseAddress = new Uri("http://localhost:5000/");
            DataContext = this;
            LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                var list = await httpClient.GetFromJsonAsync<List<Restaurant>>("api/v1/restaurant");

                Restaurants.Clear();
                if (list != null) 
                    foreach (var item in list) 
                        Restaurants.Add(item);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Сервер недоступен: {ex.Message}");
            }
        }
    }
}