// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHasSelectableItems.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Behaviors
{
    using Models.Base;

    public interface IHasSelectableItems
    {
        NavigationNode SelectedItem { get; set; }
    }
}