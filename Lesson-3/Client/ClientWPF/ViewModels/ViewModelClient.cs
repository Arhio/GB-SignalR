using ClientWPF.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;

namespace ClientWPF.ViewModels
{
    public class ViewModelClient : ViewModelBase
    {
        private ModelClient model { get; set; }

        public string MessageBox 
        { 
            get => model.MessagesBox;
            set
            {
                model.MessagesBox = value;
                OnPropertyChanged(nameof(MessageBox));
            }
        }

        public string StateConnection 
        { 
            get => model.StateConnection;
            set => OnPropertyChanged(nameof(StateConnection));
            
        }

        public SolidColorBrush StateConnectionColor
        {
            get => model.StateConnectionColor;
            set => OnPropertyChanged(nameof(StateConnectionColor));            
        }

        public string NameMethodConnect
        {
            get => model.NameMethodConnect;
            set => OnPropertyChanged(nameof(NameMethodConnect));            
        }

        public string BodyMessage
        {
            get => model.BodyMessage;
            set
            {
                model.BodyMessage = value;
                OnPropertyChanged(nameof(BodyMessage));
            }
        }

        public string NameClient
        {
            get => model.NameClient;
            set
            {
                model.NameClient = value;
                OnPropertyChanged(nameof(NameClient));
            }
        }

        public string NameSetClient
        {
            get => model.NameSetClient;
            set
            {
                model.NameSetClient = value;
                OnPropertyChanged(nameof(NameSetClient));
            }
        }

        public ViewModelClient()
        {
            model = new ModelClient();
            model.eventMessageBox += (sender, e) => MessageBox = model.MessagesBox;
            model.eventStateConnection += (sender, e) => 
            { 
                StateConnection = model.StateConnection; 
                StateConnectionColor = model.StateConnectionColor; 
            };
            model.eventNameMethodConnect += (sender, e) => NameMethodConnect = model.NameMethodConnect;
            model.eventNameClient += (sender, e) => NameClient = model.NameClient;
            model.eventNameSetClient += (sender, e) => NameSetClient = model.NameSetClient;
        }


        private RelayCommand _connect_Command;
        public RelayCommand Connect_Command => _connect_Command ?? (_connect_Command = new RelayCommand(param => model.Connecting()));

        private RelayCommand _send_Command;
        public RelayCommand Send_Command => _send_Command ?? (_send_Command = new RelayCommand(param => model.SendMessage()));

        private RelayCommand _setName_Command;
        public RelayCommand SetName_Command => _setName_Command ?? (_setName_Command = new RelayCommand(param => model.SetName()));

        private RelayCommand _getName_Command;
        public RelayCommand GetName_Command => _getName_Command ?? (_getName_Command = new RelayCommand(param => model.GetName()));


    }
}
