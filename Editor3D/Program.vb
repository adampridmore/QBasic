Imports Avalonia
Imports Avalonia.Controls.ApplicationLifetimes
Imports Avalonia.Themes.Fluent

Module Program
    Public Sub Main(args As String())
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args)
    End Sub

    Public Function BuildAvaloniaApp() As AppBuilder
        Return AppBuilder.Configure(Of Editor3DApp)() _
            .UsePlatformDetect() _
            .WithInterFont() _
            .LogToTrace()
    End Function
End Module

Public Class Editor3DApp
    Inherits Application

    Public Overrides Sub Initialize()
        Styles.Add(New FluentTheme())
    End Sub

    Public Overrides Sub OnFrameworkInitializationCompleted()
        If TypeOf ApplicationLifetime Is IClassicDesktopStyleApplicationLifetime Then
            Dim desktop = CType(ApplicationLifetime, IClassicDesktopStyleApplicationLifetime)
            desktop.MainWindow = New Editor3D.MainWindow()
        End If

        MyBase.OnFrameworkInitializationCompleted()
    End Sub
End Class
