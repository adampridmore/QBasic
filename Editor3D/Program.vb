Imports System.Windows.Forms

Module Program
    <STAThread>
    Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New Editor3D.MainForm())
    End Sub
End Module
