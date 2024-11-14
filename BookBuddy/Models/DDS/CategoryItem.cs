using System;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace BookBuddy.Models.DDS;

[EPiServerDataStore(AutomaticallyCreateStore = true, StoreName = "CategoryStore")]
public class CategoryItem
{
    public Identity Id { get; set; } = Identity.NewIdentity();
    public string Value { get; set; }
    public string Text { get; set; }
    public string Language { get; set; }
}
