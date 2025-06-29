' ConfigManager.vb
Imports System.IO
Imports System.Collections.Generic

Public Class ConfigManager
    Public Class SensorConfig
        Public Property SensorName As String = ""
        Public Property Unit As String = ""
    End Class

    ' Pole pro 5 konfigurací senzorů (indexy 0 až 4)
    Public SensorConfigs(4) As SensorConfig

    Public Sub New()
        ' Konstruktor: Zajišťuje, že každý prvek pole SensorConfigs je inicializován
        For i As Integer = 0 To 4
            Me.SensorConfigs(i) = New SensorConfig()
        Next
    End Sub

    Public Shared Function LoadConfig(filePath As String) As ConfigManager
        Dim config As New ConfigManager() ' Vytvoří novou instanci, zavolá konstruktor pro inicializaci pole

        ' Pokud konfigurační soubor neexistuje, vytvoříme výchozí
        If Not System.IO.File.Exists(filePath) Then
            SaveDefaultConfig(filePath)
            Return LoadConfig(filePath) ' Zkusit znovu načíst po vytvoření
        End If

        Dim currentSensorIndex As Integer = -1

        For Each line In System.IO.File.ReadLines(filePath)
            Dim trimmedLine = line.Trim()
            If String.IsNullOrWhiteSpace(trimmedLine) OrElse trimmedLine.StartsWith(";") OrElse trimmedLine.StartsWith("#") Then
                Continue For ' Přeskočit prázdné řádky a komentáře
            End If

            If trimmedLine.StartsWith("[Sensor") AndAlso trimmedLine.EndsWith("]") Then
                ' Parsujeme index senzoru (např. "[Sensor1]" -> index 0)
                If Integer.TryParse(trimmedLine.Substring(7, trimmedLine.Length - 8), currentSensorIndex) Then
                    currentSensorIndex -= 1 ' Převod na index pole (0-4)
                Else
                    currentSensorIndex = -1 ' Chybný formát, ignorovat
                End If
            ElseIf currentSensorIndex >= 0 AndAlso currentSensorIndex <= 4 Then
                ' Pokud jsme ve validní sekci senzoru, parsujeme vlastnosti
                If trimmedLine.StartsWith("Name=") Then
                    config.SensorConfigs(currentSensorIndex).SensorName = trimmedLine.Replace("Name=", "").Trim()
                ElseIf trimmedLine.StartsWith("Unit=") Then
                    config.SensorConfigs(currentSensorIndex).Unit = trimmedLine.Replace("Unit=", "").Trim()
                End If
            End If
        Next
        Return config
    End Function

    Public Shared Sub SaveConfig(config As ConfigManager, filePath As String)
        Dim lines As New List(Of String)()
        For i As Integer = 0 To 4
            lines.Add($"[Sensor{i + 1}]")
            lines.Add($"Name={config.SensorConfigs(i).SensorName}")
            lines.Add($"Unit={config.SensorConfigs(i).Unit}")
            lines.Add("") ' Prázdný řádek pro oddělení
        Next
        System.IO.File.WriteAllLines(filePath, lines)
    End Sub

    Private Shared Sub SaveDefaultConfig(filePath As String)
        ' Vytvoří výchozí konfigurační soubor s prázdnými/obecnými hodnotami
        Dim defaultConfig As New ConfigManager() ' Vytvoří novou instanci s inicializovaným polem SensorConfigs

        ' Přednastavení některých běžných hodnot
        defaultConfig.SensorConfigs(0).SensorName = "CPU Total"
        defaultConfig.SensorConfigs(0).Unit = "%"
        defaultConfig.SensorConfigs(1).SensorName = "CPU Package"
        defaultConfig.SensorConfigs(1).Unit = "°C"
        defaultConfig.SensorConfigs(2).SensorName = "GPU Core"
        defaultConfig.SensorConfigs(2).Unit = "%"
        defaultConfig.SensorConfigs(3).SensorName = "GPU Core"
        defaultConfig.SensorConfigs(3).Unit = "°C"
        defaultConfig.SensorConfigs(4).SensorName = "Used Memory"
        defaultConfig.SensorConfigs(4).Unit = "MB"

        SaveConfig(defaultConfig, filePath) ' Uloží výchozí konfiguraci
    End Sub

End Class