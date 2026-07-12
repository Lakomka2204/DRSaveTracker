using System;
using Avalonia.Media.Fonts;

public sealed class DRFontCollection : EmbeddedFontCollection
{
    public DRFontCollection() : base(
        new Uri("fonts:DRFonts", UriKind.Absolute),
        new Uri("avares://DRSaveTracker/Fonts")
    )
    {
        
    }
}