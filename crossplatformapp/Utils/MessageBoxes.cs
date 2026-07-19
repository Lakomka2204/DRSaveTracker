using Avalonia;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Models;

namespace crossplatformapp.Utils;

public static class MessageBoxes
{
    public static IMsBox<string> GetOwnMboxYesNo(string title,string content, MsBox.Avalonia.Enums.Icon icon)
    {
        return MessageBoxManager.GetMessageBoxCustom(
            new MsBox.Avalonia.Dto.MessageBoxCustomParams()
            {
                FontFamily = (Avalonia.Media.FontFamily)Application.Current!.Resources["DRFontFamily"]!,
                ContentHeader = title,
                ContentMessage = content,
                ButtonDefinitions = [
                new ButtonDefinition() {Name = "Yes"},
                new ButtonDefinition() 
                {
                    Name = "No", 
                    IsCancel = true, 
                    IsDefault = true
                }
                ],
                CloseOnClickAway = true,
                WindowDecorations = WindowDecorations.BorderOnly,
                Icon = icon,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInCenter = true,
            }
        );
    }
    public static IMsBox<string> GetOwnMBox(
        string title,
        string content, 
        MsBox.Avalonia.Enums.Icon icon)
    {
        return MessageBoxManager.GetMessageBoxCustom(
            new MsBox.Avalonia.Dto.MessageBoxCustomParams()
            {
                FontFamily = (Avalonia.Media.FontFamily)Application.Current!.Resources["DRFontFamily"]!,
                ContentHeader = title,
                ContentMessage = content,
                ButtonDefinitions = [
                new ButtonDefinition() 
                {
                    Name = "OK", 
                    IsCancel=true, 
                    IsDefault=true
                },
                ],
                CloseOnClickAway = true,
                WindowDecorations = WindowDecorations.BorderOnly,
                Icon = icon,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInCenter = true,
            }
        );
    }
}