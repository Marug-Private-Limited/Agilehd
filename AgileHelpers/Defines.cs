using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

internal static class Defines
{
    public static bool auto_stream_remove = false;

    public enum EffectType { Mix, DVE }

    public static string strLayerA = string.Empty;

    public static bool LayerA_State = true;

    public static string strLayerA_img_filter = string.Empty;// "FFFFFF";

    public static string strLayerB = string.Empty;

    public static bool LayerB_State = false;

    public static string strAudioLayerA = string.Empty;

    public static string strAudioLayerB = string.Empty;

    public static string strLayerC = string.Empty;

    public static string strLayerD = string.Empty;

    public static string[] strTransitionA = new string[2] { "foreground", "LayerA_FG" };

    public static string[] strTransitionB = new string[2] { "background", "LayerB_BG" };

    public static string[] strTransitionTrans = new string[2] { "scene_3d", "ahd_trn" };

    public static string[] strTransitionVirtual = new string[3] { "scene_3d", "ahd_trn", "virtual" };

    public static string[] strMotionFilter = new string[2] { "foreground", "M_Filter" };

    public static string[] strAfxMedia = new string[2] { "foreground", "AfxMedia" };

    public static string[] strCamera = new string[2] { "scene_3d", "camera" };

    public static string[] strForeground = new string[1] { "foreground" };

    public static string[] strScene_3d = new string[1] { "scene_3d" };

    public static string[] strTransitionView = new string[1] { "view" };

    public static string[] strAudioA = new string[2] { "background", "amix1" };

    public static string[] strAudioB = new string[2] { "background", "amix2" };
    public static string[] strExtAudio = new string[2] { "background", "aExt" };

    public static readonly string Props_Camera = "cx='0.0' cy='0.0' cz='0' vert='0.0' horz='0' rotate='0.0' distance='0.0' zoom='1' show='true'";

    public static readonly string Props_Transition = "show='1' h='1' w='1.0' d='1.0' z='0' rh='0'";

    public class Asset
    {
        private static string app_dir = Application.StartupPath;

        /// Assets category items.
        public const string root = @"D:\ahd_assets";

        private const string objects = "3ds";

        private const string image = "images";

        private const string mask = "masks";

        private const string media = "media";

        private const string media_clips = "clips";

        private const string media_clips_backup = "mc_bkp";

        private const string media_stills = "stills";

        private const string media_stills_backup = "ms_bkp";

        private const string motion_filter = "motion_filters";

        private const string song_project = "s_projects";

        private const string title_project = "t_projects";

        private const string trans_project = "projects";

        private const string video = "videos";

        public static string dm_gallery_path = Path.Combine(root, "dm");


        private static string make_dir(string _path)
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            return Path.GetFullPath(_path);
        }


        public static string object_path
        {
            get { return make_dir(Path.Combine(root, objects)); }
        }

        public static string image_path
        {
            get { return make_dir(Path.Combine(root, image)); }
        }

        public static string mask_path
        {
            get { return make_dir(Path.Combine(root, mask)); }
        }

        public static string media_path
        {
            get { return make_dir(Path.Combine(root, media)); }
        }

        public static string media_clips_path
        {
            get { return make_dir(Path.Combine(root, media, media_clips)); }
        }

        public static string media_clips_backup_path
        {
            get { return make_dir(Path.Combine(root, media, media_clips, media_clips_backup)); }
        }

        public static string media_stills_path
        {
            get { return make_dir(Path.Combine(root, media, media_stills)); }
        }

        public static string media_stills_backup_path
        {
            get { return make_dir(Path.Combine(root, media, media_stills, media_stills_backup)); }
        }

        public static string motion_filter_path
        {
            get { return make_dir(Path.Combine(root, motion_filter)); }
        }

        public static string song_project_path
        {
            get { return make_dir(Path.Combine(root, song_project)); }
        }

        public static string title_project_path
        {
            get { return make_dir(Path.Combine(root, title_project)); }
        }

        public static string trans_project_path
        {
            get { return make_dir(Path.Combine(root, trans_project)); }
        }

        public static string video_path
        {
            get { return make_dir(Path.Combine(root, video)); }
        }

        public static string config_path
        {
            get { return make_dir(Path.Combine(root, "Config")); }
        }

        public static string capture_path
        {
            get { return make_dir(Path.Combine(root, "Capture")); }
        }

        public static string new_playlist()
        {
            string x = Path.Combine(root, "plst_config.xml");
            if (!File.Exists(x))
            {
                File.WriteAllText(x, plst, Encoding.ASCII);
            }
            return x;
        }

        public static string into_video
        {
            get { return string.Format(@"{0}\{1}", video_path, "ahd_default.mp4"); }
        }

        /// Freeze
        public static string freeze_file
        {
            get { return string.Format(@"{0}\{1}", capture_path, "Freeze.png");  }
        }

        public static string freeze_stream = "fr";


        /// Motion Filter
        public static string filter_stream = "mf";

        /// FTB
        public static string ftb_stream = "ftb";


        /// Media gallery items.
        public const string media_boy1 = "boy1";

        public const string media_boy2 = "boy2";

        public const string media_boy3 = "boy3";

        public const string media_boy4 = "boy4";

        public const string media_boy5 = "boy5";

        public const string media_couple1 = "couple1";

        public const string media_couple2 = "couple2";

        public const string media_couple3 = "couple3";

        public const string media_couple4 = "couple4";

        public const string media_couple5 = "couple5";

        public const string media_girl1 = "girl1";

        public const string media_girl2 = "girl2";

        public const string media_girl3 = "girl3";

        public const string media_girl4 = "girl4";

        public const string media_girl5 = "girl5";


        /// Media gallery properties.
        private const string media_clips_extenstion = ".mp4";

        private const string media_stills_extension = ".png";

        /// Transition properties.
        public static string trans_project_extension = ".ahp";

        public static string trans_image_extension = ".ahi";

        public static ImageFormat trans_image_format = ImageFormat.Jpeg;

        public const int trans_image_width = 200;

        public const int trans_image_height = 150;

        static string plst = @"<m_config path='MPlaylist'>
	<video video_format='HD1080-59i' width='1920' height='1080' rate='29.97003' pixel_format='HDYC' aspect_ratio='16:9' interlace='top' scale_type='none'/>
	<audio track_split=''>
		<track source_index='0' mode='enabled' input_channels='' gain='' mute='' output_channels='' desc='Track 0'/>
	</audio>
	<file in='0.0' path='D:\Agile HD\Videos\ahd_default.mp4' start_timecode='' interlace='' aspect_ratio='' scale_type='default' format_3d='default' right_eye_video='' external_audio='' external_audio_offset='' preview.pull='false' img_stub='' img.default_duration='3600.0' img.tone='' no_breaks='false' open_file.max_wait='10000' open_url.max_wait='10000' read_url.max_wait='3000' read_file.max_wait='3000' force_mpeg_tc='false' audio_track='0' video_track='0' subtitle_track='-1' ts_program='-1' network.buffer_min='1.0' network.buffer_max='10.0' network.rate_control='true' file.buffer_min='0.0' file.buffer_max='2.0' decode='auto' decode.fallback='false' file.max_forward_rate='4.0' out_point.mode='wait' duration='10.043367' loop='false' ref_id='0x1A0A8A58'>
		<info ts_programs='0' hw_acceleration='NVidia decoder' audio_tracks='1' audio_track.used='1' video_tracks='1' video_track.used='0' subtitle_tracks='0' subtitle_track.used='' context_type='FFM' streams='2' format='mov,mp4,m4a,3gp,3g2,mj2' format_name='QuickTime / MOV' start_time='0.0' duration='10.090667' size='15475852' bitrate='12269438' bitrate_kbps='1497' metadata.major_brand='mp42' metadata.minor_version='0' metadata.compatible_brands='mp42mp41' metadata.creation_time='2017-10-30T11:39:32.000000Z' kbps_avg_video='13918.545' kbps_avg='14247.414' kbps_avg_audio='317.372'>
			<video.0 idx='0' pid='1' codec='h264' codec_name='H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10' codec_tag='avc1' profile='Main' width='1920' height='1080' has_b_frames='1' pixel_ar='0:1' display_ar='0:1' codec_frame_rate='29.97003' r_frame_rate='30000/1001' avg_frame_rate='30000/1001' time_base='1/30000' duration_ts='301301' duration='10.043' bit_rate='11827370' nb_frames='301' metadata.creation_time='2017-10-30T11:39:32.000000Z' metadata.language='eng' metadata.handler_name='Alias Data Handler' metadata.encoder='AVC Coding'/>
			<audio.0 idx='1' pid='2' codec='aac' codec_name='AAC (Advanced Audio Coding)' codec_tag='mp4a' profile='LC' format='fltp' sample_rate='48000' channels='2' bits='-32' time_base='1/48000' duration_ts='482082' duration='10.043' bit_rate='317375' nb_frames='473' metadata.creation_time='2017-10-30T11:39:32.000000Z' metadata.language='eng' metadata.handler_name='Alias Data Handler'/>
		</info>
		<video video_format='HD1080-29p' width='1920' height='1080' rate='29.97003' pixel_format='UYVY' aspect_ratio='16:9' interlace='progressive'/>
		<audio channels='2' rate='48000' track_split='2'>
			<track source_index='0' mode='enabled' input_channels='0, 1' gain='0.0, 0.0' mute='0, 0' output_channels='0, 1' desc='Track 0'/>
		</audio>
		<object imaudio.enabled='true' statistics.extended='' default_name='MFile' default_tracks='0' channels_per_track='0' internal.convert_frame='false' experimental.buffers='0' pause.fields='2' pause.use_reference='false' audio_channels='' audio_gain='' scaling_quality='auto' file.max_reverse_rate='2.0' crop='' mirror='' rotate='' overlay_rms='false' overlay_rms.pos='0.1' overlay_rms.color='green' overlay_waveform='false' overlay_waveform.pos='-0.3' overlay_waveform.color='' audio.variable_rate='true' mdelay.enabled='false' mdelay.live_preview='false' playback_rate='1.0'/>
		<mitem_props stop_in='' stop_out='' pause_in='' pause_out='' schedule_waitstart=''>
			<transition_in type='' time=''/>
			<transition_out type='' time=''/>
			<file/>
			<device/>
			<object/>
			<audio/>
		</mitem_props>
	</file>
</m_config>";


        public static string config_project = string.Format(@"{0}\config_project.xml", config_path);

        public static string config_writer = " format='mp4' video::b='HD-1080i50' ";

        public static string config_writer_filter = "MPEG-4 Files (*.mp4)|*.mp4|" + "MOV Files (*.mov)|*.mov|" + "MXF Files (*.mxf)|*.mxf|" + "MPEG-2 Files (*.mpg)|*.mpg|" +
                        "MPEG-2 TS Files (*.ts)|*.ts|" + "MKV Files (*.mkv)|*.mkv|" + "WebM Files (*.webm)|*.webm|" + "AVI Files (*.avi)|*.avi|" + "All Files (*.*)|*.*";

        public static decimal config_writer_max_duration = 30;

        public static decimal config_writer_max_pause_duration = 5;

        public static string config_writer_extension = "*.mp4";


        public static string config_media_clips = string.Format(@"{0}\config_media.xml", config_path);

        public static string config_capture = string.Format(@"{0}\config_capture.xml", config_path);

        public static string config_input_media = string.Format(@"{0}\config_input.xml", config_path);


        
    }

    public class Stills
    {
        public const string B1 = "boy1.png";
        public const string B2 = "boy2.png";
        public const string B3 = "boy3.png";
        public const string B4 = "boy4.png";
        public const string B5 = "boy5.png";

        public const string C1 = "couple1.png";
        public const string C2 = "couple2.png";
        public const string C3 = "couple3.png";
        public const string C4 = "couple4.png";
        public const string C5 = "couple5.png";

        public const string G1 = "girl1.png";
        public const string G2 = "girl2.png";
        public const string G3 = "girl3.png";
        public const string G4 = "girl4.png";
        public const string G5 = "girl5.png";
    }

    public class Tag
    {
        public const string mixer = "mixer_streams";
        public const string mixer_file = "file";
        public const string mixer_stream_id = "stream_id";
        public const string mixer_stream_idx = "stream_idx";
        public const string mixer_path = "path";
        public const string mixer_cmd = "Cmd";
        public const string mixer_loop = "loop";

        public const string scenes = "m_scenes";
        public const string scenes_active = "active_scene";
        public const string scenes_scene = "m_scene";
        public const string scenes_scene_name = "name";

        public const string scene_3d = "scene_3d";
        public const string scene_3d_camera = "camera";

        public const string id = "id";
        public const string scene_3d_trn_transition = "ahd_trn";
        public const string scene_3d_trn_duration = "duration";
        public const string scene_3d_trn_fixed = "fixed";
        public const string scene_3d_trn_swap = "swap";
        public const string scene_3d_trn_reverse = "reverse";
        public const string scene_3d_trn_virtual = "virtual";
        public const string scene_3d_trn_hold = "hold";

        public const string scene_3d_group = "group";
        public const string scene_3d_image = "img";
        public const string scene_3d_image_url = "src";
        public const string scene_3d_video = "video";
        public const string scene_3d_object = "object";
        public const string scene_3d_mask = "mask";
        public const string scene_3d_image_filter = "img_filter";

        public const string obj_layer_a = "LayerA.png";
        public const string obj_layer_b = "LayerB.png";

        public const string obj_layer_d1 = "DDR1.png";
        public const string obj_layer_d2 = "DDR2.png";

        public const string obj_layer_d3 = "DDR3.png";
        public const string obj_layer_d4 = "DDR4.png";
        public const string obj_layer_d5 = "DDR5.png";

        public const string view = "view";
        public const string mItem_Props = "mitem_props";
    }

    public enum MediaSource
    {
        None = 0x0,
        Live = 0x1,
        Video = 0x2,
        Image = 0x3,
        Audio = 0x4
    }

    public static class Filters
    {
        public static string Image = "Image Files(*.BMP;*.JPG;*.JPEG;*.JPE;*.GIF;*.PNG;*.EXIF;*.TIF;*.TIFF;*.TGA;*.TPIC)|;*.BMP;*.JPG;*.JPEG;*.JPE;*.GIF;*.PNG;*.EXIF;*.TIF;*.TIFF;*.TGA;*.TPIC";

        public static string Video = "Video Files(*.AVI;*.MP4;*.MPG;*.MPE;*.MPEG;*.MKV;*.MOV;*.M4V;*.FLV;*.MXF;*.WEBM;*.WMV;*.M2TS;*.MTS)|(;*.AVI;*.MP4;*.MPG;*.MPE;*.MPEG;*.MKV;*.MOV;*.M4V;*.FLV;*.MXF;*.WEBM;*.WMV;*.M2TS;*.MTS";

        public static string Video_w_mts = "Video Files(*.AVI;*.MP4;*.MPG;*.MPE;*.MPEG;*.MKV;*.MOV;*.M4V;*.FLV;*.MXF;*.WEBM;*.WMV)|(;*.AVI;*.MP4;*.MPG;*.MPE;*.MPEG;*.MKV;*.MOV;*.M4V;*.FLV;*.MXF;*.WEBM;*.WMV";

        public static string Audio = "Audio Files(*.WAV;*.MP3)|;*.WAV;*.MP3";

        public static string ScreenShot = "PNG files(.png)|*.png|JPEG files (.jpg)|*.jpg|TIFF files (.tiff)|*.tiff|All files (.*)|*.*";

        public static string EfxSequence = "Song Projects(*.ahs)|;*.ahs";

        public static string Effect = "Effect Projects(*.ahp)|;*.ahp";
    }

    public static class Scenes
    {
        public const string switcher = @"<m_scene video_stub='video-stub.png' image_stub='image-stub.png' name='mixer_scene'>
                    <background>
                        <video id='amix1' stream_idx='-1' show='false' w='0' h='0' audio_gain = '-80'/>
		                <video id='amix2' stream_idx='-1' show='false' w='0' h='0' audio_gain = '-80'/>
                        <video id = 'aExt' stream_idx='-1' stream_id='ExtAudio' show='true' w='0' h='0' audio_gain = '-80'/>
			            <video id='LayerB_BG' stream_idx='-1' show='true' w='1.0' h='1.0' alpha='1' audio_gain='-80'/>
		            </background>
		            <scene_3d view_angle='45.0'>
			            <camera cx='0.0' cy='0.0' cz='0' vert='0.0' horz='0' rotate='0.0' distance='0.0' zoom='1' show='true'/>
		            </scene_3d>
		            <foreground parallax-shift='0.05'>
			            <video id='LayerA_FG' stream_idx='-1' show='true' alpha='1' w='1' h='1' audio_gain='-80' x='0' y='0' sw='1' sh='1' z='0' sx='0' sy='0' orgx='0.0' orgy='0.0' orgw='1.0' orgh='1.0'/>
			            <video id='M_Filter' stream_idx='-1' stream_id='MFilter' show='false' alpha='1' w='1' h='1' audio_gain='-80'/>
                        <video id='AfxMedia' stream_idx='-1' h='0' w='0' show='false' audio_gain='-80'/>
                        <!--<video id='Freeze' stream_idx='-1' h='1' w='1' show='false' audio_gain='-80'/>
                        <video id='FTB' stream_idx='-1' stream_id='FTB' h='1' w='1' show='false' audio_gain='-80'/>-->
		            </foreground>
		            <view invoke='true'/>
	                <timeline invoke='true'/>
	            </m_scene>";

        public const string capture = @"<m_scene video_stub='video-stub.png' image_stub='image-stub.png' name='mixer_scene'>
                    <background/>
		            <scene_3d view_angle='45.0'/>
		            <foreground parallax-shift='0.05'>
			            <video id='LayerA_FG' stream_idx='-1' show='true' alpha='1' w='1' h='1' x='0' y='0' sw='1' sh='1' z='0' sx='0' sy='0'/>
		            </foreground>
		            <view invoke='true'/>
	                <timeline invoke='true'/>
	            </m_scene>";

        public static string[] LayerA = new string[2] { "foreground", "LayerA_FG" };

        public static string[] LayerB = new string[2] { "background", "LayerB_BG" };

        public static string[] TransitionHeader = new string[2] { "scene_3d", "ahd_trn" };

        public static string[] TransitionVirtual = new string[3] { "scene_3d", "ahd_trn", "virtual" };

        public static string[] MotionFilter = new string[2] { "foreground", "M_Filter" };

        //public static string[] Freeze = new string[2] { "foreground", "Freeze" };

        public static string[] AfxMedia = new string[2] { "foreground", "AfxMedia" };

        public static string[] Camera = new string[2] { "scene_3d", "camera" };

        public static string[] Foreground = new string[1] { "foreground" };

        public static string[] Scene_3D = new string[1] { "scene_3d" };

        public static string[] View = new string[1] { "view" };


        public static string song = @"<m_scene name='scene_000'>
                    <background>
		                <video id='BG_A' stream_idx='-1' stream_id='sba' show='false' w='0' h='0' audio_gain = '0'/>
			            <video id='BG_V' stream_idx='-1' stream_id='sbv' show='true' w='1.0' h='1.0' alpha='1' audio_gain='0'/>
		            </background>
		            <scene_3d view_angle='45.0'>
			            <camera />
		            </scene_3d
		            <foreground />
		            <view />
                </m_scene>";
    }

    public static class Stream
    {
        public const string strLive1 = "LA";
        public const string strLive2 = "LB";
        public const string strLive3 = "live3";
        public const string strLive4 = "live4";
        public const string strLive5 = "live5";
        public const string strDDR1 = "ddr1";
        public const string strDDR2 = "ddr2";
        public const string strDDR3 = "ddr3";
        public const string strDDR4 = "ddr4";
        public const string strDDR5 = "ddr5";
        public const string strAMix = "amix";
     

        public const string strMS = "MS";
        public const string strM1 = "M1";
        public const string strM2 = "M2";
        public const string strG1 = "G1";
        public const string strG2 = "G2";
        public const string strExtAudio = "ExtAudio";


        //public static string Live1 = "LA";
        //public static string Live2 = "LB";
        //public static string DDR1 = "DDR1";
        //public static string DDR2 = "DDR2";
        //public static string DDR3 = "DDR3";
        //public static string DDR4 = "DDR4";
        //public static string DDR5 = "DDR5";

        public const string SBV = "SBV";
        public const string SBA = "SBA";
        public const string SD1 = "SD1";
        public const string SD2 = "SD2";
        public const string SD3 = "SD3";
        public const string SD4 = "SD4";
        public const string SD5 = "SD5";

        public static string[] MediaInput = new string[]
        {
            strLive1,
            strLive2,
            //strLive3,
          //  strLive4,
          //  strLive5,
            strM1,
            strM2,
            strG1,
            strG2,
            //strDDR1,
            //strDDR2,
            //strDDR3,
            //strDDR4,
            strDDR5,
            strAMix,
             strExtAudio
        };
    }

    public static class Theme
    {
        /// Video switch
        public static Brush Switch_Default = new SolidColorBrush(Color.FromRgb(3, 16, 41));

        public static Brush Switch_Selected = new SolidColorBrush(Color.FromRgb(0, 162, 255)); //Color.FromArgb(27, 94, 206);


        /// Gallery
        public static System.Drawing.Color mGallery_Parent_Default = System.Drawing.Color.FromArgb(27, 36, 50);

        public static System.Drawing.Color mGallery_Parent_Highlight = System.Drawing.Color.FromArgb(1, 7, 19);

        public static System.Drawing.Color mGallery_Child_Default = System.Drawing.Color.FromArgb(1, 7, 19);

        public static System.Drawing.Color mGallery_Child_Highlight = System.Drawing.Color.FromArgb(8, 14, 26);


        /// Song Capture
        public static Brush mCapture_Default = new SolidColorBrush(Color.FromRgb(1, 7, 19));

        public static Brush mCapture_Highlight = new SolidColorBrush(Color.FromRgb(0, 162, 255)); //System.Drawing.Color.FromArgb(28, 107, 182);

        public static Brush mCapture_Gallery_Default = new SolidColorBrush(Color.FromRgb(0, 7, 18));//System.Drawing.Color.FromArgb(46, 52, 66);

        public static System.Drawing.Color mCapture_Highlight1 = System.Drawing.Color.FromArgb(28, 28, 28);

        public static System.Drawing.Color mCapture_Gallery_Default1 = System.Drawing.Color.FromArgb(46, 52, 66);

        /// Transition Gallery
        public static System.Drawing.Color TG_Parent_H = System.Drawing.Color.FromArgb(8, 14, 26);

        public static System.Drawing.Color TG_Parent_D = System.Drawing.Color.FromArgb(1, 7, 19);

        public static System.Drawing.Color TG_Child_H = System.Drawing.Color.FromArgb(8, 14, 26);

        public static System.Drawing.Color TG_Child_D = System.Drawing.Color.FromArgb(1, 7, 19); //Color.FromArgb(29, 42, 61);


        /// Effects
        public static Brush efx_button_default = new SolidColorBrush(Color.FromRgb(32, 31, 31));

        public static Brush efx_button_highlight = new SolidColorBrush(Color.FromRgb(27, 94, 206));

        /// Song Gallery
        public static Brush SG_Child_D = new SolidColorBrush(Color.FromRgb(1, 7, 19)); //System.Drawing.Color.FromArgb(1, 7, 19);

        public static Brush SG_Child_H = new SolidColorBrush(Color.FromRgb(27, 94, 206)); // System.Drawing.Color.FromArgb(27, 94, 206);

        public static Brush SG_Item_D = new SolidColorBrush(Color.FromRgb(1, 7, 19)); // System.Drawing.Color.FromArgb(1, 7, 19);

        public static Brush SG_Item_H = new SolidColorBrush(Color.FromRgb(27, 94, 206)); // System.Drawing.Color.FromArgb(27, 94, 206);
    } 
}
