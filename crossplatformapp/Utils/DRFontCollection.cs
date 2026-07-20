using System;
using Avalonia.Media.Fonts;
namespace crossplatformapp.Utils;

public sealed class DRFontCollection : EmbeddedFontCollection
{
    public DRFontCollection() : base(
        new Uri("fonts:DRFonts", UriKind.Absolute),
        new Uri("avares://DRSaveTracker/Assets/Fonts")
    )
    {
        
    }
}