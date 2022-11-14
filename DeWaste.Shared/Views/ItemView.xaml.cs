﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;
using DeWaste.Models.DataModels;
using DeWaste.Models.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

using System.Runtime.CompilerServices;

namespace DeWaste.Views
{
    public sealed partial class ItemView : Page , INotifyPropertyChanged
    {
        ItemViewModel ViewModel;
        IServiceProvider container = ((App)App.Current).Container;

        public ItemView()
        {
            PlasticCategoryVisibility = "Collapsed";
            PaperCategoryVisibility = "Collapsed";
            MetalCategoryVisibility = "Collapsed";
            WoodCategoryVisibility = "Collapsed";
            GlassCategoryVisibility = "Collapsed";
            this.InitializeComponent();
            ViewModel = ActivatorUtilities.GetServiceOrCreateInstance(container, typeof(ItemViewModel)) as ItemViewModel;
            DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                return;
            } 
            ViewModel.SetItem((Item)e.Parameter);
        }


        //Plastic
        private void PlasticCategoryVisibility_Click(object sender, RoutedEventArgs e)
        {
            PlasticCategoryVisibility = "Visible";
            PaperCategoryVisibility = "Collapsed";
            MetalCategoryVisibility = "Collapsed";
            WoodCategoryVisibility = "Collapsed";
            GlassCategoryVisibility = "Collapsed";
        }

        private string _PlasticCategoryVisibility;
        public string PlasticCategoryVisibility
        {
            get { return _PlasticCategoryVisibility; }
            set
            {
                if (_PlasticCategoryVisibility != value)
                {
                    _PlasticCategoryVisibility = value;
                    OnPropertyChanged();
                }
            }
        }


        //Paper
        private void PaperCategoryVisibility_Click(object sender, RoutedEventArgs e)
        {
            PlasticCategoryVisibility = "Collapsed";
            PaperCategoryVisibility = "Visible";
            MetalCategoryVisibility = "Collapsed";
            WoodCategoryVisibility = "Collapsed";
            GlassCategoryVisibility = "Collapsed";
        }


        private string _PaperCategoryVisibility;
        public string PaperCategoryVisibility
        {
            get { return _PaperCategoryVisibility; }
            set
            {
                if (_PaperCategoryVisibility != value)
                {
                    _PaperCategoryVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        //Metal
        private void MetalCategoryVisibility_Click(object sender, RoutedEventArgs e)
        {
            PlasticCategoryVisibility = "Collapsed";
            PaperCategoryVisibility = "Collapsed";
            MetalCategoryVisibility = "Visible";
            WoodCategoryVisibility = "Collapsed";
            GlassCategoryVisibility = "Collapsed";
        }


        private string _MetalCategoryVisibility;
        public string MetalCategoryVisibility
        {
            get { return _MetalCategoryVisibility; }
            set
            {
                if (_MetalCategoryVisibility != value)
                {
                    _MetalCategoryVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        //Wood
        private void WoodCategoryVisibility_Click(object sender, RoutedEventArgs e)
        {
            PlasticCategoryVisibility = "Collapsed";
            PaperCategoryVisibility = "Collapsed";
            MetalCategoryVisibility = "Collapsed";
            WoodCategoryVisibility = "Visible";
            GlassCategoryVisibility = "Collapsed";
        }


        private string _WoodCategoryVisibility;
        public string WoodCategoryVisibility
        {
            get { return _WoodCategoryVisibility; }
            set
            {
                if (_WoodCategoryVisibility != value)
                {
                    _WoodCategoryVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        //Glass
        private void GlassCategoryVisibility_Click(object sender, RoutedEventArgs e)
        {
            PlasticCategoryVisibility = "Collapsed";
            PaperCategoryVisibility = "Collapsed";
            MetalCategoryVisibility = "Collapsed";
            WoodCategoryVisibility = "Collapsed";
            GlassCategoryVisibility = "Visible";
        }


        private string _GlassCategoryVisibility;
        public string GlassCategoryVisibility
        {
            get { return _GlassCategoryVisibility; }
            set
            {
                if (_GlassCategoryVisibility != value)
                {
                    _GlassCategoryVisibility = value;
                    OnPropertyChanged();
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
