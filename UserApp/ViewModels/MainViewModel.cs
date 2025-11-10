using CommunityToolkit.Mvvm.ComponentModel;
using Entity.Models;
using Infastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.ViewModels.Base;

namespace UserApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        string message = "Для закрытия и открытия шапки нажмите F1";

        [ObservableProperty]
        string avatarPath;

        public delegate void ChangeShapka(int number);

        public ChangeShapka SetShapka;

        public Action OpenShapka;
        public Action CloseShapka;
    }
}
