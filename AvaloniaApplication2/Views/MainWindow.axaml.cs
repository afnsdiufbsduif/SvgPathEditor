using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using System.IO;
using System.Linq;
using System.Globalization;


namespace AvaloniaApplication2.Views;

public partial class MainWindow : Window
{
    private float scaleX = 1.0f;
    private float scaleY = 1.0f;
    public MainWindow()
    {
        InitializeComponent();
        message.TextChanged += Message_TextChanged;
    }

    private void Message_TextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            UpdatePath(textBox.Text);
            Command();
        }

        if (message != null && message.Text != null)
        {
            Kol.Text = message.Text.Length.ToString();
        }
        else
        {
            Kol.Text = "0";
        }
    }
    private void UpdatePath(string pathDataString)
    {
        try
        {
            var geometry = Geometry.Parse(pathDataString);

            path1.Data = geometry;
        }
        catch (Exception ex)
        {
        }
    }

    private void ClearButton_OnClick(object? sender, RoutedEventArgs e)
    {
        message.Clear();
        outputStackPanel.Children.Clear();
        Height = 425;
        message.HorizontalContentAlignment = HorizontalAlignment.Left;

    }

    private void ScaleBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (float.TryParse(SclX.Text, out float scaleX) && float.TryParse(SclY.Text, out float scaleY))
        {
            ApplyScale(scaleX, scaleY);
        }
        else
        {
            message.HorizontalContentAlignment = HorizontalAlignment.Center;

            message.Text = "Некорректные значения скалирования!";
        }
        SclX.Text = "1";
        SclY.Text = "1";
    }
    private void ApplyScale(float scaleX, float scaleY)
    {
        var transform = new TransformGroup();
        transform.Children.Add(new ScaleTransform(scaleX, scaleY));

        path1.RenderTransform = transform;
    }
    private void SclX_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (float.TryParse(SclX.Text, out float scaleXValue))
        {
            scaleX = scaleXValue;
        }
    }

    private void SclY_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (float.TryParse(SclY.Text, out float scaleYValue))
        {
            scaleY = scaleYValue;
        }
    }
    private void Round_OnClick(object? sender, RoutedEventArgs e)
    {
        if (txtround.Text != "")
        {
            scaleX = (float)Math.Round(scaleX, int.Parse(txtround.Text));
            SclX.Text = scaleX.ToString();
            scaleY = (float)Math.Round(scaleY, int.Parse(txtround.Text));
            SclY.Text = scaleY.ToString();
            ApplyScale(scaleX, scaleY);
        }
        else
        {
            message.HorizontalContentAlignment = HorizontalAlignment.Center;
            message.Text = "Некорректные значения округления!";
        }

    }
    void Command()
    {

        if (message.Text != null)
        {
            outputStackPanel.Children.Clear();
            string inputText = message.Text;
            List<string> outputLines = new List<string>();

            string currentLine = "";
            foreach (char c in inputText)
            {
                if (c == 'M' || c == 'L' || c == 'C' || c == 'A' || c == 'Q' || c == 'T')
                {
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        outputLines.Add(currentLine.Trim());
                        currentLine = "";
                    }

                    currentLine += $"{c} ";
                }
                else if (Char.IsDigit(c) || c == '-' || c == '.' || c == ',')
                {
                    currentLine += c;
                }
                else if (c == ' ')
                {
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        outputLines.Add(currentLine.Trim());
                        currentLine = "";
                    }
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
            {
                outputLines.Add(currentLine.Trim());
            }

            outputStackPanel.Children.OfType<Control>().Where(c => c is TextBox || c is Label).ToList().ForEach(c => outputStackPanel.Children.Remove(c));

            for (int i = 0; i < outputLines.Count; i++)
            {

                if (double.TryParse(outputLines[i], NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                {


                    TextBox outputTextBox = new TextBox();
                    outputTextBox.Text = value.ToString(CultureInfo.InvariantCulture);
                    outputTextBox.Name = "outputTextBox" + (i + 1);
                    outputTextBox.Text = outputLines[i];
                    outputTextBox.Margin = new Thickness(0, 0, 15, 15);
                    outputTextBox.Width = 60;
                    outputTextBox.HorizontalContentAlignment = HorizontalAlignment.Center;
                    Grid.SetColumn(outputTextBox, 2);
                    outputStackPanel.Children.Add(outputTextBox);
                }
                else
                {
                    Button button = new Button();

                    button.Background = new SolidColorBrush(Color.Parse("#4DB6AC"));

                    button.Content = outputLines[i] + ":";
                    button.Margin = new Thickness(0, 0, 5, 15);
                    button.Width = 310;
                    outputStackPanel.Children.Add(button);
                }

            }

        }
        else
        {
            message.HorizontalContentAlignment = HorizontalAlignment.Center;
            message.Text = "Некорректные значения!";

        }
    }

    private void ApplyTranslation(float translateX, float translateY)
    {
        if (path1.RenderTransform is TransformGroup transformGroup)
        {
            var translateTransform = transformGroup.Children.OfType<TranslateTransform>().FirstOrDefault();

            if (translateTransform != null)
            {
                translateTransform.X += translateX;
                translateTransform.Y += translateY;
            }
            else
            {
                translateTransform = new TranslateTransform(translateX, translateY);
                transformGroup.Children.Add(translateTransform);
            }
        }
        else
        {
            var transform = new TransformGroup();
            transform.Children.Add(new ScaleTransform(scaleX, scaleY));
            transform.Children.Add(new TranslateTransform(translateX, translateY));
            path1.RenderTransform = transform;
        }
    }

    private void Translate_OnClick(object? sender, RoutedEventArgs e)
    {
        if (float.TryParse(moveX.Text, out float translateX) && float.TryParse(moveY.Text, out float translateY))
        {
            ApplyTranslation(translateX, translateY);
        }
        else
        {
            message.HorizontalContentAlignment = HorizontalAlignment.Center;
            message.Text = "Некорректные значения перемещения!";
        }
        moveX.Text = "0";
        moveY.Text = "0";
    }


    private void SpisButton_OnClick(object? sender, RoutedEventArgs e)
    {

    }

    private void Rotate_OnClick(object? sender, RoutedEventArgs e)
    {

    }


    private void RelativeBtn_OnClick(object? sender, RoutedEventArgs e)
    {

    }

    private void AbsoluteBtn_OnClick(object? sender, RoutedEventArgs e)
    {

    }

    private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {

    }
}
