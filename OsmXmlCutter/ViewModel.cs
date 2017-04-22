using OSMtoSharp;
using OSMtoSharp.FileManagers;
using OSMtoSharp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OsmXmlCutter
{
    public class ViewModel : INotifyPropertyChanged
    {

        public ViewModel()
        {
            PrepareStartData();
        }

        private string _inputFileName;
        public string InputFileName
        {
            get
            {
                return _inputFileName;

            }
            set
            {
                _inputFileName = value;
                OnPropertyChanged(nameof(InputFileName));
            }
        }

        private string _outputFileName;
        public string OutputFileName
        {
            get
            {
                return _outputFileName;

            }
            set
            {
                _outputFileName = value;
                OnPropertyChanged(nameof(OutputFileName));
            }
        }

        private double _maxLat;
        public string MaxLat
        {
            get
            {
                return _maxLat.ToString(CultureInfo.InvariantCulture);

            }
            set
            {
                Regex regex = new Regex(@"^(\d+)([.,](\d+))?$");
                if (regex.IsMatch(value) && !string.IsNullOrEmpty(value))
                {
                    _maxLat = double.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture);
                    OnPropertyChanged(nameof(MaxLat));
                }
            }
        }

        private double _maxLon;
        public string MaxLon
        {
            get
            {
                return _maxLon.ToString(CultureInfo.InvariantCulture);

            }
            set
            {
                Regex regex = new Regex(@"^(\d+)([.,](\d+))?$");
                if (regex.IsMatch(value) && !string.IsNullOrEmpty(value))
                {
                    _maxLon = double.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture);
                    OnPropertyChanged(nameof(MaxLon));
                }
            }
        }

        private double _minLat;
        public string MinLat
        {
            get
            {
                return _minLat.ToString(CultureInfo.InvariantCulture);

            }
            set
            {
                Regex regex = new Regex(@"^(\d+)([.,](\d+))?$");
                if (regex.IsMatch(value) && !string.IsNullOrEmpty(value))
                {
                    _minLat = double.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture);
                    OnPropertyChanged(nameof(MinLat));
                }
            }
        }

        private double _minLon;
        public string MinLon
        {
            get
            {
                return _minLon.ToString(CultureInfo.InvariantCulture);

            }
            set
            {
                Regex regex = new Regex(@"^(\d+)([.,](\d+))?$");
                if (regex.IsMatch(value) && !string.IsNullOrEmpty(value))
                {
                    _minLon = double.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture);
                    OnPropertyChanged(nameof(MinLon));
                }
            }
        }

        private string _log;
        public string Log
        {
            get
            {
                return _log;

            }
            set
            {
                _log = value;
                OnPropertyChanged(nameof(Log));
            }
        }

        public ICommand StartCuttingCommand
        {
            get { return new CommandImpl(StartCutting); }
        }

        public void StartCutting()
        {
            Log = string.Empty;

            if (string.IsNullOrWhiteSpace(InputFileName))
            {
                Log += $"Input file can't be empty { Environment.NewLine}";
                return;
            }
            if (string.IsNullOrWhiteSpace(OutputFileName))
            {
                Log += $"Output file name can't be empty { Environment.NewLine}";
                return;
            }

            Log += $"Loading osm data form input file{ Environment.NewLine}";
            OsmData osmData = null;
            try
            {
                osmData = OsmParser.Parse(InputFileName, _minLon, _minLat, _maxLon, _maxLat);
            }
            catch (FileNotFoundException e)
            {

                Log += $"input file not found { Environment.NewLine}";
                return;
            }

            osmData.FillWaysNode();
            osmData.RemoveRelationsWithoutMembers();
            osmData.RemoveWaysWithoutNodes();

            Log += $"Writing osm data to output file{Environment.NewLine}{AppDomain.CurrentDomain.BaseDirectory}{OSMtoSharp.FileManagers.Constants.Constants.FileFolder}{Path.DirectorySeparatorChar}{OutputFileName}{ Environment.NewLine}";
            if (OsmXmlWriter.WriteOsmDataToXml(osmData, OutputFileName, _minLon, _minLat, _maxLon, _maxLat))
            {
                Log += $"Done! { Environment.NewLine}";
            }
        }

        private void PrepareStartData()
        {
            MinLon = "18.6762000";
            MinLat = "50.2862500";
            MaxLon = "18.6797600";
            MaxLat = "50.2877300";
            OutputFileName = "OutputFile.osm";

        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
