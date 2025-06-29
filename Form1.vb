' Form1.vb
Imports LibreHardwareMonitor.Hardware
Imports System.IO
Imports System.Linq
Imports System.Diagnostics
Imports System.IO.Ports ' Nový import pro práci se sériovými porty

Public Class Form1

    Private computer As New Computer()
    Private visitor As New UpdateVisitor()
    Private appConfig As ConfigManager
    Private txtSensorNameControls(2) As TextBox
    Private cboUnitControls(2) As ComboBox
    Private WithEvents serialPort As SerialPort ' Objekt pro sériovou komunikaci

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' --- Inicializace polí ovládacích prvků ---
        txtSensorNameControls(0) = txtSensorName1
        txtSensorNameControls(1) = txtSensorName2
        txtSensorNameControls(2) = txtSensorName3

        cboUnitControls(0) = cboUnit1
        cboUnitControls(1) = cboUnit2
        cboUnitControls(2) = cboUnit3


        ' Automatické načtení konfigurace při startu
        LoadConfigurationIntoUI()

        ' Inicializace LibreHardwareMonitor pro všechny běžné typy hardwaru
        computer.IsCpuEnabled = True
        computer.IsGpuEnabled = True
        computer.IsMemoryEnabled = True
        computer.IsNetworkEnabled = True
        computer.IsMotherboardEnabled = True


        Try
            computer.Open()
            Timer1.Interval = 1000 ' Nastavení intervalu Timeru na 1 sekundu
            Timer1.Start()

            AppendOutputLog("Aplikace spuštěna. Konfigurace načtena.")
            RefreshComPorts() ' Načtení dostupných COM portů při startu

        Catch ex As Exception
            MessageBox.Show("Chyba při inicializaci LibreHardwareMonitor: " & ex.Message & Environment.NewLine &
                            "Ujistěte se, že aplikace běží jako správce a že jsou k dispozici všechny DLL soubory LHM.",
                            "Chyba inicializace", MessageBoxButtons.OK, MessageBoxIcon.Error)
            AppendOutputLog("CHYBA: " & ex.Message)
            Me.Close()
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        UpdateHardwareInfo()
    End Sub

    Private Sub UpdateHardwareInfo()
        computer.Accept(visitor)

        txtOutputLog.Clear()
        Dim dataToSend As New List(Of String)() ' Seznam pro data, která odešleme na ESP32

        For i As Integer = 0 To 2
            Dim sensorValueFormatted As String = "N/A"
            Dim rawSensorValue As Single = 0.0F ' Pro případné surové číslo k odeslání
            Dim sensorFound As Boolean = False

            Dim currentSensorConfig = appConfig.SensorConfigs(i)

            For Each hw As IHardware In computer.Hardware
                For Each sensor As ISensor In hw.Sensors
                    If sensor.Name.Contains(currentSensorConfig.SensorName) Then
                        If sensor.Value.HasValue Then
                            rawSensorValue = sensor.Value.Value ' Uložíme surovou hodnotu
                            ' --- Formátování hodnoty pro zobrazení v UI ---
                            Select Case currentSensorConfig.Unit.ToUpper()
                                Case "%"
                                    sensorValueFormatted = $"{rawSensorValue:F1}%"
                                Case "°C"
                                    sensorValueFormatted = $"{rawSensorValue:F1}{ChrW(176)}C"
                                Case "MHZ"
                                    sensorValueFormatted = $"{rawSensorValue:F0} MHz"
                                Case "MB"
                                    sensorValueFormatted = $"{rawSensorValue:F0} MB"
                                Case "W"
                                    sensorValueFormatted = $"{rawSensorValue:F1} W"
                                Case "V"
                                    sensorValueFormatted = $"{rawSensorValue:F2} V"
                                Case "X"
                                    sensorValueFormatted = $"{rawSensorValue:F2}x"
                                Case "B/S", "KB/S", "MB/S"
                                    If rawSensorValue > 1024 * 1024 Then
                                        sensorValueFormatted = $"{rawSensorValue / (1024 * 1024):F2} MB/s"
                                    ElseIf rawSensorValue > 1024 Then
                                        sensorValueFormatted = $"{rawSensorValue / 1024:F2} KB/s"
                                    Else
                                        sensorValueFormatted = $"{rawSensorValue:F0} B/s"
                                    End If
                                Case Else
                                    sensorValueFormatted = $"{rawSensorValue:F1}"
                            End Select
                            sensorFound = True
                        End If
                        Exit For
                    End If
                Next
                If sensorFound Then Exit For
            Next

            AppendOutputLog($"Sensor {i + 1} ({currentSensorConfig.SensorName}): {sensorValueFormatted}")

            ' --- Příprava dat pro odeslání na ESP32 ---
            ' Zde si definujeme formát dat, která odešleme.
            ' Např. "S1:25.5", "S2:80", "S3:1200", kde S1 je Sensor 1, atd.
            ' ESP32 pak bude muset tento formát parsovat.
            dataToSend.Add($"{Math.Round(rawSensorValue)}{currentSensorConfig.Unit}")

            ' Můžeme poslat i jednotku, pokud to ESP32 potřebuje: dataToSend.Add($"S{i + 1}:{rawSensorValue:F1}{currentSensorConfig.Unit}")
            Dim txt As String


        Next

        ' --- Odeslání dat přes sériový port ---
        If serialPort IsNot Nothing AndAlso serialPort.IsOpen Then
            Try
                ' Spojíme všechna data do jednoho řetězce, odděleného např. středníkem nebo čárkou
                ' ESP32 pak bude muset řetězec rozdělit a parsovat.
                Dim combinedData As String = String.Join(";", dataToSend) + Environment.NewLine ' Přidáme znak nového řádku pro ukončení zprávy
                serialPort.Write(combinedData)
                AppendOutputLog($"Odesláno na ESP32: {combinedData.Trim()}") ' Odkomentujte pro ladění odesílání
            Catch ex As Exception
                AppendOutputLog($"CHYBA ODESÍLÁNÍ DAT: {ex.Message}")
                ' Automatické odpojení při chybě odesílání
                If serialPort.IsOpen Then serialPort.Close()
                UpdateConnectionStatus(False)
            End Try
        End If
    End Sub

    Private Sub btnApplyConfig_Click(sender As Object, e As EventArgs) Handles btnApplyConfig.Click
        SaveUIConfigToAppConfig()
        AppendOutputLog("Konfigurace aplikována. Hodnoty se aktualizují...")
    End Sub

    Private Sub btnSaveConfig_Click(sender As Object, e As EventArgs) Handles btnSaveConfig.Click
        SaveUIConfigToAppConfig()
        Dim configFilePath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt")
        ConfigManager.SaveConfig(appConfig, configFilePath)
        AppendOutputLog("Konfigurace uložena do config.txt.")
    End Sub

    Private Sub LoadConfigurationIntoUI()
        Dim configFilePath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt")
        appConfig = ConfigManager.LoadConfig(configFilePath)

        For i As Integer = 0 To 2
            txtSensorNameControls(i).Text = appConfig.SensorConfigs(i).SensorName
            Dim unitIndex As Integer = cboUnitControls(i).FindStringExact(appConfig.SensorConfigs(i).Unit)
            If unitIndex <> -1 Then
                cboUnitControls(i).SelectedIndex = unitIndex
            Else
                cboUnitControls(i).SelectedIndex = -1
            End If
        Next
        AppendOutputLog("Konfigurace načtena z config.txt do UI.")
    End Sub

    Private Sub SaveUIConfigToAppConfig()
        If appConfig Is Nothing Then
            appConfig = New ConfigManager()
        End If

        For i As Integer = 0 To 2
            appConfig.SensorConfigs(i).SensorName = txtSensorNameControls(i).Text.Trim()
            If cboUnitControls(i).SelectedItem IsNot Nothing Then
                appConfig.SensorConfigs(i).Unit = cboUnitControls(i).SelectedItem.ToString()
            Else
                appConfig.SensorConfigs(i).Unit = ""
            End If
        Next
    End Sub

    Private Sub AppendOutputLog(message As String)
        If txtOutputLog.InvokeRequired Then
            txtOutputLog.Invoke(Sub()
                                    txtOutputLog.AppendText(message & Environment.NewLine)
                                    txtOutputLog.ScrollToCaret()
                                End Sub)
        Else
            txtOutputLog.AppendText(message & Environment.NewLine)
            txtOutputLog.ScrollToCaret()
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If computer IsNot Nothing Then
            computer.Close()
        End If
        If serialPort IsNot Nothing AndAlso serialPort.IsOpen Then
            serialPort.Close() ' Důležité: Zavření sériového portu
        End If
    End Sub

    ' --- Nové metody pro sériovou komunikaci ---

    Private Sub RefreshComPorts()
        cboComPort.Items.Clear()
        For Each port As String In SerialPort.GetPortNames()
            cboComPort.Items.Add(port)
        Next
        If cboComPort.Items.Count > 0 Then
            cboComPort.SelectedIndex = 0 ' Vybereme první port jako výchozí
        End If
        AppendOutputLog("COM porty obnoveny.")
    End Sub

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        If serialPort IsNot Nothing AndAlso serialPort.IsOpen Then
            ' Pokud je již připojen, odpojíme
            serialPort.Close()
            UpdateConnectionStatus(False)
            AppendOutputLog($"Odpojeno od portu {serialPort.PortName}.")
        Else
            ' Pokusíme se připojit
            If cboComPort.SelectedItem Is Nothing Then
                MessageBox.Show("Prosím, vyberte COM port.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim selectedPort As String = cboComPort.SelectedItem.ToString()

            serialPort = New SerialPort(selectedPort)
            Try
                ' Zde můžete nastavit parametry sériové linky, musí odpovídat ESP32!
                serialPort.BaudRate = 9600 ' Typická rychlost pro ESP32
                serialPort.Parity = Parity.None
                serialPort.DataBits = 8
                serialPort.StopBits = StopBits.One
                serialPort.Handshake = Handshake.None

                serialPort.Open()
                UpdateConnectionStatus(True)
                AppendOutputLog($"Připojeno k portu {selectedPort}.")
                btnConnect.Text = "Odpojit"
            Catch ex As Exception
                MessageBox.Show("Nepodařilo se připojit k portu: " & ex.Message, "Chyba připojení", MessageBoxButtons.OK, MessageBoxIcon.Error)
                AppendOutputLog($"CHYBA PŘIPOJENÍ K PORTU {selectedPort}: {ex.Message}")
                UpdateConnectionStatus(False)
            End Try
        End If
    End Sub

    Private Sub UpdateConnectionStatus(isConnected As Boolean)
        If lblConnectionStatus.InvokeRequired Then
            lblConnectionStatus.Invoke(Sub() UpdateConnectionStatus(isConnected))
        Else
            If isConnected Then
                lblConnectionStatus.Text = "Stav: Připojeno"
                lblConnectionStatus.ForeColor = Color.Green
                btnConnect.Text = "Odpojit"
            Else
                lblConnectionStatus.Text = "Stav: Odpojeno"
                lblConnectionStatus.ForeColor = Color.Red
                btnConnect.Text = "Připojit"
            End If
        End If
    End Sub

    Private Sub cboComPort_DropDown(sender As Object, e As EventArgs) Handles cboComPort.DropDown
        ' Obnoví seznam portů, když uživatel klikne na ComboBox
        RefreshComPorts()
    End Sub

    Private Sub txtOutputLog_TextChanged(sender As Object, e As EventArgs) Handles txtOutputLog.TextChanged

    End Sub

    Private Sub cboUnit1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboUnit1.SelectedIndexChanged

    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False
    End Sub

    ' Můžete přidat obsluhu události DataReceived pro příjem dat z ESP32, pokud ji budete potřebovat
    ' Private Sub SerialPort_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles serialPort.DataReceived
    '     ' Zde zpracujte přijatá data
    '     Dim receivedData As String = serialPort.ReadExisting()
    '     AppendOutputLog($"Přijato z ESP32: {receivedData}")
    ' End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = SystemIcons.Application
            NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
            NotifyIcon1.BalloonTipTitle = "Verificador corriendo"
            NotifyIcon1.BalloonTipText = "Verificador corriendo"
            NotifyIcon1.ShowBalloonTip(50000)
            'Me.Hide()
            ShowInTaskbar = False
        End If
    End Sub

End Class