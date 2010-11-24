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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BogheControls;
using BogheCore.Model;
using System.Collections.ObjectModel;
using BogheApp.Items;
using BogheCore.Services;
using BogheApp.Services.Impl;
using BogheCore;
using BogheCore.Sip;
using BogheCore.Utils;

namespace BogheApp.Screens
{
    /// <summary>
    /// Interaction logic for ScreenContacts.xaml
    /// </summary>
    public partial class ScreenContacts : BaseScreen
    {
        private readonly IContactService contactService;
        private readonly ISipService sipService;
        private readonly MyObservableCollection<BaseObject> contacts;

        public ScreenContacts():base()
        {
            InitializeComponent();

            this.contacts = new MyObservableCollection<BaseObject>();
            this.contactService = Win32ServiceManager.SharedManager.ContactService;
            this.sipService = Win32ServiceManager.SharedManager.SipService;

            this.listBox.ItemTemplateSelector = new DataTemplateSelectorContacts();
            this.listBox.ItemsSource = this.contacts;

            this.contactService.onContactEvent += this.contactService_onContactEvent;            
        }

        private void buttonVoice_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(this.textBoxFreeContact.Text))
            {
                String remoteUri = UriUtils.MakeValidSipUri(this.textBoxFreeContact.Text);
                if (!String.IsNullOrEmpty(remoteUri))
                {
                    SessionWindow.MakeAudioCall(remoteUri);
                }
            }
        }

        private void buttonVisio_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(this.textBoxFreeContact.Text))
            {
                String remoteUri = UriUtils.MakeValidSipUri(this.textBoxFreeContact.Text);
                if (!String.IsNullOrEmpty(remoteUri))
                {
                    SessionWindow.MakeVideoCall(remoteUri);
                }
            }
        }

        private void buttonFile_Click(object sender, RoutedEventArgs e)
        {

        }        
    }
}