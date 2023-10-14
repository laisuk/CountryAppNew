using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CountryAppNew
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string countryFilePath = @".\Data\all.json";
        public const string AllCountryUrl = @"https://restcountries.com/v3.1/all";
        public const string JsonBackupFilePath = @".\Data\all.json.bak";
        private static List<CountryModel> CountriesData = CountriesList.GetAllCountryData(countryFilePath);
        public static bool updateComplete;

        public MainWindow()
        {
            InitializeComponent();
            var countryNameList = CountriesList.GetAllCountryNameList(CountriesData);
            cbCountryCode.ItemsSource = countryNameList;
            cbCountryCode.SelectedIndex = -1;
            lblFileDate.Content = CountriesList.GetJsonFileDate(countryFilePath);
        }

        private void cbCountryCode_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //string message = $"{cbCountryCode.SelectedIndex}: [{cbCountryCode.SelectedItem}] selected.";
            //string title = "Country Selected";
            //MessageBox.Show(message, title);
            //var currentCountry = countryList.getCurrentCountry(CountriesData, cbCountryCode.SelectedIndex);
            //fillCountryForm(currentCountry);
            return;
        }

        private void btnGetInfo_Click(object sender, RoutedEventArgs e)
        {
            if (cbCountryCode.SelectedIndex < 0) return;
            var currentCountry = CountriesList.GetCurrentCountry(CountriesData, cbCountryCode.SelectedIndex);
            fillCountryForm(currentCountry);
        }

        private void fillCountryForm(CountryModel currentCountry)
        {
            if (currentCountry != null)
            {
                tbName.Text = currentCountry?.name?.common;
                tbCapital.Text = currentCountry?.capital?[0];
                tbCode.Text = currentCountry?.cca2 + " " + currentCountry?.flag;
                tbIDD.Text = currentCountry?.idd?.root + currentCountry?.idd?.suffixes?[0];
                tbRegion.Text = currentCountry?.region;
                tbSubRegion.Text = currentCountry?.subregion;

                tbLanguages.Text = CountriesList.GetLanguages(currentCountry!.languages!);

                tbArea.Text = currentCountry?.area + " km²";
                tbLatLng.Text = $"{currentCountry?.latlng?[0]}° : {currentCountry?.latlng?[1]}°";
                tbPopulation.Text = currentCountry?.population.ToString();

                tbBorder.Text = CountriesList.GetBorders(currentCountry!);

                tbCar.Text = currentCountry?.car?.side?.ToUpper();
                tbTimeZone.Text = currentCountry?.timezones?[0];
                tbContinent.Text = CountriesList.GetContinents(currentCountry!) + $" (Independent: {(currentCountry!.independent! ? "Yes":"No")})";
                tbDemonyms.Text = CountriesList.GetDemonyms(currentCountry!);
                tbStartOfWeek.Text = currentCountry?.startOfWeek?.ToUpper();

                string currencies = CountriesList.GetCurrencies(currentCountry!.currencies!); // Dictionary<string, Currency)
                
                tbCurrency.Text = currencies;

                Uri uri = new(currentCountry?.flags?.png!);
                imgFlag.Source = new BitmapImage(uri);
                imgFlag.ToolTip = currentCountry?.flags?.alt!;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void btnUpdateData_Click(object sender, RoutedEventArgs e)
        {
            updateComplete = false;
            CountriesList.UpdateJsonDataFile(countryFilePath);
            var length = 500;

            await Task.Run(() =>
            {
                for (int i = 0; i <= length; i++)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        btnUpdateData.IsEnabled = false;
                        lblFileDate.Content = $"Updating...({i})";
                    }), DispatcherPriority.Render);

                    if (updateComplete)
                    {
                        break;
                    }
                    Thread.Sleep(50);
                }
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    lblFileDate.Content = CountriesList.GetJsonFileDate(countryFilePath);
                    btnUpdateData.IsEnabled = true;
                }), DispatcherPriority.Render);
                updateComplete = false;                
            });
        }
    }
}
