using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

public class SHelper
{
    #region Defines
    /*****************************************************************************/
    //Sense4 API
    /*
     * Packaging EL  API
     *   Lead in the API of Sense4.dll  as C# format
     */
    // ctlCode definition for S4Control
#pragma warning disable CS0414 // The field 'slElite.S4_LED_UP' is assigned but its value is never used
    static uint S4_LED_UP = 0x00000004;  // LED up
#pragma warning restore CS0414 // The field 'slElite.S4_LED_UP' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_LED_DOWN' is assigned but its value is never used
    static uint S4_LED_DOWN = 0x00000008;  // LED down
#pragma warning restore CS0414 // The field 'slElite.S4_LED_DOWN' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_LED_WINK' is assigned but its value is never used
    static uint S4_LED_WINK = 0x00000028;  // LED wink
#pragma warning restore CS0414 // The field 'slElite.S4_LED_WINK' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_GET_DEVICE_TYPE' is assigned but its value is never used
    static uint S4_GET_DEVICE_TYPE = 0x00000025;    //get device type
#pragma warning restore CS0414 // The field 'slElite.S4_GET_DEVICE_TYPE' is assigned but its value is never used
    static uint S4_GET_SERIAL_NUMBER = 0x00000026;  //get device serial
#pragma warning disable CS0414 // The field 'slElite.S4_GET_VM_TYPE' is assigned but its value is never used
    static uint S4_GET_VM_TYPE = 0x00000027;  // get VM type
#pragma warning restore CS0414 // The field 'slElite.S4_GET_VM_TYPE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_GET_DEVICE_USABLE_SPACE' is assigned but its value is never used
    static uint S4_GET_DEVICE_USABLE_SPACE = 0x00000029;  // get total space
#pragma warning restore CS0414 // The field 'slElite.S4_GET_DEVICE_USABLE_SPACE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_SET_DEVICE_ID' is assigned but its value is never used
    static uint S4_SET_DEVICE_ID = 0x0000002a;  // set device ID
#pragma warning restore CS0414 // The field 'slElite.S4_SET_DEVICE_ID' is assigned but its value is never used

    // device type definition 
#pragma warning disable CS0414 // The field 'slElite.S4_LOCAL_DEVICE' is assigned but its value is never used
    static uint S4_LOCAL_DEVICE = 0x00;     // local device 
#pragma warning restore CS0414 // The field 'slElite.S4_LOCAL_DEVICE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_MASTER_DEVICE' is assigned but its value is never used
    static uint S4_MASTER_DEVICE = 0x80;        // net master device
#pragma warning restore CS0414 // The field 'slElite.S4_MASTER_DEVICE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_SLAVE_DEVICE' is assigned but its value is never used
    static uint S4_SLAVE_DEVICE = 0xc0;     // net slave device
#pragma warning restore CS0414 // The field 'slElite.S4_SLAVE_DEVICE' is assigned but its value is never used

    // vm type definiton 
#pragma warning disable CS0414 // The field 'slElite.S4_VM_51' is assigned but its value is never used
    static uint S4_VM_51 = 0x00;        // VM51
#pragma warning restore CS0414 // The field 'slElite.S4_VM_51' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_VM_251_BINARY' is assigned but its value is never used
    static uint S4_VM_251_BINARY = 0x01;        // VM251 binary mode
#pragma warning restore CS0414 // The field 'slElite.S4_VM_251_BINARY' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_VM_251_SOURCE' is assigned but its value is never used
    static uint S4_VM_251_SOURCE = 0x02;        // VM251 source mode
#pragma warning restore CS0414 // The field 'slElite.S4_VM_251_SOURCE' is assigned but its value is never used


    // PIN type definition 
    static uint S4_USER_PIN = 0x000000a1;       // user PIN
    static uint S4_DEV_PIN = 0x000000a2;        // dev PIN
#pragma warning disable CS0414 // The field 'slElite.S4_AUTHEN_PIN' is assigned but its value is never used
    static uint S4_AUTHEN_PIN = 0x000000a3;     // autheticate Key
#pragma warning restore CS0414 // The field 'slElite.S4_AUTHEN_PIN' is assigned but its value is never used


    // file type definition 
#pragma warning disable CS0414 // The field 'slElite.S4_RSA_PUBLIC_FILE' is assigned but its value is never used
    static uint S4_RSA_PUBLIC_FILE = 0x00000006;        // RSA public file
#pragma warning restore CS0414 // The field 'slElite.S4_RSA_PUBLIC_FILE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_RSA_PRIVATE_FILE' is assigned but its value is never used
    static uint S4_RSA_PRIVATE_FILE = 0x00000007;       // RSA private file 
#pragma warning restore CS0414 // The field 'slElite.S4_RSA_PRIVATE_FILE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_EXE_FILE' is assigned but its value is never used
    static uint S4_EXE_FILE = 0x00000008;       // VM file
#pragma warning restore CS0414 // The field 'slElite.S4_EXE_FILE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_DATA_FILE' is assigned but its value is never used
    static uint S4_DATA_FILE = 0x00000009;      // data file
#pragma warning restore CS0414 // The field 'slElite.S4_DATA_FILE' is assigned but its value is never used

    // dwFlag definition for S4WriteFile
#pragma warning disable CS0414 // The field 'slElite.S4_CREATE_NEW' is assigned but its value is never used
    static uint S4_CREATE_NEW = 0x000000a5;     // create new file
#pragma warning restore CS0414 // The field 'slElite.S4_CREATE_NEW' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_UPDATE_FILE' is assigned but its value is never used
    static uint S4_UPDATE_FILE = 0x000000a6;        // update file
#pragma warning restore CS0414 // The field 'slElite.S4_UPDATE_FILE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_KEY_GEN_RSA_FILE' is assigned but its value is never used
    static uint S4_KEY_GEN_RSA_FILE = 0x000000a7;       // produce RSA key pair
#pragma warning restore CS0414 // The field 'slElite.S4_KEY_GEN_RSA_FILE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_SET_LICENCES' is assigned but its value is never used
    static uint S4_SET_LICENCES = 0x000000a8;       // set the license number for modle,available for net device only
#pragma warning restore CS0414 // The field 'slElite.S4_SET_LICENCES' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_CREATE_ROOT_DIR' is assigned but its value is never used
    static uint S4_CREATE_ROOT_DIR = 0x000000ab;        // create root directory, available for empty device only
#pragma warning restore CS0414 // The field 'slElite.S4_CREATE_ROOT_DIR' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_CREATE_SUB_DIR' is assigned but its value is never used
    static uint S4_CREATE_SUB_DIR = 0x000000ac;     // create child directory
#pragma warning restore CS0414 // The field 'slElite.S4_CREATE_SUB_DIR' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_CREATE_MODULE' is assigned but its value is never used
    static uint S4_CREATE_MODULE = 0x000000ad;      // create modle, available for net device only
#pragma warning restore CS0414 // The field 'slElite.S4_CREATE_MODULE' is assigned but its value is never used

    // the three parameters below must be bitwise-inclusive-or with S4_CREATE_NEW, only for executive file
#pragma warning disable CS0414 // The field 'slElite.S4_FILE_READ_WRITE' is assigned but its value is never used
    static uint S4_FILE_READ_WRITE = 0x00000000;      // can be read and written in executive file,default
#pragma warning restore CS0414 // The field 'slElite.S4_FILE_READ_WRITE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_FILE_EXECUTE_ONLY' is assigned but its value is never used
    static uint S4_FILE_EXECUTE_ONLY = 0x00000100;      // can NOT be read or written in executive file
#pragma warning restore CS0414 // The field 'slElite.S4_FILE_EXECUTE_ONLY' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_CREATE_PEDDING_FILE' is assigned but its value is never used
    static uint S4_CREATE_PEDDING_FILE = 0x00002000;        // create padding file
#pragma warning restore CS0414 // The field 'slElite.S4_CREATE_PEDDING_FILE' is assigned but its value is never used


    /* return value*/
    static uint S4_SUCCESS = 0x00000000;        // succeed
#pragma warning disable CS0414 // The field 'slElite.S4_UNPOWERED' is assigned but its value is never used
    static uint S4_UNPOWERED = 0x00000001;
#pragma warning restore CS0414 // The field 'slElite.S4_UNPOWERED' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_INVALID_PARAMETER' is assigned but its value is never used
    static uint S4_INVALID_PARAMETER = 0x00000002;
#pragma warning restore CS0414 // The field 'slElite.S4_INVALID_PARAMETER' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_COMM_ERROR' is assigned but its value is never used
    static uint S4_COMM_ERROR = 0x00000003;
#pragma warning restore CS0414 // The field 'slElite.S4_COMM_ERROR' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_PROTOCOL_ERROR' is assigned but its value is never used
    static uint S4_PROTOCOL_ERROR = 0x00000004;
#pragma warning restore CS0414 // The field 'slElite.S4_PROTOCOL_ERROR' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_DEVICE_BUSY' is assigned but its value is never used
    static uint S4_DEVICE_BUSY = 0x00000005;
#pragma warning restore CS0414 // The field 'slElite.S4_DEVICE_BUSY' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_KEY_REMOVED' is assigned but its value is never used
    static uint S4_KEY_REMOVED = 0x00000006;
#pragma warning restore CS0414 // The field 'slElite.S4_KEY_REMOVED' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_INSUFFICIENT_BUFFER' is assigned but its value is never used
    static uint S4_INSUFFICIENT_BUFFER = 0x00000011;
#pragma warning restore CS0414 // The field 'slElite.S4_INSUFFICIENT_BUFFER' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_NO_LIST' is assigned but its value is never used
    static uint S4_NO_LIST = 0x00000012;
#pragma warning restore CS0414 // The field 'slElite.S4_NO_LIST' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_GENERAL_ERROR' is assigned but its value is never used
    static uint S4_GENERAL_ERROR = 0x00000013;
#pragma warning restore CS0414 // The field 'slElite.S4_GENERAL_ERROR' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_UNSUPPORTED' is assigned but its value is never used
    static uint S4_UNSUPPORTED = 0x00000014;
#pragma warning restore CS0414 // The field 'slElite.S4_UNSUPPORTED' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_DEVICE_TYPE_MISMATCH' is assigned but its value is never used
    static uint S4_DEVICE_TYPE_MISMATCH = 0x00000020;
#pragma warning restore CS0414 // The field 'slElite.S4_DEVICE_TYPE_MISMATCH' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_FILE_SIZE_CROSS_7FFF' is assigned but its value is never used
    static uint S4_FILE_SIZE_CROSS_7FFF = 0x00000021;
#pragma warning restore CS0414 // The field 'slElite.S4_FILE_SIZE_CROSS_7FFF' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_DEVICE_UNSUPPORTED' is assigned but its value is never used
    static uint S4_DEVICE_UNSUPPORTED = 0x00006a81;
#pragma warning restore CS0414 // The field 'slElite.S4_DEVICE_UNSUPPORTED' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_FILE_NOT_FOUND' is assigned but its value is never used
    static uint S4_FILE_NOT_FOUND = 0x00006a82;
#pragma warning restore CS0414 // The field 'slElite.S4_FILE_NOT_FOUND' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_INSUFFICIENT_SECU_STATE' is assigned but its value is never used
    static uint S4_INSUFFICIENT_SECU_STATE = 0x00006982;
#pragma warning restore CS0414 // The field 'slElite.S4_INSUFFICIENT_SECU_STATE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_DIRECTORY_EXIST' is assigned but its value is never used
    static uint S4_DIRECTORY_EXIST = 0x00006901;
#pragma warning restore CS0414 // The field 'slElite.S4_DIRECTORY_EXIST' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_FILE_EXIST' is assigned but its value is never used
    static uint S4_FILE_EXIST = 0x00006a80;
#pragma warning restore CS0414 // The field 'slElite.S4_FILE_EXIST' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_INSUFFICIENT_SPACE' is assigned but its value is never used
    static uint S4_INSUFFICIENT_SPACE = 0x00006a84;
#pragma warning restore CS0414 // The field 'slElite.S4_INSUFFICIENT_SPACE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_OFFSET_BEYOND' is assigned but its value is never used
    static uint S4_OFFSET_BEYOND = 0x00006B00;
#pragma warning restore CS0414 // The field 'slElite.S4_OFFSET_BEYOND' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_PIN_BLOCK' is assigned but its value is never used
    static uint S4_PIN_BLOCK = 0x00006983;
#pragma warning restore CS0414 // The field 'slElite.S4_PIN_BLOCK' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_FILE_TYPE_MISMATCH' is assigned but its value is never used
    static uint S4_FILE_TYPE_MISMATCH = 0x00006981;
#pragma warning restore CS0414 // The field 'slElite.S4_FILE_TYPE_MISMATCH' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_CRYPTO_KEY_NOT_FOUND' is assigned but its value is never used
    static uint S4_CRYPTO_KEY_NOT_FOUND = 0x00009403;
#pragma warning restore CS0414 // The field 'slElite.S4_CRYPTO_KEY_NOT_FOUND' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_APPLICATION_TEMP_BLOCK' is assigned but its value is never used
    static uint S4_APPLICATION_TEMP_BLOCK = 0x00006985;
#pragma warning restore CS0414 // The field 'slElite.S4_APPLICATION_TEMP_BLOCK' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_APPLICATION_PERM_BLOCK' is assigned but its value is never used
    static uint S4_APPLICATION_PERM_BLOCK = 0x00009303;
#pragma warning restore CS0414 // The field 'slElite.S4_APPLICATION_PERM_BLOCK' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_DATA_BUFFER_LENGTH_ERROR' is assigned but its value is never used
    static int S4_DATA_BUFFER_LENGTH_ERROR = 0x00006700;
#pragma warning restore CS0414 // The field 'slElite.S4_DATA_BUFFER_LENGTH_ERROR' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_CODE_RANGE' is assigned but its value is never used
    static uint S4_CODE_RANGE = 0x00010000;
#pragma warning restore CS0414 // The field 'slElite.S4_CODE_RANGE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_CODE_RESERVED_INST' is assigned but its value is never used
    static uint S4_CODE_RESERVED_INST = 0x00020000;
#pragma warning restore CS0414 // The field 'slElite.S4_CODE_RESERVED_INST' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_CODE_RAM_RANGE' is assigned but its value is never used
    static uint S4_CODE_RAM_RANGE = 0x00040000;
#pragma warning restore CS0414 // The field 'slElite.S4_CODE_RAM_RANGE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_CODE_BIT_RANGE' is assigned but its value is never used
    static uint S4_CODE_BIT_RANGE = 0x00080000;
#pragma warning restore CS0414 // The field 'slElite.S4_CODE_BIT_RANGE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_CODE_SFR_RANGE' is assigned but its value is never used
    static uint S4_CODE_SFR_RANGE = 0x00100000;
#pragma warning restore CS0414 // The field 'slElite.S4_CODE_SFR_RANGE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_CODE_XRAM_RANGE' is assigned but its value is never used
    static uint S4_CODE_XRAM_RANGE = 0x00200000;
#pragma warning restore CS0414 // The field 'slElite.S4_CODE_XRAM_RANGE' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'slElite.S4_ERROR_UNKNOWN' is assigned but its value is never used
    static uint S4_ERROR_UNKNOWN = 0xffffffff;
#pragma warning restore CS0414 // The field 'slElite.S4_ERROR_UNKNOWN' is assigned but its value is never used

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct SENSE4_CONTEXT
    {
        public int dwIndex;     //device index
        public int dwVersion;       //version		
        public long hLock;          //device handle
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] reserve;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
        public byte[] bAtr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] bID;
        public uint dwAtrLen;
    }

    //Assume that Sense4user.dll in , if not, modify the lines below
    [DllImport(@"./Sense4.dll")]
    private static extern uint S4Enum([MarshalAs(UnmanagedType.LPArray), Out] SENSE4_CONTEXT[] s4_context, ref uint size);
    [DllImport(@"./Sense4.dll")]
    private static extern uint S4Open(ref SENSE4_CONTEXT s4_context);
    [DllImport(@"./Sense4.dll")]
    private static extern uint S4Close(ref SENSE4_CONTEXT s4_context);
    [DllImport(@"./Sense4.dll")]
    private static extern uint S4Control(ref SENSE4_CONTEXT s4Ctx, uint ctlCode, byte[] inBuff,
        uint inBuffLen, byte[] outBuff, uint outBuffLen, ref uint BytesReturned);
    [DllImport(@"./Sense4.dll")]
    private static extern uint S4CreateDir(ref SENSE4_CONTEXT s4Ctx, string DirID, uint DirSize, uint Flags);
    [DllImport(@"./Sense4.dll")]
    private static extern uint S4ChangeDir(ref SENSE4_CONTEXT s4Ctx, string Path);
    [DllImport(@"./Sense4.dll")]
    private static extern uint S4EraseDir(ref SENSE4_CONTEXT s4Ctx, string DirID);
    [DllImport(@"./Sense4.dll")]
    private static extern uint S4VerifyPin(ref SENSE4_CONTEXT s4Ctx, byte[] Pin, uint PinLen, uint PinType);
    [DllImport(@"./Sense4.dll")]
    private static extern uint S4ChangePin(ref SENSE4_CONTEXT s4Ctx, byte[] OldPin, uint OldPinLen,
        byte[] NewPin, uint NewPinLen, uint PinType);
    [DllImport(@"./Sense4.dll")]
    private static extern uint S4WriteFile(ref SENSE4_CONTEXT s4Ctx, string FileID, uint Offset,
        byte[] Buffer, uint BufferSize, uint FileSize, ref uint BytesWritten, uint Flags,
        uint FileType);
    [DllImport(@"./Sense4.dll")]
    private static extern uint S4Execute(ref SENSE4_CONTEXT s4Ctx, string FileID, byte[] InBuffer,
        uint InbufferSize, byte[] OutBuffer, uint OutBufferSize, ref uint BytesReturned);
    #endregion

    public static void checkDevice(out string msg, out short success)
    {
        msg = "";
        success = 0;

        /// Check device.
        uint size = 0;
        uint ret = S4Enum(null, ref size);
        if (size == 0) { msg = "Dongle not found!"; return; }

        /// Check device memory.
        SENSE4_CONTEXT[] si = new SENSE4_CONTEXT[size / Marshal.SizeOf(typeof(SENSE4_CONTEXT))];
        ret = S4Enum(si, ref size);
        if (ret != S4_SUCCESS) { msg = "Insufficient memory!"; return; }

        /// Open device.
        ret = S4Open(ref si[0]);
        if (ret != S4_SUCCESS) { msg = "Can't access the dongle"; return; }

        ///// Check root directory.
        //ret = S4ChangeDir(ref si[0], "\\");
        //if (ret != S4_SUCCESS) { S4Close(ref si[0]); msg = "Can't find the root directory"; return; }

        /// Verify developer pin.
        ret = S4VerifyPin(ref si[0], Constants.new_dev_pin, 24, S4_DEV_PIN);
        if (ret != S4_SUCCESS) { S4Close(ref si[0]); msg = "Merchant identification failed"; return; }

        /// Verify user pin.
        ret = S4VerifyPin(ref si[0], Constants.usr_pin, 8, S4_USER_PIN);
        if (ret != S4_SUCCESS) { S4Close(ref si[0]); msg = "User authentication failed"; return; }

        /// Read private key file.
        int m = Convert.ToInt32(ReadData(si[0], "5100", 0, 4));
        int splt = 0;
        int n = 0;
        string pky = "";
        while ((splt * 240) < m)
        {
            n = (m - (splt * 240) >= 240) ? 240 : m - (splt * 240);
            pky += ReadData(si[0], "510" + Convert.ToString(splt + 1), 0, n);
            splt++;
        }

        /// Read data1
        string data1 = ReadData(si[0], "5200", 0, 14);
        if (string.Compare(data1, Constants.mCode, false) != 0) { S4Close(ref si[0]); msg = "Product identification failed"; return; }

        /// Read data2
        string data2 = ReadData(si[0], "5201", 0, 88);
        byte[] a = Convert.FromBase64String(data2);
        if (string.IsNullOrEmpty(data2)) { S4Close(ref si[0]); msg = "User identification failed"; return; }
        try
        {
            data2 = DecryptFromPriText(a, pky);
        }
        catch { S4Close(ref si[0]); msg = "User identification failed"; return; }

        /// Read data3
        string data3 = ReadData(si[0], "5202", 0, 88);
        a = Convert.FromBase64String(data3);
        if (string.IsNullOrEmpty(data3)) { S4Close(ref si[0]); msg = "Device identification failed"; return; }
        try
        {
            data3 = DecryptFromPriText(a, pky);
        }
        catch { S4Close(ref si[0]); msg = "Device identification failed"; return; }

        GetSerialNo(out string d3, out short s);
        if (string.Compare(data3, d3) != 0) { S4Close(ref si[0]); msg = "Device identification failed"; return; }

        msg = string.Format(@"MS{0} - {1}", data2, d3);
        success = 1;

        S4Close(ref si[0]);
    }

    static RSAParameters getPubKey(string filename)
    {
        RSAParameters param = new RSAParameters();
        try
        {
            byte[] modB = new byte[128];
            byte[] pubexpB = new byte[128];

            BinaryReader br = new BinaryReader(File.Open(filename, FileMode.Open));
            br.Read(modB, 0, 128);
            br.Read(pubexpB, 0, 128);
            br.Close();

            param.Modulus = trimLeadZeros(modB);
            param.Exponent = trimLeadZeros(pubexpB);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return param;
    }

    static byte[] trimLeadZeros(byte[] buf)
    {
        if (buf[0] != 0)
            return buf;
        int i = 0;
        for (i = 0; i < buf.Length; i++)
            if (buf[i] != 0)
                break;
        byte[] res = new byte[buf.Length - i];
        for (int j = 0; j < res.Length; j++)
            res[j] = buf[i + j];
        return res;
    }

    static string ReadData(SENSE4_CONTEXT cx, string fileId, int startAt, int length)
    {
        byte[] inBuff = new byte[250];
        byte[] outBuff = new byte[250];
        uint r1 = 0, r2 = 0;

        try
        {
            inBuff[0] = (byte)startAt;
            inBuff[1] = (byte)length;
            BitConverter.GetBytes(Convert.ToUInt16(fileId, 16)).CopyTo(inBuff, 2);

            r1 = S4Execute(ref cx, "0003", inBuff, 240, outBuff, 240, ref r2);
            if (r1 == S4_SUCCESS)
            {
                inBuff = new byte[r2];
                Array.Copy(outBuff, inBuff, r2);
                ASCIIEncoding encoding = new ASCIIEncoding();
                return encoding.GetString(inBuff);
            }
        }
        catch { }

        return string.Empty;
    }

    static string DecryptFromPriText(byte[] plain, string priF)
    {
        byte[] decrypted = null;
        using (var rsa = new RSACryptoServiceProvider(512))
        {
            rsa.PersistKeyInCsp = false;
            rsa.FromXmlString(priF);
            decrypted = rsa.Decrypt(plain, true);
        }
        return Encoding.ASCII.GetString(decrypted);
    }

    static void GetSerialNo(out string msg, out short success)
    {
        msg = "";
        success = 0;

        /// Check device.
        uint size = 0;
        uint ret = S4Enum(null, ref size);
        if (size == 0) { msg = "Dongle not found!"; return; }

        /// Check device memory.
        SENSE4_CONTEXT[] si = new SENSE4_CONTEXT[size / Marshal.SizeOf(typeof(SENSE4_CONTEXT))];
        ret = S4Enum(si, ref size);
        if (ret != S4_SUCCESS) { msg = "Insufficient memory!"; return; }

        /// Open device.
        ret = S4Open(ref si[0]);
        if (ret != S4_SUCCESS) { msg = "Can't access the dongle"; return; }

        byte[] inBuf = new byte[128];
        byte[] outBuf = new byte[8];
        uint i = 0;
        ret = S4Control(ref si[0], S4_GET_SERIAL_NUMBER, inBuf, (uint)inBuf.Length, outBuf, (uint)outBuf.Length, ref i);
        S4Close(ref si[0]);

        msg = BitConverter.ToString(outBuf).Replace("-", "");
        success = 1;
    }
}
