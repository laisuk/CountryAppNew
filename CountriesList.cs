using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using CountryAppNew;
public partial class CountriesList
{
    public static List<CountryModel> GetAllCountryData(string filePath)
    {
        try
        {
            var jsonText = File.ReadAllText(filePath);
            var countryData = JsonSerializer.Deserialize<List<CountryModel>>(jsonText);
            IEnumerable<CountryModel> query = countryData!.OrderBy(x => x.name!.common);

            return query.ToList();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static List<string> GetAllCountryNameList(List<CountryModel> allCountries)
    {
        var allCountryList = allCountries
            .Select(x => x.name?.common)
            .ToList();

        return allCountryList!;
    }

    public static CountryModel GetCurrentCountry(List<CountryModel> allCountries, int id)
    {
        return allCountries[id];
    }

    public static string GetCurrencies(Dictionary<string, Currency> currencies)
    {
        if (currencies == null) { return "N/A"; }

        List<string> currencyList = new List<string>();

        foreach (var currency in currencies)
        {
            currencyList.Add($"{currency.Key} {currency.Value.name} ({currency.Value.symbol ?? "N/A"})");
        }
      
        var _currencies = string.Join(",\n", currencyList!);

        return _currencies;
    }

    public static string GetLanguages(Dictionary<string, string> languages)
    {
        if (languages == null) { return "N/A"; }

        List<string> countryLanguage = new();

        foreach(var language in languages)
        {
            countryLanguage.Add($"{language.Value}");
        }
        var languageList = string.Join(", ", countryLanguage);

        return languageList;
    }

    public static string GetBorders(CountryModel allCountries)
    {
        if (allCountries?.borders != null)
        {
            return string.Join(", ", allCountries?.borders!);
        }
        else
        { return "None"; }
    }

    public static string GetContinents(CountryModel allCountries)
    {
        if (allCountries.continents != null)
        {
            return string.Join(", ", allCountries?.continents!);
        }
        else
        { return "None"; }
    }

    public static string GetDemonyms(CountryModel allCountries)
    {
        var demonyms = $"{allCountries?.demonyms?.First().Value.m} (M)\n{allCountries?.demonyms?.First().Value.f} (F)";
        return demonyms;
    }

    public static string GetJsonFileDate(string filePath)
    {
        return File.GetLastWriteTime(filePath).ToString();
    }

    public static async void UpdateJsonDataFile(string filePath)
    {
        string allCountryUrl = MainWindow.AllCountryUrl;
        string backupFilePath = MainWindow.JsonBackupFilePath;

        if (File.Exists(filePath))
        {
            try
            {
                File.Copy(filePath, backupFilePath, true);
                //string successMassage = $"File backup done: {backupFilePath} @ " + getJsonFileDate(backupFilePath);
                //MessageBox.Show(successMassage, "File Backup");
            }
            catch (HttpRequestException e)
            {
                string errorMassage = "File backup error: " + e.Message;
                MessageBox.Show(errorMassage, GetJsonFileDate(filePath));

                //throw;
            }
        }

        using var client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(allCountryUrl);
        var content = await response.Content.ReadAsStringAsync();

        try
        {
            File.WriteAllText(filePath, content);
            string successMassage = $"File update SUCCESS: {filePath} @ " + GetJsonFileDate(filePath);
            MessageBox.Show(successMassage, "JSON Data File Update");
        }
        catch (Exception e)
        {
            string errorMassage = "File update error: " + e.Message;
            MessageBox.Show(errorMassage, GetJsonFileDate(filePath));

            //throw;
        }
    }   

}





