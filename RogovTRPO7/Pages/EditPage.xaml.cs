using RogovTRPO7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RogovTRPO7.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        HttpClient httpClient = new HttpClient();
        private int? restaurantId = null;
        public Restaurant CurrentRestaurant { get; set; }
        private Restaurant OriginalRestaurant { get; set; }
        public EditPage(Restaurant? restaurant = null)
        {
            InitializeComponent();
            httpClient.BaseAddress = new Uri("http://localhost:5000/");

            if (restaurant == null)
            {
                Title = "Добавить ресторан";
                CurrentRestaurant = new Restaurant();
            }
            else
            {
                Title = "Редактировать ресторан";
                CurrentRestaurant = new Restaurant
                {
                    Id = restaurant.Id,
                    Name = restaurant.Name,
                    Cuisine = restaurant.Cuisine,
                    Rating = restaurant.Rating,
                    AverageCheck = restaurant.AverageCheck,
                    IsOpen = restaurant.IsOpen
                };
                restaurantId = restaurant.Id;

                OriginalRestaurant = new Restaurant
                {
                    Id = restaurant.Id,
                    Name = restaurant.Name,
                    Cuisine = restaurant.Cuisine,
                    Rating = restaurant.Rating,
                    AverageCheck = restaurant.AverageCheck,
                    IsOpen = restaurant.IsOpen
                };
            }
            DataContext = CurrentRestaurant;
        }
        private async void BtnPostPut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpResponseMessage responce;

                if (restaurantId == null) 
                    responce = await httpClient.PostAsJsonAsync("api/v1/restaurant", CurrentRestaurant);
                else 
                    responce = await httpClient.PutAsJsonAsync($"api/v1/restaurant/{restaurantId}", CurrentRestaurant);

                switch (responce.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                        MessageBox.Show("Успешно");
                        break;
                    case HttpStatusCode.BadRequest:
                        MessageBox.Show("Неверные данные, проверьте заполненность полей и диапазоны значений");
                        break;
                    case HttpStatusCode.NotFound:
                        MessageBox.Show("Ресторан не найден");
                        break;
                    default:
                        MessageBox.Show($"Ошибка: {responce.StatusCode}");
                        break;
                }
            }
            catch (HttpRequestException ex) { 
                MessageBox.Show(ex.Message);
            }
            catch (JsonException)
            {
                MessageBox.Show("Ошибка джейсон");
            }
        }
        private async void BtnPatch_Click(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage responce;
            if(restaurantId == null)
            {
                MessageBox.Show("Patch нельзя при создании");
                return;
            }
            try {
                RestaurantDTO PatchRestaurant = new RestaurantDTO();

                if (CurrentRestaurant.Name != OriginalRestaurant.Name) PatchRestaurant.Name = CurrentRestaurant.Name;
                if (CurrentRestaurant.Cuisine != OriginalRestaurant.Cuisine) PatchRestaurant.Cuisine = CurrentRestaurant.Cuisine;
                if (CurrentRestaurant.Rating != OriginalRestaurant.Rating) PatchRestaurant.Rating = CurrentRestaurant.Rating;
                if (CurrentRestaurant.AverageCheck != OriginalRestaurant.AverageCheck) PatchRestaurant.AverageCheck = CurrentRestaurant.AverageCheck;
                if (CurrentRestaurant.IsOpen != OriginalRestaurant.IsOpen) PatchRestaurant.IsOpen = CurrentRestaurant.IsOpen;

                responce = await httpClient.PatchAsJsonAsync($"api/v1/restaurant/{restaurantId}", PatchRestaurant);

                switch (responce.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                        MessageBox.Show("Успешно");
                        break;
                    case HttpStatusCode.BadRequest:
                        MessageBox.Show("Неверные данные, проверьте заполненность полей и диапазоны значений");
                        break;
                    case HttpStatusCode.NotFound:
                        MessageBox.Show("Ресторан не найден");
                        break;
                    default:
                        MessageBox.Show($"Ошибка: {responce.StatusCode}");
                        break;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (JsonException)
            {
                MessageBox.Show("Ошибка джейсон");
            }
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
