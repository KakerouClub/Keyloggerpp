﻿using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Logging;

namespace KeyloggerIDE.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        Logger.TryGet(LogEventLevel.Fatal, LogArea.Control)?.Log(this, "Avalonia Infrastructure");
        System.Diagnostics.Debug.WriteLine("System Diagnostics Debug");
        InitializeComponent();
        Closing += MainView.OnExit;
    }

}
