using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Exceptions;
using Newtonsoft.Json;
using MP3Bot.Model;
using static MP3Bot.Model.TopListModel;

namespace MP3Bot
{
    class MP3bot1
    {
        TelegramBotClient botClient = new TelegramBotClient("5349248072:AAF7jY_C8jbF_-D7NwNggvTMj16B6q9Ds_M");
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
        public async Task Start()
        {
            botClient.StartReceiving(HandlerUpdateAsync, HandlerError, receiverOptions, cancellationToken);
            var botMe = await botClient.GetMeAsync();
            Console.WriteLine($"Бот {botMe.Username} почав працювати");
            Console.ReadKey();
        }

        private Task HandlerError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Помилка в телеграм бот АПІ:\n {apiRequestException.ErrorCode}" +
                $"\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandlerMessageAsync(botClient, update.Message);
            }
            //add
            //
            if (update?.Type == UpdateType.CallbackQuery)
            {
                await HandlerCallbackQuery(botClient, update.CallbackQuery);
            }
            //
        }
        WebClient webClient = new WebClient();

        private async Task HandlerCallbackQuery(ITelegramBotClient botClient, CallbackQuery? callbackQuery)
        {
            if (callbackQuery.Data.StartsWith("Search music"))
            {
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Enter a song title:", replyMarkup: new ForceReplyMarkup { Selective = true });
            }
            else
            if (callbackQuery.Message.ReplyToMessage != null && callbackQuery.Message.ReplyToMessage.Text.Contains("Enter a song title:"))
            {
                try
                {
                    string name = callbackQuery.Message.Text;
                    var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/{name}");
                    var result = JsonConvert.DeserializeObject<MusicSearchModel>(json);

                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Song title: {result.tracks.hits.FirstOrDefault().track.title}" +
                        $"\nArtist: {result.tracks.hits.FirstOrDefault().track.subtitle}" +
                        $"\nShazam Link { result.tracks.hits.FirstOrDefault().track.share.href}" +
                        $"\nSong Image: {result.tracks.hits.FirstOrDefault().track.share.image}" +
                        $"\nArtist avatar: {result.artists.hits.FirstOrDefault().artist.avatar}" +
                        $"\nArtist name: {result.artists.hits.FirstOrDefault().artist.name}" +
                        $"\nArtist link: {result.artists.hits.FirstOrDefault().artist.weburl}");
                }
                catch { await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Invalid form"); }
            }
            if (callbackQuery.Data.StartsWith("Search by term"))
            {
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Enter a term", replyMarkup: new ForceReplyMarkup { Selective = true });
            }
            else
            if (callbackQuery.Message.ReplyToMessage != null && callbackQuery.Message.ReplyToMessage.Text.Contains("Enter a term"))
            {
                try
                {
                    string term = callbackQuery.Message.Text;
                    var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/AutoComplete?term={term}");
                    var result = JsonConvert.DeserializeObject<AutoCompleteModel>(json);

                    for (int i = 0; i < result.hints.Count; i++)
                    {
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Term: {result.hints[i].Term}");
                    }
                }
                catch { await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Invalid form"); }

            }
            else
            if (callbackQuery.Data.StartsWith("Top list"))
            {
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "List:");

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/TopList");

                var result = JsonConvert.DeserializeObject<TopListModel>(json);

                for (int i = 0; i < result.tracks.Count; i++)
                {
                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Title: {result.tracks[i].title}" +
                        $"\nSubtitle: {result.tracks[i].subtitle}" +
                        $"\nLink: {result.tracks[i].share.href}" +
                        $"\nImage: {result.tracks[i].share.image}");
                }

                return;
            }
            else
            if (callbackQuery.Data.StartsWith("Details"))
            {
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Details:");

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/Details");
                var result = JsonConvert.DeserializeObject<DetailsModel>(json);


                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Type: {result.type}" +
                    $"\nKey: {result.key}\nTitle: {result.title}" +
                    $"\nImage: {result.images.background}" +
                    $"\nSubject: {result.share.subject}" +
                    $"\nLink: {result.share.href}" +
                    $"\nSnapchat: {result.share.snapchat}" +
                    $"\nGenre: {result.genres.primary}" +
                    $"\nОther data: {result.sections.FirstOrDefault().metadata[0].title}" +
                    $"\n {result.sections.FirstOrDefault().metadata[0].text}" +
                    $"\n {result.sections.FirstOrDefault().metadata[1].title}" +
                    $"\n {result.sections.FirstOrDefault().metadata[1].text}" +
                    $"\n {result.sections.FirstOrDefault().metadata[2].title} {result.sections.FirstOrDefault().metadata[2].text}");
            }
            else
            if (callbackQuery.Data.StartsWith("Count"))
            {
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Count:");

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/Count");
                var result = JsonConvert.DeserializeObject<CountModel>(json);

                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Id: {result.id}" +
                    $"\nTotal: {result.total}" +
                    $"\nType: {result.type}");
            }
            else
            if (callbackQuery.Data.StartsWith("Music recommendations"))
            {
                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/Recommendations");
                var result = JsonConvert.DeserializeObject<RecommendModel>(json);
                for (int i = 0; i < result.tracks.Count; i++)
                {
                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Song title: {result.tracks[i].title}" +
                        $"\nArtist: {result.tracks[i].subtitle}" +
                        $"\nLink: {result.tracks[i].url}" +
                        $"\nSubject: {result.tracks[i].share.subject}" +
                        $"\nImage: {result.tracks[i].share.image}" +
                        $"\nShazam link: {result.tracks[i].share.href}");
                }
            }
            else
            if (callbackQuery.Data.StartsWith("Charts by countries"))
            {
                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/TracksList");
                var result = JsonConvert.DeserializeObject<TracksListModel>(json);
                for (int i = 0; i < result.countries.Length; i++)
                {
                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Country: {result.countries[i].name}" +
                        $"\nId of the chart: {result.countries[i].listid}");
                }
            }
            else
            if (callbackQuery.Data.StartsWith("Top songs this month"))
            {
                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/Tracks");
                var result = JsonConvert.DeserializeObject<TracksModel>(json);
                for (int i = 0; i < result.tracks.Length; i++)
                {
                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Song title: {result.tracks[i].title}" +
                        $"\nArtist: {result.tracks[i].subtitle}" +
                        $"\nSubject: {result.tracks[i].share.subject}" +
                        $"\nShazam link: {result.tracks[i].share.href}" +
                        $"\nImage: {result.tracks[i].share.image}");
                }
            }
            else
            if (callbackQuery.Data.StartsWith("Podcasts"))
            {
                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/Podcasts");
                var result = JsonConvert.DeserializeObject<PodcastModel>(json);
                for (int i = 0; i < result.podcasts.items.Length; i++)
                {
                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Total count: {result.podcasts.totalCount}" +
                        $"\nPodcast title: {result.podcasts.items[i].data.name}" +
                        $"\nLink: {result.podcasts.items[i].data.uri}" +
                        $"\nType: {result.podcasts.items[i].data.type}" +
                        $"\nPublisher: {result.podcasts.items[i].data.publisher.name}" +
                        $"\nMedia type: {result.podcasts.items[i].data.mediaType}");
                }
            }
            else
            if (callbackQuery.Data.StartsWith("Game of thrones audio book"))
            {
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Audio books");

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/SpecificBook");
                var result = JsonConvert.DeserializeObject<List<SpecificBookModel>>(json);
                for (int i = 0; i < result.Count; i++)
                {
                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Title: {result[i].title}\nPart: {result[i].part}\nLink: {result[i].url}");
                }

            }
            else
            if (callbackQuery.Data.StartsWith("All game of thrones audiobooks"))
            {
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Audio books");

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/AllBooks");
                var result = JsonConvert.DeserializeObject<List<AllBooksModel>>(json);
                for (int i = 0; i < result.Count; i++)
                {
                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Title: {result[i].book}\nPart: {result[i].part}\nLink: {result[i].url}");
                }
            }
            else

            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text: $"You choose: \n{callbackQuery.Data}");
            return;
        }

        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Select a command /keyboard or /inline ");
                return;
            }
            else
            if (message.Text == "/inline")
            {
                InlineKeyboardMarkup keyboardMarkup = new
                        (
                            new[]
                            {
                                new[]
                                {
                                   InlineKeyboardButton.WithCallbackData("Search music", $"Search music"),
                                   InlineKeyboardButton.WithCallbackData("Search by term", $"Search by term")
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Top list", $"Top list"),
                                    InlineKeyboardButton.WithCallbackData("Details", $"Details")
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Count", $"Count"),
                                    InlineKeyboardButton.WithCallbackData("Music recommendations", $"Music recommendations")
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Charts by countries", $"Charts by countries"),
                                    InlineKeyboardButton.WithCallbackData("Top songs this month", $"Top songs this month")
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Podcasts", $"Podcasts"),
                                    InlineKeyboardButton.WithCallbackData("Game of thrones audio book", $"Game of thrones audio book")
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("All game of thrones audiobooks", $"All game of thrones audiobooks"),
                                   
                                },
                            }
                        );
                await botClient.SendTextMessageAsync(message.Chat.Id, "Select a command:", replyMarkup: keyboardMarkup);
                return;
            }
            else
            if (message.Text == "/keyboard")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                    new[]
                        {
                        new KeyboardButton [] { "Search music", "Search by term"},
                        new KeyboardButton [] { "Top list", "Details"},
                        new KeyboardButton [] { "Count", "Music recommendations"},
                        new KeyboardButton [] { "Charts by countries", "Top songs this month "},
                        new KeyboardButton [] { "Podcasts", "Game of thrones audio book"},
                        new KeyboardButton [] { "All game of thrones audiobooks", "Playlists"},
                        }
                    )
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберіть пункт меню:", replyMarkup: replyKeyboardMarkup);
                return;
            }
            else
            if (message.Text == "Search music")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Enter a song title:", replyMarkup: new ForceReplyMarkup { Selective = true });
            }
            else
            if (message.ReplyToMessage != null && message.ReplyToMessage.Text.Contains("Enter a song title:"))
            {
                try
                {
                    string name = message.Text;
                    var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/{name}");
                    var result = JsonConvert.DeserializeObject<MusicSearchModel>(json);

                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Song title: {result.tracks.hits.FirstOrDefault().track.title}" +
                        $"\nArtist: {result.tracks.hits.FirstOrDefault().track.subtitle}" +
                        $"\nShazam Link { result.tracks.hits.FirstOrDefault().track.share.href}" +
                        $"\nSong Image: {result.tracks.hits.FirstOrDefault().track.share.image}" +
                        $"\nArtist avatar: {result.artists.hits.FirstOrDefault().artist.avatar}" +
                        $"\nArtist name: {result.artists.hits.FirstOrDefault().artist.name}" +
                        $"\nArtist link: {result.artists.hits.FirstOrDefault().artist.weburl}");
                }
                catch { await botClient.SendTextMessageAsync(message.Chat.Id, $"Invalid form"); }

            }
            if (message.Text == "Search by term")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Enter a term", replyMarkup: new ForceReplyMarkup { Selective = true});
            }
            else
            if (message.ReplyToMessage != null && message.ReplyToMessage.Text.Contains("Enter a term"))
            {
                try
                {
                    string term = message.Text;
                    var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/AutoComplete?term={term}");
                    var result = JsonConvert.DeserializeObject<AutoCompleteModel>(json);

                    for (int i = 0; i < result.hints.Count; i++)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Term: {result.hints[i].Term}");
                    }
                }
                catch { await botClient.SendTextMessageAsync(message.Chat.Id, $"Invalid form"); }

            }
            if (message.Text == "Top list")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "List:");

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/TopList");

                var result = JsonConvert.DeserializeObject<TopListModel>(json);

                for (int i = 0; i < result.tracks.Count; i++)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Title: {result.tracks[i].title}" +
                        $"\nSubtitle: {result.tracks[i].subtitle}" +
                        $"\nLink: {result.tracks[i].share.href}" +
                        $"\nImage: {result.tracks[i].share.image}");
                }

                return;
            }
            else
            if (message.Text == "Details")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Details:");

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/Details");
                var result = JsonConvert.DeserializeObject<DetailsModel>(json);
                

                await botClient.SendTextMessageAsync(message.Chat.Id, $"Type: {result.type}" +
                    $"\nKey: {result.key}\nTitle: {result.title}" +
                    $"\nImage: {result.images.background}" +
                    $"\nSubject: {result.share.subject}"+
                    $"\nLink: {result.share.href}"+
                    $"\nSnapchat: {result.share.snapchat}"+
                    $"\nGenre: {result.genres.primary}"+
                    $"\nОther data: {result.sections.FirstOrDefault().metadata[0].title}"+
                    $"\n {result.sections.FirstOrDefault().metadata[0].text}"+
                    $"\n {result.sections.FirstOrDefault().metadata[1].title}" +
                    $"\n {result.sections.FirstOrDefault().metadata[1].text}"+
                    $"\n {result.sections.FirstOrDefault().metadata[2].title} {result.sections.FirstOrDefault().metadata[2].text}");

            }
            else
            if (message.Text == "Count")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Count:");

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/Count");
                var result = JsonConvert.DeserializeObject<CountModel>(json);

                await botClient.SendTextMessageAsync(message.Chat.Id, $"Id: {result.id}"+
                    $"\nTotal: {result.total}"+
                    $"\nType: {result.type}");

            }
            else
            if (message.Text == "Music recommendations")
            {
               

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/Recommendations");
                var result = JsonConvert.DeserializeObject<RecommendModel>(json);
                for (int i = 0; i < result.tracks.Count; i++)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Song title: {result.tracks[i].title}"+
                        $"\nArtist: {result.tracks[i].subtitle}"+
                        $"\nLink: {result.tracks[i].url}"+
                        $"\nSubject: {result.tracks[i].share.subject}"+
                        $"\nImage: {result.tracks[i].share.image}"+
                        $"\nShazam link: {result.tracks[i].share.href}");
                }


            }
            else
            if (message.Text == "Charts by countries")
            {


                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/TracksList");
                var result = JsonConvert.DeserializeObject<TracksListModel>(json);
                for (int i = 0; i < result.countries.Length; i++)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Country: {result.countries[i].name}"+
                        $"\nId of the chart: {result.countries[i].listid}");
                }


            }
            else
            if (message.Text == "Top songs this month")
            {


                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/Tracks");
                var result = JsonConvert.DeserializeObject<TracksModel>(json);
                for (int i = 0; i < result.tracks.Length; i++)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Song title: {result.tracks[i].title}" +
                        $"\nArtist: {result.tracks[i].subtitle}" +
                        $"\nSubject: {result.tracks[i].share.subject}" +
                        $"\nShazam link: {result.tracks[i].share.href}" +
                        $"\nImage: {result.tracks[i].share.image}");
                }


            }
            else
             if (message.Text == "Podcasts")
            {
               

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/Podcasts");
                var result = JsonConvert.DeserializeObject<PodcastModel>(json);
                for (int i = 0; i < result.podcasts.items.Length; i++)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Total count: {result.podcasts.totalCount}"+
                        $"\nPodcast title: {result.podcasts.items[i].data.name}"+
                        $"\nLink: {result.podcasts.items[i].data.uri}"+
                        $"\nType: {result.podcasts.items[i].data.type}"+
                        $"\nPublisher: {result.podcasts.items[i].data.publisher.name}"+
                        $"\nMedia type: {result.podcasts.items[i].data.mediaType}");
                }


            }
            else
            if (message.Text == "Game of thrones audio book")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Audio books");

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/SpecificBook");
                var result = JsonConvert.DeserializeObject<List<SpecificBookModel>>(json);
                for (int i = 0; i < result.Count; i++)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Title: {result[i].title}\nPart: {result[i].part}\nLink: {result[i].url}");
                }


            }
            else
            if (message.Text == "All game of thrones audiobooks")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Audio books");

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/AllBooks");
                var result = JsonConvert.DeserializeObject<List<AllBooksModel>>(json);
                for (int i = 0; i < result.Count; i++)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Title: {result[i].book}\nPart: {result[i].part}\nLink: {result[i].url}");
                }


            }
            else
            if (message.Text == "Playlists")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Playlists:");

                var json = webClient.DownloadString($"https://apimusicaudiobook.herokuapp.com/Music/Playlists");
                var result = JsonConvert.DeserializeObject<PlaylistsModel>(json);
                for (int i = 0; i < 20; i++)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Added at: {result.items[i].added_at}"+
                        $"\nAlbum type: {result.items[i].track.album.album_type}"+
                        $"\nLink: {result.items[i].track.external_urls.spotify}"+
                        $"\nSong title: {result.items[i].track.name}"+
                        $"\nRelease date: {result.items[i].track.album.release_date}"+
                        $"\nTotal tracks: {result.items[i].track.album.total_tracks}"+
                        $"\nPlaylist link: {result.items[i].track.album.uri}");
                }


            }





        }
    }
}
