using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Xml.Linq;
using CsvHelper;
using System.Reflection.Metadata;
using System.Globalization;

namespace WebScraper
{
    class Program
    {
        public class WeatherData
        {
            //Create the classes to hold the data for our city weather information
            public string? City { get; set; }
            public string? Temperature { get; set; }
            public string? Conditions { get; set; }
        }
        static void Main(string[] args)
        {
            // Create the string from the URL of the website we want to scrape
            String url = "https://weather.com/weather/today/l/44205ba0272ad652327f1e43ace498ed2dfceb38df0642ee4574d388c79489ea";

            //  Instantiate the variables from the HTML pack 
            var httpClient = new HttpClient();
            var htmlDocument = new HtmlDocument();

            //Create an HTML variable with the HttpClient we are getting information from, sending a get request to the URL, and getting the result of that request
            var html = httpClient.GetStringAsync(url).Result;

            // Takes the HTML document information, and loads it with our HTML variable
            htmlDocument.LoadHtml(html);

            /*
             * The following code takes our declared function above, and declares new variables as document information from the website's HTML code.
             * Select a single node from the website's code, and create a get request from the website.
             * Then make a new variable which displays the inner text of that singular node, then
             * trim the text to only show our desired information, and write it out to the console
             */


            // Get tempeture
            var temperatureElement = htmlDocument.DocumentNode.SelectSingleNode("//span[@class='CurrentConditions--tempValue--zUBSz']");

            var temperature = temperatureElement.InnerText.Trim();

            Console.WriteLine("Tempeture is: " + temperature);


            // Get name of item

            var conditionElement = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='CurrentConditions--phraseValue---VS-k']");

            var conditions = conditionElement.InnerText.Trim();

            Console.WriteLine("Conditions: " + conditions);

            // Get name of city 

            var cityElement = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='CurrentConditions--location--yub4l']");

            var city = cityElement.InnerText.Trim();

            Console.WriteLine("Location: " + city);


            // Create a weather data object
            var weatherData = new WeatherData
            {
                City = city,
                Temperature = temperature,
                Conditions = conditions
            };

            // Export to CSV
            exportToCSV(new List<WeatherData> { weatherData });
        }

        static void exportToCSV(List<WeatherData> weatherData)
        {
            using (var sWriter = new StreamWriter("weatherData.csv"))
            using (var csv = new CsvWriter(sWriter, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(weatherData);
            }
        }
    }
}