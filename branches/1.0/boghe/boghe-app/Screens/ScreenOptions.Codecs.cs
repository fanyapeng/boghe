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
using org.doubango.tinyWRAP;
using System.Windows.Data;
using System.ComponentModel;
using BogheCore.Model;

namespace BogheApp.Screens
{
    partial class ScreenOptions
    {
        List<Codec> codecs;

        private void InitializeCodecs()
        {
            codecs = new List<Codec>(new Codec[]{
                new Codec("PCMA", "PCMA (8 KHz)", tdav_codec_id_t.tdav_codec_id_pcma),
                new Codec("PCMU", "PCMU (8 KHz)", tdav_codec_id_t.tdav_codec_id_pcmu),
                new Codec("GSM", "GSM (8 KHz)", tdav_codec_id_t.tdav_codec_id_gsm),
            });
            if (SipStack.isCodecSupported(tdav_codec_id_t.tdav_codec_id_amr_nb_oa))
            {
                codecs.Add(new Codec("AMR-NB-OA", "AMR Narrow Band Octet Aligned (8 KHz)", tdav_codec_id_t.tdav_codec_id_amr_nb_oa));
            }
            if (SipStack.isCodecSupported(tdav_codec_id_t.tdav_codec_id_amr_nb_be))
            {
                codecs.Add(new Codec("AMR-NB-BE", "AMR Narrow Band Bandwidth Efficient (8 KHz)", tdav_codec_id_t.tdav_codec_id_amr_nb_be));
            }
            if (SipStack.isCodecSupported(tdav_codec_id_t.tdav_codec_id_ilbc))
            {
                codecs.Add(new Codec("iLBC", "internet Low Bitrate Codec (8 KHz)", tdav_codec_id_t.tdav_codec_id_ilbc));
            }
            if (SipStack.isCodecSupported(tdav_codec_id_t.tdav_codec_id_speex_nb))
            {
                codecs.Add(new Codec("Speex-NB", "Speex Narrow Band (8 KHz)", tdav_codec_id_t.tdav_codec_id_speex_nb));
            }
            if (SipStack.isCodecSupported(tdav_codec_id_t.tdav_codec_id_g729ab))
            {
                codecs.Add(new Codec("G.729", "G729 Annex A/B (8 KHz)", tdav_codec_id_t.tdav_codec_id_g729ab));
            }

            codecs.AddRange(new Codec[]
            {
                new Codec("MP4V-ES", "MPEG-4 Part 2", tdav_codec_id_t.tdav_codec_id_mp4ves_es),
                new Codec("Theora", "Theora", tdav_codec_id_t.tdav_codec_id_theora),
                new Codec("H264-BP10", "H.264 Base Profile 1.0", tdav_codec_id_t.tdav_codec_id_h264_bp10),
                new Codec("H264-BP20", "H.264 Base Profile 2.0", tdav_codec_id_t.tdav_codec_id_h264_bp20),
                new Codec("H264-BP30", "H.264 Base Profile 3.0", tdav_codec_id_t.tdav_codec_id_h264_bp30),
                new Codec("H.263", "H.263", tdav_codec_id_t.tdav_codec_id_h263),
                new Codec("H.263+", "H.263-1998", tdav_codec_id_t.tdav_codec_id_h263p),
                new Codec("H.263++", "H.263-2000", tdav_codec_id_t.tdav_codec_id_h263pp),
            });

            this.listBoxCodecs.ItemsSource = codecs;
            ICollectionView view = CollectionViewSource.GetDefaultView(this.listBoxCodecs.ItemsSource);
            view.GroupDescriptions.Add(new PropertyGroupDescription("CodecType"));
        }

        private void LoadCodecs()
        {
            codecs.ForEach(x => x.IsEnabled = ((x.Id & (tdav_codec_id_t)this.sipService.Codecs) == x.Id));
        }        

        private bool UpdateCodecs()
        {
            tdav_codec_id_t codecIds = tdav_codec_id_t.tdav_codec_id_none;
            this.codecs.ForEach(x => codecIds |= x.IsEnabled ? x.Id : tdav_codec_id_t.tdav_codec_id_none);
            this.sipService.Codecs = (int)codecIds;

            this.configurationService.Set(Configuration.ConfFolder.MEDIA,
                        Configuration.ConfEntry.CODECS, this.sipService.Codecs);

            return true;
        }

        class Codec
        {
            readonly String name;
            readonly String description;
            readonly tdav_codec_id_t id;
            bool enabled;

            internal Codec(String name, String description, tdav_codec_id_t id)
            {
                this.name = name;
                this.description = description;
                this.id = id;
            }

            public String Name
            {
                get { return this.name; }
            }

            public String Description
            {
                get { return this.description; }
            }

            public tdav_codec_id_t Id
            {
                get { return this.id; }
            }

            public bool IsEnabled
            {
                get { return this.enabled; }
                set { this.enabled = value; }
            }

            public String CodecType
            {
                get
                {
                    switch (this.id)
                    {
                        case tdav_codec_id_t.tdav_codec_id_pcma:
                        case tdav_codec_id_t.tdav_codec_id_pcmu:
                        case tdav_codec_id_t.tdav_codec_id_gsm: 
                        case tdav_codec_id_t.tdav_codec_id_amr_nb_oa:
                        case tdav_codec_id_t.tdav_codec_id_amr_nb_be:
                        case tdav_codec_id_t.tdav_codec_id_ilbc:
                        case tdav_codec_id_t.tdav_codec_id_speex_nb:
                        case tdav_codec_id_t.tdav_codec_id_g729ab:
                            return "Audio Codecs";
                        default:
                            return "Video Codecs";
                    }
                }
            }
        }
    }
}
