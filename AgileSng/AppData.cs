using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace AgileHDWPF.AgileSng
{
    public enum EffectType { Mix, DVE }

    public class Stream
    {
        public string StreamId { get; set; }
        public string Path { get; set; }
        public string Cmd { get; set; }

        public Stream(string _streamId, string _path, string _cmd)
        {
            StreamId = _streamId;
            Path = _path;
            Cmd = _cmd;
        }
    }

    public static class AppData
    {
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

            public static string into_video
            {
                get { return string.Format(@"{0}\{1}", video_path, "ahd_intro.mp4"); }                
            }

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
        }

        public class Scene
        {
            public const string switcher = @"<m_scene video_stub='video-stub.png' image_stub='image-stub.png' name='mixer_scene_001'>
                    <background>
                        <video id='amix1' stream_idx='-1' show='false' w='0' h='0' audio_gain = '-80'/>
		                <video id='amix2' stream_idx='-1' show='false' w='0' h='0' audio_gain = '-80'/>
			            <video id='LayerB_BG' stream_idx='-1' show='true' w='1.0' h='1.0' alpha='1' audio_gain='-80'/>
		            </background>
		            <scene_3d view_angle='45.0'>
			            <camera cx='0.0' cy='0.0' cz='0' vert='0.0' horz='0' rotate='0.0' distance='0.0' zoom='1' show='true'/>
		            </scene_3d>
		            <foreground parallax-shift='0.05'>
			            <video id='LayerA_FG' stream_idx='-1' show='true' alpha='1' w='1' h='1' audio_gain='-80' x='0' y='0' sw='1' sh='1' z='0' sx='0' sy='0' orgx='0.0' orgy='0.0' orgw='1.0' orgh='1.0'/>
			            <video id='M_Filter' stream_idx='-1' stream_id='MFilter' show='false' alpha='1' w='1' h='1' audio_gain='-80'/>
                        <video id='Freeze' stream_idx='-1' h='1' w='1' show='false' audio_gain='-80'/>
                        <video id='FTB' stream_idx='-1' stream_id='FTB' h='1' w='1' show='false' audio_gain='-80'/>
		            </foreground>
		            <view invoke='true'/>
	                <timeline invoke='true'/>
	            </m_scene>";

            private const string switcher2 = @"<m_scene video_stub='video-stub.png' image_stub='image-stub.png' name='mixer_scene_001'>
	            <background show='true'>
		            <video id='L1' stream_idx='0' show='1' w='1' h='1' audio_gain = '1'/>
		            <video id='L2' stream_idx='1' show='1' w='1' h='1' audio_gain = '1'/>
		
		            <video id='D1' stream_idx='2' show='1' w='1' h='1' audio_gain = '1'/>
		            <video id='D2' stream_idx='3' show='1' w='1' h='1' audio_gain = '1'/>
		            <video id='D3' stream_idx='4' show='1' w='1' h='1' audio_gain = '1'/>
		            <video id='D4' stream_idx='5' show='1' w='1' h='1' audio_gain = '1'/>
		            <video id='A5' stream_idx='6' show='1' w='1' h='1' audio_gain = '1'/>
		
		            <video id='LayerB_BG' stream_idx='1' show='1' w='1' h='1' alpha='1' audio_gain = '-80'/>
	            </background>
	            <scene_3d view_angle='45.0'>
		            <camera cx='0.0' cy='0.0' cz='0' vert='0.0' horz='0' rotate='0.0' distance='0.0' zoom='1' show='true' />
		            <group id='Transition' show='true' h='1.0' w='1.0' d='1.0' rh='0' x='0'>
			            <group id='Virtual' show='1' h='1' w='1.0' d='1.0' z='0' rh='0'>
				            <video id='LayerB_B' stream_idx='1' show='1' w='1.83' h='1.83' z='-1.0' alpha='1' audio_gain = '-80' />
				            <group id='Effect' h='1.0' w='1.0' d='1.0' show='true' />
				            <video id='LayerA_A' stream_idx='1' show='1.0' w='1.0' h='1.0' z='0.0' alpha='1' audio_gain = '-80' />
			            </group>
		            </group>
	            </scene_3d>
	            <foreground parallax-shift='0.05'>
		            <video id='LayerA_FG' stream_idx='0' show='1' alpha='1' w='1' h='1' audio_gain = '-80'/>
		            <video id='M_Filter' stream_id='MFilter' show='0' alpha='1' w='1' h='1' alpha='1' audio_gain = '-80'/>
	            </foreground>
                <view invoke='true'/>
	            <timeline invoke='true'/>
            </m_scene>";
        }

        public class Stream
        {
            public const string strLive1 = "live1";
            public const string strLive2 = "live2";
            public const string strLive3 = "live3";
            public const string strLive4 = "live4";
            public const string strLive5 = "live5";
            public const string strDDR1 = "ddr1";
            public const string strDDR2 = "ddr2";
            public const string strDDR3 = "ddr3";
            public const string strDDR4 = "ddr4";
            public const string strDDR5 = "ddr5";
            public const string strDDR6 = "ddr6";
            public const string strDDR7 = "ddr7";
            public const string strDDR8 = "ddr8";
            public const string strDDR9 = "ddr9";
            public const string strDDR10 = "ddr10";
            public const string strAMix = "amix";

            public static string[] InputSource = new string[11] { strLive1, strLive2, strLive3, strLive4, strLive5, strDDR1, strDDR2, strDDR3, strDDR4, strDDR5, strAMix };
            public static string[] InputSourceAllowed = new string[7] { strLive1, strLive2, strDDR1, strDDR2, strDDR3, strDDR4, strAMix };
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
            public const string mixer_cmd = "cmd";
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
            public const string obj_layer_d6 = "DDR6.png";
            public const string obj_layer_d7 = "DDR7.png";
            public const string obj_layer_d8 = "DDR8.png";
            public const string obj_layer_d9 = "DDR9.png";
            public const string obj_layer_d10 = "DDR10.png";

            public const string view = "view";
        }


        public static string strLayerA = string.Empty;

        public static string strLayerB = string.Empty;

        public static string strLayerA_img_filter = string.Empty;

        public static double VirtualZero = 0.0;

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

        public static readonly string Props_Camera = "cx='0.0' cy='0.0' cz='0' vert='0.0' horz='0' rotate='0.0' distance='0.0' zoom='1' show='true'";

        public static readonly string Props_Transition = "show='1' h='1' w='1.0' d='1.0' z='0' rh='0'";


        public class VideoOutputFormat
        {
            public const int SD_Width = 720;

            public const int SD_PalHeight = 576;

            public const int SD_NtscHeight = 480;

            public const int HD1080_Width = 1920;

            public const int HD1080_Height = 1080;

            public const int HD720_Width = 1280;

            public const int HD720_Height = 720;

            public const int UHD_Width = 3840;

            public const int UHD_Height = 2160;
        }

        public static bool LayerA_State = true;
        public static bool LayerB_State = false;


        public class Song
        {
            public const string Header = @"
<s_project name=""{0}"" version=""1.0"">
    <video path=""{1}"" loop=""{2}"" mute=""{3}""/>
    <audio path=""{4}"" loop=""{5}"" mute=""{6}""/>
    <items/>
</s_project>";

            public const string Detail = @"<item file=""{0}"" duration=""{1}"" hold=""{2}""/>";

            public const string Title = "ahd_song_";

            public const string Extension = ".ahs";

            public static string Scene = @"<m_scene name='scene_000'>
                    <background>
		                <video id='BG_A' stream_idx='-1' stream_id='sba' show='false' w='0' h='0' audio_gain = '0'/>
			            <video id='BG_V' stream_idx='-1' stream_id='sbv' show='true' w='1.0' h='1.0' alpha='1' audio_gain='0'/>
		            </background>
		            <scene_3d view_angle='45.0'>
			            <camera />
		            </scene_3d>
		            <foreground />
		            <view />
                </m_scene>";

            public class Stream
            {
                public static string SBV = "sbv";
                public static string SBA = "sba";

                public static string SLA = "sla";
                public static string SLB = "slb";

                public static string SD1 = "sd1";
                public static string SD2 = "sd2";
                public static string SD3 = "sd3";
                public static string SD4 = "sd4";
                public static string SD5 = "sd5";

                public static string SB1 = "sb1";
                public static string SB2 = "sb2";
                public static string SB3 = "sb3";
                public static string SB4 = "sb4";
                public static string SB5 = "sb5";

                public static string SC1 = "sc1";
                public static string SC2 = "sc2";
                public static string SC3 = "sc3";
                public static string SC4 = "sc4";
                public static string SC5 = "sc5";

                public static string SG1 = "sg1";
                public static string SG2 = "sg2";
                public static string SG3 = "sg3";
                public static string SG4 = "sg4";
                public static string SG5 = "sg5";
            }
        }

        public class Filters
        {
            static public string audio = "Audio Files(*.WAV;*.MP3)|;*.WAV;*.MP3";
            static public string video = "Video Files(*.AVI;*.MP4;*.MPG;*.MPE;*.MPEG;*.MKV;*.MOV;*.M4V;*.FLV;*.MXF;*.WEBM;*.WMV;*.M2TS;*.MTS)|(;*.AVI;*.MP4;*.MPG;*.MPE;*.MPEG;*.MKV;*.MOV;*.M4V;*.FLV;*.MXF;*.WEBM;*.WMV;*.M2TS;*.MTS";
            static public string image = "Image Files |*.jpeg; *.jpg; *.png";
            static public string allImage = "Image Files(*.BMP;*.JPG;*.JPEG;*.JPE;*.GIF;*.PNG;*.EXIF;*.TIF;*.TIFF;*.TGA;*.TPIC)|;*.BMP;*.JPG;*.JPEG;*.JPE;*.GIF;*.PNG;*.EXIF;*.TIF;*.TIFF;*.TGA;*.TPIC";
            static public string playlist = "MPlaylist Files (*.mpl, *.xml)|*.mpl;*.xml;*.mlp|All Files|*.*";
            //static public string ticker = "Ticker Files(*.TXT)|*.TXT";
            static public string transition = "Transition Files(*.XML)|*.xml";
            static public string s_project = "Song Project Files(*.ahs)|*.ahs";
        }
    }
}