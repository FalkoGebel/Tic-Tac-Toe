using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Tic_Tac_Toe
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GameConfig.InitGameConfig();
            InitControlValues();
        }

        private void InitControlValues()
        {
            PlayerOneSymbolTextBox.Text = GameConfig.GetPlayerOneSymbol();
            PlayerTwoSymbolTextBox.Text = GameConfig.GetPlayerTwoSymbol();
        }
        
        /// <summary>
        /// Initializes the window size to a quarter of the screen width and center it again.
        /// </summary>
        private void InitWindowSizeAndLocation()
        {
            double screenWidth = (Left + Width / 2) * 2;
            double screenHeight = (Top + Height / 2) * 2;
            Width = (Left + Width / 2) / 2;
            Height = Width * 1.5;
            Left = screenWidth / 2 - Width / 2;
            Top = screenHeight / 2 - Height / 2;
        }

        /// <summary>
        /// Gets the controls of the given object with a special type.
        /// </summary>
        /// <typeparam name="T">The type of the wanted objects.</typeparam>
        /// <param name="depObj">The object to get the controls from.</param>
        /// <returns>The controls of the special type.</returns>
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) { yield return (T)Enumerable.Empty<T>(); }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) { continue; }
                if (ithChild is T t) { yield return t; }
                foreach (T childOfChild in FindVisualChildren<T>(ithChild)) { yield return childOfChild; }
            }
        }

        /// <summary>
        /// Removes the content of all field buttons and enables them.
        /// </summary>
        private void ResetFieldButtons()
        {
            foreach (Button btn in FindVisualChildren<Button>(Window))
            {
                if (btn.Name == "")
                {
                    btn.Content = "";
                    btn.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Sets the font size for all field buttons according to their width.
        /// </summary>
        private void SetFieldButtonsFontSize()
        {
            foreach (Button btn in FindVisualChildren<Button>(Window))
            {
                if (btn.Name == "") { btn.FontSize = btn.ActualWidth * 0.75; }
            }
        }

        private void ValidateControlValues()
        {
            if (PlayerOneSymbolTextBox.Text.Length != 1) { PlayerOneSymbolTextBox.Text = GameConfig.GetPlayerOneSymbol(); }
            PlayerOneSymbolTextBox.Text = PlayerOneSymbolTextBox.Text.ToUpper();
            if (!GameConfig.IsValidSymbol(PlayerOneSymbolTextBox.Text)) { PlayerOneSymbolTextBox.Text = GameConfig.GetPlayerOneSymbol(); }
            else { GameConfig.SetPlayerOneSymbol(PlayerOneSymbolTextBox.Text); }
            
            while (PlayerTwoSymbolTextBox.Text == PlayerOneSymbolTextBox.Text) { PlayerTwoSymbolTextBox.Text = GameConfig.SetAndGetNewRandomSymbolForPlayerTwo();  }
            
            if (PlayerTwoSymbolTextBox.Text.Length != 1) { PlayerTwoSymbolTextBox.Text = GameConfig.GetPlayerTwoSymbol(); }
            PlayerTwoSymbolTextBox.Text = PlayerTwoSymbolTextBox.Text.ToUpper();
            if (!GameConfig.IsValidSymbol(PlayerTwoSymbolTextBox.Text)) { PlayerTwoSymbolTextBox.Text = GameConfig.GetPlayerTwoSymbol(); }
            else { GameConfig.SetPlayerTwoSymbol(PlayerTwoSymbolTextBox.Text); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitWindowSizeAndLocation();
            SetFieldButtonsFontSize();
            SetupColorComboBoxes();
        }

        /// <summary>
        /// Prepares the combo boxes for the colors to choose from the configuration.
        /// </summary>
        private void SetupColorComboBoxes()
        {
            foreach (ComboBox cb in FindVisualChildren<ComboBox>(Window))
            {
                if (cb.Name.Contains("Colors"))
                {
                    Brush colorToSelect = Brushes.Black;
                    if (cb.Name.Contains("PlayerOne")) { colorToSelect = GameConfig.GetPlayerOneColor(); }
                    if (cb.Name.Contains("PlayerTwo")) { colorToSelect = GameConfig.GetPlayerTwoColor(); }
                    foreach (Brush color in GameConfig.GetValidColors())
                    {
                        ComboBoxItem cbi = new ComboBoxItem();
                        TextBlock tb = new TextBlock
                        {
                            Background = color,
                            Width = cb.ActualWidth * 0.75
                        };
                        cbi.Content = tb;
                        if (color == colorToSelect) { cbi.IsSelected = true; }
                        _ = cb.Items.Add(cbi);
                    }
                }
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ValidateControlValues();
            Button btn = (Button)sender;
            if (!btn.IsEnabled) { return; }
            btn.Foreground = GameConfig.GetCurrentPlayerPreviewColor();
            btn.Content = GameConfig.GetCurrentPlayerSymbol();
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            if (!btn.IsEnabled) { return; }
            btn.Content = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ValidateControlValues();
            Button btn = (Button)sender;
            if (!btn.IsEnabled) { return; }
            btn.Foreground = GameConfig.GetCurrentPlayerColor();
            btn.Content = GameConfig.GetCurrentPlayerSymbol();
            btn.IsEnabled = false;
            GameConfig.ToggleCurrentPlayer();
            SetEnabledPlayerGrids(false);
        }

        private void SetEnabledPlayerGrids(bool enabled)
        {
            PlayerOneGrid.IsEnabled = enabled;
            PlayerTwoGrid.IsEnabled = enabled;
        }
        
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetFieldButtons();
            if (GameConfig.GetCurrentPlayerSymbol() != GameConfig.GetPlayerOneSymbol()) { GameConfig.ToggleCurrentPlayer(); }
            SetEnabledPlayerGrids(true);
        }

        private void PlayerOneSymbolTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (PlayerOneSymbolTextBox.Text.Length > 0)
                e.Handled = true;
        }

        private void PlayerTwoSymbolTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (PlayerTwoSymbolTextBox.Text.Length > 0)
                e.Handled = true;
        }

        private void PlayerColorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangePlayerColorFromSelection((ComboBox)sender);
        }

        private void ChangePlayerColorFromSelection(ComboBox cb)
        {
            ComboBoxItem cbi = (ComboBoxItem)cb.SelectedItem;
            TextBlock tb = (TextBlock)cbi.Content;
            if (cb.Name.Contains("PlayerOne")) { GameConfig.SetPlayerOneColor(tb.Background); }
            else { GameConfig.SetPlayerTwoColor(tb.Background); }
        }
    }
}
