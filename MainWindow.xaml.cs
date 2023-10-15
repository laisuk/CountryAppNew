using HttpProgressBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CountryAppNew
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string countryFilePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, @"Data\all.json");
        public const string AllCountryUrl = @"https://restcountries.com/v3.1/all";
        public static string JsonBackupFilePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, @"Data\all.json.bak");
        private static List<CountryModel> CountriesData = CountriesList.GetAllCountryData(countryFilePath);

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
            btnUpdateData.IsEnabled = false;
            using HttpClient client = new();

            // Create a Progress<T> object and an event handler to report the progress
            var progress = new Progress<float>();
            progress.ProgressChanged += (sender, value) =>
            {
                // Display the progress percentage
                lblFileDate.Content = $"Updating...({((int)value)}%)";
                
            };

            // Make an HTTP GET request to an API endpoint that returns JSON
            // Use the HttpCompletionOption.ResponseHeadersRead option to get the headers first
            HttpResponseMessage response = await client.GetAsync(AllCountryUrl, HttpCompletionOption.ResponseHeadersRead);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Get the content length from the headers
                long? contentLength = response.Content.Headers.ContentLength;

                // Read the response content as a stream
                using var stream = await response.Content.ReadAsStreamAsync();
                // Create a memory stream to store the downloaded data
                using var memoryStream = new MemoryStream();
                // Copy the content stream to the memory stream, reporting the progress
                await stream.CopyToAsync(memoryStream, 81920, contentLength, progress);

                // Rewind the memory stream
                memoryStream.Position = 0;

                // Read the memory stream as a string
                using var reader = new StreamReader(memoryStream);
                string content = reader.ReadToEnd();

                try
                {
                    File.WriteAllText(countryFilePath, content, Encoding.UTF8);
                    lblFileDate.Content = CountriesList.GetJsonFileDate(countryFilePath);

                    string successMassage = $"File update SUCCESS: {countryFilePath} @ {lblFileDate.Content}";
                    MessageBox.Show(successMassage, "JSON Data File Update");
                }
                catch (Exception)
                {
                    lblFileDate.Content = $"Update file save error.";
                }       
            }
            else
            {
                // Display the status code and reason phrase
                //Console.WriteLine($"Status code: {(int)response.StatusCode}");
                //Console.WriteLine($"Reason: {response.ReasonPhrase}");
                lblFileDate.Content = $"Update error. \nstatus code: {(int)response.StatusCode}";
            }
            btnUpdateData.IsEnabled = true;

        }
    }
}



