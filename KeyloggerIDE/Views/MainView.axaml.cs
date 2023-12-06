﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using KeyloggerIDE.ViewModels;
using static KeyloggerIDE.ViewModels.TabControlViewModel;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System.Diagnostics;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit;
using AvaloniaEdit.Editing;
using DynamicData;

namespace KeyloggerIDE.Views;

public partial class MainView : UserControl
{
    private TabControlViewModel tabControlViewModel;

    private CompletionWindow completionWindow;

    private Popup? popup;

    public MainView()
    {
        InitializeComponent();
        popup = this.FindControl<Popup>("About");

        // init tab control and editor
        TabView.DataContext = tabControlViewModel = new TabControlViewModel();
        tabControlViewModel.InitTabControl(AvalonEditor);

        // set editor callbacks
        AvalonEditor.TextArea.TextEntering += editor_TextArea_TextEntered;
    }

    private void OnAboutButton_Click(object sender, RoutedEventArgs e)
    {
        if (popup != null)
        {
            if (popup.IsOpen)
            {
                popup.IsOpen = false;
            }
            else
            {
                popup.IsOpen = true;
            }
        }
    }

    private void TabView_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (TabView != null)
        {
            tabControlViewModel.ChangeSelection(TabView, AvalonEditor);
        }
    }

    private void AvalonEditor_OnTextChanged(object? sender, EventArgs e)
    {
        tabControlViewModel.ChangeFileStatus(sender, TabView);
    }

    void editor_TextArea_TextEntered(object sender, TextInputEventArgs e)
    {
        if (char.IsAsciiLetter(e.Text[0]))
        {
            // Open code completion after the user has entered a matching letter:
            completionWindow = new CompletionWindow(AvalonEditor.TextArea);
            IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;

            data.Add(new MyCompletionData("MsgBox", "Shows a message box"));
            data.Add(new MyCompletionData("MousePress", "Mouse press event(without release)"));
            data.Add(new MyCompletionData("MouseRelease", "Mouse release event"));
            data.Add(new MyCompletionData("Send", "Send text to be typed"));
            data.Add(new MyCompletionData("SendInput", "Send key combinations with Ctrl or Alt"));

            completionWindow.Show();
            completionWindow.Closed += delegate {
                completionWindow = null;
            };
        }
    }
}
