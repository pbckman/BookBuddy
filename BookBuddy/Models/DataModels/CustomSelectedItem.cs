using System;
using BootstrapBlazor.Components;
using EPiServer.Shell.ObjectEditing;

namespace BookBuddy.Models.DataModels;

public class CustomSelectedItem : SelectItem
{
    public bool Selected { get; set; }
}
