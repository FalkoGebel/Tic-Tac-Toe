using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Tic_Tac_Toe
{
    internal static class GameConfig
    {
        private static readonly string ValidSymbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly List<Brush> ValidColors = new List<Brush>
        { 
            Brushes.Red,
            Brushes.Blue,
            Brushes.Black,
            Brushes.Green,
            Brushes.Brown,
            Brushes.Orange
        };
        private static string CurrentPlayerSymbol;
        private static string PlayerOneSymbol;
        private static string PlayerTwoSymbol;
        private static Brush CurrentPlayerColor;
        private static Brush PlayerOneColor;
        private static Brush PlayerTwoColor;
        private static readonly Random GameRandom = new Random();

        internal static void InitGameConfig()
        {
            PlayerOneSymbol = "X";
            PlayerTwoSymbol = "O";
            while (PlayerTwoSymbol == PlayerOneSymbol) { PlayerTwoSymbol = ValidSymbols[GameRandom.Next(0, ValidSymbols.Length)].ToString(); }
            PlayerOneColor = ValidColors[0];
            PlayerTwoColor = ValidColors[1];
            CurrentPlayerSymbol = PlayerOneSymbol;
            CurrentPlayerColor = PlayerOneColor;
        }

        internal static Brush GetCurrentPlayerColor()
        {
            return CurrentPlayerColor;
        }

        /// <summary>
        /// Returns the Current Player Color with an opacity of 0.5.
        /// </summary>
        /// <returns></returns>
        internal static Brush GetCurrentPlayerPreviewColor()
        {
            Brush previewColor = CurrentPlayerColor.Clone();
            previewColor.Opacity = 0.5;
            return previewColor;
        }

        internal static string GetCurrentPlayerSymbol()
        {
            return CurrentPlayerSymbol;
        }

        internal static string GetPlayerOneSymbol()
        {
            return PlayerOneSymbol;
        }

        internal static string GetPlayerTwoSymbol()
        {
            return PlayerTwoSymbol;
        }

        internal static List<Brush> GetValidColors()
        {
            return ValidColors;
        }

        internal static bool IsValidSymbol(string symbol)
        {
            return ValidSymbols.Contains(symbol);
        }

        /// <summary>
        /// Changes the symbol for player one to the new symbol and current player to if player one is current player.
        /// </summary>
        /// <param name="symbol">New symbol für player one.</param>
        internal static void SetPlayerOneSymbol(string symbol)
        {
            if (CurrentPlayerSymbol == PlayerOneSymbol) { CurrentPlayerSymbol = symbol; }
            PlayerOneSymbol = symbol;
        }

        /// <summary>
        /// Switches the current player (symbol and color) between player one symbol and player two symbol.
        /// </summary>
        internal static void ToggleCurrentPlayer()
        {
            if (CurrentPlayerSymbol == PlayerOneSymbol)
            {
                CurrentPlayerSymbol = PlayerTwoSymbol;
                CurrentPlayerColor = PlayerTwoColor;
            }
            else
            {
                CurrentPlayerSymbol = PlayerOneSymbol;
                CurrentPlayerColor = PlayerOneColor;
            }
        }

        internal static string SetAndGetNewRandomSymbolForPlayerTwo()
        {
            PlayerTwoSymbol = ValidSymbols[GameRandom.Next(0, ValidSymbols.Length)].ToString();
            return PlayerTwoSymbol;
        }

        /// <summary>
        /// Changes the symbol for player two to the new symbol and current player to if player two is current player.
        /// </summary>
        /// <param name="symbol">New symbol für player two.</param>
        internal static void SetPlayerTwoSymbol(string symbol)
        {
            if (CurrentPlayerSymbol == PlayerTwoSymbol) { CurrentPlayerSymbol = symbol; }
            PlayerTwoSymbol = symbol;
        }

        internal static Brush GetPlayerOneColor()
        {
            return PlayerOneColor;
        }

        internal static Brush GetPlayerTwoColor()
        {
            return PlayerTwoColor;
        }

        internal static void SetPlayerOneColor(Brush color)
        {
            if (CurrentPlayerSymbol == PlayerOneSymbol) { CurrentPlayerColor = color; }
            PlayerOneColor = color;
        }

        internal static void SetPlayerTwoColor(Brush color)
        {
            if (CurrentPlayerSymbol == PlayerTwoSymbol) { CurrentPlayerColor = color; }
            PlayerTwoColor = color;
        }
    }
}
