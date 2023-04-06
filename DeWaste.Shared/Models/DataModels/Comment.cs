using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace DeWaste.Models.DataModels
{
    public class Comment : INotifyPropertyChanged
    {
        [JsonIgnore]
        private int _id;
        public int id {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("id");
            }
        }


        [JsonIgnore]
        private Guid _user_id;
        public Guid user_id
        {
            get => _user_id;
            set
            {
                _user_id = value;
                OnPropertyChanged("user_id");
            }
        }

        [JsonIgnore]
        public int _item_id;
        public int item_id
        {
            get => _item_id;
            set
            {
                _item_id = value;
                OnPropertyChanged("item_id");
            }
        }

        [JsonIgnore]
        public DateTimeOffset _timestamp { get; set; }
        public DateTimeOffset timestamp
        {
            get => _timestamp;
            set
            {
                _timestamp = value;
                OnPropertyChanged("timestamp");
            }
        }

        [JsonIgnore]
        public string _content { get; set; }
        public string content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged("content");
            }
        }

        [JsonIgnore]
        public int _likes { get; set; }
        public int likes
        {
            get => _likes;
            set
            {
                _likes = value;
                OnPropertyChanged("likes");
            }
        }

        [JsonIgnore]
        public int _dislikes { get; set; }
        public int dislikes
        {
            get => _dislikes;
            set
            {
                _dislikes = value;
                OnPropertyChanged("dislikes");
            }
        }


        [JsonIgnore]
        public bool isLiked = false;
        [JsonIgnore]
        public bool isDisliked = false;
        [JsonIgnore]
        public bool isUsersComment;

        [JsonIgnore]
        public string Date { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
