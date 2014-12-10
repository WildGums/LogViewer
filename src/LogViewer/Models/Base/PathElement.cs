// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PathElement.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models.Base
{
    using Catel.Data;
    using YAXLib;

    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public abstract class PathElement : ModelBase
    {
        [YAXSerializableField]
        public string Name { get; set; }

        [YAXSerializableField]
        public bool? IsChecked { get; set; } 
    }
}