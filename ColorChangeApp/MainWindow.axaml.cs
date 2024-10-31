using Avalonia.Controls;
using Avalonia.Media;
using System;

namespace ColorChangeApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Назначение обработчиков событий для кнопок
            KhakiButton.Click += ChangeColor_OnClick;
            RedButton.Click += ChangeColor_OnClick;
            MediumVioletRedButton.Click += ChangeColor_OnClick;
            BisqueButton.Click += ChangeColor_OnClick;
            LemonChiffonButton.Click += ChangeColor_OnClick;
            PowderBlueButton.Click += ChangeColor_OnClick;
            MintCreamButton.Click += ChangeColor_OnClick;
            MaroonButton.Click += ChangeColor_OnClick;
            RosyBrownButton.Click += ChangeColor_OnClick;
            LightPinkButton.Click += ChangeColor_OnClick;
        }

        private void ChangeColor_OnClick(object? sender, EventArgs e)
        {
            if (sender is Button button && ColorDisplay != null)
            {
                var colorName = button.Content?.ToString();
                if (colorName != null)
                {
                    ColorDisplay.Background = new SolidColorBrush(Color.Parse(colorName));
                }
            }
        }
    }
}
