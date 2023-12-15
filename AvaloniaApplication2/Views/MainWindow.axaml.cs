using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using System.IO;
using System.Linq;
using Avalonia.Controls.Primitives;
using System.Threading.Tasks;
using System.Globalization;

namespace AvaloniaApplication2.Views;

public partial class MainWindow : Window
{
    private float scaleX = 1.0f;
    private float scaleY = 1.0f;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ClearButton_OnClick(object? sender, RoutedEventArgs e)
    {
       message.Clear();
       outputStackPanel.Children.Clear();
       Height = 425;

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
    private void SpisButton_OnClick(object? sender, RoutedEventArgs e)
    {
       
     
        Command();
    }

    private void Round_OnClick(object? sender, RoutedEventArgs e)
    {
        if(txtround.Text != "")
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
            Height = 800;
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
                    outputTextBox.Margin = new Thickness(0, 0, 0, 5); 
                    outputTextBox.Width = 60; 
                    outputTextBox.HorizontalContentAlignment = HorizontalAlignment.Center;

                    Grid.SetColumn(outputTextBox, i % 7); 
                    outputStackPanel.Children.Add(outputTextBox);
                }
                else
                {
                    Label label = new Label();
                    label.Content = outputLines[i] + ":";
                    label.Margin = new Thickness(0, 0, 5, 0);

                    Grid.SetColumn(label, i % 7); 
                    outputStackPanel.Children.Add(label);
                }
            }
        }
            else
            {
            message.HorizontalContentAlignment = HorizontalAlignment.Center;

            message.Text = "Некорректные значения!";
           
            }    
    }


    private void Translate_OnClick(object? sender, RoutedEventArgs e)
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
