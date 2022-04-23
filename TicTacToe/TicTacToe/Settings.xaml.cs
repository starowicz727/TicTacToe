using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicTacToe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public static string pickedMode = "Easy";
        public Settings()
        {
            InitializeComponent();
           // label1.Text = GameState.mode;
            MarkRadioButton();
        }

        private void MarkRadioButton() //jeśli gracz najpierw wszedł w grę to mode ma byc domyslnie easy
        {
            if (GameState.mode == "Easy")
            {
                rbtnEasy.IsChecked = true;
            } 
            else if (GameState.mode == "Medium")
            {
                rbtnMedium.IsChecked = true;
            }
            else if (GameState.mode == "Hard")
            {
                rbtnHard.IsChecked = true;
            }
        }

        private void rbtn_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton clickedBtn = sender as RadioButton;
            pickedMode = clickedBtn.Value.ToString();

          //  label1.Text = pickedMode;
        }

    }
}