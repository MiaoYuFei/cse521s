using System.Buffers.Binary;

namespace firmware.Command
{
    public class SetSelectParametersCommand : BaseCommand
    {
        public enum SelParamMemBankEnum : byte
        {
            RFU = 0x0,
            EPC = 0x1,
            TID = 0x2,
            User = 0x3,
        }

        private byte m_SelParamTarget = 0x0;

        public byte SelParamTarget
        {
            get => m_SelParamTarget;
            internal set
            {
                m_SelParamTarget = value;
                SelParam = (byte)((SelParam & 0x1f) | (byte)(m_SelParamTarget << 5));
            }
        }

        private byte m_SelParamAction = 0x0;

        public byte SelParamAction
        {
            get => m_SelParamAction;
            internal set
            {
                m_SelParamAction = value;
                SelParam = (byte)((SelParam & 0xe3) | (byte)((m_SelParamAction & 0x7) << 2));
            }
        }

        private SelParamMemBankEnum m_SelParamMemBank = SelParamMemBankEnum.RFU;

        public SelParamMemBankEnum SelParamMemBank
        {
            get => m_SelParamMemBank;
            internal set
            {
                m_SelParamMemBank = value;
                SelParam = (byte)((SelParam & 0xfc) | ((byte)m_SelParamMemBank & 0x3));
            }
        }

        private byte m_SelParam = 0x0;

        public byte SelParam {
            get => m_SelParam;
            private set
            {
                m_SelParam = value;
                Parameter[0] = m_SelParam;
            }
        }

        private uint m_Ptr = 0;

        public uint Ptr
        {
            get => m_Ptr;
            private set
            {
                m_Ptr = value;
                Span<byte> ptrBytes = new byte[sizeof(uint)];
                BinaryPrimitives.WriteUInt32BigEndian(ptrBytes, m_Ptr);
                Array.Copy(ptrBytes.ToArray(), 0, Parameter, 1, sizeof(uint));
            }
        }

        private byte m_MaskLen = 0;

        public byte MaskLen {
            get => m_MaskLen;
            private set
            {
                m_MaskLen = value;
                Parameter[5] = m_MaskLen;
            }
        }

        private bool m_Truncate = false;

        public bool Truncate {
            get => m_Truncate;
            private set
            {
                m_Truncate = value;
                Parameter[6] = m_Truncate ? (byte)0x80 : (byte)0x00;
            }
        }

        private byte[] m_Mask = Array.Empty<byte>();

        public byte[] Mask
        {
            get => m_Mask;
            private set
            {
                m_Mask = value;
                Array.Copy(m_Mask, 0, Parameter, 7, m_Mask.Length);
            }
        }

        public SetSelectParametersCommand(byte selParamTarget, byte selParamAction, SelParamMemBankEnum selParamMemBank, uint ptr, byte maskLen, bool truncate, byte[] mask) : base((ushort)(7 + maskLen / 8))
        {
            if (maskLen % 8 != 0 || mask.Length != maskLen / 8)
            {
                throw new ArgumentException();
            }

            CommandType = CommandTypeEnum.SET_SELECT_PARA;

            SelParamTarget = selParamTarget;
            SelParamAction = selParamAction;
            SelParamMemBank = selParamMemBank;
            Ptr = ptr;
            MaskLen = maskLen;
            Truncate = truncate;
            Mask = mask;
        }
    }
}
