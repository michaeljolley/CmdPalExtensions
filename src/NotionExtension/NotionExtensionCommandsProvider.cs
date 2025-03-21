using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Windows.Foundation;

namespace NotionExtension;

public partial class NotionExtensionCommandsProvider : CommandProvider
{
    private readonly ListItem _listItem = new(new NotionListPage());

    public NotionExtensionCommandsProvider()
    {
        DisplayName = "Notion";
    }

    public override ICommandItem[] TopLevelCommands() => [_listItem];

    public override IFallbackCommandItem[] FallbackCommands() => [];
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "This is sample code")]
public sealed partial class NotionListPage : DynamicListPage
{
    private readonly List<ListItem> _items = [];
    private readonly SaveCommand _saveCommand = new();
    private readonly CopyTextCommand _copyContextCommand;
    private readonly CommandContextItem _copyContextMenuItem;
    private static readonly CompositeFormat ErrorMessage = System.Text.CompositeFormat.Parse("blah");

    public NotionListPage()
    {
        Icon = new IconInfo("\ue8ef"); // Calculator
        Name = "Notion";
        PlaceholderText = "placeholder";
        Id = "com.baldbeardedbuilder.cmdpal.notion";

        _copyContextCommand = new CopyTextCommand(string.Empty);
        _copyContextMenuItem = new CommandContextItem(_copyContextCommand);

        _items.Add(new(_saveCommand) { Icon = new IconInfo("\uE94E") });

        UpdateSearchText(string.Empty, string.Empty);

        _saveCommand.SaveRequested += HandleSave;
    }

    private void HandleSave(object sender, object args)
    {
        var lastResult = _items[0].Title;
        if (!string.IsNullOrEmpty(lastResult))
        {
            var li = new ListItem(new CopyTextCommand(lastResult))
            {
                Title = _items[0].Title,
                Subtitle = _items[0].Subtitle,
                TextToSuggest = lastResult,
            };
            _items.Insert(1, li);
            _items[0].Subtitle = string.Empty;
            SearchText = lastResult;
            this.RaiseItemsChanged(this._items.Count);
        }
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        var firstItem = _items[0];
        if (string.IsNullOrEmpty(newSearch))
        {
            firstItem.Title = "more placeholder";
            firstItem.Subtitle = string.Empty;
            firstItem.MoreCommands = [];
        }
        else
        {
            _copyContextCommand.Text = ParseQuery(newSearch, out var result) ? result : string.Empty;
            firstItem.Title = result;
            firstItem.Subtitle = newSearch;
            firstItem.MoreCommands = [_copyContextMenuItem];
        }
    }

    internal static bool ParseQuery(string equation, out string result)
    {
        try
        {
            var resultNumber = new DataTable().Compute(equation, null);
            result = resultNumber.ToString() ?? string.Empty;
            return true;
        }
        catch (Exception e)
        {
            result = string.Format(CultureInfo.CurrentCulture, ErrorMessage, e.Message);
            return false;
        }
    }

    public override IListItem[] GetItems() => _items.ToArray();
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "This is sample code")]
public sealed partial class SaveCommand : InvokableCommand
{
    public event TypedEventHandler<object, object> SaveRequested;

    public SaveCommand()
    {
        Name = "save command name";
    }

    public override ICommandResult Invoke()
    {
        SaveRequested?.Invoke(this, this);
        return CommandResult.KeepOpen();
    }
}
