using DeWaste.Models.DataModels;
using DeWaste.WebServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Diagnostics;
using DeWaste.Logging;

namespace DeWaste.Services
{
    public class DataHandler : IDataHandler
    {
        DatabaseApi databaseApi = null;

        private ObservableCollection<Suggestion> suggestions = new ObservableCollection<Suggestion>();

        private Dictionary<int, Item> items = new Dictionary<int, Item>();

        private Guid UID = Guid.Empty;


        private string suggestionsPath = "suggestions.json";
        private string itemsPath = "items.json";
        private string idPath = "id.json";


        IServiceProvider container;
        IFileHandler fileHandler;
        ILogger logger;

        bool StringIsNullOrEmpty(string str)
        {
            return str == null || str == "";
        }

        private async void LoadUID()
        {
            try
            {
                var str_id = await fileHandler.ReadFileContentsAsync(idPath);

                if (StringIsNullOrEmpty(str_id))
                {
                    UID = Guid.NewGuid();
                    await fileHandler.WriteDataToFileAsync(idPath, UID.ToString());
                }
                else
                {
                    UID = Guid.Parse(str_id);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
                //UID = "Test";
            }
        }



        private async void SaveSuggestions()
        {
            try
            {
                string data = JsonSerializer.Serialize<ObservableCollection<Suggestion>>(suggestions);
                await fileHandler.WriteDataToFileAsync(suggestionsPath, data);
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }
        }
        
        private async void LoadSavedSuggestions()
        {
            try
            {
                string data = await fileHandler.ReadFileContentsAsync(suggestionsPath);
                if (!StringIsNullOrEmpty(data))
                {
                    suggestions = JsonSerializer.Deserialize<ObservableCollection<Suggestion>>(data);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }
        }

        private async void LoadSavedItems()
        {
            try
            {
                string data = await fileHandler.ReadFileContentsAsync(itemsPath);

                if (!StringIsNullOrEmpty(data))
                {
                    items = JsonSerializer.Deserialize<Dictionary<int, Item>>(data);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }
        }

        private async void SaveItems()
        {
            try
            {
                string data = JsonSerializer.Serialize<Dictionary<int, Item>>(items);
                await fileHandler.WriteDataToFileAsync(itemsPath, data);
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }
        }


        public DataHandler(IServiceProvider container)
        {
            this.container = container;
            databaseApi = new DatabaseApi(container);
            logger = container.GetService(typeof(ILogger)) as ILogger;
            fileHandler = (IFileHandler)container.GetService(typeof(IFileHandler));
            LoadSavedSuggestions();
            LoadSavedItems();
            LoadUID();
        }
        

        public async Task<ObservableCollection<Suggestion>> GetSimilar(string name)
        {
            try
            {
                ObservableCollection<Suggestion> res = await databaseApi.GetSuggestions();
                suggestions = new ObservableCollection<Suggestion>(suggestions.Union(res, new UniqueSuggestionIDComparer()));

                Regex similarStrings = new Regex(name, RegexOptions.IgnoreCase);
                var filteredRes = new ObservableCollection<Suggestion>(suggestions.Where(sug => similarStrings.IsMatch(sug.name)));

                SaveSuggestions();


                return filteredRes;
            }
 
            catch (Exception ex)
            {
                logger.Log(ex.Message);
                return new ObservableCollection<Suggestion>();
            }
        }

        public async Task<Item> GetItemById(int id)
        {
            try
            {
                Item item = await databaseApi.GetItem(id: id);

                if (item != null)
                {
                    if (item.categories == null)
                        item.categories = await databaseApi.GetCategories(id: id);
                    items[id] = item;
                    SaveItems();
                }

                return items[id];
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
                return null;
            }
        }

        public async Task<ObservableCollection<Comment>> GetCommentsByItemID(int id)
        {
            ObservableCollection<Comment> comments = await databaseApi.GetComments(id);

            foreach (Comment comment in comments)
            {
                comment.Date = comment.timestamp.ToLocalTime().ToString("g");
                comment.isUsersComment = comment.user_id == UID;
                Rating rating = await databaseApi.GetRating(comment.id, UID.ToString());
                if (rating != null)
                {
                    if (rating.is_liked)
                    {
                        comment.isLiked = true;
                    }
                    else
                    {
                        comment.isDisliked = true;
                    }
                }
            }
            
            //sort comments first by likes then timestamp
            comments = new ObservableCollection<Comment>(comments.OrderByDescending(comment => comment.likes - comment.dislikes).ThenByDescending(comment => comment.timestamp));

            return comments;
        }

        public async Task<Comment> SubmitComment(int item_id, string content)
        {
            Comment comment = new Comment()
            {
                content = content,
                item_id = item_id,
                user_id = UID,
                timestamp = DateTimeOffset.UtcNow
            };
            Comment received = await databaseApi.PostComment(comment);

            received.Date = comment.timestamp.ToLocalTime().ToString("g");
            received.isUsersComment = comment.user_id == UID;

            return received;
        }

        public async Task<Comment> DeleteComment(int id)
        {
            Comment comment = await databaseApi.DeleteComment(id);
            return comment;
        }

        public async Task<Comment> LikeComment(Comment comment)
        {
            Rating rating = new Rating()
            {
                comment_id = comment.id,
                is_liked = true,
                user_id = UID
            };
            if (comment.isLiked)
            {
                comment.isLiked = false;
                comment.likes--;
                await databaseApi.DeleteRating(comment.id, UID.ToString());
            }
            else if(comment.isDisliked)
            {
                comment.isDisliked = false;
                comment.dislikes--;
                comment.isLiked = true;
                comment.likes++;

                await databaseApi.PutRating(rating);
            }
            else
            {
                comment.isLiked = true;
                comment.likes++;
                
                await databaseApi.PostRating(rating);
            }

            Comment newComment = await databaseApi.UpdateComment(comment);
            newComment.Date = newComment.timestamp.ToLocalTime().ToString("g");
            newComment.isLiked = comment.isLiked;
            newComment.isDisliked = comment.isDisliked;
            newComment.isUsersComment = newComment.user_id == UID;

            return newComment;
        }

        public async Task<Comment> DislikeComment(Comment comment)
        {
            Rating rating = new Rating()
            {
                comment_id = comment.id,
                is_liked = false,
                user_id = UID
            };
            if (comment.isDisliked)
            {
                comment.isDisliked = false;
                comment.dislikes--;
                await databaseApi.DeleteRating(comment.id, UID.ToString());
            }
            else if (comment.isLiked)
            {
                comment.isDisliked = true;
                comment.dislikes++;
                comment.isLiked = false;
                comment.likes--;

                await databaseApi.PutRating(rating);
            }
            else
            {
                comment.isDisliked = true;
                comment.dislikes++;

                await databaseApi.PostRating(rating);
            }

            Comment newComment = await databaseApi.UpdateComment(comment);
            newComment.Date = newComment.timestamp.ToLocalTime().ToString("g");
            newComment.isLiked = comment.isLiked;
            newComment.isDisliked = comment.isDisliked;
            newComment.isUsersComment = newComment.user_id == UID;

            return newComment;
        }
    }
}
