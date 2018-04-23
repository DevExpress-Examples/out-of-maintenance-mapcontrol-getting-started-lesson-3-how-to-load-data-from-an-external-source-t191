#Region "#SourceCode"
Imports DevExpress.UI.Xaml.Map
Imports System
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports System.IO
Imports System.Xml.Linq
Imports Windows.ApplicationModel
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls

Public NotInheritable Class MainPage
    Inherits Page

    Private Const filepath As String = "Assets\ships.xml"

    Private pvtShipwrecks As New ObservableCollection(Of MapItem)()
    Public ReadOnly Property Shipwrecks() As ObservableCollection(Of MapItem)
        Get
            Return pvtShipwrecks
        End Get
    End Property


    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        LoadShipwrecks()
        Me.DataContext = Me
    End Sub

    Private Sub LoadShipwrecks()
        Dim xmlFilepath As String = Path.Combine(Package.Current.InstalledLocation.Path, filepath)
        Dim document As XDocument = XDocument.Load(xmlFilepath)
        For Each element As XElement In document.Element("Ships").Elements()
            Dim location As New GeoPoint() With { _
                .Latitude = Convert.ToDouble(element.Element("Latitude").Value, CultureInfo.InvariantCulture), _
                .Longitude = Convert.ToDouble(element.Element("Longitude").Value, CultureInfo.InvariantCulture) _
            }
            Dim e As New MapCustomElement() With { _
                .Location = location, _
                .ContentTemplate = TryCast(Me.Resources("itemTemplate"), DataTemplate) _
            }
            e.Attributes.Add(New MapItemAttribute() With {.Name = "Description", .Value = element.Element("Description").Value})
            e.Attributes.Add(New MapItemAttribute() With {.Name = "Name", .Value = element.Element("Name").Value})
            e.Attributes.Add(New MapItemAttribute() With {.Name = "Year", .Value = element.Element("Year").Value})

            pvtShipwrecks.Add(e)
        Next element
    End Sub
End Class
#End Region
