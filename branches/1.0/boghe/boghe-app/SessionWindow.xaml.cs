﻿/*
* Boghe IMS/RCS Client - Copyright (C) 2010 Mamadou Diop.
*
* Contact: Mamadou Diop <diopmamadou(at)doubango.org>
*	
* This file is part of Boghe Project (http://code.google.com/p/boghe)
*
* Boghe is free software: you can redistribute it and/or modify it under the terms of 
* the GNU General Public License as published by the Free Software Foundation, either version 3 
* of the License, or (at your option) any later version.
*	
* Boghe is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
* See the GNU General Public License for more details.
*	
* You should have received a copy of the GNU General Public License along 
* with this program; if not, write to the Free Software Foundation, Inc., 
* 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BogheCore.Sip;
using BogheCore.Services;
using BogheApp.Services.Impl;
using BogheCore;
using System.Timers;
using System.Globalization;
using log4net;
using System.Reflection;
using BogheCore.Model;
using org.doubango.tinyWRAP;
using BogheControls;
using BogheApp.Items;
using System.Collections.Specialized;
using BogheControls.Utils;

namespace BogheApp
{
    /// <summary>
    /// Interaction logic for SessionWindows.xaml
    /// </summary>
    public partial class SessionWindow : Window
    {
        private static ILog LOG = LogManager.GetLogger(typeof(SessionWindow));
        private static List<SessionWindow> windows = new List<SessionWindow>();

        private bool isHeld = false;
        private MyMsrpSession chatSession = null;
        private MyAVSession avSession = null;
        private readonly List<MyMsrpSession> fileTransferSessions;
        private readonly String remotePartyUri = null;

        private readonly IContactService contactService;
        private readonly ISipService sipService;
        private readonly IHistoryService historyService;
        private readonly ISoundService soundService;
        private readonly IConfigurationService configurationService;

        private readonly Timer timerCall;

        private HistoryChatEvent chatHistoryEvent;
        private HistoryAVCallEvent avHistoryEvent;

        private readonly VideoDisplay videoDisplayLocal;
        private readonly VideoDisplay videoDisplayRemote;

        private readonly MyObservableCollection<HistoryEvent> historyDataSource;

        public SessionWindow(String remotePartyUri)
            : base()
        {
            InitializeComponent();

            this.remotePartyUri = remotePartyUri;
            this.Title = String.Empty;
            this.buttonCallOrAnswer.Tag = "Call";

            this.fileTransferSessions = new List<MyMsrpSession>();

            this.videoDisplayLocal = new VideoDisplay();
            this.videoDisplayLocal.Visibility = Visibility.Hidden;
            this.videoDisplayRemote = new VideoDisplay();
            this.videoDisplayRemote.Visibility = Visibility.Hidden;
            this.videoDisplayRemote.ToolTip = this.borderVideoDispalyRemote.ToolTip;

            this.borderVideoDispalyRemote.Child = this.videoDisplayRemote;
            this.borderVideoDispalyLocal.Child = this.videoDisplayLocal;

            this.labelInfo.Content = String.Empty;
            this.timerCall = new Timer(1000);
            this.timerCall.AutoReset = true;
            this.timerCall.Elapsed += this.timerCall_Elapsed;

            // Services
            this.contactService = Win32ServiceManager.SharedManager.ContactService;
            this.sipService = Win32ServiceManager.SharedManager.SipService;
            this.historyService = Win32ServiceManager.SharedManager.HistoryService;
            this.soundService = Win32ServiceManager.SharedManager.SoundService;
            this.configurationService = Win32ServiceManager.SharedManager.ConfigurationService;

            // Messaging
            this.historyDataSource = new MyObservableCollection<HistoryEvent>();
            this.historyCtrl.ItemTemplateSelector = new DataTemplateSelectorMessaging();
            this.historyCtrl.ItemsSource = this.historyDataSource;

            // Register to SIP events
            this.sipService.onInviteEvent += this.sipService_onInviteEvent;

            lock (SessionWindow.windows)
            {
                SessionWindow.windows.Add(this);
            }
        }

        public static List<SessionWindow> Windows
        {
            get { return SessionWindow.windows; }
        }

        private bool IsHeld
        {
            get { return this.isHeld; }
            set
            {
                if (this.isHeld != value)
                {
                    this.isHeld = value;
                    if (this.isHeld)
                    {
                        this.MenuItemCall_HoldResume.Header = "Resume Call";
                        this.MenuItemCall_HoldResumeImage.Source = MyImageConverter.FromBitmap(Properties.Resources.call_resume_16);
                    }
                    else
                    {
                        this.MenuItemCall_HoldResume.Header = "Hold Call";
                        this.MenuItemCall_HoldResumeImage.Source = MyImageConverter.FromBitmap(Properties.Resources.call_hold_16);
                    }
                }
            }
        }

        private MyAVSession AVSession
        {
            get { return this.avSession; }
            set { 
                this.avSession = value;
                this.IsHeld = false;
            }
        }

        private MyMsrpSession ChatSession
        {
            get { return this.chatSession; }
            set 
            {
                if (this.chatSession != null)
                {
                    this.chatSession.onMsrpEvent -= this.ChatSession_onMsrpEvent;
                    if (this.chatHistoryEvent != null)
                    {
                        this.historyService.AddEvent(this.chatHistoryEvent);
                    }
                }
                if ((this.chatSession = value) != null)
                {
                    this.chatSession.onMsrpEvent += this.ChatSession_onMsrpEvent;
                    this.chatHistoryEvent = new HistoryChatEvent(this.remotePartyUri);
                    this.chatHistoryEvent.SipSessionId = value.Id;
                }
            }
        }

        public bool CanMakeAudioVideoCall(String remoteUri)
        {
            if (String.Equals(this.remotePartyUri, remoteUri))
            {
                return (this.AVSession == null);
            }
            return false;
        }

        public bool CanStartChat(String remoteUri)
        {
            if (String.Equals(this.remotePartyUri, remoteUri))
            {
                return (this.ChatSession == null);
            }
            return false;
        }

        public bool CanReceiveCall(MyInviteSession session)
        {
            if (String.Equals(this.remotePartyUri, session.RemotePartyUri))
            {
                if (session is MyMsrpSession)
                {
                    if (session.MediaType == MediaType.Chat)
                    {
                        return (this.ChatSession == null);
                    }
                    else
                    {
                        // Can always receive file transfer session as long as it's from the same remote uri
                        return true;
                    }
                }
                else if (session is MyAVSession)
                {
                    return (this.AVSession == null);
                }
            }
            return false;
        }

        public bool CanReceiveFile(String remoteUri)
        {
            return String.Equals(this.remotePartyUri, remoteUri);
        }

        public bool CanSendFile(String remoteUri)
        {
            return String.Equals(this.remotePartyUri, remoteUri);
        }

        public void MakeAudioCall(String remoteUri)
        {
            System.Diagnostics.Debug.Assert(this.AVSession == null);

            this.AVSession = MyAVSession.CreateOutgoingSession(Win32ServiceManager.SharedManager.SipService.SipStack, MediaType.Audio);
            this.Show();
            this.AVSession.MakeCall(remoteUri);

            this.InitializeView();
        }

        public void MakeVideoCall(String remoteUri)
        {
            System.Diagnostics.Debug.Assert(this.AVSession == null);

            this.AVSession = MyAVSession.CreateOutgoingSession(Win32ServiceManager.SharedManager.SipService.SipStack, MediaType.AudioVideo);
            this.Show();
            this.AVSession.MakeCall(remoteUri);

            this.InitializeView();
        }

        public void StartChat(String remoteUri)
        {
            System.Diagnostics.Debug.Assert(this.ChatSession == null);

            this.Show();
            this.InitializeView();
        }

        public void ReceiveCall(MyInviteSession session)
        {
            bool isAV = (session is MyAVSession);

            if(isAV)
            {
                System.Diagnostics.Debug.Assert(this.AVSession == null);

                this.AVSession = session as MyAVSession;
                this.avHistoryEvent = new HistoryAVCallEvent(((session.MediaType & MediaType.Video) == MediaType.Video), session.RemotePartyUri);
                this.avHistoryEvent.SipSessionId = session.Id;
                this.avHistoryEvent.Status = HistoryEvent.StatusType.Missed;
                
            }
            else if (session is MyMsrpSession)
            {
                MyMsrpSession msrpSession = session as MyMsrpSession;
                msrpSession.SuccessReport = this.MsrpSuccessReport;
                msrpSession.FailureReport = this.MsrpFailureReport;
                msrpSession.OmaFinalDeliveryReport = this.MsrpOmaFinalDeliveryReport;

                if (session.MediaType == MediaType.Chat)
                {
                    System.Diagnostics.Debug.Assert(this.ChatSession == null);

                     this.ChatSession = msrpSession;
                }
                else if (session.MediaType == MediaType.FileTransfer)
                {
                    HistoryFileTransferEvent @event = new HistoryFileTransferEvent(this.remotePartyUri, msrpSession.FilePath);
                    @event.Status = HistoryEvent.StatusType.Incoming;
                    @event.SipSessionId = session.Id;
                    @event.MsrpSession = msrpSession;
                    this.AddMessagingEvent(@event);
                }
                else
                {
                    throw new Exception("Unsupported session Type");
                }
            }
            else
            {
                throw new Exception("Unsupported session Type");
            }
            
            this.InitializeView();
            this.Show();


            if (isAV)
            {
                if (((session.MediaType & MediaType.Video) == MediaType.Video))
                {
                    this.AttachDisplays();
                }
                this.soundService.PlayRingTone();
            }
            else if (session.MediaType == MediaType.Chat && this.ChatSession != null)
            {
                this.ChatSession.Accept();
            }
        }

        public void SendFile(String remoteUri, String filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
                fileDialog.Multiselect = false;
                Nullable<bool> result = fileDialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    filePath = fileDialog.FileName;
                }
            }

            if (!String.IsNullOrEmpty(filePath))
            {
                this.Show();

                this.InitializeView();
                this.SendFile(filePath);
            }
        }

        private void timerCall_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                            new EventHandler<ElapsedEventArgs>(this.timerCall_Elapsed), sender, new object[] { e });
                    return;
                }
                 
                if (this.avHistoryEvent != null)
                {
                    TimeSpan duration = (DateTime.Now - this.avHistoryEvent.StartTime);
                    this.labelDuration.Content = string.Format("{0:D2}:{1:D2}:{2:D2}", duration.Hours, duration.Minutes, duration.Seconds);
                }
            }
            catch (TargetInvocationException ex)
            {
                LOG.Error(ex);
            }
        }

        private void buttonCallOrAnswer_Click(object sender, RoutedEventArgs e)
        {
            if ("Call".Equals(this.buttonCallOrAnswer.Tag) && this.AVSession == null)
            {
                this.AVSession = MyAVSession.CreateOutgoingSession(this.sipService.SipStack, MediaType.AudioVideo);
                this.AVSession.MakeCall(this.remotePartyUri);
            }
            else if (this.AVSession != null)
            {
                this.AVSession.AcceptCall();
            }
        }

        private void buttonHangUp_Click(object sender, RoutedEventArgs e)
        {
            if (this.AVSession != null)
            {
                this.AVSession.HangUpCall();
            }
        }

        private void buttonSendText_Click(object sender, RoutedEventArgs e)
        {
            if (this.ChatSession == null)
            {
                this.ChatSession = this.CreateOutgoingSession(MediaType.Chat);
            }
            HistoryShortMessageEvent @event = new HistoryShortMessageEvent(this.remotePartyUri);
            @event.Status = HistoryEvent.StatusType.Outgoing;
            @event.SipSessionId = this.ChatSession.Id;
            @event.Content = this.textBoxInput.Text;
            this.AddMessagingEvent(@event);

            this.ChatSession.SendMessage(this.textBoxInput.Text);
            this.textBoxInput.Text = String.Empty;
        }

        private void SessionWindowName_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.AVSession != null && this.AVSession.IsActive)
            {
                this.AVSession.HangUpCall();
            }
            if (this.ChatSession != null && this.ChatSession.IsActive)
            {
                this.ChatSession.HangUp();
            }

            lock (this.fileTransferSessions)
            {
                this.fileTransferSessions.ForEach((x) =>
                {
                    if (x != null && x.IsActive)
                    {
                        x.HangUp();
                    }
                });
            }
        }

        private void SessionWindowName_Closed(object sender, EventArgs e)
        {
            this.AVSession = null;
            this.ChatSession = null;

            lock (SessionWindow.windows)
            {
                SessionWindow.windows.Remove(this);
            }
        }

        private void labelVideoDisplayRemote_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (this.AVSession != null && this.AVSession.IsConnected && this.AVSession.MediaSessionMgr != null)
            //{
            //    this.VideoDisplayRemote.Width = this.labelVideoDisplayRemote.Width;
            //}
        }

        private void SessionWindowName_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.AVSession != null)
            {
                
            }

            this.historyDataSource.CollectionChanged += (_sender, _e) =>
            {
                switch (_e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                    case NotifyCollectionChangedAction.Remove:
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Reset:
                        this.historyCtrlScrollViewer.ScrollToEnd();
                        break;
                }
            };
        }
    }
}
