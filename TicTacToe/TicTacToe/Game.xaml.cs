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

    public partial class Game : ContentPage
    {
        private GameState ourGameState;
        public Game()
        {
            InitializeComponent();
            ourGameState = new GameState();
          
            if (!IsPlayerFirst()) // jeśli gracz nie zaczyna pierwszy, niech komputer wykona jeden ruch 
            {
                ComputerMove();
            }
            ShowGameState(ourGameState.State);
        }

        private void ShowGameState(char[,] state)
        {
           // btn00.Text = GameState.mode;
            btn00.Text = state[0, 0].ToString();
            btn01.Text = state[0, 1].ToString();
            btn02.Text = state[0, 2].ToString();
            btn10.Text = state[1, 0].ToString();
            btn11.Text = state[1, 1].ToString();
            btn12.Text = state[1, 2].ToString();
            btn20.Text = state[2, 0].ToString();
            btn21.Text = state[2, 1].ToString();
            btn22.Text = state[2, 2].ToString();
        }

        private void btn_Clicked(object sender, EventArgs e)
        {
            bool successfulClick = false; // jeśli gracz nacisnął przycisk nie zajęty
            if (!ourGameState.EndOfTheGame())
            {
                Button clickedButton = sender as Button;
                if (clickedButton == btn00) { successfulClick = ourGameState.PlayerMove(0, 0); }
                if (clickedButton == btn01) { successfulClick = ourGameState.PlayerMove(0, 1); }
                if (clickedButton == btn02) { successfulClick = ourGameState.PlayerMove(0, 2); }
                if (clickedButton == btn10) { successfulClick = ourGameState.PlayerMove(1, 0); }
                if (clickedButton == btn11) { successfulClick = ourGameState.PlayerMove(1, 1); }
                if (clickedButton == btn12) { successfulClick = ourGameState.PlayerMove(1, 2); }
                if (clickedButton == btn20) { successfulClick = ourGameState.PlayerMove(2, 0); }
                if (clickedButton == btn21) { successfulClick = ourGameState.PlayerMove(2, 1); }
                if (clickedButton == btn22) { successfulClick = ourGameState.PlayerMove(2, 2); }

                ShowGameState(ourGameState.State);
            }
            if (!ourGameState.EndOfTheGame() && successfulClick == true)
            {
                ComputerMove();
                ShowGameState(ourGameState.State);
            }

            if (ourGameState.EndOfTheGame())
            {
                DisplayAlert("The End", ourGameState.Winner, "OK");
            }
        }

        private void ComputerMove()
        {
            if(GameState.mode == "Easy")
            {
                ourGameState.ComputerMoveEasy();
            }
            else if(GameState.mode == "Medium")
            {
                ourGameState.ComputerMoveMedium();
            }
            else if(GameState.mode == "Hard")
            {
                ourGameState.ComputerMoveHard();
            }
        }
       
        private bool IsPlayerFirst()
        {
            bool isFirst = false;
            Random rand = new Random();
            int first = rand.Next(0, 2);
            if(first == 0) // jak wylosujemy 0 - zaczyna gracz 
            {
                isFirst = true;
            }

            
            return isFirst;
        }
    }
}