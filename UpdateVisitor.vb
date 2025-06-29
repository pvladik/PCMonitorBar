Public Class UpdateVisitor
    Implements IVisitor

    Public Sub VisitComputer(computer As IComputer) Implements IVisitor.VisitComputer
        computer.Traverse(Me)
    End Sub

    Public Sub VisitHardware(hardware As IHardware) Implements IVisitor.VisitHardware
        hardware.Update()
        For Each subHardware As IHardware In hardware.SubHardware
            subHardware.Accept(Me)
        Next
    End Sub

    Public Sub VisitSensor(sensor As ISensor) Implements IVisitor.VisitSensor
        ' Tato metoda se zavolá pro každý senzor, ale pro náš účel ji není potřeba implementovat
    End Sub

    Public Sub VisitParameter(parameter As IParameter) Implements IVisitor.VisitParameter
        ' Tato metoda se zavolá pro každý parametr, ale pro náš účel ji není potřeba implementovat
    End Sub

End Class