using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Shell.ObjectEditing;

namespace BookBuddy.Business.Extenders;

public class MetadataExtender : IMetadataExtender
{
    public void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            foreach (var property in metadata.Properties)
            {
                if (property.PropertyName == "PageVisibleInMenu")
                {
                    property.ShowForEdit = false;
                }

                foreach (var attribute in property.Attributes)
                {
                    if (attribute is ScaffoldColumnAttribute scaffoldColumnAttribute)
                    {
                        if (scaffoldColumnAttribute.Scaffold == false)
                        {
                            property.ShowForEdit = false;
                        }
                    }
                }
            }
        }
}
