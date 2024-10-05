namespace Robotarm_Fun
{
    using Newtonsoft.Json;

    using SharpModbusLib;
    using SharpModbusLib.Models;

    using System.IO;
    using System.Text;
    using System.Text.Json;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel MainVM { get; set; } = new MainViewModel();

        public Modbus Modbus { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainVM;


            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Waypoints_Config.json");

            if (File.Exists(destPath))
            {
                loadWaypointsFromJson();
            }
            else
            {
                List<RobotWaypoint> waypointsList = new List<RobotWaypoint>();

                string jsonStr = System.Text.Json.JsonSerializer.Serialize(waypointsList);
                File.WriteAllText(destPath, jsonStr);

                loadWaypointsFromJson();
            }


            //RobotWaypoint waypoint1 = new RobotWaypoint {xCord = 12, yCord = 13, zCord = 34, armRotation = 78, Name = "Mit seje waypoint" };

            //MainVM.robotWaypoints.Add(waypoint1);
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {   
                connectBtn.IsEnabled = false;
                await Task.Run(() => {
                    Modbus = new Modbus(MainVM.ipAddress, MainVM.port);
                });
                connectBtn.IsEnabled = true;

                MainVM.isConnected = true;

            } catch (Exception ex) {
                connectBtn.IsEnabled = true;
                MainVM.isConnected = false;
                MessageBox.Show(ex.Message, "Error while trying to connect to robot through modbus!");
            }
        }

        private void MoveToCords_Click(object sender, RoutedEventArgs e)
        {
            Int16 x = (Int16)MainVM.xCord;
            Int16 y = (Int16)MainVM.yCord;
            Int16 z = (Int16)MainVM.zCord;

            SharpModbusLib.Models.WriteSuccessResponse response;
            SharpModbusLib.Models.WriteSuccessResponse responseCoil;

            response = Modbus.writeMultipleHoldingRegisters(200, new short[] { x, y, z });
            responseCoil = Modbus.writeSingleCoil(1, true);

            MessageBox.Show(response.responseFrame);
        }

        private void MoveToCordID_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GoToSelectedWaypoint_Click(object sender, RoutedEventArgs e)
        {
            RobotWaypoint selectedWaypoint = null;

            try
            {
                selectedWaypoint = (RobotWaypoint)WaypointsGrid.SelectedItem;

            }catch (Exception ex) {

            }
            if (selectedWaypoint != null)
            {
                Int16 x = (Int16)selectedWaypoint.xCord;
                Int16 y = (Int16)selectedWaypoint.yCord;
                Int16 z = (Int16)selectedWaypoint.zCord;
                Int16 armRotation = (Int16)selectedWaypoint.armRotation;

                SharpModbusLib.Models.WriteSuccessResponse response;
                SharpModbusLib.Models.WriteSuccessResponse responseCoil;

                Modbus.writeSingleHoldingRegister(100, 3);
                Thread.Sleep(100);
                response = Modbus.writeMultipleHoldingRegisters(200, new short[] { x, y, z, armRotation });
                Thread.Sleep(100);
                responseCoil = Modbus.writeSingleCoil(10, true);
            }
        }

        private void loadWaypointsFromJson()
        {
            using (StreamReader r = new StreamReader("Waypoints_Config.json"))
            {
                string json = r.ReadToEnd();
                MainVM.robotWaypoints = JsonConvert.DeserializeObject<List<RobotWaypoint>>(json);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Waypoints_Config.json");

            string jsonStr = System.Text.Json.JsonSerializer.Serialize(MainVM.robotWaypoints);
            File.WriteAllText(destPath, jsonStr);
        }

        private async void RunInSequence_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => {

                Modbus.writeSingleHoldingRegister(100, 3);

                foreach (RobotWaypoint waypoint in MainVM.robotWaypoints)
                {
                    Int16 x = (Int16)waypoint.xCord;
                    Int16 y = (Int16)waypoint.yCord;
                    Int16 z = (Int16)waypoint.zCord;
                    Int16 armRotation = (Int16)waypoint.armRotation;

                    SharpModbusLib.Models.WriteSuccessResponse response;
                    SharpModbusLib.Models.WriteSuccessResponse responseCoil;

                    response = Modbus.writeMultipleHoldingRegisters(200, new short[] { x, y, z, armRotation });
                    Thread.Sleep(100);
                    responseCoil = Modbus.writeSingleCoil(10, true);
                    Thread.Sleep(100);

                    ReadDiscreteInputsResponse coil10response = Modbus.readCoils(10, 1);

                    while(coil10response.DiscreteInputs[0].value == true)
                    {
                        Thread.Sleep(100);
                        coil10response = Modbus.readCoils(10, 1);
                    }

                }

            });

            MessageBox.Show("Finished running");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Modbus.writeSingleCoil(5, false);
            Thread.Sleep(200);
            Modbus.writeSingleCoil(0, false);
            Thread.Sleep(200);
            Modbus.writeSingleCoil(5, true);
            Thread.Sleep(200);
            Modbus.writeSingleCoil(0, true);


            //Modbus.writeSingleCoil(3, false);
            //Modbus.writeSingleCoil(3, true);

        }
    }
}