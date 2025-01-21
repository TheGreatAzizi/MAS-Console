using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;
using System.Net;
using HtmlAgilityPack;

namespace MAS
{

    class Program
    {


        #region full screen seting
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_MAXIMIZE = 3;
        #endregion

        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {

            #region full screen
            // Get the handle of the console window
            IntPtr handle = GetConsoleWindow();

            // Maximize the console window
            ShowWindow(handle, SW_MAXIMIZE);

            // Adjust the window size to full screen
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            // Prevent the console from closing

            #endregion

            //-------------------------------\\

            #region time
            PrintWithAnimation(ConsoleColor.Cyan, "");
            PrintWithAnimation(ConsoleColor.Cyan, "       ╔══════════════════════════════════════════════════╗");
            PrintWithAnimation(ConsoleColor.Cyan, "\t      Current Date and Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  "));
            PrintWithAnimation(ConsoleColor.Cyan, "\t                " + TimeZoneInfo.Local.DisplayName + "                ");
            PrintWithAnimation(ConsoleColor.Cyan, "\t        Welcome to the most random program :)     ");
            PrintWithAnimation(ConsoleColor.Cyan, "       ╚══════════════════════════════════════════════════╝");
            #endregion
            
            //-------------------------------\\

            #region switch
            Console.WriteLine("");
            string khar,khar2;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\t      AniList or IMDB (al/im): ");
            khar = Console.ReadLine();
            switch (khar)
            {
                case "im":
                    Console.WriteLine("");
                    Console.Write("\t      movies or series: ");
                    khar2 = Console.ReadLine();
                    switch (khar2)
                    {
                        case "movies":
                            #region top 10 movie
                            Console.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\t                    ╟────────────────────────────────────────────────────top 10 movies in imdb──────────────────────────────────────────────────────╢");
                            string apiKey2 = "3ac315c9";
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("\t                    ╔═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
                            Console.WriteLine("\t                    ║ Name                                      ║ Genre           ║ Score             ║ Director            ║ Writer                ║", Console.ForegroundColor = ConsoleColor.DarkYellow);
                            Console.WriteLine("\t                    ╟───────────────────────────────────────────╫─────────────────╫───────────────────╫─────────────────────╫───────────────────────╫");

                            int movieCount = 0;

                            for (int i = 1; i <= 10 && movieCount < 10; i++)
                            {
                                string url = $"http://www.omdbapi.com/?apikey={apiKey2}&type=movie&s=top&page={i}";
                                var response = await client.GetStringAsync(url);
                                var movieList = JObject.Parse(response)["Search"];

                                foreach (var movie in movieList)
                                {
                                    string title = movie["Title"].ToString();
                                    if (title.Length > 40)
                                    {
                                        continue;
                                    }

                                    string imdbID = movie["imdbID"].ToString();
                                    var detailUrl = $"http://www.omdbapi.com/?apikey={apiKey2}&i={imdbID}";
                                    var detailResponse = await client.GetStringAsync(detailUrl);
                                    var details = JObject.Parse(detailResponse);

                                    int votes = Convert.ToInt32(details["imdbVotes"].ToString().Replace(",", ""));
                                    double score = Convert.ToDouble(details["imdbRating"]);
                                    string genre = details["Genre"] != null ? details["Genre"].ToString().Split(',')[0] : "N/A";
                                    string writer = details["Writer"] != null ? details["Writer"].ToString().Split(',')[0].Trim() : "N/A";
                                    string director = details["Director"] != null ? details["Director"].ToString().Split(',')[0].Trim() : "N/A";

                                    Console.WriteLine($"\t                    ║ {title.PadRight(40)}  ║ {genre.PadRight(15)} ║ {score.ToString().PadRight(5) + votes.ToString().PadRight(12)} ║ {director.PadRight(20)}║ {writer.PadRight(20)}  ║", Console.ForegroundColor = ConsoleColor.Yellow);
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine("\t                    ╟───────────────────────────────────────────╫─────────────────╫───────────────────╫─────────────────────╫───────────────────────╫");

                                    movieCount++;
                                    if (movieCount >= 10)
                                    {
                                        break;
                                    }
                                }
                            }

                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("\t                    ╚═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");

                            #endregion
                            break;
                        case "series":
                            #region top 10 seriese 
                            Console.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\t                    ╟──────────────────────────────────────────────────────────top 10 series in imdb───────────────────────────────────────────────────────────╢");
                            string apiKey = "3ac315c9";
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("\t                    ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
                            Console.WriteLine("\t                    ║ Name                                      ║ Genre           ║ Score             ║ Episodes ║ Writer                ║ Director            ║", Console.ForegroundColor = ConsoleColor.DarkYellow);
                            Console.WriteLine("\t                    ╟───────────────────────────────────────────╫─────────────────╫───────────────────╫──────────╫───────────────────────╫─────────────────────╢");

                            int seriesCount = 0;

                            for (int i = 1; i <= 10 && seriesCount < 10; i++)
                            {
                                string url = $"http://www.omdbapi.com/?apikey={apiKey}&type=series&s=top&page={i}";
                                var response = await client.GetStringAsync(url);
                                var seriesList = JObject.Parse(response)["Search"];

                                foreach (var series in seriesList)
                                {
                                    string title = series["Title"].ToString();

                                    if (title.Length > 40)
                                    {
                                        continue;
                                    }

                                    string imdbID = series["imdbID"].ToString();

                                    var detailUrl = $"http://www.omdbapi.com/?apikey={apiKey}&i={imdbID}";
                                    var detailResponse = await client.GetStringAsync(detailUrl);
                                    var details = JObject.Parse(detailResponse);

                                    int votes = Convert.ToInt32(details["imdbVotes"].ToString().Replace(",", ""));
                                    double score = Convert.ToDouble(details["imdbRating"]);

                                    string episodes = details["totalSeasons"] != null ? details["totalSeasons"].ToString() : "N/A";
                                    string genre = details["Genre"] != null ? details["Genre"].ToString().Split(',')[0] : "N/A";
                                    string writer = details["Writer"] != null ? details["Writer"].ToString().Split(',')[0].Trim() : "N/A";
                                    string director = details["Director"] != null ? details["Director"].ToString().Split(',')[0].Trim() : "N/A";
                                    Console.WriteLine($"\t                    ║ {title.PadRight(40)}  ║ {genre.PadRight(15)} ║ {score.ToString().PadRight(5) + votes.ToString().PadRight(12)} ║ {episodes.PadRight(7)}  ║ {writer.PadRight(20)}  ║ {director.PadRight(20)}║", Console.ForegroundColor = ConsoleColor.Yellow);
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine("\t                    ╟───────────────────────────────────────────╫─────────────────╫───────────────────╫──────────╫───────────────────────╫─────────────────────╢");

                                    seriesCount++;
                                    if (seriesCount >= 10)
                                    {
                                        break;
                                    }
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("\t                    ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
                            #endregion
                          break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t      shoma kheyli khari!");
                            break;
                    }
                  break;
                case "al":
                    string ggg;
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\t      To continue, you must have a VPN, if confirmed, enter the word (ok): ");
                    ggg = Console.ReadLine();
                    switch (ggg)
                    {
                        case "ok":
                            Console.WriteLine("");
                            string gggg;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("\t      Top 10 anime list or anime search (t10/as): ");
                            gggg = Console.ReadLine();
                            switch (gggg)
                            {
                                case "t10":
                                    #region top 10 anime
                                    var client = new HttpClient();
                                    var query = @"
            query {
              Page(page: 1, perPage: 10) {
                media(type: ANIME, sort: SCORE_DESC) {
                  title {
                    romaji
                  }
                  genres
                  averageScore
                  episodes
                  staff {
                    edges {
                      node {
                        name {
                          full
                        }
                        primaryOccupations
                      }
                    }
                  }
                }
              }
            }";

                                    var jsonString = new JObject(
                                        new JProperty("query", query)
                                    ).ToString();

                                    var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                                    var response2 = await client.PostAsync("https://graphql.anilist.co", content);
                                    var responseString = await response2.Content.ReadAsStringAsync();

                                    var json = JObject.Parse(responseString);
                                    var topAnime = json["data"]["Page"]["media"];

                                    Console.WriteLine("");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\t                    ╟──────────────────────────────────────────────────────────top 10 anime in anilist─────────────────────────────────────────────────────────╢");
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine("\t                    ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
                                    Console.WriteLine("\t                    ║ Name                                      ║ Genre           ║ Score             ║ Episodes ║ Writer                ║ Director            ║", Console.ForegroundColor = ConsoleColor.DarkYellow);
                                    Console.WriteLine("\t                    ╟───────────────────────────────────────────╫─────────────────╫───────────────────╫──────────╫───────────────────────╫─────────────────────╢");

                                    foreach (var anime in topAnime)
                                    {
                                        string title = anime["title"]["romaji"].ToString();
                                        if (title.Length > 40)
                                        {
                                            title = title.Substring(0, title.Length / 2) + "...";
                                        }

                                        string genre = anime["genres"] != null ? anime["genres"][0].ToString() : "N/A";
                                        string score = anime["averageScore"] != null ? anime["averageScore"].ToString() : "N/A";
                                        string episodes = anime["episodes"] != null ? anime["episodes"].ToString() : "N/A";
                                        string writer = "N/A";
                                        string director = "N/A";

                                        foreach (var staff in anime["staff"]["edges"])
                                        {
                                            var occupations = staff["node"]["primaryOccupations"].ToString().ToLower();
                                            if (writer == "N/A" && occupations.Contains("writer"))
                                            {
                                                writer = staff["node"]["name"]["full"].ToString();
                                            }
                                            if (director == "N/A" && occupations.Contains("director"))
                                            {
                                                director = staff["node"]["name"]["full"].ToString();
                                            }
                                        }

                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine($"\t                    ║ {title.PadRight(40)}  ║ {genre.PadRight(15)} ║ {score.PadRight(5)}             ║ {episodes.PadRight(7)}  ║ {writer.PadRight(20)}  ║ {director.PadRight(20)}║");
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine("\t                    ╟───────────────────────────────────────────╫─────────────────╫───────────────────╫──────────╫───────────────────────╫─────────────────────╢");
                                    }

                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine("\t                    ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
                                    #endregion
                                    break;
                                case "as":
                                    #region anime search
                                    try
                                    {
                                        Console.WriteLine("");
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write("\t      Please enter the Romaji or English name of the anime: ");
                                        string HR_animeName = Console.ReadLine();

                                        string HR_query = @"
                query ($search: String) {
                    Media(search: $search, type: ANIME) {
                        id
                        title {
                            romaji
                            english
                        }
                        description
                        episodes
                        averageScore
                        genres
                        startDate {
                            year
                            month
                            day
                        }
                        studios {
                            edges {
                                node {
                                    name
                                }
                            }
                        }
                        relations {
                            edges {
                                node {
                                    title {
                                        romaji
                                        english
                                    }
                                    type
                                    format
                                }
                            }
                        }
                    }
                    Page(perPage: 10) {
                        media(search: $search, type: ANIME) {
                            title {
                                romaji
                                english
                            }
                        }
                    }
                }";

                                        var HR_variables = new
                                        {
                                            search = HR_animeName
                                        };

                                        var HR_jsonString = new JObject(
                                            new JProperty("query", HR_query),
                                            new JProperty("variables", JObject.FromObject(HR_variables))
                                        ).ToString();

                                        var HR_content = new StringContent(HR_jsonString, System.Text.Encoding.UTF8, "application/json");

                                        ServicePointManager.ServerCertificateValidationCallback +=
                                            (sender, cert, chain, sslPolicyErrors) => true;

                                        using (HttpClient HR_client = new HttpClient())
                                        {
                                            HR_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                            var HR_response = await HR_client.PostAsync("https://graphql.anilist.co", HR_content);
                                            var HR_responseString = await HR_response.Content.ReadAsStringAsync();

                                            if (!HR_response.IsSuccessStatusCode)
                                            {
                                                Console.WriteLine($"\t      An error occurred: {HR_response.StatusCode} - {HR_responseString}");
                                                return;
                                            }

                                            var HR_json = JObject.Parse(HR_responseString);
                                            var HR_media = HR_json["data"]["Media"];
                                            var HR_relatedMedia = HR_json["data"]["Page"]["media"];

                                            if (HR_media != null)
                                            {
                                                string description = HR_media["description"].ToString().Replace("<br>", "").Replace("<br />", "");

                                                Console.ForegroundColor = ConsoleColor.White;
                                                Console.WriteLine("");
                                                Console.WriteLine("********************************************** Anime Information *********************************************");
                                                Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
                                                Console.ForegroundColor = ConsoleColor.White;
                                                Console.WriteLine($"Title (Romaji): {HR_media["title"]["romaji"]}");
                                                Console.WriteLine($"Title (English): {HR_media["title"]["english"] ?? "N/A"}");
                                                Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
                                                Console.WriteLine($"Description: " + description);
                                                Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
                                                Console.WriteLine($"Episodes: {HR_media["episodes"]}");
                                                Console.WriteLine($"Average Score: {HR_media["averageScore"]}");
                                                Console.WriteLine($"Genres: {string.Join(", ", HR_media["genres"])}");
                                                Console.WriteLine($"Start Date: {HR_media["startDate"]["year"]}-{HR_media["startDate"]["month"]}-{HR_media["startDate"]["day"]}");
                                                Console.WriteLine($"Studios: {string.Join(", ", HR_media["studios"]["edges"].Select(edge => edge["node"]["name"]))}");
                                                Console.WriteLine("--------------------------------------------------------------------------------------------------------------");

                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine("Related Seasons and Spin-offs:");
                                                foreach (var relation in HR_media["relations"]["edges"])
                                                {
                                                    var titleRomaji = relation["node"]["title"]["romaji"];
                                                    var titleEnglish = relation["node"]["title"]["english"] ?? "N/A";
                                                    var type = relation["node"]["type"];
                                                    var format = relation["node"]["format"];
                                                    Console.WriteLine($"{titleRomaji} (English: {titleEnglish}) - Type: {type}, Format: {format}");
                                                }
                                                Console.ForegroundColor = ConsoleColor.White;
                                                Console.WriteLine("**************************************************************************************************************");
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\t      Anime not found.");
                                            }

                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                            Console.WriteLine("");
                                            Console.WriteLine("\t                    ╔══════════════════════════════════════════════════╗");
                                            Console.WriteLine("\t                    ║ Maybe you meant one of these:                    ║");
                                            Console.WriteLine("\t                    ╟──────────────────────────────────────────────────╢");
                                            if (HR_relatedMedia.HasValues)
                                            {
                                                foreach (var HR_anime in HR_relatedMedia)
                                                {
                                                    var titleRomaji = HR_anime["title"]["romaji"].ToString();
                                                    var titleEnglish = HR_anime["title"]["english"]?.ToString() ?? "N/A";

                                                    if (titleRomaji.Length > 20)
                                                    {
                                                        titleRomaji = titleRomaji.Substring(0, 20) + "...";
                                                    }
                                                    if (titleEnglish.Length > 20)
                                                    {
                                                        titleEnglish = titleEnglish.Substring(0, 20) + "...";
                                                    }

                                                    Console.WriteLine($"\t                    ║ Title (Romaji): {titleRomaji.PadRight(30)}   ║", Console.ForegroundColor = ConsoleColor.Yellow);
                                                    Console.WriteLine($"\t                    ║ Title (English): {titleEnglish.PadRight(30)}  ║", Console.ForegroundColor = ConsoleColor.Yellow);
                                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                                    Console.WriteLine("\t                    ╟──────────────────────────────────────────────────╢");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine($"\t      An error occurred: NotFound - {HR_responseString}");
                                            }

                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                            Console.WriteLine("\t                    ╚══════════════════════════════════════════════════╝");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"\t      An error occurred: {ex.Message}");
                                    }
                                    #endregion
                                    break;
                                default:
                                    Console.WriteLine("");
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\t      shoma kheyli khari!");
                                    break;
                            }                           
                        break;
                        default:
                            Console.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t      shoma kheyli khari!");
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t      shoma kheyli khari!");
                    break;
            }
            #endregion

            //--------------------------------\\

            Console.ReadKey();

        }
        
        #region time seting
        static void PrintWithAnimation(ConsoleColor textColor, string text, ConsoleColor? backgroundColor = null, bool clearAfter = false)
        {
            if (backgroundColor.HasValue)
            {
                Console.BackgroundColor = backgroundColor.Value;
            }
            Console.ForegroundColor = textColor;

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(5);
            }
            Console.WriteLine();

            if (clearAfter)
            {
                Console.ResetColor();
            }
        }
        #endregion

    }
}