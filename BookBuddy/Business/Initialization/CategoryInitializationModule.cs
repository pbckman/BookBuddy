using System;
using BookBuddy.Business.Helpers;
using BookBuddy.Models.DDS;
using EPiServer.Data.Dynamic;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace BookBuddy.Business.Initialization;

[ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
public class CategoryInitializationModule : IInitializableModule
{
    public void Initialize(InitializationEngine context)
    {
        var store = DynamicDataStoreFactory.Instance.GetStore(typeof(CategoryItem)) ?? DynamicDataStoreFactory.Instance.CreateStore(typeof(CategoryItem));

        if (store.Items<CategoryItem>().Count() == 0)
        {
            System.Diagnostics.Debug.WriteLine("Adding initial category data");
            var categories = new List<CategoryItem>();
            categories.AddRange(InitialCategoryData.CategoryItemsEN);
            categories.AddRange(InitialCategoryData.CategoryItemsSV);
            foreach (var category in categories)
            {
                store.Save(category);
            }
        }
    }

    public void Uninitialize(InitializationEngine context)
    {
        throw new NotImplementedException();
    }
}
