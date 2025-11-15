using Entity.Models;
using Infastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UserApp.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для CheckQwest.xaml
    /// </summary>
    public partial class CheckQwest : Window
    {
        User User { get; set; }
        public CheckQwest(User user)
        {
            InitializeComponent();

            User = user;
            QwestText.Text = user.QuestForAuth.Title;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (QuestForAuthService.VerifyAnswerAsync(User.QuestForAuth.Id, AnswerText.Text).Result)
            {
                this.DialogResult = true; // Закрывает окно с результатом true
            }
            else
            {
                MessageBox.Show("Неверно, попробуйте ещё");
            }
        }
    }
}
