using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DeWaste.Logging;
using DeWaste.Models.DataModels;

namespace DeWaste.WebServices
{
    public class DatabaseApi : WebApiBase
    {
        IServiceProvider container;
        ILogger logger;

        string url = "http://localhost:5159/";

        public DatabaseApi(IServiceProvider container)
        {
            this.container = container;
            logger = container?.GetService(typeof(ILogger)) as ILogger;
        }

        public async Task<ObservableCollection<Item>> GetSimilar(string name)
        {
            try
            {
                var result = await this.GetAsync(url + "SimilarItems/" + name.ToLower());

                if (result != null)
                {
                    return JsonSerializer.Deserialize<ObservableCollection<Item>>(result);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }
            return new ObservableCollection<Item>();
        }
        
        public async Task<ObservableCollection<Suggestion>> GetSuggestions()
        {
            try
            {
                var result = await this.GetAsync(url + "SimilarItems");

                if (result != null)
                {
                    return JsonSerializer.Deserialize<ObservableCollection<Suggestion>>(result);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }
            return new ObservableCollection<Suggestion>();
        }

        public async Task<Item> GetItem(int id)
        {
            try
            {
                var result = await GetAsync(url + "Items/" + id);

                if (result != null)
                {
                    return JsonSerializer.Deserialize<Item>(result);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }

            return null;
        }

        public async Task<ObservableCollection<Category>> GetCategories(int id)
        {
            try
            {
                var result = await GetAsync(url + "Categories/" + id);

                if (result != null)
                {
                    Guid.NewGuid();
                    return JsonSerializer.Deserialize<ObservableCollection<Category>>(result);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }

            return new ObservableCollection<Category>();
        }

        public async Task<ObservableCollection<Comment>> GetComments(int id)
        {
            try
            {
                var result = await GetAsync(url + "Comments/" + id);

                if (result != null)
                {
                    return JsonSerializer.Deserialize<ObservableCollection<Comment>>(result);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }

            return new ObservableCollection<Comment>();
        }
        
        public async Task<Rating> GetRating(int comment_id, string user_id)
        {
            try
            {
                var result = await GetAsync(url + "Ratings/" + comment_id + "/" + user_id);

                if (result != null)
                {
                    return JsonSerializer.Deserialize<Rating>(result);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }

            return null;
        }

        public async Task<Rating> DeleteRating(int comment_id, string user_id)
        {
            try
            {
                var result = await DeleteAsync(url + "Ratings/" + comment_id + "/" + user_id);

                if (result != null)
                {
                    return JsonSerializer.Deserialize<Rating>(result);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }

            return null;
        }

        public async Task<Rating> PostRating(Rating rating)
        {
            try
            {
                var result = await PostAsync(url + "Ratings", JsonSerializer.Serialize(rating));

                if (result != null)
                {
                    return JsonSerializer.Deserialize<Rating>(result);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }

            return null;
        }

        public async Task<Rating> PutRating(Rating rating)
        {
            try
            {
                var result = await PutAsync(url + "Ratings", JsonSerializer.Serialize(rating));

                if (result != null)
                {
                    return JsonSerializer.Deserialize<Rating>(result);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }

            return null;
        }
        
        public async Task<Comment> UpdateComment(Comment comment)
        {
            try
            {
                var result = await PutAsync(url + "Comments", JsonSerializer.Serialize(comment));

                if (result != null)
                {
                    return JsonSerializer.Deserialize<Comment>(result);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }

            return null;
        }

        public async Task<Comment> PostComment(Comment comment)
        {
            try
            {
                var result = await PostAsync(url + "Comments", JsonSerializer.Serialize(comment));

                if (result != null)
                {
                    return JsonSerializer.Deserialize<Comment>(result);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }
            return null;
        }

        public async Task<Comment> DeleteComment(int id)
        {
            try
            {
                var result = await DeleteAsync(url + "Comments/" + id);

                if (result != null)
                {
                    return JsonSerializer.Deserialize<Comment>(result);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
            }
            return null;
        }
    }
}
