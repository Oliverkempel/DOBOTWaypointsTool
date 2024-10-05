namespace Robotarm_Fun
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

		private bool _isConnected = false;

		public bool isConnected
		{
			get { return _isConnected; }
			set 
			{ 
				_isConnected = value; 
				OnPropertyChanged();
            }
        }

		private string _ipAddress = "192.168.1.6";

		public string ipAddress
		{
			get { return _ipAddress; }
			set 
			{ 
				_ipAddress = value;
				OnPropertyChanged();
            }
        }

		private int _port = 502;

		public int port
		{
			get { return _port; }
			set 
			{ 
				_port = value;
                OnPropertyChanged();
            }
        }




		private int _xCord = 0;

		public int xCord
		{
			get { return _xCord; }
			set 
			{ 
				_xCord = value;
				OnPropertyChanged();
			}
		}

		private int _yCord = 0;

		public int yCord
		{
			get { return _yCord; }
			set 
			{ 
				_yCord = value;
                OnPropertyChanged();
            }
		}

		private int _zCord = 0;

		public int zCord
		{
			get { return _zCord; }
			set 
			{ 
				_zCord = value;
                OnPropertyChanged();
            }
		}

		private int _toolHeadRotation = 0;

		public int toolHeadRotation 
		{
			get { return _toolHeadRotation; }
			set 
			{ 
				_toolHeadRotation = value;
                OnPropertyChanged();
            }
		}

		private int _pointId = 1;

		public int pointId
		{
			get { return _pointId; }
			set 
			{ 
				_pointId = value; 
                OnPropertyChanged();
            }
        }

		private List<RobotWaypoint> _robotWaypoints = new List<RobotWaypoint>();

		public List<RobotWaypoint> robotWaypoints
		{
			get { return _robotWaypoints; }
			set { _robotWaypoints = value; }
		}




		protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
