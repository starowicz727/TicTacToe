using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
    class GameState
    {
        public static string mode;
        char[,] state;
        private string winner = ""; //może przyjąć wartości "", "You won", "Computer won", "Draw"

        public GameState()
        {
            state = new char[,] { { ' ', ' ', ' ' }, { ' ', ' ', ' ' }, { ' ', ' ', ' ' } };
            mode = Settings.pickedMode;
        }
        public char[,] State { get => state; }
        public string Winner { get => winner; }

        public bool PlayerMove(int x, int y)
        {
            bool success = false; //czy udało się kliknąć w nie zajęty przycisk

            if(state[x,y] == ' ') //jeśli na danym przycisku nie ma X ani O
            {
                state[x, y] = 'X'; 
                success = true;
            }

            return success;
        }

        public void ComputerMoveEasy() // w trybie Easy komputer losuje randomowe miejsce na planszy
        {
            Random rand = new Random();
            int x = rand.Next(0, 3);
            int y = rand.Next(0, 3);

            while (state[x, y] != ' ')
            {
                x = rand.Next(0, 3);
                y = rand.Next(0, 3);
            }

            state[x, y] = 'O';
        }

        public void ComputerMoveMedium() 
        {//1. jeśli w jakimś rzędzie/kolumnie/przekątnej są 2 O, to dopisujemy trzecie i komputer wygrywa
         //2. jeśli w jakimś rzędzie/kolumnie/przekątnej są dwa X, to dopisujemy O i gracz nie wygrywa
         //3. jeśli powayższe opcje nie występują, to losujemy miejsce dla O

            // ~~~~~~~~~~~~~~~~~~~~~~punkt 1~~~~~~~~~~~~~~~~~~~~~~ 
            for (int i = 0; i < state.GetLength(0); i++) 
            {
                if (CheckRows('O','O')){ return; }
                if (CheckColumns('O', 'O')) { return; }
            }
            if (CheckDiagonal1('O', 'O')) { return; }
            if (CheckDiagonal2('O', 'O')) { return; }

            // ~~~~~~~~~~~~~~~~~~~~~~punkt 2~~~~~~~~~~~~~~~~~~~~~~ 
            for (int i = 0; i < state.GetLength(0); i++)
            {
                if (CheckRows('X', 'O')) { return; }
                if (CheckColumns('X', 'O')) { return; }
            }
            if (CheckDiagonal1('X', 'O')) { return; }
            if (CheckDiagonal2('X', 'O')) { return; }

            // ~~~~~~~~~~~~~~~~~~~~~~punkt 3~~~~~~~~~~~~~~~~~~~~~~ 
            ComputerMoveEasy();
        }

        public bool CheckDiagonal1(char currentPlayer, char writeXorO) // przekątna \  funkcja zwraca true gdy znajdzie miejsce 
        {
            if (state[0, 0] == currentPlayer && state[1, 1] == currentPlayer && state[2, 2] == ' ')
            {
                state[2, 2] = writeXorO; return true;
            }
            if (state[1, 1] == currentPlayer && state[2, 2] == currentPlayer && state[0, 0] == ' ')
            {
                state[0, 0] = writeXorO; return true;
            }
            if (state[0, 0] == currentPlayer && state[2, 2] == currentPlayer && state[1, 1] == ' ')
            {
                state[1, 1] = writeXorO; return true;
            }
            return false;
        }
        public bool CheckDiagonal2(char currentPlayer, char writeXorO) // przekątna / funkcja zwraca true gdy znajdzie miejsce 
        {
            if (state[0, 2] == currentPlayer && state[1, 1] == currentPlayer && state[2, 0] == ' ')
            {
                state[2, 0] = writeXorO; return true;
            }
            if (state[1, 1] == currentPlayer && state[2, 0] == currentPlayer && state[0, 2] == ' ')
            {
                state[0, 2] = writeXorO; return true;
            }
            if (state[0, 2] == currentPlayer && state[2, 0] == currentPlayer && state[1, 1] == ' ')
            {
                state[1, 1] = writeXorO; return true;
            }
            return false;
        }
        public bool CheckRows(char currentPlayer, char writeXorO) //czy są 2 takie same znaki w rzędzie
        { 
            for(int i = 0; i < state.GetLength(1); i++)
            {
                if( state[i, 0]==state[i, 1]  && state[i, 0] == currentPlayer) //jesli w rzędzie element 0==1 
                {
                    if(state[i, 2]==' ') { state[i, 2] = writeXorO; return true; }
                }
                if (state[i, 1] == state[i, 2] && state[i, 1] == currentPlayer) //jesli 1==2 
                {
                    if (state[i, 0] == ' ') { state[i, 0] = writeXorO; return true; }
                }
                if (state[i, 0] == state[i, 2] && state[i, 0] == currentPlayer) //jesli 0==2 
                {
                    if (state[i, 1] == ' ') { state[i, 1] = writeXorO; return true; }
                }
            }
            return false;
        }
        public bool CheckColumns(char currentPlayer, char writeXorO) // czy są 2 takie same znaki w kolumnie
        {
            for (int i = 0; i < state.GetLength(1); i++)
            {
                if (state[0, i] == state[1, i] && state[0, i] == currentPlayer) //jesli w kolumnie element 0==1 
                {
                    if (state[2, i] == ' ') { state[2, i] = writeXorO; return true; }
                }
                if (state[1, i] == state[2, i] && state[1, i] == currentPlayer) //jesli 1==2 
                {
                    if (state[0, i] == ' ') { state[0, i] = writeXorO; return true; }
                }
                if (state[0, i] == state[2, i] && state[0, i] == currentPlayer) //jesli 0==2 
                {
                    if (state[1, i] == ' ') { state[1, i] = writeXorO; return true; }
                }
            }
            return false;
        }

        public void ComputerMoveHard() //w trybie hard użyty będzie algorytm minimax
        {
            int bestScore = int.MinValue;
            int[] move = new int[2]; // {x,y} 

            for (int i = 0; i< state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    if(state[i,j] == ' ') //jeśli przeszukiwane miejsce nie jest zajęte
                    {
                        state[i, j] = 'O'; // O - komputer - stawiamy znak O i testujemy czy to najlepszy wynik
                        int score = MiniMax(state, 0, false); //w minimax szukamy najlepszego wyniku = najlepszego miejsca do zaznaczenia O
                        //drzewo dopuszczalnych rozwiązań przeszukujemy od 0 (obecnej sytuacji), isMaximizing=false, bo teraz jest kolej na gracza
                        state[i, j] = ' '; // od razu usuwam O do następnych testów w pętli 
                        if(score > bestScore)
                        {
                            bestScore = score;
                            move[0] = i; move[1] = j;
                        }
                    }
                }
            }
            state[move[0], move[1]] = 'O';
        }

        private int MiniMax(char[,] board, int depth, bool isMaximizing)
        {
            if (EndOfTheGame() == true) 
            {
                if (winner == "You won") //gdy wygrywa gracz nasz wynik += -1  => decyzja nieopłacalna 
                {
                    return -1;
                }
                else if (winner == "Computer won") //gdy wygrywa komputer nasz wynik+= +1 => decyzja opłacalna
                {
                    return 1;
                }
                else if (winner == "Draw") //gdy jest remis nasz wynik+= 0 => decyzja neutralna
                {
                    return 0;
                }
            }

            if (isMaximizing) //gdy komputer musi wykonać ruch 
            {
                int bestScore = int.MinValue;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i,j] == ' ') // czy dane miejsce nie jest zajęte
                        {
                            board[i, j] = 'O'; // komputer
                            int score = MiniMax(board, ++depth, false);
                            board[i, j] = ' ';
                            bestScore = Math.Max(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
            else //gdy gracz wykonuje ruch 
            {
                int bestScore = int.MaxValue;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == ' ') // czy dane miejsce nie jest zajęte
                        {
                            board[i, j] = 'X'; // gracz 
                            int score = MiniMax(board, ++depth, true);
                            board[i, j] = ' ';
                            bestScore = Math.Min(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }

        }

        public bool EndOfTheGame()
        {
            bool end = false;

            if (AnyWinner()) // gdy ktoś wygra
            {
                end = true;
            }
            else if(!DoesArrayContain(state, ' ')) // gdy wszystkie pola zostaną zapełnione i nikt do tej pory nie wygra
            {
                winner = "Draw";
                end = true;
            }
            
            return end;
        }

        private bool AnyWinner()
        {
            bool end = false;
            //pierwszy rząd
            if(state[0, 0]!= ' ' && state[0, 0] == state[0, 1] && state[0, 1] == state[0, 2]) 
            {
                end = true;
                winner = state[0, 0] == 'X' ? "You won" : "Computer won"; // gracz używa X, a komputer O
            }// drugi rząd
            else if (state[1, 0] != ' ' && state[1, 0] == state[1, 1] && state[1, 1] == state[1, 2])
            {
                end = true;
                winner = state[1, 0] == 'X' ? "You won" : "Computer won";
            }// trzeci rząd
            else if (state[2, 0] != ' ' && state[2, 0] == state[2, 1] && state[2, 1] == state[2, 2])
            {
                end = true;
                winner = state[2, 0] == 'X' ? "You won" : "Computer won";
            }//pierwsza kolumna
            else if (state[0, 0] != ' ' && state[0, 0] == state[1, 0] && state[1, 0] == state[2, 0])
            {
                end = true;
                winner = state[0, 0] == 'X' ? "You won" : "Computer won";
            }//druga kolumna
            else if (state[0, 1] != ' ' && state[0, 1] == state[1, 1] && state[1, 1] == state[2, 1])
            {
                end = true;
                winner = state[0, 1] == 'X' ? "You won" : "Computer won";
            }//trzecia kolumna
            else if (state[0, 2] != ' ' && state[0, 2] == state[1, 2] && state[1, 2] == state[2, 2])
            {
                end = true;
                winner = state[0, 2] == 'X' ? "You won" : "Computer won";
            }//pierwsza przekątna /
            else if (state[0, 2] != ' ' && state[0, 2] == state[1, 1] && state[1, 1] == state[2, 0])
            {
                end = true;
                winner = state[0, 2] == 'X' ? "You won" : "Computer won";
            }//druga przekątna \
            else if (state[0, 0] != ' ' && state[0, 0] == state[1, 1] && state[1, 1] == state[2, 2])
            {
                end = true;
                winner = state[0, 0] == 'X' ? "You won" : "Computer won";
            }

            return end;
        }

        private bool DoesArrayContain(char[,] array, char element)
        {
            bool answer = false;
            for (int i = 0; i < array.GetLength(0); i++)  //array.len
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i,j] == element)
                    {
                        answer = true;
                        break;
                    }
                }
            }

            return answer;
        }

    }

}
