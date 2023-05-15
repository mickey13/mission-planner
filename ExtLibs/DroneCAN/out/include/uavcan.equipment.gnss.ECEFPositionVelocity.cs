
using uint8_t = System.Byte;
using uint16_t = System.UInt16;
using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

using int8_t = System.SByte;
using int16_t = System.Int16;
using int32_t = System.Int32;
using int64_t = System.Int64;

using float32 = System.Single;

using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace DroneCAN
{
    public partial class DroneCAN 
    {
        public partial class uavcan_equipment_gnss_ECEFPositionVelocity: IDroneCANSerialize 
        {
            public const int UAVCAN_EQUIPMENT_GNSS_ECEFPOSITIONVELOCITY_MAX_PACK_SIZE = 99;
            public const ulong UAVCAN_EQUIPMENT_GNSS_ECEFPOSITIONVELOCITY_DT_SIG = 0x24A5DA4ABEE3A248;

            [MarshalAs(UnmanagedType.ByValArray,SizeConst=3)] public Single[] velocity_xyz = new Single[3];
            [MarshalAs(UnmanagedType.ByValArray,SizeConst=3)] public int64_t[] position_xyz_mm = new int64_t[3];
            public uint8_t covariance_len; [MarshalAs(UnmanagedType.ByValArray,SizeConst=36)] public Single[] covariance = Enumerable.Range(1, 36).Select(i => new Single()).ToArray();

            public void encode(dronecan_serializer_chunk_cb_ptr_t chunk_cb, object ctx, bool fdcan = false)
            {
                encode_uavcan_equipment_gnss_ECEFPositionVelocity(this, chunk_cb, ctx, fdcan);
            }

            public void decode(CanardRxTransfer transfer, bool fdcan = false)
            {
                decode_uavcan_equipment_gnss_ECEFPositionVelocity(transfer, this, fdcan);
            }

            public static uavcan_equipment_gnss_ECEFPositionVelocity ByteArrayToDroneCANMsg(byte[] transfer, int startoffset, bool fdcan = false)
            {
                var ans = new uavcan_equipment_gnss_ECEFPositionVelocity();
                ans.decode(new DroneCAN.CanardRxTransfer(transfer.Skip(startoffset).ToArray()), fdcan);
                return ans;
            }
        }
    }
}